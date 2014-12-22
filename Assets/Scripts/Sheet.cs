using UnityEngine;
using System.Collections;

public class Sheet : MonoBehaviour 
{
	public int frameCountWidth = 3;
	public int frameCountHeight = 3;

	void Awake () 
	{
		renderer.material.SetFloat("_FrameCountWidth", frameCountWidth);
		renderer.material.SetFloat("_FrameCountHeight", frameCountHeight);
	}

	public void SetFrameIndex (float index)
	{
		renderer.material.SetFloat("_FrameIndex", index);
	}
}
