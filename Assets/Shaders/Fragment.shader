Shader "Custom/Fragment" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_Speed ("Speed", Range(1.0, 100.0)) = 1.0
		_Lines ("Lines", Range(1.0, 1000.0)) = 10.0
		_Radius ("Radius", Range(0.01, 0.2)) = 0.1
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
		float _Speed;
		float _Lines;
		float _Radius;

		uniform float _TimeElapsed;
		uniform float _AnimElapsed;
		uniform float _AnimDelay;
		uniform float4 _Mouse;

		struct Input 
        {
            float2 uv_MainTex;
            float4 screenPos;
        };

        void surf (Input IN, inout SurfaceOutput o) 
        {
            float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
            float screenRatio = _ScreenParams.x / _ScreenParams.y;
            screenUV.x -= 0.5;
            screenUV.y -= 0.5;
            screenUV.x *= screenRatio;
            _Mouse.x *= screenRatio;

			half3 color = _Color.rgb;

			float dist = distance(screenUV.xy, _Mouse.xy);
			float circle;

			float ratio = clamp(_AnimElapsed / _AnimDelay, 0.0, 1.0);
			_Radius = _Radius + sin(ratio * 3.1416) * 0.1;

			circle = step(0.5, cos(dist * _Lines - _TimeElapsed * _Speed));
			circle -= 1.0 - step(dist, _Radius);

			float2 p = _Mouse.xy - screenUV.xy;
			float angle = (atan2(p.y, p.x) + 3.1416) / 6.28319;
			circle -= dist / _Radius;

			o.Emission = color;
			o.Alpha = circle;
		}

		ENDCG
	} 
	FallBack "Diffuse"
}
