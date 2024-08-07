using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

/// <summary>
/// ラスボス本戦　セル選択カーソル
/// </summary>
public class BossGameBUICellCursor : MonoBehaviour
{
    #region 定数

    /// <summary>4隅パーツのX座標オフセット</summary>
    private const float PARTS_X = 50f;
    /// <summary>4隅パーツのY座標オフセット</summary>
    private const float PARTS_Y = 28f;

    #endregion

    #region メンバー

    public Transform parts_ld;
    public Transform parts_rd;
    public Transform parts_ru;
    public Transform parts_lu;
    public GameObject star;

    #endregion

    /// <summary>
    /// 表示 幅と高さは仕様上かならず奇数
    /// </summary>
    /// <param name="centerX"></param>
    /// <param name="centerY"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="showStar"></param>
    public void Show(Vector2Int center, int width, int height, bool showStar)
    {
        gameObject.SetActive(true);

        // 最大・最小座標
        var maxPos = BossGameSystemB.GetCellPosition(new Vector2Int(BossGameSystemB.CELL_X_COUNT - 1, BossGameSystemB.CELL_Y_COUNT - 1));
        var minPos = BossGameSystemB.GetCellPosition(new Vector2Int(0, 0));
        maxPos.x += PARTS_X;
        maxPos.y += PARTS_Y;
        minPos.x -= PARTS_X;
        minPos.y -= PARTS_Y;

        // 中心座標
        var centerPos = BossGameSystemB.GetCellPosition(center);
        transform.localPosition = centerPos;
        maxPos -= centerPos;
        minPos -= centerPos;

        // ★の表示
        star.SetActive(showStar);

        // 上下左右の位置設定
        var addX = (width / 2) * BossGameSystemB.CELL_WIDTH + PARTS_X;
        var addY = (height / 2) * BossGameSystemB.CELL_HEIGHT + PARTS_Y;
        // フィールドの範囲を超えない
        var rightX = addX > maxPos.x ? maxPos.x : addX;
        var leftX = -addX < minPos.x ? minPos.x : -addX;
        var upY = addY > maxPos.y ? maxPos.y : addY;
        var btmY = -addY < minPos.y ? minPos.y : -addY;

        parts_ld.transform.localPosition = new Vector3(leftX, btmY);
        parts_rd.transform.localPosition = new Vector3(rightX, btmY);
        parts_ru.transform.localPosition = new Vector3(rightX, upY);
        parts_lu.transform.localPosition = new Vector3(leftX, upY);
    }
}
