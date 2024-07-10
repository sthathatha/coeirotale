using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// まつかりすくレーザー
/// </summary>
public class MatukaGameBLaser : MonoBehaviour
{
    /// <summary>先端からヘッドの中心までの距離</summary>
    private const float HEAD_OFFSET = 128f;
    /// <summary>本体素材の幅ピクセル</summary>
    private const float BODY_WIDTH = 64f;

    /// <summary>座標に掛け算して使う　プレイヤー：1 エネミー：-1</summary>
    public int player_pos_mul;

    public GameObject head;
    public SpriteRenderer body;

    /// <summary>
    /// 表示
    /// </summary>
    /// <param name="show"></param>
    public void Show(bool show)
    {
        head.SetActive(show);
        body.gameObject.SetActive(show);
    }

    /// <summary>
    /// 位置指定
    /// </summary>
    /// <param name="x">先端の位置（ワールド座標）</param>
    public void SetPos(float x)
    {
        var baseX = transform.localPosition.x;
        ///頭の位置
        var locHead = x - baseX + HEAD_OFFSET * player_pos_mul;
        head.transform.localPosition = new Vector3(locHead, 0, 0);

        //ボディの幅と位置
        body.transform.localPosition = new Vector3(locHead / 2f, 0, 0);
        float widthP = Mathf.Abs(locHead);
        float widthScale = widthP / BODY_WIDTH * 100f;
        body.transform.localScale = new Vector3(widthScale, 100f, 0);
    }
}
