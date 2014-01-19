using UnityEngine;
using System;
using System.Collections;
public class Player : MonoBehaviour
{
    public int movementSpeed = 150;
    private GameObject projectile, bomb, model, bullet;
    Transform spawnPoint;
    private int healthPointsCurrent = 100;
    //private int ArmorPointsCurrent = 0;
    private Texture2D walkAnimation;
    private Camera playerCamera;
    private float fullAutomaticFireRate = 0.05f;
    private bool fullAutomaticFireReady = true;

    private bool walking;
    private bool canReceiveDamage = true;

    // TODO create state mechanism....

    // TODO blinking hero while !canReceiveDamage

    private static Player _singleton;
    private float _z_spray;
    private Vector3 eulerAngles;
    private Player() { }

    public enum PlayerState { Dead, Alive };
    public PlayerState state;
    private bool once;
    private bool visible = true;
    private GUIStyle centeredStyle;
    public Font font;
    public AudioClip hurtSound, shootSound;

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
        spawnPoint = model.transform.FindChild("Spawnpoint1").gameObject.transform;
        playerCamera = Camera.main;

        state = PlayerState.Alive;
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
                    audio.PlayOneShot(shootSound);
                    bullet = Instantiate(projectile, new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z + 5), spawnPoint.rotation) as GameObject;
                    bullet.transform.Rotate(bullet.transform.rotation.x, bullet.transform.rotation.y, UnityEngine.Random.Range(-10f, 10f) + bullet.transform.rotation.z);
                    StartCoroutine(SetCooldownFullAutomaticFire());
                }
            }
            //if (Input.GetKeyDown(KeyCode.F))
            //{
            //    Instantiate(bomb, spawnPoint.position, spawnPoint.rotation);
            //}
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
            state = PlayerState.Dead;
            return false;
        }
    }

    public void ReceiveDamage(int dmg)
    {
        if (canReceiveDamage)
        {
            audio.PlayOneShot(hurtSound);
            healthPointsCurrent -= dmg;
            StartCoroutine(Invincible());
        }
    }

    private IEnumerator Invincible()
    {
        canReceiveDamage = false;
        yield return new WaitForSeconds(1.5f);
        // TODO animation
        canReceiveDamage = true;
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
            walking = false;
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
            if (!walking)
            {
                walking = true;
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
                if (!walking)
                {
                    walking = true;
                    //StartCoroutine(AnimateWalk());
                }
                transform.Translate(new Vector3(0, MovementDirection.y * Time.deltaTime, 0));
            }
        }
    }
    void OnGUI()
    {

        // GUI.Label(new Rect(Screen.width - Screen.width * 0.1f, 20, 100, 100), "Armor: " + ArmorPointsCurrent);

        if (state == PlayerState.Dead)
        {
            if (once)
            {
                once = false;
                StartCoroutine(Timer());
            }
            centeredStyle = GUI.skin.GetStyle("Label");
            centeredStyle.alignment = TextAnchor.UpperCenter;
            centeredStyle.font = font;
            if (visible) GUI.Label(new Rect((Screen.width / 2) - 400, Screen.height / 2 + 50, 800, 500), "<color=white><size=15>Press any key to restart the game</size></color>", centeredStyle);
            GUI.Label(new Rect((Screen.width / 2) - 400, Screen.height / 2 - 160, 800, 500), "<color=red><size=40>YOU DIED!</size></color>", centeredStyle);
        }
        else
        {
            GUI.Label(new Rect(312, 0, 200, 100), "HP: " + healthPointsCurrent);
        }
    }

    private IEnumerator Timer()
    {
        while (true)
        {
            if (visible)
            {
                yield return new WaitForSeconds(1.2f);
                visible = false;
            }
            else
            {
                yield return new WaitForSeconds(0.2f);
                visible = true;
            }
        }
    }

    IEnumerator AnimateWalk()
    {
        int Counter1 = 0;
        int Counter2 = 0;
        model.renderer.material.mainTexture = walkAnimation;
        model.renderer.material.mainTextureScale = new Vector2(1f / (walkAnimation.width / 64f), 1f / (walkAnimation.height / 64f));
        while (walking)
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