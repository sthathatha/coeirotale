using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ＊＊＊＊　ホーリーの玉１個
/// </summary>
public class BossGameAHolyBall : MonoBehaviour
{
    /// <summary>
    /// 表示
    /// </summary>
    /// <param name="pos">場所</param>
    /// <param name="time">表示時間</param>
    /// <param name="priority">表示優先度加算</param>
    public void Show(Vector3 pos, float time, int priority)
    {
        transform.localPosition = pos;
        gameObject.SetActive(true);
        GetComponent<SpriteRenderer>().sortingOrder += priority;
        Destroy(gameObject, time);
    }
}
