using UnityEngine;
using System.Collections;
public class HitEnemy : MonoBehaviour
{
    public float WaitForFrameTime = 0.1f;
    public float TileWidth = 16;
    Transform model;
    private bool once = true;
    public AudioClip explosiveSound;

    void Awake()
    {
        model = transform.FindChild("Model");
    }
    void Start()
    {
        StartCoroutine(AnimateEffect());
    }
    IEnumerator AnimateEffect()
    {
        if (once) {
            once = !once;
            audio.PlayOneShot(explosiveSound);
        }
        int Counter1 = 0;
        while (Counter1 < (model.renderer.material.mainTexture.width / TileWidth) - 1)
        {
            model.renderer.material.mainTextureOffset = new Vector2(1f / (model.renderer.material.mainTexture.width / TileWidth) * Counter1, 0);
            Counter1++;
            yield return new WaitForSeconds(WaitForFrameTime);
        }
        Destroy(gameObject);
        //gameObject.SetActive(false);
    }
}