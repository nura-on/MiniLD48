using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour 
{

	// Use this for initialization
	void Start ()
	{
		Destroy(gameObject, 2f);
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.Translate(Vector3.up * Time.deltaTime * 600, Space.Self);
		transform.FindChild("Model").transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z));
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.tag == "Enemy")
		{
			Destroy(gameObject);
			Instantiate(Resources.Load("Blood") as GameObject, transform.position, Quaternion.identity);
		}
	}
}
