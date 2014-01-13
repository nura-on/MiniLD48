using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour {

    public GameObject[] spawnPoints = new GameObject[5];

    private Game _gm;
    private float _spawnFrequency = 0.5f;

    GameObject _bombman;

    void OnLevelWasLoaded(int level) {
        if (level == 1) { 
            
        }
    }

    void Awake() {
        _gm = Game.Instance;
        _bombman = Resources.Load("Enemy/Bombman/Bombman") as GameObject;
    }

	// Use this for initialization
	void Start () {
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

                Instantiate(_bombman, spawnPoints[0].transform.position, Quaternion.Euler(Vector3.zero));
            }
            yield return new WaitForSeconds(_spawnFrequency);
        }
    }
}
