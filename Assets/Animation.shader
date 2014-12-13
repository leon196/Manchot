Shader "Custom/Animation" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
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

		uniform float _TimeElapsed;

		struct Input 
        {
            float2 uv_MainTex;
            float4 screenPos;
        };

        void surf (Input IN, inout SurfaceOutput o) 
        {
            float2 uv = IN.uv_MainTex;
            uv.x /= 7.0;
            uv.x += 1.0/7.0 * fmod(floor(_TimeElapsed * 10.0), 7.0);
            uv.y /= 3.0;
			half4 c = tex2D (_MainTex, uv);
			o.Emission = c.rgb;
			o.Alpha = c.a;
		}

		ENDCG
	} 
	FallBack "Diffuse"
}
