using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エフェクト基本クラス
/// </summary>
public abstract class BossGameBEffectBase : MonoBehaviour
{
    public BossGameSystemB system;
    public SpriteRenderer model;
    protected Vector3 basePosition;

    /// <summary>
    /// 実行後自分でDestroy
    /// </summary>
    /// <returns></returns>
    public void PlayAndDestroy(Vector3 pos)
    {
        basePosition = pos;
        transform.localPosition = basePosition;

        model.sortingLayerName = "FieldObject";
        model.sortingOrder = 20000;
        model.gameObject.SetActive(false);
        gameObject.SetActive(true);
        StartCoroutine(BasePlayCoroutine());
    }

    /// <summary>
    /// 実行コルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator BasePlayCoroutine()
    {
        model.gameObject.SetActive(true);
        yield return Play();
        // 再生が終わったら居なくなる
        Destroy(gameObject);
    }

    /// <summary>
    /// 実行内容　派生先で実装
    /// </summary>
    /// <returns></returns>
    protected abstract IEnumerator Play();
}
