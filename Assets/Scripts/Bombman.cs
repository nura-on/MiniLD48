using UnityEngine;
using System.Collections;

public class Bombman : MonsterBasic
{

    public enum State { Spawn, Alive, Dead }
    public State state;

    private Vector3 _direction, _velocity, _targetPos, _distance, _relativeUp;
    private Transform _player;
    private Material _mat;

    public int maxMovementSpeed = 200;

	public int DistanceToBurst = 32;

    void Awake()
    {
        _player = GameObject.Find("Player").transform;
        _mat = transform.FindChild("Model").renderer.material;
    }
    // backup
    void OnLevelWasLoaded(int Level)
    {
        if (Level == 1)
        {
            if (_player == null) _player = GameObject.Find("Player").transform;
        }
    }

    // Use this for initialization
    void Start()
    {
        state = State.Spawn;
		BaiscStart();
    }

    // Update is called once per frame
    void Update()
    {
		BaiscUpdate();
        if (state == State.Spawn)
        {
            state = State.Alive;
            StartCoroutine(_Animation());
        }
        if (state == State.Alive)
        {
            // TODO check animation and switch if necessary

            // kinematic seek
            _KinematicSeek();

            // burst within radius
			if (CheckRadius())
			{
				BurstWithinRadius();
			}
        }
        if (state == State.Dead)
        {
            // TODO either remove or replace with some decal sprite
        }
    }

	private bool CheckRadius ()
	{
		if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(_player.position.x, _player.position.y)) < DistanceToBurst)
		{
			return true;
		}
		return false;
	}

	private void BurstWithinRadius ()
	{
		_player.GetComponent<Player>().ReceiveDamage(20);
		Instantiate(Resources.Load("Explosion") as GameObject, transform.position, Quaternion.identity);
		Instantiate(Resources.Load("Blood") as GameObject, new Vector3(transform.position.x, transform.position.y, transform.position.z - 1), Quaternion.identity);
		Instantiate(Resources.Load("Crater") as GameObject, new Vector3(transform.position.x, transform.position.y, -3), Quaternion.identity);
		Destroy(gameObject);
	}

    private IEnumerator _Animation()
    {
        int i = 0;
        Texture tex = _mat.mainTexture;
        _mat.mainTextureScale = new Vector2(1f / (tex.width / 32f), 1f);
        while (state == State.Alive)
        {
            _mat.mainTextureOffset = new Vector2(1f / (tex.width / 32f) * i, 0);
            if (i == (tex.width / 32) - 1)
            {
                i = 0;
            }
            else
            {
                i++;
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void _KinematicSeek()
    {
        _targetPos = _player.position;
        _distance = _targetPos - transform.position;
        _direction = _distance.normalized;
        _velocity = _direction * maxMovementSpeed;

        transform.Translate(_velocity * Time.deltaTime);
    }
}
