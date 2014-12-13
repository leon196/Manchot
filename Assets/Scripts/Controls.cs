using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Controls : MonoBehaviour 
{
	public SpriteCollider cursor;

	private SpriteCollider[] objects;
	private GameObject arm;
	private GameObject hand;
	private GameObject[] fingers;
	private SpriteCollider[] nerves;

	private Text textComponent;

	private GameObject background;
	private Sprite backgroundSprite;
	private float parallaxSpeed = 2000f;

	private bool modeSelection = true;

	private Vector3 mousePosition;
	private Vector2 mouseRatio;
	private bool mouseClic;
	private float windowWidth;
	private float windowHeight;
	private float orthoSize;
	private float screenRatio;

	private float cursorAnimStart = 0f;
	private float cursorAnimDelay = 0.2f;

	private float armAnimStart = 0f;
	private float armAnimEnd = 0f;
	private float armAnimDelay = 3f;
	private float armWidth = 800f;

	private bool won = false;

	// Level Design
	private int selectedLevel = 0;
	private int[] levelFingers = new int[] { 1, 1, 1, 1, 1 };
	private int[,] levels = new int[,] { { 0, 0, 1, 0, 0 }, { 0, 0, 1, 0, 0 }, { 0, 0, 1, 0, 0 }, { 0, 0, 1, 0, 0 } };
	private GameObject[] levelScenes;
	private string[] levelTexts = new string[] { "", "", "", "Shifoumi" };

	void ShowSceneSelection ()
	{
		arm.SetActive(false);
		background.SetActive(true);
		modeSelection = true;

		for (int i = 0; i < levelScenes.Length; ++i)
		{
			levelScenes[i].SetActive(false);
		}
	}

	void ShowSceneAction ()
	{
		Vector3 armPosition = arm.transform.position;
		armPosition.x = -armWidth;
		arm.transform.position = armPosition;
		arm.SetActive(true);
		armAnimStart = Time.time;

		won = false;

		background.SetActive(false);
		levelScenes[selectedLevel].SetActive(true);

		textComponent.SetupText(levelTexts[selectedLevel]);

		modeSelection = false;
		for (int i = 0; i < 5; ++i)
		{
			levelFingers[i] = 1;
			fingers[i].GetComponent<SpriteAnimation>().Open();
		}

	}

	bool CheckVictory ()
	{
		for (int i = 0; i < 5; ++i)
		{
			int finger = levelFingers[i];
			int levelFinger = levels[selectedLevel, i];
			if (finger != levelFinger)
			{
				return false;
			}
		}
		return true;
	}

	void Start () 
	{
		// 
		textComponent = GetComponent<Text>();
		textComponent.SetupText("Coucou");

		//
		levelScenes = new GameObject[4];
		objects = new SpriteCollider[4];
		for (int i = 0; i < 4; ++i)
		{
			levelScenes[i] = GameObject.Find("Level"+(i+1));
			levelScenes[i].SetActive(false);
			objects[i] = GameObject.Find("Object"+(i+1)).GetComponent<SpriteCollider>();
		}

		// 
		arm = GameObject.Find("Arm");
		hand = GameObject.Find("Hand");
		nerves = new SpriteCollider[5];
		fingers = new GameObject[5];
		for (int i = 0; i < 5; ++i)
		{
			nerves[i] = GameObject.Find("Nerve"+(i+1)).GetComponent<SpriteCollider>();
			fingers[i] = GameObject.Find("Finger"+(i+1));
		}

		// Background
		background = GameObject.Find("Background");
		backgroundSprite = background.GetComponent<SpriteRenderer>().sprite;

		// Mouse
		mousePosition = new Vector3();
		mouseRatio = new Vector2();
		mouseClic = false;

		// Dimensions
		windowWidth = Screen.width;
		windowHeight = Screen.height;
		orthoSize = Camera.main.orthographicSize;
		screenRatio = Screen.width / (float)Screen.height;

		Shader.SetGlobalFloat("_AnimDelay", cursorAnimDelay);
		Shader.SetGlobalFloat("_AnimElapsed", 0f);

		// Timing
		cursorAnimStart = Time.time;
		cursorAnimDelay = 0.5f;

		// Start Game
		ShowSceneSelection();
	}
	
	void Update () 
	{
		// Mouse
		mousePosition = Input.mousePosition;
		mouseRatio.x = (mousePosition.x / windowWidth - 0.5f);
		mouseRatio.y = (mousePosition.y / windowHeight - 0.5f);
		mousePosition.x = mouseRatio.x * orthoSize * 2f * screenRatio;
		mousePosition.y = mouseRatio.y * orthoSize * 2f;
		mouseClic = Input.GetMouseButtonDown(0);

		// Shader
		Shader.SetGlobalVector("_Mouse", new Vector4(mouseRatio.x, mouseRatio.y, 0f, 0f));
		Shader.SetGlobalFloat("_TimeElapsed", Time.time);
		Shader.SetGlobalFloat("_AnimElapsed", Time.time - cursorAnimStart);

		// Move Cursor
		Vector3 cursorPosition = cursor.transform.position;
		cursorPosition.x = mousePosition.x;
		cursorPosition.y = mousePosition.y;
		cursor.transform.position = cursorPosition;

		if (modeSelection)
		{
			// Background Parallax
			float velocity = Mathf.Clamp(mouseRatio.x, -1f, 1f);
			Vector3 backgroundPosition = background.transform.position;
			backgroundPosition.x -= velocity * Time.deltaTime * parallaxSpeed;
			float min = (-backgroundSprite.bounds.size.x) / 2f + orthoSize * screenRatio;
			float max = backgroundSprite.bounds.size.x / 2f - orthoSize * screenRatio;
			backgroundPosition.x = Mathf.Clamp(backgroundPosition.x, min, max);

			// Apply Parallax
			background.transform.position = backgroundPosition;

			// Nerves Collision Test
			for (int i = 0; i < objects.Length; ++i)
			{
				bool collision = cursor.collidesWith(objects[i]);
				if (mouseClic && collision)
				{
					selectedLevel = i;
					ShowSceneAction();
				}
			}
		}
		else 
		{
			// Arm animation

			if (won)
			{
				float ratioHand = (Time.time - armAnimStart) / armAnimDelay;
				hand.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Sin(ratioHand * 30f) * 20f);
				if (armAnimStart + armAnimDelay < Time.time)
				{
					ShowSceneSelection();
				}
			}
			else
			{
				float ratioArm = (Time.time - armAnimStart) / armAnimDelay;
				ratioArm = Mathf.Clamp(ratioArm, 0f, 1f);
				Vector3 armPosition = arm.transform.position;
				armPosition.x = -armWidth * (1f - ratioArm);
				arm.transform.position = armPosition;
			}

			// Nerves Collision Test
			for (int i = 0; i < nerves.Length; ++i)
			{
				bool collision = cursor.collidesWith(nerves[i]);
				if (mouseClic && collision)
				{
					// Animation
					fingers[i].GetComponent<SpriteAnimation>().Toggle();

					// Game Logic
					levelFingers[i] = 1 - levelFingers[i];
					if (CheckVictory())
					{
						textComponent.SetupText("Bravo");
						won = true;
						armAnimStart = Time.time;
					}

					cursorAnimStart = Time.time;
				}
			}
		}
	}
}
