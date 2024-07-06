using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 爆発エフェクト
/// </summary>
public class BossGameAFireBomb : MonoBehaviour
{
    public Sprite bombA;
    public Sprite bombB;
    public Sprite bombC;
    public Sprite bombD;

    /// <summary>
    /// 再生した後消える
    /// </summary>
    /// <param name="renderPriority"></param>
    public void PlayAndDestroy(int renderPriority)
    {
        GetComponent<SpriteRenderer>().sortingOrder += renderPriority;
        gameObject.SetActive(true);
        StartCoroutine(PlayCoroutine());
    }

    /// <summary>
    /// 再生コルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayCoroutine()
    {
        var model = GetComponent<SpriteRenderer>();

        model.sprite = bombA;
        yield return new WaitForSeconds(0.08f);
        model.sprite = bombB;
        yield return new WaitForSeconds(0.08f);
        model.sprite = bombC;
        yield return new WaitForSeconds(0.08f);
        model.sprite = bombD;
        yield return new WaitForSeconds(0.08f);

        Destroy(gameObject);
    }
}
