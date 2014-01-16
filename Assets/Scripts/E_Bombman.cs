using UnityEngine;
using System.Collections;

public class E_Bombman : MonsterBasic
{

    private Vector3 _direction, _velocity, _targetPos, _distance, _relativeUp;
    //private Material _mat;

    public int maxMovementSpeed = 200;

    public int distanceToBurst = 32;
    private GameObject _explo;
    private GameObject _crater;
    private GameObject _blood;

    void Awake()
    {
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
        base.state = State.Spawn;
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
                BurstWithinRadius();
            }
        }
        if (base.state == State.Dead)
        {
            // TODO either remove or replace with some decal sprite
            currentHP = 0;
            Destroy(gameObject);
        }
    }

    private void BurstWithinRadius()
    {
        base.player.ReceiveDamage(20);
        Instantiate(_explo, transform.position, Quaternion.identity);
        Instantiate(_blood, new Vector3(transform.position.x, transform.position.y, transform.position.z - 1), Quaternion.identity);
        Instantiate(_crater, new Vector3(transform.position.x, transform.position.y, -3), Quaternion.identity);
        state = State.Dead;
    }

    private void _KinematicSeek()
    {
        _targetPos = base.player.transform.position;
        _distance = _targetPos - transform.position;
        _direction = _distance.normalized;
        _velocity = _direction * maxMovementSpeed;

        transform.Translate(_velocity * Time.deltaTime);
    }
}
