﻿using UnityEngine;
using System;
using System.Collections;
public class Player : MonoBehaviour
{
	private int MovementSpeed = 30;
	private GameObject	SpawnPoint1;
	private GameObject Projectile;
	private GameObject Model;
	private int HealthPointsCurrent = 100;
	private int ArmorPointsCurrent = 0;
	private Texture2D WalkAnimation;
	void Awake ()
	{
		WalkAnimation = Resources.Load("WalkAnimation") as Texture2D;

		Projectile 	= Resources.Load		("Projectile") as GameObject;

		SpawnPoint1 = transform.FindChild	("Model/Spawnpoint1").gameObject;
		Model 		= transform.FindChild	("Model").		gameObject;
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
			if (Input.GetKeyDown(KeyCode.Mouse0))
			{
				Instantiate(Projectile, SpawnPoint1.transform.position, SpawnPoint1.transform.rotation);
			}
		}
		FitModelToPixel();
	}
	void FitModelToPixel ()
	{
		Model.transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z));
	}
	bool CheckIfAlive()
	{
		if (HealthPointsCurrent > 0)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	Vector2 CheckMovement ()
	{
		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
		{
			Vector2 MovementDirection = Vector2.zero;
			if (Input.GetKey(KeyCode.W))
			{
				MovementDirection.y += MovementSpeed;
			}
			if (Input.GetKey(KeyCode.A))
			{
				MovementDirection.x -= MovementSpeed;
			}
			if (Input.GetKey(KeyCode.S))
			{
				MovementDirection.y -= MovementSpeed;
			}
			if (Input.GetKey(KeyCode.D))
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
				StartCoroutine(AnimateWalk());
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
					StartCoroutine(AnimateWalk());
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