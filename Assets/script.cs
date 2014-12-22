using UnityEngine;
using System.Collections;

public class script : MonoBehaviour 
{
	private Vector4 mouse;

	void Start () 
	{
		mouse = new Vector4();
	}
	
	void Update () 
	{
		mouse.x = Input.mousePosition.x / Screen.width;
		mouse.y = Input.mousePosition.y / Screen.height;
		
		Shader.SetGlobalFloat("_TimeElapsed", Time.time);
		Shader.SetGlobalVector("_Mouse", mouse);
	}	
}
