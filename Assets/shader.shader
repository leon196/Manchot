Shader "Custom/shader" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
		_Radius ("Radius", Range (0.0, 1.0)) = 0.5
		_Circles ("Circles", Range (1.0, 100.0)) = 10
		_Thickness ("Thickness", Range (0.0, 1.0)) = 0.5
	}
	SubShader 
	{
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
		float _Radius;
		float _Circles;
		float _Thickness;

		uniform float4 _Mouse;
		uniform float _TimeElapsed;

		struct Input 
		{
			float2 uv_MainTex;
            float4 screenPos;
		};

		void surf (Input IN, inout SurfaceOutput o) 
		{
            float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
			float screenRatio = _ScreenParams.x / _ScreenParams.y;
            screenUV.x *= screenRatio;
            screenUV.x += (1.0 - screenRatio) / 2.0;
            _Mouse.x *= screenRatio;
            _Mouse.x += (1.0 - screenRatio) / 2.0;

			half3 color = _Color.rgb;
			float dist = distance(screenUV, _Mouse.xy);
			float angle = atan2(screenUV.y - _Mouse.y, screenUV.x - _Mouse.x) /  6.28318530718;
			//float circle = 1.0 - step(_Radius, dist);
			angle = fmod(angle + _TimeElapsed, 1.0);
			dist = fmod(dist * _Circles, 1.0);
			dist -= angle * _Radius;
			float  circle = step(_Thickness, dist);
			o.Emission = color;
			o.Alpha = circle;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
