// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

//This is meant to be more AO-like, esp. for jellies!
//Shader "Swing Star/SwingStarFakeAO"//originally based on "Custom/DiffuseTintNDotLScalar_v2" 
Shader "Swing Star/Jelly"//originally based on "Custom/DiffuseTintNDotLScalar_v2" 
{
	Properties
	{
		//Alternatives:
		//Dark Jelly: _Diffuse1 = 2, _Diffuse2=10
		//Bright Jelly: _Diffuse1 =-2.4, _Diffuse2=10
		////////////////////////////////////////////////

		//_AlbedoTex ("Texture", 2D) = "white" {}
		_MainTex ("Texture", 2D) = "white" {}
		//_Tint ("Tint", Color) = (1,1,1)
		_Color ("Tint", Color) = (1,1,1)
		//_AmbientColor("Ambient Color", Color) = (0, 0, 0)
		_EmissionColor("Ambient Color", Color) = (0, 0, 0)
		_DiffuseExponent("Diffuse Exponent", Float) = 1
		_DiffuseExponent2("Diffuse Exponent 2", Float) = 10
		//_FogContribution("Fo
		_FogFraction("Fog Fraction", Range(0.0, 1.0)) = 1.0 //1 = full fog, 0 = no fog

		_RunnersVisionColor("Runners Vision", Color) = (0, 0, 0, 0)
		//_RestrictHDRLights("Restrict HDR Lights", Range(0.0, 1.0)) = 1.0 //1.0 for actual light, 0.0 for normalized intensity
		_RestrictHDRLights("Restrict HDR Lights", Range(0.0, 1.0)) = 0.0 //1.0 for actual light, 0.0 for normalized intensity
		//_RestrictHDRLights("Restrict HDR Lights", Color) = (0,0,0) //1.0 for actual light, 0.0 for normalized intensity
		_ScaleLightingIntensity("ScaleLightingIntensity", Float) = 1.0
		//_Alpha("Alpha", Range(0.0, 1.0)) = 1.0 
	}
	//Fog:
	//http://www.software7.com/blog/unity-custom-fog-shader-on-windows-phone-8/

	SubShader
	{
		Tags {
		
		"RenderType"="Opaque"
		}
		Fog {Mode Off} //legacy fog off
		//Fog {Mode On}
		//Fog {Mode Exp}
		/*
		"Queue" = "Transparent+102" 
		"RenderType"="Transparent"
		
		//////////////////
		//Fog {Mode Off} //legacy fog off
		Cull Off
		ZWrite Off
		//ZTest Always
		////ZTest True
		//Cull Front
		//Blend SrcAlpha OneMinusSrcAlpha // Alpha blending
		//Blend SrcAlpha One
		Blend SrcAlpha OneMinusSrcAlpha // Alpha blending
		//Blend One Zero
		
		//////////////////
		*/
		LOD 200
		
		Pass
		{
			Tags {"LightMode" = "ForwardBase" }
			CGPROGRAM
			// Use shader model 3.0 target, to get nicer looking lighting
			//#pragma target 3.0
		
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			//TODO: figure out why texture tiling isn't working (or is it?)
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			//struct Input {
			//	float2 uv_AlbedoTex;
			//};

			//uniform half4 unity_FogColor; //builtin!
			/*
			uniform half4 unity_FogStart; //builtin!
			uniform half4 unity_FogEnd; //builtin!
			uniform half4 unity_FogDensity; //builtin!
			*/
			//uniform half4 unity_FogDensity; //builtin!//?//NOT

			sampler2D _MainTex;//_AlbedoTex;

			float4 _MainTex_ST;//_AlbedoTex_ST;
			fixed3 _Color;
			fixed4 _EmissionColor;
			float _DiffuseExponent;
			float _DiffuseExponent2;
			half _FogFraction;
			fixed4 _RunnersVisionColor;
			fixed _RestrictHDRLights;
			fixed _ScaleLightingIntensity;
			//fixed _Alpha;

			struct appdata_t {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float3 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				float3 normal: NORMAL; //Q: nointerpolation
				float2 uv : TEXCOORD0;

				//float3 light : LIGHTCOLOR;//DESU; //HACK
				//float fogDistance : DISTANCE;
				//float3 objectPosition : MY_OBJECT_POSITION;
				//float3 objectNormal : MY_OBJECT_NORMAL;
				//float fogFactor : FOG0;
				UNITY_FOG_COORDS(10)//0)//;
			};

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				UNITY_TRANSFER_FOG(o, o.vertex);
				//float4 viewPos = mul(UNITY_MATRIX_MV, v.vertex);
				//o.fogDistance = viewPos.z / viewPos.w;
				//o.fogDistance =o.vertex.z / o.vertex.w;
				//float fogDistance =o.vertex.z / o.vertex.w;
				//float fogDistance =viewPos.z / viewPos.w;
				/*
				float fogRange = unity_FogEnd.x - unity_FogStart.x;
				fogDistance = (fogDistance - unity_FogStart.x) / fogRange;
				*/
				//Q: cast to 3x3??
				
				
				//o.normal = v.normal;// mul(unity_ObjectToWorld, float4(v.normal, 0)).xyz;
				o.normal = float3(v.normal.y, -v.normal.x, v.normal.z);

				//o.uv = MultiplyUV( UNITY_MATRIX_TEXTURE0, v.texcoord ); //apply uv transform stuffs! (for texture0)
				o.uv = TRANSFORM_TEX( v. texcoord, _MainTex);//_AlbedoTex);
				//o.uv = MultiplyUV( UNITY_MATRIX_TEXTURE1, v.texcoord ); //apply uv transform stuffs! (for texture0)

				//o.light = WorldSpaceLightPos0.rgb;
				//float f = cameraVertDist * unity_FogDensity;
				//float f = fogDistance * unity_FogDensity;
				//o.fogFactor = saturate(1 / pow(2.71828,  f));
				return o;
			}
			
			fixed4 frag (v2f i) : COLOR
			{
				fixed3 N = normalize(i.normal); //in world
				
				fixed3 mults[8] = {
					fixed3(1,1,1), fixed3(-1,1,1), 
					fixed3(1,-1,1), fixed3(-1,-1,1),
					fixed3(1,1,-1), fixed3(-1,1,-1),
					fixed3(1,-1,-1), fixed3(-1,-1,-1)
				};

				fixed NDotL = 0.0f;
				/*
				[unroll]
				for(int index=0; index<8; index++)
				{
					//fixed3 L = normalize(_WorldSpaceLightPos0.xyz * mults[index]);
					fixed3 L = normalize(fixed3(1,0,0) * mults[index]);
					NDotL = max(NDotL, saturate(dot(N, L)));
					
				}
				*/
				{
					fixed3 L1, L2;
					L1 = normalize(fixed3(1,0,0) * mults[0]);
					L2 = normalize(fixed3(1,0,0) * mults[1]);
					NDotL = max( saturate(dot(N, L1)), saturate(dot(N, L2)) );
				}
				
				NDotL = 1 - NDotL;

				//NDotL += 1.0f;
				NDotL += 0.5f;//0.25f;

				float3 albedo = tex2D(_MainTex, i.uv).rgb;
				//float3 fogColor = unity_FogColor;//0;

				float3 lightColor = _LightColor0.rgb;//1;
				lightColor = lerp(lightColor, normalize(lightColor), _RestrictHDRLights); //HACK!
				lightColor = lightColor * _ScaleLightingIntensity;
				//else float3(1,1,1);

				//float fogU = 0.0f;
				/*
				float FogRange =2.0f;//1.0f;//5.0f;//10.0f;// 1.0f;//10.0f;//100.0f; //HACK
				float fd = i.fogDistance;
				//fogU = fd / FogRange;
				fogU = fd / FogRange;
				//fogU = (fd*fd)/ FogRange;
				fogU *= fogU;
				fogU = clamp(fogU, 0.0f, 1.0f);
				*/
				//float fd = i.fogFactor;
				//fogU = 1;
				//Q: distance?
				
				float3 ambient = _EmissionColor.rgb * _EmissionColor.a;//scale by ambient alpha, for easier editing/manipulation
				float3 diffuse = _Color.rgb;
				
				diffuse *= (1.0 - ambient); //total diffuse contribution limited by ambient term contribution
				//float diffExp2 = pow(NDotL-0.5, _DiffuseExponent2);
				float diffExp2 = pow(NDotL, _DiffuseExponent2);
				//diffExp2 = (1.0-diffExp2);
				//[nobranch]
				/*if (_DiffuseExponent2 < 0)
				{
					diffExp2 = 0; 
				}
				*/

				float diffExp1 = pow(NDotL, _DiffuseExponent);
				//diffExp1 = saturate(1.0f - diffExp1);
				//diffExp1 = saturate(1.0f - diffExp1);
				//diffuse *= pow(NDotL, _DiffuseExponent) + diffExp2;
				diffuse *= diffExp1 + diffExp2;
				//diffuse *= 1.5f;//2.0f; //HACK
				//Q: multiply output by directional light, or just diffuse term??
				
				fixed3 output = (ambient + diffuse) * albedo * lightColor; // + specular
				{//apply "runner's vision" experimental-mirror's-edge-style reddening:
					fixed3 runnerAdd = _RunnersVisionColor.rgb;// * _RunnersVisionColor.a;
					fixed3 runnerTint = (1.0f - runnerAdd);//(1.0f - _RunnersVisionColor.rgb) * _RunnersVisionColor.a; 
					//output = (runnerTint * output) + runnerAdd;

					//output = (lerp(output*runnerTint,runnerTint,_RunnersVisionColor.a)) + runnerAdd;
					//output = (lerp(output*runnerTint,runnerAdd,_RunnersVisionColor.a));

					//output = (lerp(output*runnerTint,runnerAdd,_RunnersVisionColor.a));
					float u = _RunnersVisionColor.a;
					u = u * 0.5f; //[0,1] => [0, 0.5] // looks better?
					//output = (lerp(output*lerp(float3(0,0,0),runnerTint,u),runnerAdd,u));
					
					//output = (lerp(output*lerp(float3(1,1,1),runnerTint,u),runnerAdd,u));
					//output = (lerp(output*lerp(float3(1,1,1),runnerTint,u*2.0f),runnerAdd,u));
					//output = (lerp(output*lerp(float3(1,1,1),runnerTint,u*2.0f),runnerAdd,u)) + lerp(float3(0,0,0),runnerAdd,u*2.0f);
					//output = (lerp(output*lerp(float3(1,1,1),runnerTint,u*2.0f),runnerAdd,u)) + lerp(float3(0,0,0),runnerAdd,u*1.5f);
					output = (lerp(output*lerp(float3(1,1,1),runnerTint,u),runnerAdd,u)) + lerp(float3(0,0,0),runnerAdd,u*1.5f);
				}

				fixed3 outputPrime = output;
				
				UNITY_APPLY_FOG(i.fogCoord, outputPrime);

				outputPrime = lerp(output,outputPrime,_FogFraction);//apply fog fraction interpolation!

				//return fixed4(output, 1);
				return fixed4(outputPrime, 1);
				//return fixed4(outputPrime, _Alpha);
			}
			ENDCG
		}
	}
	//FallBack "Diffuse"
}