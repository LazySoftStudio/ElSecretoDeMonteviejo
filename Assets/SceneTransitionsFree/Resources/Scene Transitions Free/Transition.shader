// Covers the rect with _Color as _Progress goes 0 -> 1, in the pattern picked by _Style. Every pixel
// gets a threshold v in 0..1 and is covered once progress passes it. A UI shader: on a RawImage, or
// through Graphics.Blit.
Shader "Scene Transitions Free/Transition"
{
    Properties
    {
        [PerRendererData] _MainTex ("Texture", 2D) = "white" {}
        _Progress ("Progress", Range(0, 1)) = 0
        // 0 fade, 1 dither, 2 wipe, 3 diamonds, 4 circles, 5 iris, 6 clock, 7 blinds, 8 checker, 9 noise,
        // 10 spiral, 11 curtain, 12 zigzag, 13 shards, 14 bars, 15 hexagons, 16 tiles, 17 mask, 18 triangles,
        // 19 waves, 20 rays, 21 box, 22 interlace, 23 rings, 24 bubbles, 25 pinwheel
        [IntRange] _Style ("Style", Range(0, 25)) = 0
        _Color ("Color", Color) = (0, 0, 0, 1)
        _Softness ("Softness", Range(0, 0.5)) = 0.04
        // Direction of travel in degrees: 0 = left to right, 90 = bottom to top.
        _Angle ("Angle", Range(-180, 180)) = 0
        // Pattern cells across the rect's height.
        _Cells ("Cells", Range(1, 64)) = 10
        // Where iris, clock, spiral, rays, box, rings and pinwheel close to, in UV (y up).
        _Center ("Center", Vector) = (0.5, 0.5, 0, 0)
        // Runs the pattern backwards: iris opens, curtain splits, wipe starts at the other side.
        [ToggleUI] _Flip ("Flip", Float) = 0
        // Snaps the pattern to blocks of this many screen pixels, for pixel-art games. 0 = smooth.
        [IntRange] _PixelSize ("Pixel Size", Range(0, 32)) = 0
        _Seed ("Seed", Float) = 0
        // Greyscale rule image for the mask style: black covers first, white last.
        [NoScaleOffset] _Mask ("Mask", 2D) = "white" {}

        [HideInInspector] _StencilComp ("Stencil Comparison", Float) = 8
        [HideInInspector] _Stencil ("Stencil ID", Float) = 0
        [HideInInspector] _StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector] _StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector] _StencilReadMask ("Stencil Read Mask", Float) = 255
        [HideInInspector] _ColorMask ("Color Mask", Float) = 15
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "PreviewType" = "Plane" }
        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
        Cull Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha  // alpha adds up: an opaque screen stays opaque
        ColorMask [_ColorMask]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "TransitionCommon.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _Mask;
            float4 _Color, _Center;
            float _Progress, _Style, _Softness, _Angle, _Cells, _Flip, _PixelSize, _Seed;

            v2f vert(appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }

            float sweep(float2 q, float2 dir, float aspect)
            {
                float extent = 0.5 * (abs(dir.x) * aspect + abs(dir.y));
                return saturate(dot(q, dir) / extent * 0.5 + 0.5);
            }

            // The patterns work in UV with y down from the top.
            float4 frag(v2f i) : SV_Target
            {
                float2 uv = float2(i.uv.x, 1.0 - i.uv.y);
                float2 px = float2(abs(ddx(uv.x)), abs(ddy(uv.y)));
                float aspect = px.y / px.x;
                if (_PixelSize >= 1.0)
                {
                    float2 b = px * _PixelSize;
                    uv = (floor(uv / b) + 0.5) * b;
                }
                float2 q = (uv - 0.5) * float2(aspect, 1.0);
                float a = radians(-_Angle);
                float2 dir = float2(cos(a), sin(a));
                float2 perp = float2(-dir.y, dir.x);
                float2 center = float2(_Center.x, 1.0 - _Center.y);
                float2 d = q - (center - 0.5) * float2(aspect, 1.0);
                int style = (int)round(_Style);
                float v = 0.0;

                if (style == 1) {  // dither
                    float dot_px = max(_PixelSize, 1.0 / (px.y * _Cells * 8.0));
                    v = tr_bayer8(uv / (px * dot_px)) + 1.0 / 128.0;
                } else if (style == 2) {  // wipe
                    v = sweep(q, dir, aspect);
                } else if (style == 3 || style == 4) {  // diamonds, circles
                    float2 g = q * _Cells;
                    float2 id = floor(g) + 0.5;
                    float2 f = g - id;
                    float local = style == 3 ? abs(f.x) + abs(f.y) : length(f) * 1.4142;
                    v = sweep(id / _Cells, dir, aspect) * 0.5 + local * 0.5;
                } else if (style == 5) {  // iris
                    float2 c = (center - 0.5) * float2(aspect, 1.0);
                    float2 far = max(abs(float2(-0.5 * aspect, -0.5) - c), abs(float2(0.5 * aspect, 0.5) - c));
                    v = 1.0 - length(d) / length(far);
                } else if (style == 17) {  // mask
                    v = tex2Dlod(_Mask, float4(uv.x, 1.0 - uv.y, 0.0, 0.0)).r;
                }
                if (_Flip > 0.5)
                    v = 1.0 - v;

                float s = max(_Softness, 0.001);
                float amount = style == 0 ? _Progress : smoothstep(v, v + s, _Progress * (1.0 + s));
                return float4(_Color.rgb, _Color.a * amount);
            }
            ENDCG
        }
    }
}
