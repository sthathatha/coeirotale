using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 汎用座標
/// </summary>
public class GeneralPosition : MonoBehaviour
{
    public int id = 0;

    /// <summary>
    /// 座標取得
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPosition()
    {
        return gameObject.transform.position;
    }

    /// <summary>
    /// 開始時
    /// </summary>
    private void Start()
    {
        // エディタ用のSpriteは消す
        var sprite = GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            sprite.enabled = false;
        }
    }
}
