using UnityEngine;
using System.Collections;

public class DealDmg : MonoBehaviour
{
	public int DMG;
	public string Tag;
	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.tag == Tag)
		{
			MonsterBasic mBasic = col.GetComponent<MonsterBasic>();
			if (mBasic)
			{
				mBasic.ReceiveDamage(DMG);
			}
		}
	}
}