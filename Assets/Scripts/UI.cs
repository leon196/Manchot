using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI : MonoBehaviour 
{
	public GameObject hintPrefab;

	private List<GameObject> hints;

	void Awake () 
	{
		hints = new List<GameObject>();
	}
	
	public void SetupHints (List<float> levelFingers) 
	{
		CleanHints();

		float hintSize = 128f;
		float width = levelFingers.Count * hintSize;
		for (int i = 0; i < levelFingers.Count; ++i)
		{
			float x = -width/2f + i * hintSize + hintSize/2f;
			GameObject hint = Instantiate(hintPrefab, new Vector3(x, -Camera.main.orthographicSize + hintSize/2f, -400f), Quaternion.identity) as GameObject;
			hint.GetComponent<Sheet>().SetFrameIndex(levelFingers[i]);
			hints.Add(hint);
		}
	}

	public void CleanHints ()
	{
		for (int i = 0; i < hints.Count; ++i)
		{
			Destroy(hints[i]);
		}
		
		hints = new List<GameObject>();
	}
}
