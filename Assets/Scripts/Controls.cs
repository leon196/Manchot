using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Controls : MonoBehaviour 
{
	public SpriteCollider cursor;

	private GameObject infoTxt; 
	private GameObject infoImg;

	private SpriteCollider[] objects;
	private GameObject arm;
	private GameObject hand;
	private GameObject[] fingers;
	private SpriteCollider[] nerves;

	private Text textComponent;
	private UI uiComponent;
	private GameObject finalVictory;

	private GameObject background;
	private Sprite backgroundSprite;
	private float parallaxSpeed = 1000f;

	private bool modeSelection = true;
	private bool modeTuto = false;

	private float modeTutoTimeStart = 0f;
	private float modeTutoTimeDelay = 3f;

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
	private float armAnimStartDelay = 1f;
	private float armAnimEndDelay = 3f;
	private float armWidth = 800f;

	private bool modeMenu = true;
	private bool won = false;

	private GameObject backgroundMenu;
	private SpriteCollider buttonPlay;

	private AudioSource musicMenu;
	private AudioSource musicGame;
	private AudioSource musicVictory;

	// Level Design
	private int selectedLevel = 0;
	private int[] levelFingers = new int[] { 1, 1, 1, 1, 1 };
	private int[,] levels = new int[,] { 
		{ 0, 0, 0, 0, 0 }, 
		{ 1, 0, 1, 1, 0 }, 
		{ 0, 1, 0, 0, 1 }, 
		{ 0, 1, 1, 0, 0 }, 
		{ 0, 0, 0, 0, 1 }, 
		{ 1, 0, 0, 0, 1 }, 
		{ 0, 1, 0, 0, 0 }
	};
	private GameObject[] levelScenes;
	private string[] levelTexts = new string[] { "Banana", "Piano", "Metal", "Shifoumi", "chocolat", "telephone", "lampe" };
	private bool[] levelsDone;

	void ShowMenu ()
	{
		modeMenu = true;
		modeTuto = false;
		backgroundMenu.SetActive(true);
		musicMenu.Play();
		musicGame.Stop();
		musicVictory.Stop();
		infoImg.SetActive(false);
		infoTxt.SetActive(false);
	}

	void ShowTuto ()
	{
		modeMenu = false;
		modeTuto = true;
		infoImg.SetActive(true);
		infoTxt.SetActive(true);
		modeTutoTimeStart = Time.time;
	}

	void ShowGame ()
	{
		infoImg.SetActive(false);
		infoTxt.SetActive(false);
		modeMenu = false;
		modeTuto = false;
		backgroundMenu.SetActive(false);
		musicMenu.Stop();
		musicGame.Play();
	}

	void ShowSceneSelection ()
	{
		infoImg.SetActive(false);
		infoTxt.SetActive(false);
		arm.SetActive(false);
		backgroundMenu.SetActive(false);
		background.SetActive(true);
		modeSelection = true;
		modeMenu = false;
		modeTuto = false;
		musicMenu.Stop();
		musicGame.Play();

		uiComponent.CleanHints();

		for (int i = 0; i < levelScenes.Length; ++i)
		{
			levelScenes[i].SetActive(false);
		}
	}

	void ShowSceneAction ()
	{
		// Reset positin & orientation
		Vector3 armPosition = arm.transform.position;
		armPosition.x = -armWidth;
		arm.transform.position = armPosition;
		hand.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

		// Show Arm and animate
		arm.SetActive(true);
		armAnimStart = Time.time;

		// Reset Game Logic
		won = false;

		// Hide Piece
		background.SetActive(false);

		// Show Level
		levelScenes[selectedLevel].SetActive(true);

		// Text Level
		textComponent.SetupText(levelTexts[selectedLevel]);
		List<float> hihi = new List<float>();
		for (int i = 0; i < 5; ++i)
		{
			if (levels[selectedLevel, i] == 1)
			{
				hihi.Add((float)i);
			}
		}
		uiComponent.SetupHints(hihi);

		// Set mode
		modeSelection = false;

		// Set fingers
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

	bool CheckFinalVictory ()
	{
		for (int i = 0; i < levelsDone.Length; ++i)
		{
			if (!levelsDone[i])
			{
				return false;
			}
		}
		return true;
	}

	void ShowFinalVictory ()
	{
		finalVictory.SetActive(true);
		finalVictory.GetComponentInChildren<SpriteAnimation>().Restart();
		textComponent.SetupText("wooooooow");
		musicGame.Stop();
		musicVictory.Play();
		StartCoroutine(Restart());
	}

	IEnumerator Restart ()
	{
		yield return new WaitForSeconds(21f);
		
		won = false;

		int count = 7;
		levelsDone = new bool[count];
		for (int i = 0; i < levelsDone.Length; ++i)
		{
			levelScenes[i].SetActive(false);
			levelsDone[i] = false;
		}
		finalVictory.SetActive(false);
		musicMenu.Play();
		musicVictory.Stop();
		mousePosition = new Vector3();
		mouseRatio = new Vector2();
		mouseClic = false;

		// Timing
		cursorAnimStart = Time.time;
		cursorAnimDelay = 0.5f;

		ShowMenu();
	}

	void Start () 
	{
		//Screen.showCursor = false;
		musicMenu = GameObject.Find("MusiqueMenu").GetComponent<AudioSource>();
		musicGame = GameObject.Find("MusiqueGame").GetComponent<AudioSource>();
		musicVictory = GameObject.Find("MusiqueVictory").GetComponent<AudioSource>();

		musicMenu.Play();

		// 1 : ouvrir en continu
		// 2 : fermer (en continu)
		// 5 : impulsion 200ms

		// 
		textComponent = GetComponent<Text>();
		uiComponent = GetComponent<UI>();
		finalVictory = GameObject.Find("FinalVictory");
		finalVictory.SetActive(false);

		//
		infoTxt = GameObject.Find("InfoTxt");
		infoImg = GameObject.Find("InfoImg");

		backgroundMenu = GameObject.Find("backgroundMenu");
		buttonPlay = GameObject.Find("jouer").GetComponent<SpriteCollider>();

		//
		int count = 7;
		levelScenes = new GameObject[count];
		objects = new SpriteCollider[count];
		levelsDone = new bool[count];
		for (int i = 0; i < count; ++i)
		{
			levelScenes[i] = GameObject.Find("Level"+(i+1));
			levelScenes[i].SetActive(false);
			objects[i] = GameObject.Find("Object"+(i+1)).GetComponent<SpriteCollider>();
			levelsDone[i] = false;
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
		ShowMenu();
	}
	
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}

		// Mouse
		mousePosition = Input.mousePosition;
		mouseRatio.x = (mousePosition.x / windowWidth - 0.5f);
		mouseRatio.y = (mousePosition.y / windowHeight - 0.5f);
		mousePosition.x = mouseRatio.x * orthoSize * 2f * screenRatio;
		mousePosition.y = mouseRatio.y * orthoSize * 2f;
		mouseClic = Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1);

		// Shader
		Shader.SetGlobalVector("_Mouse", new Vector4(mouseRatio.x, mouseRatio.y, 0f, 0f));
		Shader.SetGlobalFloat("_TimeElapsed", Time.time);
		Shader.SetGlobalFloat("_AnimElapsed", Time.time - cursorAnimStart);

		// Move Cursor
		Vector3 cursorPosition = cursor.transform.position;
		cursorPosition.x = mousePosition.x;
		cursorPosition.y = mousePosition.y;
		cursor.transform.position = cursorPosition;

		if (modeMenu)
		{
			bool collision = cursor.collidesWith(buttonPlay);
			if (mouseClic && collision)
			{
				ShowTuto();
			}
		}
		else if (modeTuto)
		{
			if (mouseClic && modeTutoTimeStart + modeTutoTimeDelay < Time.time)
			{
				ShowSceneSelection();
			}
		}
		else if (modeSelection)
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
				if (mouseClic && collision && !levelsDone[i])
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
				float ratioHand = (Time.time - armAnimStart) / armAnimEndDelay;
				hand.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Sin(ratioHand * 30f) * 5f);
				if (armAnimStart + armAnimEndDelay < Time.time)
				{
					ShowSceneSelection();
					if (CheckFinalVictory())
					{
						ShowFinalVictory();
					}
				}
			}
			else
			{
				float ratioArm = (Time.time - armAnimStart) / armAnimStartDelay;
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

					// Cursor Anim
					cursorAnimStart = Time.time;
				}
			}

			// Check Victory
			if (!won && mouseClic && CheckVictory())
			{
				textComponent.SetupText("Bravo");
				won = true;
				levelsDone[selectedLevel] = true;
				armAnimStart = Time.time;

				AudioSource levelSoundReward = levelScenes[selectedLevel].GetComponentInChildren<AudioSource>();
				if (levelSoundReward) {
					levelSoundReward.Play();
				}

				//
				/*if (GetComponent<network>().detected)
				{
					SendMessage("SendSerialMessage", "5");
				}*/

			}
		}
	}
}
