using UnityEngine;
using System;
using System.Collections;
public class Player : MonoBehaviour
{
	public int MovementSpeed = 150;
	private GameObject	SpawnPoint1;
	private GameObject Projectile, bomb;
	private GameObject Model;
	private int HealthPointsCurrent = 100;
	private int ArmorPointsCurrent = 0;
	private Texture2D WalkAnimation;
	private Camera PlayerCamera;
	private float FullAutomaticFireRate = 0.05f;
	private bool FullAutomaticFireReady = true;

	void Awake ()
	{
		WalkAnimation = Resources.Load("WalkAnimation") as Texture2D;

		Projectile 	= Resources.Load("Bullet") as GameObject;

        bomb = Resources.Load("Bomb") as GameObject;
        Model = transform.FindChild("Model").gameObject;
        SpawnPoint1 = Model.transform.FindChild("Spawnpoint1").gameObject;
		PlayerCamera = Camera.main;
	}
	void Update ()
	{
		//Checks if the Character is Alive
		if (CheckIfAlive())
		{
			//Movement
			PerformMovement(CheckMovement());

			//Rotation
			PerformRotation();

			//Attack
			if (Input.GetKey(KeyCode.Mouse0))
			{
				if (FullAutomaticFireReady)
				{
					Instantiate(Projectile, SpawnPoint1.transform.position, SpawnPoint1.transform.rotation);
					StartCoroutine(SetCooldownFullAutomaticFire());
				}
			}
			if (Input.GetKeyDown(KeyCode.F))
			{
				Instantiate(bomb, SpawnPoint1.transform.position, SpawnPoint1.transform.rotation);
			}
		}
		FitModelToPixel();
		FitCameraToPixel();
	}
	IEnumerator SetCooldownFullAutomaticFire ()
	{
		FullAutomaticFireReady = false;
		yield return new WaitForSeconds(FullAutomaticFireRate);
		FullAutomaticFireReady = true;
	}
	void FitModelToPixel ()
	{
		Model.transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z));
	}

	void FitCameraToPixel ()
	{
		PlayerCamera.transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), PlayerCamera.transform.position.z);
	}

	bool CheckIfAlive()
	{
		if (HealthPointsCurrent > 0)
		{
			return true;
		}
		else
		{
            Game.Instance.state = Game.GameState.NotRunning;
			return false;
		}
	}

	public void ReceiveDamage (int dmg)
	{
		HealthPointsCurrent -= dmg;
	}

	Vector2 CheckMovement ()
	{
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.RightArrow))
		{
			Vector2 MovementDirection = Vector2.zero;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
			{
				MovementDirection.y += MovementSpeed;
			}
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
			{
				MovementDirection.x -= MovementSpeed;
			}
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
			{
				MovementDirection.y -= MovementSpeed;
			}
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
			{
				MovementDirection.x += MovementSpeed;
			}
			return MovementDirection;
		}
		else
		{
			Walking = false;
			return Vector2.zero;
		}
	}
	void PerformRotation ()
	{
		//Rotation
		if 		(Input.mousePosition.x >= Screen.width / 2 && Input.mousePosition.y > Screen.height / 2)
		{
			Model.transform.eulerAngles = new Vector3(0, 0, -Mathf.Rad2Deg * Mathf.Atan((Input.mousePosition.x * 1f - Screen.width / 2f) / (Input.mousePosition.y  * 1f - Screen.height / 2f)));
		}
		else if (Input.mousePosition.x > Screen.width / 2 && Input.mousePosition.y <= Screen.height / 2)
		{
			Model.transform.eulerAngles = new Vector3(0, 0, -90 - Mathf.Rad2Deg * Mathf.Atan((Screen.height / 2f - Input.mousePosition.y  * 1f) / (Input.mousePosition.x * 1f - Screen.width / 2f)));
		}
		else if (Input.mousePosition.x <= Screen.width / 2 && Input.mousePosition.y < Screen.height / 2)
		{
			Model.transform.eulerAngles = new Vector3(0, 0, -180 - Mathf.Rad2Deg * Mathf.Atan((Screen.width / 2f - Input.mousePosition.x * 1f) / (Screen.height / 2f - Input.mousePosition.y  * 1f)));
		}
		else if (Input.mousePosition.x < Screen.width / 2 && Input.mousePosition.y >= Screen.height / 2)
		{
			Model.transform.eulerAngles = new Vector3(0, 0, -270 - Mathf.Rad2Deg * Mathf.Atan((Input.mousePosition.y  * 1f - Screen.height / 2f) / (Screen.width / 2f - Input.mousePosition.x * 1f)));
		}
	}
	void PerformMovement (Vector2 MovementDirection)
	{
		if (MovementDirection.x != 0)
		{
			if (!Walking)
			{
				Walking = true;
				//StartCoroutine(AnimateWalk());
			}
			if (MovementDirection.y == 0)
			{
				transform.Translate(new Vector3(MovementDirection.x * Time.deltaTime, 0, 0));
			}
			else
			{
				transform.Translate(new Vector3(MovementDirection.x * Time.deltaTime / Mathf.Sqrt(2), MovementDirection.y * Time.deltaTime / Mathf.Sqrt(2), 0));
			}
		}
		else
		{
			if (MovementDirection.y != 0)
			{
				if (!Walking)
				{
					Walking = true;
					//StartCoroutine(AnimateWalk());
				}
				transform.Translate(new Vector3(0, MovementDirection.y * Time.deltaTime, 0));
			}
		}
	}
	void OnGUI ()
	{
		GUI.Label(new Rect(Screen.width - Screen.width * 0.1f, 0, 100, 100), "HP: " + HealthPointsCurrent);
		GUI.Label(new Rect(Screen.width - Screen.width * 0.1f, 20, 100, 100), "Armor: " + ArmorPointsCurrent);
	}
	private bool Walking;
	IEnumerator AnimateWalk ()
	{
		int Counter1 = 0;
		int Counter2 = 0;
		Model.renderer.material.mainTexture = WalkAnimation;
		Model.renderer.material.mainTextureScale = new Vector2(1f / (WalkAnimation.width / 64f), 1f / (WalkAnimation.height / 64f));
		while(Walking)
		{
			Model.renderer.material.mainTextureOffset	= new Vector2(1f / (WalkAnimation.width / 64f) * Counter1, 1f / (WalkAnimation.height / 64f) * Counter2);
			if (Counter1 == (WalkAnimation.width / 64) - 1)
			{
				Counter1 = 0;
				if (Counter2 == (WalkAnimation.height / 64) - 1)
				{
					Counter2 = 0;
				}
				else
				{
					Counter2++;
				}
			}
			else
			{
				Counter1++;
			}
			yield return new WaitForSeconds(0.1f);
		}
	}
}