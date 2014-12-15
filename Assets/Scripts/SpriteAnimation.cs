using UnityEngine;
using System.Collections;

public class SpriteAnimation : MonoBehaviour 
{
	public int frameCountWidth = 3;
	public int frameCountHeight = 3;
	private float frameDelay = 0.02f;

	private int frameIndex = 0;
	private float frameTime = 0f;
	private int frameDirection = 1;

	void Start () 
	{
		renderer.material.SetFloat("_FrameCountWidth", frameCountWidth);
		renderer.material.SetFloat("_FrameCountHeight", frameCountHeight);

		frameIndex = 0;
		frameTime = Time.time;
	}

	public void Restart ()
	{
		frameIndex = 0;
		frameTime = Time.time;
		renderer.material.SetFloat("_FrameIndex", frameIndex);
	}
	
	void Update () 
	{
		if (frameTime + frameDelay < Time.time)
		{
			frameTime = Time.time;

			frameIndex = (int)Mathf.Clamp(frameIndex + frameDirection, 0, frameCountWidth * frameCountHeight - 1);
			renderer.material.SetFloat("_FrameIndex", frameIndex);
		}
	}

	public void Toggle ()
	{
		frameDirection *= -1;
	}

	public void Open ()
	{
		frameDirection = 1;
	}

	public void Close ()
	{
		frameDirection = -1;
	}
}
