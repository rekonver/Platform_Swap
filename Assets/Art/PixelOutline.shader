Shader "Custom/AllPixelsToColor" {
    Properties {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _ReplaceColor ("Replacement Color", Color) = (1,0,0,1)
    }
    SubShader {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        LOD 100

        Pass {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _ReplaceColor;

            struct appdata_t {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                // Читаємо альфу з текстури
                fixed4 texColor = tex2D(_MainTex, i.uv);

                // Замінюємо RGB на потрібний колір, зберігаючи альфу з текстури
                return fixed4(_ReplaceColor.rgb, texColor.a);
            }
            ENDCG
        }
    }
}
