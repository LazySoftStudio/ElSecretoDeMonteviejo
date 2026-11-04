// Proyección triplanar en espacio de mundo para las piezas del ayuntamiento que no tienen UVs buenas.
// El tamaño va en unidades de Unity: lo que ocupa una repetición de la textura.
Shader "ARTE/Triplanar"
{
    Properties
    {
        _BaseMap ("Textura", 2D) = "white" {}
        _BaseColor ("Color", Color) = (1, 1, 1, 1)
        [Normal] _BumpMap ("Normal", 2D) = "bump" {}
        _BumpScale ("Intensidad normal", Float) = 1
        _Tamano ("Tamaño de la textura", Float) = 5
        _Nitidez ("Nitidez de la mezcla", Range(1, 16)) = 6
        _Smoothness ("Suavidad", Range(0, 1)) = 0.3
        _Metallic ("Metálico", Range(0, 1)) = 0

        [Toggle(_TECHO)] _UsarTecho ("Otra textura en la cara de abajo", Float) = 0
        _TechoMap ("Textura abajo", 2D) = "white" {}
        [Normal] _TechoBumpMap ("Normal abajo", 2D) = "bump" {}
        _TechoTamano ("Tamaño abajo", Float) = 5
        _TechoSmoothness ("Suavidad abajo", Range(0, 1)) = 0.1
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "Queue" = "Geometry" }
        LOD 300

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        CBUFFER_START(UnityPerMaterial)
            half4 _BaseColor;
            half _BumpScale;
            float _Tamano;
            half _Nitidez;
            half _Smoothness;
            half _Metallic;
            float _TechoTamano;
            half _TechoSmoothness;
        CBUFFER_END

        TEXTURE2D(_BaseMap);      SAMPLER(sampler_BaseMap);
        TEXTURE2D(_BumpMap);      SAMPLER(sampler_BumpMap);
        TEXTURE2D(_TechoMap);     SAMPLER(sampler_TechoMap);
        TEXTURE2D(_TechoBumpMap); SAMPLER(sampler_TechoBumpMap);

        struct Attributes
        {
            float4 positionOS : POSITION;
            float3 normalOS : NORMAL;
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };
        ENDHLSL

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag

            #pragma shader_feature_local _TECHO

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile_fragment _ _SHADOWS_SOFT _SHADOWS_SOFT_LOW _SHADOWS_SOFT_MEDIUM _SHADOWS_SOFT_HIGH
            #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
            #pragma multi_compile _ _FORWARD_PLUS
            #pragma multi_compile_fog
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                half fogFactor : TEXCOORD2;
            };

            Varyings vert(Attributes input)
            {
                Varyings o = (Varyings)0;
                UNITY_SETUP_INSTANCE_ID(input);
                VertexPositionInputs p = GetVertexPositionInputs(input.positionOS.xyz);
                o.positionCS = p.positionCS;
                o.positionWS = p.positionWS;
                o.normalWS = TransformObjectToWorldNormal(input.normalOS);
                o.fogFactor = ComputeFogFactor(p.positionCS.z);
                return o;
            }

            // Mezcla "whiteout" de las tres normales (Ben Golus), con la U invertida en las caras negativas
            // para que la textura no salga en espejo.
            void Proyectar(TEXTURE2D_PARAM(colorTex, colorSmp), TEXTURE2D_PARAM(normalTex, normalSmp),
                           float3 pos, float tamano, half3 n, half3 w, out half3 color, out half3 tX, out half3 tY, out half3 tZ)
            {
                half3 s = half3(n.x < 0 ? -1 : 1, n.y < 0 ? -1 : 1, n.z < 0 ? -1 : 1);
                float2 uvX = pos.zy / tamano;
                float2 uvY = pos.xz / tamano;
                float2 uvZ = pos.xy / tamano;
                uvX.x *= s.x;
                uvY.x *= s.y;
                uvZ.x *= -s.z;

                color = SAMPLE_TEXTURE2D(colorTex, colorSmp, uvX).rgb * w.x
                      + SAMPLE_TEXTURE2D(colorTex, colorSmp, uvY).rgb * w.y
                      + SAMPLE_TEXTURE2D(colorTex, colorSmp, uvZ).rgb * w.z;

                tX = UnpackNormalScale(SAMPLE_TEXTURE2D(normalTex, normalSmp, uvX), _BumpScale);
                tY = UnpackNormalScale(SAMPLE_TEXTURE2D(normalTex, normalSmp, uvY), _BumpScale);
                tZ = UnpackNormalScale(SAMPLE_TEXTURE2D(normalTex, normalSmp, uvZ), _BumpScale);
                tX.x *= s.x;
                tY.x *= s.y;
                tZ.x *= -s.z;
                tX = half3(tX.xy + n.zy, abs(tX.z) * n.x);
                tY = half3(tY.xy + n.xz, abs(tY.z) * n.y);
                tZ = half3(tZ.xy + n.xy, abs(tZ.z) * n.z);
            }

            half4 frag(Varyings input) : SV_Target
            {
                half3 n = normalize(input.normalWS);
                half3 w = pow(abs(n), _Nitidez);
                w /= (w.x + w.y + w.z);

                half3 color, tX, tY, tZ;
                Proyectar(TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap), TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap),
                          input.positionWS, _Tamano, n, w, color, tX, tY, tZ);
                half suavidad = _Smoothness;

                #if defined(_TECHO)
                    // cara de abajo de los forjados: el techo de la planta inferior
                    half3 colorT, tXt, tYt, tZt;
                    Proyectar(TEXTURE2D_ARGS(_TechoMap, sampler_TechoMap), TEXTURE2D_ARGS(_TechoBumpMap, sampler_TechoBumpMap),
                              input.positionWS, _TechoTamano, n, w, colorT, tXt, tYt, tZt);
                    half abajo = n.y < -0.5 ? 1 : 0;
                    color = lerp(color, colorT, abajo);
                    tX = lerp(tX, tXt, abajo);
                    tY = lerp(tY, tYt, abajo);
                    tZ = lerp(tZ, tZt, abajo);
                    suavidad = lerp(suavidad, _TechoSmoothness, abajo);
                #endif

                half3 normalWS = normalize(tX.zyx * w.x + tY.xzy * w.y + tZ.xyz * w.z);

                InputData inputData = (InputData)0;
                inputData.positionWS = input.positionWS;
                inputData.positionCS = input.positionCS;
                inputData.normalWS = normalWS;
                inputData.viewDirectionWS = GetWorldSpaceNormalizeViewDir(input.positionWS);
                inputData.shadowCoord = TransformWorldToShadowCoord(input.positionWS);
                inputData.fogCoord = input.fogFactor;
                inputData.bakedGI = SampleSH(normalWS);
                inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(input.positionCS);
                inputData.shadowMask = half4(1, 1, 1, 1);

                SurfaceData surface = (SurfaceData)0;
                surface.albedo = color * _BaseColor.rgb;
                surface.metallic = _Metallic;
                surface.smoothness = suavidad;
                surface.normalTS = half3(0, 0, 1);
                surface.occlusion = 1;
                surface.alpha = 1;

                half4 c = UniversalFragmentPBR(inputData, surface);
                c.rgb = MixFog(c.rgb, inputData.fogCoord);
                return c;
            }
            ENDHLSL
        }

        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }
            ZWrite On
            ZTest LEqual
            ColorMask 0

            HLSLPROGRAM
            #pragma target 3.0
            #pragma vertex vertSombra
            #pragma fragment fragVacio
            #pragma multi_compile_instancing
            #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

            float3 _LightDirection;
            float3 _LightPosition;

            float4 vertSombra(Attributes input) : SV_POSITION
            {
                UNITY_SETUP_INSTANCE_ID(input);
                float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
                float3 normalWS = TransformObjectToWorldNormal(input.normalOS);
                #if _CASTING_PUNCTUAL_LIGHT_SHADOW
                    float3 luz = normalize(_LightPosition - positionWS);
                #else
                    float3 luz = _LightDirection;
                #endif
                float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, luz));
                #if UNITY_REVERSED_Z
                    positionCS.z = min(positionCS.z, UNITY_NEAR_CLIP_VALUE);
                #else
                    positionCS.z = max(positionCS.z, UNITY_NEAR_CLIP_VALUE);
                #endif
                return positionCS;
            }

            half4 fragVacio() : SV_Target { return 0; }
            ENDHLSL
        }

        Pass
        {
            Name "DepthOnly"
            Tags { "LightMode" = "DepthOnly" }
            ZWrite On
            ColorMask R

            HLSLPROGRAM
            #pragma target 3.0
            #pragma vertex vertProf
            #pragma fragment fragProf
            #pragma multi_compile_instancing

            float4 vertProf(Attributes input) : SV_POSITION
            {
                UNITY_SETUP_INSTANCE_ID(input);
                return TransformObjectToHClip(input.positionOS.xyz);
            }

            half fragProf() : SV_Target { return 0; }
            ENDHLSL
        }

        Pass
        {
            Name "DepthNormals"
            Tags { "LightMode" = "DepthNormals" }
            ZWrite On

            HLSLPROGRAM
            #pragma target 3.0
            #pragma vertex vertNormales
            #pragma fragment fragNormales
            #pragma multi_compile_instancing

            struct VaryingsN
            {
                float4 positionCS : SV_POSITION;
                float3 normalWS : TEXCOORD0;
            };

            VaryingsN vertNormales(Attributes input)
            {
                VaryingsN o;
                UNITY_SETUP_INSTANCE_ID(input);
                o.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                o.normalWS = TransformObjectToWorldNormal(input.normalOS);
                return o;
            }

            half4 fragNormales(VaryingsN input) : SV_Target
            {
                return half4(NormalizeNormalPerPixel(input.normalWS), 0);
            }
            ENDHLSL
        }
    }

    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}
