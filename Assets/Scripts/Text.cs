using UnityEngine;
using System.Collections;

public class Text : MonoBehaviour {

	public Font textPrefab;

	private Font[] letters;

	private float startTime;
	private float textDelay = 3f;

	void Awake () 
	{
		letters = new Font[0];
	}


	float smoothstep (float min, float max, float value) {
		float x = Mathf.Max(0f, Mathf.Min(1f, (value-min)/(max-min)));
		return x*x*(3f - 2f*x);
	}
	
	void Update () 
	{
		float ratio = Mathf.Clamp((Time.time - startTime) / textDelay, 0f, 1f);

		for (int i = 0; i < letters.Length; ++i)
		{
			float delay = smoothstep(i*0.5f / ((float)letters.Length), 1f, ratio);
			Font letter = letters[i];
			Vector3 scale = letter.transform.localScale;
			scale.x = letter.scaleOriginal.x * Mathf.Sin(delay * 3.1418f);
			scale.y = letter.scaleOriginal.y * Mathf.Sin(delay * 3.1418f);
			letter.transform.localScale = scale;
		}
	}

	public void SetupText (string text)
	{
		for (int i = 0; i < letters.Length; ++i)
		{
			Font letter = letters[i];
			Destroy(letter.gameObject);
		}

		//string[] textList = text.Split('');
		//letters = new Font[textList.Length];
		letters = new Font[text.Length];
		float sizeLetter = 128f;
		float width = text.Length * sizeLetter;
		for (int i = 0; i < text.Length; ++i)
		{
			float x = - width / 2f + sizeLetter * i + sizeLetter/2f;
			letters[i] = Instantiate(textPrefab, new Vector3(x, 0f, -300f), Quaternion.identity) as Font;
			string letter = text[i] + "";
			letters[i].SetLetter(letter.ToLower());
		}

		startTime = Time.time;
	}
}
