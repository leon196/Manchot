Shader "Custom/Animation" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
	}
	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" }
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off
		Lighting Off
		LOD 200
		CGPROGRAM
		#pragma surface surf Lambert
		#include "UnityCG.cginc"

		sampler2D _MainTex;
		float4 _Color;

		uniform float _TimeElapsed;
		uniform float _FrameCountWidth;
		uniform float _FrameCountHeight;
		uniform float _FrameIndex;

		struct Input 
        {
            float2 uv_MainTex;
            float4 screenPos;
        };

        void surf (Input IN, inout SurfaceOutput o) 
        {
            float2 uv = IN.uv_MainTex;

            uv.x /= _FrameCountWidth;
            uv.x += (fmod(_FrameIndex, _FrameCountWidth) / _FrameCountWidth);

            uv.y /= _FrameCountHeight;
            uv.y += (1.0 / _FrameCountHeight) * floor(_FrameIndex / _FrameCountWidth);

			half4 c = tex2D (_MainTex, uv);
			o.Emission = c.rgb * _Color.rgb;
			o.Alpha = c.a;
		}

		ENDCG
	} 
	FallBack "Diffuse"
}
