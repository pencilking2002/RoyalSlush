// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld' //REVERTED

Shader "Custom/SwingStar" 
{
	Properties
	{
		_EmissionColor("Ambient Color", Color) = (0, 0, 0)
		
		[Header(Diffuse)]
		
		_Color ("Tint", Color) = (1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_DiffuseExponent("Diffuse Exponent", Float) = 1
		[Toggle(ZEROLUNCH_DIFFUSE_LIGHTING)] _EnableDiffuseLighting("Enable Diffuse Lighting", Float) = 1
		//[Toggle(ZEROLUNCH_DISABLE_DIFFUSE_LIGHTING)] _DisableDiffuseLighting("Disable Diffuse Lighting", Float) = 0

		[Toggle(ZEROLUNCH_ENABLE_FOG)] _EnableFog ("Enable Fog", Float) = 1
		_FogFraction("Fog Fraction", Range(0.0, 1.0)) = 1.0 //1 = full fog, 0 = no fog

		//_RestrictHDRLights("Restrict HDR Lights", Range(0.0, 1.0)) = 1.0 //1.0 for actual light, 0.0 for normalized intensity
		_RestrictHDRLights("Restrict HDR Lights", Range(0.0, 1.0)) = 0.0 //1.0 for actual light, 0.0 for normalized intensity
		//_RestrictHDRLights("Restrict HDR Lights", Color) = (0,0,0) //1.0 for actual light, 0.0 for normalized intensity
		_ScaleLightingIntensity("ScaleLightingIntensity", Float) = 1.0

		[Header(Runners Vision)]
		[Toggle(ZEROLUNCH_RUNNERS_VISION)] _EnableRunnersVision("Enabled Runners Vision", Float) = 0
		_RunnersVisionColor("Runners Vision Color", Color) = (0, 0, 0, 0)
		
		
		[Header(Advanced Properties)]
		[Toggle(ZEROLUNCH_ADVANCED)] _EnableAdvanced("Enabled Advanced Settings", Float) = 0
		_MaxColorIntensity("Max Color Intensity", Color) = (1,1,1,1)
		
		[Header(Rim Lighting)]
		[Toggle(ZEROLUNCH_RIM_LIGHTING)] _EnableRimLighting("Enable Rim Lighting", Float) = 0
			_RimColor("Rim Lighting Color", Color) = (1,1,1,1)
			_RimExponent("Rim Lighting Exponent", Range(0.001, 4.0)) = 1.0
	}
	//Fog:
	//http://www.software7.com/blog/unity-custom-fog-shader-on-windows-phone-8/

	SubShader
	{
		Tags { "RenderType"="Opaque" }
		Fog {Mode Off} //legacy fog off
		//Fog {Mode On}
		//Fog {Mode Exp}
	

		LOD 200
		
		Pass
		{
			Tags {"LightMode" = "ForwardBase" }

			//Hopefully deal with some of the boss z-fighting?
			//Q: can we make this adjustable per-material via some kind of property (then elbows can be adjusted in front of limps not just this shader in front of the cage...)
			Offset -1, -1

			CGPROGRAM
			// Use shader model 3.0 target, to get nicer looking lighting
			//#pragma target 3.0
		
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog

			//#pragma shader_feature ZEROLUNCH_DIFFUSE_LIGHTING
			#pragma multi_compile __ ZEROLUNCH_DIFFUSE_LIGHTING
			#pragma shader_feature ZEROLUNCH_ENABLE_FOG
			#pragma shader_feature ZEROLUNCH_RUNNERS_VISION
			#pragma shader_feature ZEROLUNCH_ADVANCED
			//#pragma shader_feature ZEROLUNCH_RIM_LIGHTING

			//ensure we compile rim+no rim shader variants:
			#pragma multi_compile __ ZEROLUNCH_RIM_LIGHTING
			
			//TODO: figure out why texture tiling isn't working (or is it?)
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			
			
			
			sampler2D _MainTex;//_AlbedoTex;

			float4 _MainTex_ST;//_AlbedoTex_ST;
			fixed3 _Color;
			fixed4 _EmissionColor;
			float _DiffuseExponent;
			half _FogFraction;
			fixed4 _RunnersVisionColor;
			fixed _RestrictHDRLights;
			fixed _ScaleLightingIntensity;
			
			#ifdef ZEROLUNCH_ADVANCED
				fixed3 _MaxColorIntensity;
			#endif
			#ifdef ZEROLUNCH_RIM_LIGHTING
				fixed3 _RimColor;
				float _RimExponent;
			#endif
			struct appdata_t {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float3 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				float3 normal: NORMAL; //Q: nointerpolation
				float2 uv : TEXCOORD0;
				#ifdef ZEROLUNCH_ENABLE_FOG
					//number is which texcoord-semantic to use such as :TexCoord10
					UNITY_FOG_COORDS(10)
				#endif
				#ifdef ZEROLUNCH_RIM_LIGHTING
					float3 positionVs : POSITION_IN_VIEW_SPACE;
				#endif
			};

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				#ifdef ZEROLUNCH_RIM_LIGHTING
					o.positionVs = mul(UNITY_MATRIX_MV, v.vertex);
				#endif
				#ifdef ZEROLUNCH_ENABLE_FOG
					UNITY_TRANSFER_FOG(o, o.vertex);
				#endif
				o.normal = mul(unity_ObjectToWorld, float4(v.normal, 0)).xyz;
				o.uv = TRANSFORM_TEX( v. texcoord, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : COLOR
			{
				fixed3 N = normalize(i.normal); //in world
				fixed3 L = normalize(_WorldSpaceLightPos0.xyz);
				fixed NDotL = saturate(dot(N, L));
				//NDotL += 1.0f;
				NDotL += 0.5f;//0.25f;

				float3 albedo = tex2D(_MainTex, i.uv).rgb;
				//float3 fogColor = unity_FogColor;//0;

				float3 lightColor = _LightColor0.rgb;//1;
				lightColor = lerp(lightColor, normalize(lightColor), _RestrictHDRLights); //HACK!
				lightColor = lightColor * _ScaleLightingIntensity;
				
				float3 ambient = _EmissionColor.rgb * _EmissionColor.a;//scale by ambient alpha, for easier editing/manipulation
				float3 diffuse = _Color.rgb;
				
				diffuse *= (1.0 - ambient); //total diffuse contribution limited by ambient term contribution
				diffuse *= pow(NDotL, _DiffuseExponent);
				//diffuse *= 1.5f;//2.0f; //HACK
				//Q: multiply output by directional light, or just diffuse term??
				//fixed3 output = ambient * albedo * lightColor;
				fixed3 output;
				//albedo = 1;
				#ifdef ZEROLUNCH_DIFFUSE_LIGHTING
					output = (ambient + diffuse) * albedo * lightColor; // + specular
					//return float4(0,1,0,1);
				#else
					//output += (diffuse) * albedo * lightColor; // + specular
					 output = ambient * albedo * lightColor;
					 //return float4(1,0,0,1);
				#endif
				#ifdef ZEROLUNCH_RIM_LIGHTING
					//output = ambient * lightColor * 2.0f;
				
				{
					float3 NVs = normalize(mul(UNITY_MATRIX_V, float4(N, 0)).xyz);
					float3 V = normalize(-i.positionVs); // - float3(0,0,0);
					//return float4(V, 1);
					//return float4(N, 1);
					//return float4(NVs, 1);
					//float NDotV = saturate(dot(N, V));
					float NDotV = saturate(dot(NVs, V));
					//return float4(float3(1,1,1)*NDotV,1);
					//output += _RimColor * (1.0-pow(NDotV, _RimExponent)) * lightColor;
					output += _RimColor * (1.0-pow(NDotV, _RimExponent));
					//output = 1;
					//return float4(1,1,1,1);
				}
				#endif

				#ifdef ZEROLUNCH_RUNNERS_VISION
				{//apply "runner's vision" experimental-mirror's-edge-style reddening:
					fixed3 runnerAdd = _RunnersVisionColor.rgb;// * _RunnersVisionColor.a;
					fixed3 runnerTint = (1.0f - runnerAdd);
					float u = _RunnersVisionColor.a;
					u = u * 0.5f; //[0,1] => [0, 0.5] // looks better?
					output = (lerp(output*lerp(float3(1,1,1),runnerTint,u),runnerAdd,u)) + lerp(float3(0,0,0),runnerAdd,u*1.5f);
				}
				#endif

				#ifdef ZEROLUNCH_ADVANCED
					output = min(_MaxColorIntensity.rgb, output);
				#endif

				//////////////////////////////
				fixed3 outputPrime = output;
				#ifdef ZEROLUNCH_ENABLE_FOG
					UNITY_APPLY_FOG(i.fogCoord, outputPrime);
					outputPrime = lerp(output,outputPrime,_FogFraction);//apply fog fraction interpolation!
				#endif

				//#ifdef ZEROLUNCH_ADVANCED
				//	return float4(1,0,0,1);
				//#endif
				
				return fixed4(outputPrime, 1);
			}
			ENDCG
		}
	}
	//FallBack "Diffuse"
}