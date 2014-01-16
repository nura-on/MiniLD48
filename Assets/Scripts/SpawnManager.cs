using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    private GameObject[] spawnPoints = new GameObject[8];

    private Game _gm;
    private float _spawnFrequency = 1f;

    Transform _player;

    public enum EnemyTypes { BombMan, Eye }
    public List<GameObject> enemyPrefabs, enemies;

    private SpawnManager() { }
    private static SpawnManager _singleton;
    public static SpawnManager Instance { get { return (_singleton == null ? new SpawnManager() : _singleton); } }

    void Awake()
    {
        _singleton = this;

        enemyPrefabs = new List<GameObject>(2);
        _gm = Game.Instance;
        //enemyPrefabs.Add(Resources.Load("Enemies/Bombman/Bombman") as GameObject);
        enemyPrefabs.Add(Resources.Load("Enemies/Eye/Eye") as GameObject);
    }

    void Start()
    {
        StartCoroutine(_SpawnEnemies());
    }

    private IEnumerator _SpawnEnemies()
    {
        while (true)
        {
            if (_gm.state == Game.GameState.InWave)
            {
                // TODO random enemy
                // TODO depending on wave
                enemies.Add(Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)], spawnPoints[Random.Range(0, 8)].transform.position, Quaternion.Euler(Vector3.zero)) as GameObject);
            }
            yield return new WaitForSeconds(Random.Range(_spawnFrequency - 0.5f, _spawnFrequency));
        }
    }

    void OnLevelWasLoaded(int level)
    {
        if (level == 1)
        {
            _player = Player.Instance.gameObject.transform;
            _RearrangeSpawnPoints();
        }
    }

    private void _RearrangeSpawnPoints()
    {
        for (uint i = 0; i < spawnPoints.Length; i++)
        {
            spawnPoints[i] = new GameObject();
            spawnPoints[i].transform.parent = _player;
        }
        spawnPoints[0].transform.localPosition = new Vector2(0, 360);
        spawnPoints[1].transform.localPosition = new Vector2(0, -360);
        spawnPoints[2].transform.localPosition = new Vector2(360, 0);
        spawnPoints[3].transform.localPosition = new Vector2(-360, 0);
        spawnPoints[4].transform.localPosition = new Vector2(360, 360);
        spawnPoints[5].transform.localPosition = new Vector2(-360, 360);
        spawnPoints[6].transform.localPosition = new Vector2(360, -360);
        spawnPoints[7].transform.localPosition = new Vector2(-360, -360);
    }
}
