using UnityEngine;
using System.Collections;

public class SpriteAnimation : MonoBehaviour 
{
	void Start ()
	{
	
	}
	
	void Update () 
	{
		Shader.SetGlobalFloat("_TimeElapsed", Time.time);
	}
}
