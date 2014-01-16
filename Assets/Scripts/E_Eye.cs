using UnityEngine;
using System.Collections;

public class E_Eye : MonsterBasic
{

    private int maxMovementSpeed = 140;
    private int shootingRange = 200;
    private float timeToTarget = 0.5f;//....

    // Use this for initialization
    void Awake()
    {
        base.Awake();
        base.resolution = 64f;
        base.player = Player.Instance;
        currentHP = 120; // 3hits
        base.animationTimer = 0.1f;
    }

    // Use this for initialization
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        // TODO atk range

        if (base.state == State.Spawn)
        {
            base.StartAnimation();
        }
        if (base.state == State.Alive && Game.Instance.state == Game.GameState.InWave)
        {
            //infuckingheritance
            base.Update();

            // kinematic seek


            // shoot in range
            if (base.IsInRangeOfPlayer(shootingRange))
            {
                _Shoot();
            }
            _KineticArrive();
        }
        if (base.state == State.Dead)
        {
            // TODO either remove or replace with some decal sprite

            // TODO random chance of exploding
            currentHP = 0;
            Destroy(gameObject);
        }
    }

    private void _KineticArrive()
    {
        base.direction = base.player.transform.position - transform.position;
        if (base.direction.magnitude < shootingRange) 
            base.velocity = Vector2.zero;
        else
        {
            base.velocity = base.direction / timeToTarget;

            if (base.velocity.magnitude > maxMovementSpeed)
                base.velocity = base.velocity.normalized * maxMovementSpeed;

            transform.Translate(base.velocity * Time.deltaTime);
        }
    }

    private void _Shoot()
    {
        Debug.Log("PEW! PEW! PEW!");
    }
}
