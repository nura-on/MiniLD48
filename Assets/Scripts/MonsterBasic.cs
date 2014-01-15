using UnityEngine;
using System.Collections;
public class MonsterBasic : MonoBehaviour
{
    public enum State { Spawn, Alive, Dead }
   
    protected State state;
    protected int currentHP = 100;
    protected float animationTimer = 0.05f;
    protected Material mat;
    protected Player player;

    private int xDiff, yDiff;
   
    private Transform model;

    // Use this for initialization
    protected void Awake()
    {
        mat = transform.FindChild("Model").renderer.material;

        player = Player.Instance;
        model = transform.FindChild("Model");
    }

    // Update is called once per frame
    protected void Update()
    {
        xDiff = 0;
        yDiff = 0;

        xDiff = Mathf.RoundToInt(transform.position.x - player.transform.position.x);
        yDiff = Mathf.RoundToInt(transform.position.y - player.transform.position.y);

        if (xDiff <= 0 && yDiff < 0)
        {
            model.eulerAngles = new Vector3(0, 0, 270 + Mathf.Rad2Deg * Mathf.Atan(Mathf.Abs(yDiff * 1f / xDiff * 1f)));
        }
        else if (xDiff < 0 && yDiff >= 0)
        {
            model.eulerAngles = new Vector3(0, 0, 270 - Mathf.Rad2Deg * Mathf.Atan(Mathf.Abs(yDiff * 1f / xDiff * 1f)));
        }
        else if (xDiff >= 0 && yDiff > 0)
        {
            model.eulerAngles = new Vector3(0, 0, 90 + Mathf.Rad2Deg * Mathf.Atan(Mathf.Abs(yDiff * 1f / xDiff * 1f)));
        }
        else if (xDiff > 0 && yDiff <= 0)
        {
            model.eulerAngles = new Vector3(0, 0, 90 - Mathf.Rad2Deg * Mathf.Atan(Mathf.Abs(yDiff * 1f / xDiff * 1f)));
        }
        model.position = new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
    }

    public void ReceiveDamage(int dmg)
    {
        currentHP -= dmg;
        if (CheckIfDead())
        {
            state = State.Dead;
        }
    }

    bool CheckIfDead()
    {
        if (currentHP <= 0)
        {
            return true;
        }
        return false;
    }

    protected bool IsInRangeOfPlayer(int distance)
    {
        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(player.transform.position.x, player.transform.position.y)) < distance)
        {
            return true;
        }
        return false;
    }

    protected IEnumerator Animation()
    {
        int i = 0;
        Texture tex = mat.mainTexture;
        mat.mainTextureScale = new Vector2(1f / (tex.width / 32f), 1f);
        while (state == State.Alive && Game.Instance.state == Game.GameState.InWave)
        {
            mat.mainTextureOffset = new Vector2(1f / (tex.width / 32f) * i, 0);
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

    internal void StartAnimation()
    {
        state = State.Alive;
        StartCoroutine(Animation());
    }
}