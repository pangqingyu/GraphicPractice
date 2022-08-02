Shader "CG/BlinnPhongGouraud"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _SpecularColor("Specular Color", Color) = (0.7, 0.7, 0.7, 1)
        _AmbientColor("Ambient Color", Color) = (0.1, 0.1, 0.1, 1)
        _Shininess("Shininess", Range(0.1, 50)) = 10
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            uniform fixed4 _LightColor0;
            uniform fixed4 _SpecularColor;
            uniform fixed4 _AmbientColor;
            uniform float _Shininess;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                float3 L = normalize(WorldSpaceLightDir(v.vertex));
                float3 V = normalize(WorldSpaceViewDir(v.vertex));
                float3 N = normalize(UnityObjectToWorldNormal(v.normal));
                float3 H = normalize((L + V) / 2);

                o.color = max(dot(L, N), 0) * _LightColor0;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return tex2D(_MainTex, i.uv) * i.color;
            }
            ENDCG
        }
    }
}
