
Shader "Custom/TranspAdditiveScroll"
{
	Properties
	{
		[HDR] _Color("Color", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_BumpMap ("Normal map", 2D) = "bump" {}
		_ScrollTex("Scroll Texture", 2D) = "black" {}
		[HDR] _ScrollTexColor ("Scroll Texture Color", Color) = (1,1,1,1)
		_ScrollSpeed("Scroll Speed", Vector) = (1,0,0,0)
		[Toggle] _AdditiveScroll("Additive Scroll", Int) = 1
		[Toggle] _UseUnscaledTime("Use Unscaled Time", Int) = 0
		[Header(Rimlight)][NoScaleOffset] _RimTex ("Rimlight ramp", 2D) = "white" {}
		[HDR] _RimColor ("Rimlight Color", Color) = (0,0,0,0)
		[Header(Intersection)][HDR] _IntersColor("Intersection Highlight Color", Color) = (0,0,0,0)
		_IntersThresholdMax("Intersection Highlight Threshold", Float) = 1
		_IntersHardness("Intersection Hardness", Float) = 2
	}
	SubShader
	{
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" "IgnoreProjector" = "True" }

		Pass
		{

			ZWrite Off
			Blend SrcAlpha One

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float2 uvScroll : TEXCOORD1;
				float3 tangentSpaceLightDir : TEXCOORD2;
				float4 projPos : TEXCOORD3;
			};

			uniform sampler2D _CameraDepthTexture;
			sampler2D _RimTex;
			sampler2D _ScrollTex;
			sampler2D _MainTex;
			sampler2D _BumpMap;

			fixed4 _Color;
			fixed4 _RimColor;
			fixed4 _IntersColor;
			fixed4 _ScrollTexColor;
			half4 _ScrollSpeed;
			fixed _AdditiveScroll;
			fixed _UseUnscaledTime;
			float4 _ScrollTex_ST;
			float4 _MainTex_ST;
			float _unscaledTime;

			half _IntersHardness;
			half _IntersThresholdMax;
			
			v2f vert (appdata v)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				float time = _UseUnscaledTime > 0 ? _unscaledTime * 0.05 : _Time.x;
				o.uvScroll = TRANSFORM_TEX(v.uv, _ScrollTex) + frac(time * _ScrollSpeed);

				float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
				float3 binormal = cross(normalize(v.normal), normalize(v.tangent.xyz));
				float3x3 rotation = float3x3(v.tangent.xyz, binormal, v.normal);
				o.tangentSpaceLightDir = mul(rotation, viewDir);

				o.projPos = ComputeScreenPos(o.vertex);
				COMPUTE_EYEDEPTH(o.projPos.z);

				return o;
			}
			
			half4 frag (v2f i) : SV_Target
			{
				half3 tangentSpaceNormal = (tex2D(_BumpMap, i.uv).rgb * 2.0) - 1.0;
                half rampSample = dot(tangentSpaceNormal, i.tangentSpaceLightDir);
				fixed4 rim = tex2D(_RimTex, rampSample.xx) * _RimColor;

				float sceneZ = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
				half4 diff = (1.0 - min(pow(abs(sceneZ - i.projPos.z) / _IntersThresholdMax, _IntersHardness), 1.0)) * _IntersColor;

				fixed4 col = tex2D(_MainTex, i.uv) * _Color;
				fixed4 scroll = tex2D(_ScrollTex, i.uvScroll) * _ScrollTexColor;
				if (_AdditiveScroll) col += scroll;
				else col *= scroll;
				return rim + col + diff;
			}

			ENDCG
		}
	}
}
