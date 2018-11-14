Shader "Light/AditiveBlur" 
{
    Properties
     {
        _TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
        _MainTex ("Particle Texture", 2D) = "white" {}
        _InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
        _Magnitude("Blur Amount", Range(0,10)) = 0.005

    }

    Category
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
        Blend SrcAlpha One
        ColorMask RGB
        Cull Off Lighting Off ZWrite Off

        SubShader 
        {
            Pass 
            {
    
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma target 2.0
                #pragma multi_compile_particles
                #pragma multi_compile_fog
                #include "UnityCG.cginc"
    
                sampler2D _MainTex;
                fixed4 _TintColor;
                float _Magnitude;
                struct appdata_t {
                    float4 vertex : POSITION;
                    float4 color : COLOR;
                    float2 uv : TEXCOORD0;
                    UNITY_VERTEX_INPUT_INSTANCE_ID
                };
    
                struct v2f {
                    float4 vertex : SV_POSITION;
                   float4 color : COLOR;
                    float2 uv : TEXCOORD0;
                    UNITY_FOG_COORDS(1)
                    #ifdef SOFTPARTICLES_ON
                    float4 projPos : TEXCOORD2;
                    #endif
                    UNITY_VERTEX_OUTPUT_STEREO
                };
    
                float4 _MainTex_ST;
    
                float4 avgg(float2 uv)
        	        {
        	             // accumulate the color for this pixel by sampling neighboring pixels.
        	             float4 col = tex2D(_MainTex, uv); // center pixel color.
                 
        	             // top row.
        	             col += tex2D(_MainTex, float2(uv.x + (_Magnitude * 2), uv.y - (_Magnitude * 2)));
        	             col += tex2D(_MainTex, float2(uv.x + (_Magnitude * 1), uv.y - (_Magnitude * 2)));
           	             col += tex2D(_MainTex, float2(uv.x  +(_Magnitude)            , uv.y - (_Magnitude * 2)));
        	             col += tex2D(_MainTex, float2(uv.x + (_Magnitude * 1), uv.y - (_Magnitude * 2)));
        	             col += tex2D(_MainTex, float2(uv.x + (_Magnitude * 2), uv.y - (_Magnitude * 2)));
                 
        	             // 2nd row.
        	             col += tex2D(_MainTex, float2(uv.x + (_Magnitude * 2), uv.y - (_Magnitude * 1)));
        	             col += tex2D(_MainTex, float2(uv.x + (_Magnitude * 1), uv.y - (_Magnitude * 1)));
        	             col += tex2D(_MainTex, float2(uv.x  +(_Magnitude) , uv.y - (_Magnitude * 1)));
        	             col += tex2D(_MainTex, float2(uv.x + (_Magnitude * 1), uv.y - (_Magnitude * 1)));
        	             col += tex2D(_MainTex, float2(uv.x + (_Magnitude * 2), uv.y - (_Magnitude * 1)));
                 
        	             // middle row (note that we occluded middle pixel because it's handled above.
        	             col += tex2D(_MainTex, float2(uv.x + (_Magnitude * 2), uv.y));
        	             col += tex2D(_MainTex, float2(uv.x + (_Magnitude * 1), uv.y));
        	             col += tex2D(_MainTex, float2(uv.x + (_Magnitude * 1), uv.y));
        	             col += tex2D(_MainTex, float2(uv.x + (_Magnitude * 2), uv.y));
                 
        	             
        	             // 4th row.
        	             col += tex2D(_MainTex, float2(uv.x + (_Magnitude * 2), uv.y + (_Magnitude * 1)));
        	             col += tex2D(_MainTex, float2(uv.x + (_Magnitude * 1), uv.y + (_Magnitude * 1)));
        	             col += tex2D(_MainTex, float2(uv.x +(_Magnitude)          , uv.y + (_Magnitude * 1)));
        	             col += tex2D(_MainTex, float2(uv.x + (_Magnitude * 1), uv.y + (_Magnitude * 1)));
        	             col += tex2D(_MainTex, float2(uv.x + (_Magnitude * 2), uv.y + (_Magnitude * 1)));
                 
        	             // bottom row.
        	             col += tex2D(_MainTex, float2(uv.x + (_Magnitude * 2), uv.y + (_Magnitude * 2)));
        	             col += tex2D(_MainTex, float2(uv.x + (_Magnitude * 1), uv.y + (_Magnitude * 2)));
        	             col += tex2D(_MainTex, float2(uv.x +(_Magnitude)        , uv.y + (_Magnitude * 2)));
        	             col += tex2D(_MainTex, float2(uv.x + (_Magnitude * 1), uv.y + (_Magnitude * 2)));
        	             col += tex2D(_MainTex, float2(uv.x + (_Magnitude * 2), uv.y + (_Magnitude * 2)));
                 
        	             col /= 25; // normalize values
        	             return col;
        	   
                      }
                v2f vert (appdata_t v)
                {
                    v2f o;
                    UNITY_SETUP_INSTANCE_ID(v);
                    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    #ifdef SOFTPARTICLES_ON
                    o.projPos = ComputeScreenPos (o.vertex);
                    COMPUTE_EYEDEPTH(o.projPos.z);
                    #endif
                    o.color = avgg(v.uv);
                    
                    o.uv = TRANSFORM_TEX(v.uv,_MainTex);
                    o.uv = v.uv;
                    UNITY_TRANSFER_FOG(o,o.vertex);
                    return o;
                }
               
                UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
                float _InvFade;   
                float4 frag (v2f i) : SV_Target
                {
                    #ifdef SOFTPARTICLES_ON
                    float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
                    float partZ = i.projPos.z;
                    float fade = saturate (_InvFade * (sceneZ-partZ));
                    i.color.a *= fade;
                    #endif
                    float4 col;
                    //col=avgg(i.uv);
                    col = 2.0f * i.color * _TintColor * tex2D(_MainTex, i.uv);
                    UNITY_APPLY_FOG_COLOR(i.fogCoord, col, fixed4(0,0,0,0)); // fog towards black due to our blend mode

                    return col;
                }
        
	    	    
                ENDCG
            }
        }
    }
}