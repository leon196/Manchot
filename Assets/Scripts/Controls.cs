using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour 
{
	public SpriteCollider cursor;
	public Texture2D[] textures;
	private SpriteCollider[] spriteColliders;

	private GameObject background;
	private Sprite backgroundSprite;
	private float parallaxSpeed = 10000f;

	private Vector3 mousePosition;
	private Vector2 mouseRatio;
	private float windowWidth;
	private float windowHeight;
	private float orthoSize;
	private float screenRatio;

	private int cursorAnimIndex;
	private float cursorAnimStart;
	private float cursorAnimDelay;

	void Start () 
	{
		// Colliders
		spriteColliders = GetComponentsInChildren<SpriteCollider>();

		// Background
		background = GameObject.Find("Background");
		backgroundSprite = background.GetComponent<SpriteRenderer>().sprite;

		// Mouse
		mousePosition = new Vector3();
		mouseRatio = new Vector2();

		// Dimensions
		windowWidth = Screen.width;
		windowHeight = Screen.height;
		orthoSize = Camera.main.orthographicSize;
		screenRatio = Screen.width / (float)Screen.height;

		// Timing
		cursorAnimStart = Time.time;
		cursorAnimDelay = 0.5f;
	}
	
	void Update () 
	{
		// Get Mouse Position
		mousePosition = Input.mousePosition;
		mouseRatio.x = (mousePosition.x / windowWidth - 0.5f);
		mouseRatio.y = (mousePosition.y / windowHeight - 0.5f);
		mousePosition.x = mouseRatio.x * orthoSize * 2f * screenRatio;
		mousePosition.y = mouseRatio.y * orthoSize * 2f;

		// Move Cursor
		cursor.transform.position = mousePosition;

		// Collision Test
		for (int i = 0; i < spriteColliders.Length; ++i)
		{
			if (cursor.collidesWith(spriteColliders[i]))
			{
				Debug.Log("Collision");
			}
		}

		// Animation
		if (cursorAnimStart + cursorAnimDelay < Time.time)
		{
			cursorAnimStart = Time.time;
		}

		// Background Parallax
		float velocity = Mathf.Clamp(mouseRatio.x, -1f, 1f);
		Vector3 backgroundPosition = background.transform.position;
		backgroundPosition.x -= velocity * Time.deltaTime * parallaxSpeed;
		float min = -backgroundSprite.bounds.size.x / 2 + windowWidth * 2f;
		float max = backgroundSprite.bounds.size.x / 2 - windowWidth * 2f;
		backgroundPosition.x = Mathf.Clamp(backgroundPosition.x, min, max);

		background.transform.position = backgroundPosition;
	}
}
