using UnityEngine;
using System.Collections;

public class Animation : MonoBehaviour {

	void Start () 
	{
	
	}
	
	void Update () 
	{
		Shader.SetGlobalFloat("_TimeElapsed", Time.time);
	}
}
