using UnityEngine;
using System.Collections;

public class DealDmg : MonoBehaviour
{
	public int damage;
	public string tag;
	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.tag == tag)
		{
			MonsterBasic mBasic = col.GetComponent<MonsterBasic>();
			if (mBasic)
			{
				mBasic.ReceiveDamage(damage);
			}
		}
	}
}