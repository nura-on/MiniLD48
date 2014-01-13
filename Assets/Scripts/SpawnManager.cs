using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{

    private GameObject[] spawnPoints = new GameObject[8];

    private Game _gm;
    private float _spawnFrequency = 1f;

    GameObject _bombman, _player;

    void OnLevelWasLoaded(int level)
    {
        if (level == 1)
        {
            _player = GameObject.Find("Player");
            _RearrangeSpawnPoints();
        }
    }

    private void _RearrangeSpawnPoints()
    {
        for (uint i = 0; i < spawnPoints.Length; i++)
        {
            spawnPoints[i] = new GameObject();
            spawnPoints[i].transform.parent = _player.transform;
        }
        spawnPoints[0].transform.localPosition = new Vector2(0, 340);
        spawnPoints[1].transform.localPosition = new Vector2(0, -340);
        spawnPoints[2].transform.localPosition = new Vector2(340, 0);
        spawnPoints[3].transform.localPosition = new Vector2(-340, 0);
        spawnPoints[4].transform.localPosition = new Vector2(340, 340);
        spawnPoints[5].transform.localPosition = new Vector2(-340, 340);
        spawnPoints[6].transform.localPosition = new Vector2(340, -340);
        spawnPoints[7].transform.localPosition = new Vector2(-340, -340);
    }

    void Awake()
    {
        _gm = Game.Instance;
        _bombman = Resources.Load("Enemy/Bombman/Bombman") as GameObject;
    }

    // Use this for initialization
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
                // TODO random spawn point 
                // TODO random enemy
                // TODO depending on wave

                Instantiate(_bombman, spawnPoints[Random.Range(0, 8)].transform.position, Quaternion.Euler(Vector3.zero));
            }
            yield return new WaitForSeconds(Random.Range(_spawnFrequency - 0.5f, _spawnFrequency));
        }
    }
}
