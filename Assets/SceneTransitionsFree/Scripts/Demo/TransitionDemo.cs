using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace HeyHeyThere.SceneTransitionsFree
{
    /// <summary>
    /// The demo: a meadow or a dungeon, and a button per style that covers the screen with it, swaps
    /// the scenery and reveals. Built on Start on a Screen Space - Camera canvas, so the screen styles
    /// (which draw the main camera) see the buttons too.
    /// </summary>
    public class TransitionDemo : MonoBehaviour
    {
        public const float W = 1280f, H = 720f;  // the canvas's units, as the demo's window
        static readonly Color Cover = new Color(0.06f, 0.05f, 0.12f);

        /// <summary>The props, 32 pixels a side, by name.</summary>
        public Sprite[] art;
        public bool dungeon;
        /// <summary>Leaves the buttons out, for the store's shots.</summary>
        public bool sceneryOnly;

        public Canvas canvas;
        RawImage backdrop;
        Text title;
        RectTransform root, props;
        Font font;

        void Start()
        {
            if (!canvas)
                Build();
        }

        public void Build()
        {
            DemoInput.Ensure();
            font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            var cam = Camera.main;
            canvas = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster)).GetComponent<Canvas>();
            canvas.transform.SetParent(transform, false);
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = cam;
            canvas.planeDistance = 1f;
            var scaler = canvas.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(W, H);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
            backdrop = Add<RawImage>("Backdrop", 0f, 0f, W, H, canvas.transform);
            Stretch(backdrop.rectTransform);
            root = Add<RectTransform>("Demo", 0f, 0f, W, H, canvas.transform);  // W x H in the middle, the rest backdrop
            root.anchorMin = root.anchorMax = root.pivot = new Vector2(0.5f, 0.5f);
            root.anchoredPosition = Vector2.zero;
            props = Add<RectTransform>("Props", 0f, 0f, W, H);
            title = Label("Title", "", 40, 28f, 18f, 600f, 56f, new Color(1f, 1f, 1f, 0.9f));
            Scenery();
            if (sceneryOnly)
                return;
            Label("Hint", "Scene Transitions Free  ·  pick a style", 16, 30f, 70f, 600f, 28f, new Color(1f, 1f, 1f, 0.7f));
            var styles = SceneTransition.Styles;
            int cols = styles.Count > 32 ? 12 : 8;
            float bw = (W - 40f - (cols - 1) * 6f) / cols, bh = cols == 8 ? 50f : 44f;
            for (int i = 0; i < styles.Count; i++)
            {
                string style = styles[i];
                var b = Add<Image>(style, 20f + (i % cols) * (bw + 6f), H * 0.66f + (i / cols) * (bh + 6f), bw, bh);
                var button = b.gameObject.AddComponent<Button>();
                button.targetGraphic = b;
                var colors = button.colors;
                colors.normalColor = colors.selectedColor = new Color(0.1f, 0.1f, 0.13f, 0.62f);
                colors.highlightedColor = new Color(0.2f, 0.2f, 0.26f, 0.75f);
                colors.pressedColor = new Color(0.36f, 0.31f, 0.6f, 0.85f);
                button.colors = colors;
                var text = Label("Text", style, 16, 0f, 0f, bw, bh, new Color(0.88f, 0.88f, 0.9f), b.rectTransform);
                text.alignment = TextAnchor.MiddleCenter;
                var at = b.rectTransform;
                button.onClick.AddListener(() => StartCoroutine(Go(style, at)));
            }
        }

        IEnumerator Go(string style, RectTransform from)
        {
            var o = new SceneTransition.Options { CenterOn = from, Color = Cover };
            yield return SceneTransition.Cover(style, o);
            dungeon = !dungeon;
            Scenery();
            yield return null;
            yield return SceneTransition.Reveal(style, o);
        }

        /// <summary>The backdrop, the props and the title of the scenery shown.</summary>
        public void Scenery()
        {
            Kill(backdrop.texture);
            backdrop.texture = Backdrop(dungeon, 1920, 1080);
            foreach (var t in props.Cast<Transform>().ToList())
                Kill(t.gameObject);
            var place = dungeon ? new[] { ("chest", 0.2f), ("crystal", 0.42f), ("ghost", 0.62f), ("potion", 0.82f) }
                : new[] { ("bush", 0.16f), ("mushroom", 0.36f), ("slime", 0.6f), ("coin", 0.84f) };
            foreach (var (name, x) in place)
            {
                var sprite = art.First(s => s.name == name);
                var size = sprite.rect.size * 4f;
                var image = Add<Image>(name, W * x - size.x * 0.5f, H * 0.62f - size.y, size.x, size.y, props);
                image.sprite = sprite;
            }
            title.text = dungeon ? "DUNGEON" : "MEADOW";
        }

        /// <summary>A scenery's sky, hills or bricks, and ground, at w x h.</summary>
        public static Texture2D Backdrop(bool dungeon, int w, int h)
        {
            float s = h / H;
            var px = new Color32[w * h];
            Color top = dungeon ? new Color(0.18f, 0.13f, 0.3f) : new Color(0.45f, 0.72f, 0.95f);
            Color bottom = dungeon ? new Color(0.1f, 0.07f, 0.16f) : new Color(0.85f, 0.93f, 1f);
            float horizon = H * 0.62f;
            for (int y = 0; y < h; y++)
            {
                float gy = y / s;  // in canvas units, down from the top
                for (int x = 0; x < w; x++)
                {
                    float gx = x / s;
                    Color c;
                    if (gy >= horizon)
                        c = gy < horizon + 8f ? (dungeon ? new Color(0.35f, 0.3f, 0.45f) : new Color(0.45f, 0.72f, 0.4f))
                            : dungeon ? new Color(0.2f, 0.16f, 0.26f) : new Color(0.3f, 0.5f, 0.28f);
                    else
                    {
                        c = Color.Lerp(top, bottom, gy / horizon);
                        if (dungeon)
                        {
                            int row = Mathf.FloorToInt(gy / 36f);
                            float bx = gx - (row % 2) * 36f;
                            int col = Mathf.FloorToInt(bx / 72f);
                            if (row < 12 && bx - col * 72f < 70f && gy - row * 36f < 34f)
                                c = Color.Lerp(c, Color.white, 0.035f + 0.02f * (((row * 7 + col * 3) % 3 + 3) % 3));
                        }
                        else
                        {
                            float dx = gx - W * 0.82f, dy = gy - H * 0.18f;
                            if (dx * dx + dy * dy < H * 0.08f * H * 0.08f)
                                c = new Color(1f, 0.92f, 0.6f);
                            for (int i = 0; i < 3; i++)
                                if (gy >= H * (0.5f + i * 0.05f) - Mathf.Sin(gx * 0.006f + i * 2f) * H * 0.06f)
                                    c = new Color(0.35f, 0.62f, 0.35f) * (1f - 0.12f * i);
                        }
                    }
                    c.a = 1f;
                    px[(h - 1 - y) * w + x] = c;
                }
            }
            var tex = new Texture2D(w, h, TextureFormat.RGBA32, false) { name = dungeon ? "Dungeon" : "Meadow", wrapMode = TextureWrapMode.Clamp };
            tex.SetPixels32(px);
            tex.Apply(false, true);
            return tex;
        }

        T Add<T>(string name, float x, float y, float w, float h, Transform parent = null) where T : Component
        {
            var go = new GameObject(name, typeof(RectTransform));
            var r = (RectTransform)go.transform;
            r.SetParent(parent ? parent : root, false);
            r.anchorMin = r.anchorMax = r.pivot = new Vector2(0f, 1f);  // top left, y down
            r.anchoredPosition = new Vector2(x, -y);
            r.sizeDelta = new Vector2(w, h);
            return typeof(T) == typeof(RectTransform) ? (T)(Component)r : go.AddComponent<T>();
        }

        Text Label(string name, string text, int size, float x, float y, float w, float h, Color color, Transform parent = null)
        {
            var t = Add<Text>(name, x, y, w, h, parent);
            t.font = font;
            t.fontSize = size;
            t.color = color;
            t.text = text;
            t.raycastTarget = false;
            return t;
        }

        static void Kill(Object o)
        {
            if (!o)
                return;
            if (Application.isPlaying)
                Destroy(o);
            else
                DestroyImmediate(o);
        }

        static void Stretch(RectTransform r)
        {
            r.anchorMin = Vector2.zero;
            r.anchorMax = Vector2.one;
            r.offsetMin = r.offsetMax = Vector2.zero;
        }
    }
}
