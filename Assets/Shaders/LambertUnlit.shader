// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Unlit/LambertUnlit"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "black" {}
        _LambertMul ("Lambert Multiplier", Range(0, 1)) = 1
        _RimMul ("Rim Power", Range(0, 10)) = 1
        _RimColor ("Rim Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "LightMode" = "ForwardBase" "RenderType" = "Opaque"}
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
                float3 normal: NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float3 viewDir : TEXCOORD2;
                fixed3 normalWorld : TEXCOORD3;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
            half _LambertMul;
            half _RimMul;
            half4  _RimColor;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
                
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = normalize(UnityWorldSpaceViewDir(mul(unity_ObjectToWorld, v.vertex)));

                half4 posWorld = mul( unity_ObjectToWorld, v.vertex );
                o.normalWorld = normalize(mul(half4(v.normal, 0.0), unity_WorldToObject).xyz);

                //TRANSFER_VERTEX_TO_FRAGMENT(o);


				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
                float3 normal = normalize(i.normal);
                float ndotl = dot(normal, _WorldSpaceLightPos0);

                float rim = 1-saturate(dot(normal, normalize(i.viewDir)));

                //half atten = LIGHT_ATTENUATION(i);

				fixed4 col = tex2D(_MainTex, i.uv);
                col.rgb += pow (rim, _RimMul) * _RimColor;
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col + (lerp(0,ndotl,_LambertMul));
			}
			ENDCG
		}
	}
}
