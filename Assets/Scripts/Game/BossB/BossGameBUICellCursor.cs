using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ラスボス本戦　セル選択カーソル
/// </summary>
public class BossGameBUICellCursor : MonoBehaviour
{
    private const float PARTS_X = 50f;
    private const float PARTS_Y = 28f;

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
    public void Show(int centerX, int centerY, int width, int height, bool showStar)
    {
        // 中心座標
        var centerPos = BossGameSystemB.GetCellLocation(centerX, centerY);
        transform.localPosition = centerPos;

        // ★の表示
        star.SetActive(showStar);

        // 上下左右の位置設定
        var addX = (width / 2) * BossGameSystemB.CELL_WIDTH + PARTS_X;
        var addY = (height / 2) * BossGameSystemB.CELL_HEIGHT + PARTS_Y;

        parts_ld.transform.localPosition = new Vector3(-addX, -addY);
        parts_rd.transform.localPosition = new Vector3(addX, -addY);
        parts_ru.transform.localPosition = new Vector3(addX, addY);
        parts_lu.transform.localPosition = new Vector3(-addX, addY);
    }
}
