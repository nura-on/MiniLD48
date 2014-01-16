using UnityEngine;
using System;
using System.Collections;
public class Player : MonoBehaviour
{
    public int movementSpeed = 150;
    private GameObject spawnPoint, projectile, bomb, model;
    private int healthPointsCurrent = 100;
    //private int ArmorPointsCurrent = 0;
    private Texture2D walkAnimation;
    private Camera playerCamera;
    private float fullAutomaticFireRate = 0.05f;
    private bool fullAutomaticFireReady = true;

    private static Player _singleton;
    private Player() { }
    public static Player Instance
    {
        get
        {
            return (_singleton == null ? new Player() : _singleton);
        }
    }

    void Awake()
    {
        _singleton = this;

        walkAnimation = Resources.Load("WalkAnimation") as Texture2D;

        projectile = Resources.Load("Bullet") as GameObject;

        bomb = Resources.Load("Bomb") as GameObject;
        model = transform.FindChild("Model").gameObject;
        spawnPoint = model.transform.FindChild("Spawnpoint1").gameObject;
        playerCamera = Camera.main;
    }
    void Update()
    {
        //Checks if the Character is Alive
        if (CheckIfAlive() && Game.Instance.state == Game.GameState.InWave)
        {
            //Movement
            PerformMovement(CheckMovement());

            //Rotation
            PerformRotation();

            //Attack
            if (Input.GetKey(KeyCode.Mouse0))
            {
                // TODO crosshair

                if (fullAutomaticFireReady)
                {
                    Instantiate(projectile, spawnPoint.transform.position, spawnPoint.transform.rotation);
                    StartCoroutine(SetCooldownFullAutomaticFire());
                }
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                Instantiate(bomb, spawnPoint.transform.position, spawnPoint.transform.rotation);
            }
        }
        FitModelToPixel();
        FitCameraToPixel();
    }
    IEnumerator SetCooldownFullAutomaticFire()
    {
        fullAutomaticFireReady = false;
        yield return new WaitForSeconds(fullAutomaticFireRate);
        fullAutomaticFireReady = true;
    }
    void FitModelToPixel()
    {
        model.transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z));
    }

    void FitCameraToPixel()
    {
        playerCamera.transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), playerCamera.transform.position.z);
    }

    bool CheckIfAlive()
    {
        if (healthPointsCurrent > 0)
        {
            return true;
        }
        else
        {
            Game.Instance.state = Game.GameState.NotRunning;
            return false;
        }
    }

    public void ReceiveDamage(int dmg)
    {
        healthPointsCurrent -= dmg;
    }

    Vector2 CheckMovement()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            Vector2 MovementDirection = Vector2.zero;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                MovementDirection.y += movementSpeed;
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                MovementDirection.x -= movementSpeed;
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                MovementDirection.y -= movementSpeed;
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                MovementDirection.x += movementSpeed;
            }
            return MovementDirection;
        }
        else
        {
            Walking = false;
            return Vector2.zero;
        }
    }
    void PerformRotation()
    {
        //Rotation
        if (Input.mousePosition.x >= Screen.width / 2 && Input.mousePosition.y > Screen.height / 2)
        {
            model.transform.eulerAngles = new Vector3(0, 0, -Mathf.Rad2Deg * Mathf.Atan((Input.mousePosition.x * 1f - Screen.width / 2f) / (Input.mousePosition.y * 1f - Screen.height / 2f)));
        }
        else if (Input.mousePosition.x > Screen.width / 2 && Input.mousePosition.y <= Screen.height / 2)
        {
            model.transform.eulerAngles = new Vector3(0, 0, -90 - Mathf.Rad2Deg * Mathf.Atan((Screen.height / 2f - Input.mousePosition.y * 1f) / (Input.mousePosition.x * 1f - Screen.width / 2f)));
        }
        else if (Input.mousePosition.x <= Screen.width / 2 && Input.mousePosition.y < Screen.height / 2)
        {
            model.transform.eulerAngles = new Vector3(0, 0, -180 - Mathf.Rad2Deg * Mathf.Atan((Screen.width / 2f - Input.mousePosition.x * 1f) / (Screen.height / 2f - Input.mousePosition.y * 1f)));
        }
        else if (Input.mousePosition.x < Screen.width / 2 && Input.mousePosition.y >= Screen.height / 2)
        {
            model.transform.eulerAngles = new Vector3(0, 0, -270 - Mathf.Rad2Deg * Mathf.Atan((Input.mousePosition.y * 1f - Screen.height / 2f) / (Screen.width / 2f - Input.mousePosition.x * 1f)));
        }
    }
    void PerformMovement(Vector2 MovementDirection)
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
    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width - Screen.width * 0.1f, 0, 100, 100), "HP: " + healthPointsCurrent);
        // GUI.Label(new Rect(Screen.width - Screen.width * 0.1f, 20, 100, 100), "Armor: " + ArmorPointsCurrent);
    }
    private bool Walking;
    IEnumerator AnimateWalk()
    {
        int Counter1 = 0;
        int Counter2 = 0;
        model.renderer.material.mainTexture = walkAnimation;
        model.renderer.material.mainTextureScale = new Vector2(1f / (walkAnimation.width / 64f), 1f / (walkAnimation.height / 64f));
        while (Walking)
        {
            model.renderer.material.mainTextureOffset = new Vector2(1f / (walkAnimation.width / 64f) * Counter1, 1f / (walkAnimation.height / 64f) * Counter2);
            if (Counter1 == (walkAnimation.width / 64) - 1)
            {
                Counter1 = 0;
                if (Counter2 == (walkAnimation.height / 64) - 1)
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