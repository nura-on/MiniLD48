using UnityEngine;
using System.Collections;

/// <summary>
/// chose that stupid name on purpose ;)
/// https://www.youtube.com/watch?v=AJdEqssNZ-U&list=PL5xVeqDOX7ZwU5SkooCf8dMSTWYyRk_r_&index=1
/// </summary>
public class CameraGameFeel : MonoBehaviour
{

    Transform _player;
    Vector3 _newPos;

    void Awake()
    {
        _player = GameObject.Find("Player").transform;
        transform.position = new Vector3(_player.position.x, _player.position.y, transform.position.z);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _newPos = new Vector3(_player.position.x, _player.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, _newPos, 0.5f * Time.deltaTime);
        transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z));
    }
}
