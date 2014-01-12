using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {


    GameObject explo, crater;

    void Awake() {
        crater = Resources.Load("Crater") as GameObject;
        explo = Resources.Load("Explosion") as GameObject;
    }

	void Start ()
	{
		Destroy(gameObject, 2f);
	}

	void Update ()
	{
		transform.Translate(Vector3.up * Time.deltaTime * 200, Space.Self);
		transform.FindChild("Model").transform.Rotate(0, 0, 1000 * Time.deltaTime);
		transform.FindChild("Model").transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z));
	}
	
	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.tag == "Enemy")
		{
			Destroy(gameObject);
            Instantiate(explo, transform.position, Quaternion.identity);
            Instantiate(crater, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
		}
	}
}