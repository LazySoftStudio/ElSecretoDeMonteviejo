using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace HeyHeyThere.SceneTransitionsFree
{
    /// <summary>
    /// Full-screen transitions with nothing to set up: the first call makes a canvas over everything,
    /// kept across scene loads.
    /// <code>
    /// SceneTransition.ChangeScene("Level2", "diamonds");
    /// yield return SceneTransition.Cover("iris", new SceneTransition.Options { CenterOn = player });
    /// // ...teleport the player, load a save...
    /// yield return SceneTransition.Reveal("iris");
    /// </code>
    /// A style is a name from <see cref="Styles"/> or any greyscale texture (black covers first). Each
    /// call returns the coroutine playing it, to yield on; starting one while another plays replaces it.
    /// </summary>
    public class SceneTransition : MonoBehaviour
    {
        /// <summary>The shaders' and masks' folder under Resources.</summary>
        public const string Folder = "Scene Transitions Free";

        /// <summary>The canvas's sorting order: canvases sorted above it stay on top of the transition.</summary>
        public const int SortingOrder = 1000;

        /// <summary>Every pattern, in the order of the transition shader's _Style; "mask" is
        /// <see cref="MaskStyle"/>, a rule texture's style rather than a pattern.</summary>
        public static readonly string[] AllPatterns = { "fade", "dither", "wipe", "diamonds", "circles", "iris", "clock", "blinds",
            "checker", "noise", "spiral", "curtain", "zigzag", "shards", "bars", "hexagons", "tiles", "mask", "triangles",
            "waves", "rays", "box", "interlace", "rings", "bubbles", "pinwheel" };

        public const int MaskStyle = 17;

        /// <summary>The screen shader's styles, in the order of its _Style.</summary>
        public static readonly string[] ScreenStyles = { "pixelate", "swirl", "ripple", "zoom", "slice", "melt", "tv", "wave", "blur" };

        static readonly string[] patterns = { "fade", "dither", "wipe", "diamonds", "circles", "iris" };

        public static readonly int Progress = Shader.PropertyToID("_Progress"), Style = Shader.PropertyToID("_Style"),
            ColorId = Shader.PropertyToID("_Color"), Softness = Shader.PropertyToID("_Softness"), Angle = Shader.PropertyToID("_Angle"),
            Cells = Shader.PropertyToID("_Cells"), Center = Shader.PropertyToID("_Center"), Flip = Shader.PropertyToID("_Flip"),
            PixelSize = Shader.PropertyToID("_PixelSize"), Seed = Shader.PropertyToID("_Seed"), Mask = Shader.PropertyToID("_Mask"),
            Strength = Shader.PropertyToID("_Strength"), FadeStart = Shader.PropertyToID("_FadeStart");

        /// <summary>A transition's settings; any left null keeps the style's default.</summary>
        public class Options
        {
            /// <summary>Seconds per half (cover, reveal). Default 0.45.</summary>
            public float? Duration;
            /// <summary>Default black.</summary>
            public Color? Color;
            /// <summary>Maps time 0..1 to progress 0..1. Default sine in and out.</summary>
            public Func<float, float> Ease;
            /// <summary>ChangeScene reveals with this style instead.</summary>
            public string RevealStyle;
            /// <summary>Where iris, clock, spiral, swirl, ripple and zoom close to: a viewport point (0..1, y up).</summary>
            public Vector2? Center;
            /// <summary>Or an object to close on: a UI element, or one the main camera sees. Read when the call is made.</summary>
            public Transform CenterOn;
            /// <summary>Direction in degrees: 0 = left to right, 90 = bottom to top.</summary>
            public float? Angle;
            /// <summary>Pattern cells across the screen's height.</summary>
            public float? Cells;
            /// <summary>Edge blur, 0..0.5.</summary>
            public float? Softness;
            /// <summary>Runs the pattern backwards: iris opens, curtain splits.</summary>
            public bool? Flip;
            /// <summary>Snaps the pattern to blocks of this many screen pixels, for pixel-art games.</summary>
            public float? PixelSize;
            /// <summary>Screen styles: how far they distort, and when the colour comes in (0..1).</summary>
            public float? Strength, FadeStart;
            /// <summary>Varies the random patterns.</summary>
            public float? Seed;

            public Options Clone() => (Options)MemberwiseClone();
        }

        static SceneTransition instance;
        static Shader transition, screen;
        static bool screenLooked;
        static Dictionary<string, Texture2D> masks;

        RawImage cover;
        Material material;  // the cover's, made here (without one, cover.material is UGUI's shared default)
        Coroutine running;
        AsyncOperation loading;
        RenderTexture capture;
        bool capturing;

        /// <summary>The patterns this package's transition shader has.</summary>
        public static IReadOnlyList<string> Patterns => patterns;

        /// <summary>The screen shader, if this package has it.</summary>
        public static Shader ScreenShader
        {
            get
            {
                if (!screenLooked)
                {
                    screen = Resources.Load<Shader>($"{Folder}/Screen");
                    screenLooked = true;
                }
                return screen;
            }
        }

        public static Shader TransitionShader => transition ? transition : transition = Resources.Load<Shader>($"{Folder}/Transition");

        /// <summary>The rule masks in Resources/Scene Transitions Free/Masks, by name.</summary>
        public static IReadOnlyDictionary<string, Texture2D> Masks =>
            masks ??= Resources.LoadAll<Texture2D>($"{Folder}/Masks").OrderBy(t => t.name, StringComparer.Ordinal).ToDictionary(t => t.name);

        /// <summary>Every style by name: the patterns, the screen styles, the masks.</summary>
        public static IReadOnlyList<string> Styles =>
            patterns.Concat(ScreenShader ? ScreenStyles : new string[0]).Concat(Masks.Keys).ToArray();

        public static bool IsPlaying => instance && instance.running != null;

        /// <summary>Covers the screen. The cover stays until Reveal.</summary>
        public static Coroutine Cover(string style = "fade", Options options = null) => Play(style, null, options, 0f, 1f);

        public static Coroutine Cover(Texture mask, Options options = null) => Play(null, mask, options, 0f, 1f);

        /// <summary>Uncovers the screen, playing the style backwards.</summary>
        public static Coroutine Reveal(string style = "fade", Options options = null) => Play(style, null, options, 1f, 0f);

        public static Coroutine Reveal(Texture mask, Options options = null) => Play(null, mask, options, 1f, 0f);

        /// <summary>Covers, loads the scene (in the background while it covers), reveals. The scene must be in the build's scene list.</summary>
        public static Coroutine ChangeScene(string scene, string style = "fade", Options options = null) =>
            Change(() => SceneManager.LoadSceneAsync(scene), style, null, options);

        public static Coroutine ChangeScene(int buildIndex, string style = "fade", Options options = null) =>
            Change(() => SceneManager.LoadSceneAsync(buildIndex), style, null, options);

        public static Coroutine ChangeScene(string scene, Texture mask, Options options = null) =>
            Change(() => SceneManager.LoadSceneAsync(scene), null, mask, options);

        /// <summary>
        /// A material for a style, options applied, to drive _Progress yourself (an Animator, a slider)
        /// on a RawImage or through Graphics.Blit. An unknown name gives fade.
        /// </summary>
        public static Material MakeMaterial(string style, Options options = null)
        {
            Material m;
            int screenStyle = Array.IndexOf(ScreenStyles, style);
            if (screenStyle >= 0 && ScreenShader)
            {
                m = new Material(ScreenShader) { name = style };
                m.SetFloat(Style, screenStyle);
            }
            else if (style != null && Masks.TryGetValue(style, out var mask))
            {
                m = MakeMaterial(mask);
                m.name = style;
                m.SetFloat(Softness, 0.08f);
            }
            else
            {
                if (Array.IndexOf(patterns, style) < 0)
                    Debug.LogError($"SceneTransition: unknown style {style}, using fade");
                m = new Material(TransitionShader) { name = style };
                m.SetFloat(Style, Array.IndexOf(patterns, style) < 0 ? 0 : Array.IndexOf(AllPatterns, style));
            }
            Apply(m, options);
            return m;
        }

        public static Material MakeMaterial(Texture mask, Options options = null)
        {
            var m = new Material(TransitionShader) { name = mask ? mask.name : "mask" };
            m.SetFloat(Style, MaskStyle);
            m.SetTexture(Mask, mask);
            Apply(m, options);
            return m;
        }

        static void Apply(Material m, Options o)
        {
            if (o == null)
                return;
            o = Resolved(o);
            if (o.Color is Color c) m.SetColor(ColorId, c);
            if (o.Center is Vector2 at) m.SetVector(Center, at);
            if (o.Angle is float angle) m.SetFloat(Angle, angle);
            if (o.Cells is float cells) m.SetFloat(Cells, cells);
            if (o.Softness is float soft) m.SetFloat(Softness, soft);
            if (o.Flip is bool flip) m.SetFloat(Flip, flip ? 1f : 0f);
            if (o.PixelSize is float size) m.SetFloat(PixelSize, size);
            if (o.Strength is float strength) m.SetFloat(Strength, strength);
            if (o.FadeStart is float start) m.SetFloat(FadeStart, start);
            if (o.Seed is float seed) m.SetFloat(Seed, seed);
        }

        /// <summary>The options with CenterOn turned into a viewport point, so it outlives the scene it points into.</summary>
        static Options Resolved(Options o)
        {
            if (o == null || !o.CenterOn)
                return o;
            o = o.Clone();
            if (o.CenterOn is RectTransform rect && rect.GetComponentInParent<Canvas>() is Canvas canvas)
            {
                var root = canvas.rootCanvas;
                var cam = root.renderMode == RenderMode.ScreenSpaceOverlay ? null : root.worldCamera;
                Vector2 point = RectTransformUtility.WorldToScreenPoint(cam, rect.TransformPoint(rect.rect.center));
                o.Center = new Vector2(point.x / Screen.width, point.y / Screen.height);
            }
            else if (Camera.main)
                o.Center = Camera.main.WorldToViewportPoint(o.CenterOn.position);
            o.CenterOn = null;
            return o;
        }

        static SceneTransition Instance
        {
            get
            {
                if (instance)
                    return instance;
                var go = new GameObject("SceneTransition", typeof(Canvas), typeof(GraphicRaycaster));
                DontDestroyOnLoad(go);
                var canvas = go.GetComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.sortingOrder = SortingOrder;
                instance = go.AddComponent<SceneTransition>();
                instance.cover = new GameObject("Cover", typeof(RectTransform), typeof(RawImage)).GetComponent<RawImage>();
                var r = instance.cover.rectTransform;
                r.SetParent(go.transform, false);
                r.anchorMin = Vector2.zero;
                r.anchorMax = Vector2.one;
                r.offsetMin = r.offsetMax = Vector2.zero;
                instance.cover.raycastTarget = true;  // clicks stop at the cover
                instance.cover.enabled = false;
                return instance;
            }
        }

        static Coroutine Play(string style, Texture mask, Options options, float from, float to)
        {
            options = Resolved(options);
            return Instance.Begin(instance.Run(style, mask, options, from, to));
        }

        static Coroutine Change(Func<AsyncOperation> load, string style, Texture mask, Options options)
        {
            options = Resolved(options);
            return Instance.Begin(instance.Changing(load, style, mask, options));
        }

        Coroutine Begin(IEnumerator routine)
        {
            if (running != null)
                StopCoroutine(running);
            if (loading != null)  // a replaced ChangeScene still changes scene
                loading.allowSceneActivation = true;
            loading = null;
            running = StartCoroutine(Tracked(routine));
            return running;
        }

        IEnumerator Tracked(IEnumerator routine)
        {
            yield return routine;
            running = null;
        }

        IEnumerator Changing(Func<AsyncOperation> load, string style, Texture mask, Options o)
        {
            var op = load();
            if (op == null)
                yield break;
            op.allowSceneActivation = false;
            loading = op;
            yield return Run(style, mask, o, 0f, 1f);
            loading = null;
            op.allowSceneActivation = true;
            while (!op.isDone)
                yield return null;
            yield return null;
            yield return Run(o?.RevealStyle ?? style, o?.RevealStyle != null ? null : mask, o, 1f, 0f);
        }

        IEnumerator Run(string style, Texture mask, Options o, float from, float to)
        {
            if (material)
                Destroy(material);
            var m = material = mask ? MakeMaterial(mask, o) : MakeMaterial(style, o);
            cover.material = m;
            m.SetFloat(Progress, from);
            capturing = m.shader == ScreenShader;
            cover.texture = capturing ? Capture() : null;
            cover.enabled = true;
            float duration = o?.Duration ?? 0.45f;
            var ease = o?.Ease ?? SineInOut;
            for (float t = 0f; t < duration; t += Time.unscaledDeltaTime)
            {
                m.SetFloat(Progress, Mathf.LerpUnclamped(from, to, ease(t / duration)));
                yield return null;
            }
            m.SetFloat(Progress, to);
            capturing = false;  // covered, the colour hides the screen; uncovered, the cover goes
            if (to == 0f)
                cover.enabled = false;
        }

        static float SineInOut(float t) => 0.5f - 0.5f * Mathf.Cos(Mathf.PI * t);

        /// <summary>The render texture the main camera is drawn into while a screen style plays.</summary>
        RenderTexture Capture()
        {
            if (!capture || capture.width != Screen.width || capture.height != Screen.height)
            {
                if (capture)
                    capture.Release();
                capture = new RenderTexture(Screen.width, Screen.height, 24) { name = "SceneTransition screen" };
            }
            Render();
            return capture;
        }

        void LateUpdate()
        {
            if (capturing)
                Render();
        }

        void Render()
        {
            var cam = Camera.main;
            if (!cam || cam.targetTexture)
                return;
            cam.targetTexture = capture;
            cam.Render();
            cam.targetTexture = null;
        }

        void OnDestroy()
        {
            if (capture)
                capture.Release();
        }
    }
}
