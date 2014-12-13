using UnityEngine;
using System.Collections;

public class Font : MonoBehaviour 
{
	public int frameCountWidth = 3;
	public int frameCountHeight = 3;

	private int frameIndex = 0;
	private float frameTime = 0f;
	private float frameDelay = 1f;

	public Vector3 scaleOriginal;

	private string[] font = new string[] {"a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };

	void Start () 
	{
		renderer.material.SetFloat("_FrameCountWidth", frameCountWidth);
		renderer.material.SetFloat("_FrameCountHeight", frameCountHeight);

		scaleOriginal = transform.localScale;
		transform.localScale = Vector3.zero;
	}

	int fontIndex (string letter)
	{
		for (int i = 0; i < font.Length; ++i)
		{
			if (font[i] == letter)
			{
				return i;
			}
		}
		return -1;
	} 

	public void SetLetter (string letter)
	{
		frameIndex = fontIndex(letter); 
		frameIndex = (int)Mathf.Clamp(frameIndex, 0, frameCountWidth * frameCountHeight - 1);
		renderer.material.SetFloat("_FrameIndex", frameIndex);

		frameTime = Time.time;
	}
}
