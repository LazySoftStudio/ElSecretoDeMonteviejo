// Shared helpers for the Scene Transitions Free shaders.

#define TR_TAU 6.2831853

float tr_hash(float2 p)
{
    p = frac(p * float2(123.34, 456.21));
    p += dot(p, p + 45.32);
    return frac(p.x * p.y);
}

float tr_noise(float2 p)
{
    float2 i = floor(p);
    float2 f = frac(p);
    float2 u = f * f * (3.0 - 2.0 * f);
    return lerp(lerp(tr_hash(i), tr_hash(i + float2(1.0, 0.0)), u.x),
                lerp(tr_hash(i + float2(0.0, 1.0)), tr_hash(i + float2(1.0, 1.0)), u.x), u.y);
}

// 0..1, spread to cover the range.
float tr_fbm(float2 p)
{
    float v = 0.0;
    float a = 0.5;
    for (int i = 0; i < 4; i++)
    {
        v += a * tr_noise(p);
        p = p * 2.03 + 17.0;
        a *= 0.5;
    }
    return saturate((v - 0.15) / 0.7);
}

// Ordered-dither thresholds in [0, 1).
float tr_bayer2(float2 a)
{
    a = floor(a);
    return frac(dot(a, float2(0.5, a.y * 0.75)));
}
float tr_bayer4(float2 a) { return tr_bayer2(0.5 * a) * 0.25 + tr_bayer2(a); }
float tr_bayer8(float2 a) { return tr_bayer4(0.5 * a) * 0.25 + tr_bayer2(a); }

// Hex grid of unit width: xy = offset from the cell centre, zw = cell id.
float4 tr_hex(float2 p)
{
    const float2 s = float2(1.0, 1.7320508);
    float4 hc = floor(float4(p, p - float2(0.5, 0.866)) / s.xyxy) + 0.5;
    float4 h = float4(p - hc.xy * s, p - (hc.zw + 0.5) * s);
    return dot(h.xy, h.xy) < dot(h.zw, h.zw) ? float4(h.xy, hc.xy) : float4(h.zw, hc.zw + 0.5);
}

// 0 at a hex centre, 0.5 on its edges.
float tr_hex_dist(float2 p)
{
    p = abs(p);
    return max(dot(p, float2(0.5, 0.8660254)), p.x);
}

// The result takes b's sign, as GLSL's mod (HLSL's fmod takes a's).
float tr_mod(float a, float b) { return a - b * floor(a / b); }
