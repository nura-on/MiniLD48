using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class E_Bombman : MonsterBasic
{

    public int maxMovementSpeed = 200;

    public int distanceToBurst = 32;
    private GameObject _blood, _explo, _crater;
    public int explosionDamage = 40;

    void Awake()
    {
        base.resolution = 32f;
        base.player = Player.Instance;

        _explo = Resources.Load("Explosion") as GameObject;
        _blood = Resources.Load("Blood") as GameObject;
        _crater = Resources.Load("Crater") as GameObject;

        base.Awake();

        base.currentHP = 1;
    }
    // backup
    void OnLevelWasLoaded(int level)
    {
        if (level == 1)
        {
            if (base.player == null) base.player = Player.Instance;
        }
    }

    // Use this for initialization
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (base.state == State.Spawn)
        {
            base.StartAnimation();
        }
        if (base.state == State.Alive && Game.Instance.state == Game.GameState.InWave)
        {
            //infuckingheritance
            base.Update();

            // kinematic seek
            _KinematicSeek();

            // burst within radius
            if (base.IsInRangeOfPlayer(distanceToBurst))
            {
                Explode();
            }
        }
        if (base.state == State.Dead)
        {
            // TODO either remove or replace with some decal sprite

            // TODO random chance of exploding
            currentHP = 0;
            Destroy(gameObject);
        }
    }

    private void Explode()
    {
        //ExplosionDamage(transform.position, distanceToBurst * 1.5f);
        base.player.ReceiveDamage(20);
        Instantiate(_explo, transform.position, Quaternion.identity);
        Instantiate(_blood, new Vector3(transform.position.x, transform.position.y, transform.position.z - 1), Quaternion.identity);
        Instantiate(_crater, new Vector3(transform.position.x, transform.position.y, -3), Quaternion.identity);
        state = State.Dead;
    }

    // TODO buggy
    void ExplosionDamage(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, 100);
        int i = 0;
        while (i < hitColliders.Length)
        {

            if (hitColliders[i] == transform.collider)
            {
                continue;
            }
            else if (hitColliders[i].tag == "Model")
            {
                hitColliders[i].transform.parent.GetComponent<MonsterBasic>().ReceiveDamage(explosionDamage);
                Debug.Log(hitColliders[i].transform.parent.GetComponent<MonsterBasic>().currentHP);
            }
            else if (hitColliders[i].tag == "Enemy")
            {
                hitColliders[i].transform.GetComponent<MonsterBasic>().ReceiveDamage(explosionDamage);
                Debug.Log(hitColliders[i].transform.GetComponent<MonsterBasic>().currentHP);
            }

            if (hitColliders[i].transform.GetComponent<MonsterBasic>() != null) Debug.Log("YOLO");
            i++;
        }
    }


    private void _KinematicSeek()
    {
        base.targetPos = base.player.transform.position;
        base.distance = base.targetPos - transform.position;
        base.direction = base.distance.normalized;
        base.velocity = base.direction * maxMovementSpeed;

        transform.Translate(velocity * Time.deltaTime);
    }
}
