using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// つくよみちゃん戦　プレイヤーフィールド表示
/// </summary>
public class TukuyomiGamePlayField : MonoBehaviour
{
    #region 定数

    private const float WALL_SIZE = 6f;
    private const float CELL_SIZE = 48f;
    private const float KUGIRI_SIZE = 2f;

    #endregion

    #region メンバー

    public Transform wallR;
    public Transform wallL;
    public Transform wallU;
    public Transform wallD;

    public Transform kugiriDummy;
    private List<Transform> kugiris = new List<Transform>();

    public Transform targetCell;

    #endregion

    #region 変数

    /// <summary>フィールド幅</summary>
    private float fieldWidth;
    /// <summary>フィールド高さ</summary>
    private float fieldHeight;
    /// <summary>セルモードのX数</summary>
    private int fieldCellX;
    /// <summary>セルモードのY数</summary>
    private int fieldCellY;

    #endregion

    /// <summary>
    /// 初期化
    /// </summary>
    void Start()
    {
        kugiriDummy.gameObject.SetActive(false);
        Hide();
    }

    /// <summary>
    /// 表示
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public void Show(float width, float height)
    {
        targetCell.gameObject.SetActive(false);
        DeleteKugiri();
        var wHeight = height + WALL_SIZE * 2f;
        var wWidth = width + WALL_SIZE * 2f;
        var wX = (width + WALL_SIZE) / 2f;
        var wY = (height + WALL_SIZE) / 2f;

        wallR.localScale = new Vector3(WALL_SIZE, wHeight, 1f);
        wallL.localScale = new Vector3(WALL_SIZE, wHeight, 1f);
        wallU.localScale = new Vector3(wWidth, WALL_SIZE, 1f);
        wallD.localScale = new Vector3(wWidth, WALL_SIZE, 1f);
        wallR.localPosition = new Vector3(wX, 0f);
        wallL.localPosition = new Vector3(-wX, 0f);
        wallU.localPosition = new Vector3(0f, wY);
        wallD.localPosition = new Vector3(0f, -wY);
        wallR.gameObject.SetActive(true);
        wallL.gameObject.SetActive(true);
        wallU.gameObject.SetActive(true);
        wallD.gameObject.SetActive(true);

        fieldWidth = width;
        fieldHeight = height;
        fieldCellX = 0;
        fieldCellY = 0;
    }

    /// <summary>
    /// マス目つき表示
    /// </summary>
    /// <param name="cellX"></param>
    /// <param name="cellY"></param>
    public void ShowCellField(int cellX, int cellY)
    {
        // フィールド表示
        var fieldW = cellX * CELL_SIZE;
        var fieldH = cellY * CELL_SIZE;
        Show(fieldW, fieldH);
        fieldCellX = cellX;
        fieldCellY = cellY;

        var kugiriX = -fieldW / 2f;
        var xkugiriSize = new Vector3(KUGIRI_SIZE, fieldH, 1f);
        var kugiriY = -fieldH / 2f;
        var ykugiriSize = new Vector3(fieldW, KUGIRI_SIZE, 1f);
        // 区切り線作成
        for (var x = 0; x < cellX - 1; ++x)
        {
            kugiriX += CELL_SIZE;
            var k = Instantiate(kugiriDummy);
            k.SetParent(transform);
            k.localScale = xkugiriSize;
            k.localPosition = new Vector3(kugiriX, 0f);
            k.gameObject.SetActive(true);
            kugiris.Add(k);
        }
        for (var y = 0; y < cellY - 1; ++y)
        {
            kugiriY += CELL_SIZE;
            var k = Instantiate(kugiriDummy);
            k.SetParent(transform);
            k.localScale = ykugiriSize;
            k.localPosition = new Vector3(0f, kugiriY);
            k.gameObject.SetActive(true);
            kugiris.Add(k);
        }

    }

    /// <summary>
    /// ターゲットセル表示
    /// </summary>
    /// <param name="cellLoc"></param>
    public void ShowTargetCell(Vector2 cellLoc)
    {
        targetCell.gameObject.SetActive(true);
        targetCell.localPosition = GetCellPosition(cellLoc);
    }

    /// <summary>
    /// 非表示
    /// </summary>
    public void Hide()
    {
        wallR.gameObject.SetActive(false);
        wallL.gameObject.SetActive(false);
        wallU.gameObject.SetActive(false);
        wallD.gameObject.SetActive(false);
        DeleteKugiri();
        targetCell.gameObject.SetActive(false);

        fieldWidth = -1f;
        fieldHeight = -1f;
        fieldCellX = 0;
        fieldCellY = 0;
    }

    /// <summary>
    /// 区切り線削除
    /// </summary>
    public void DeleteKugiri()
    {
        foreach (var k in kugiris)
        {
            Destroy(k.gameObject);
        }
        kugiris.Clear();
    }

    /// <summary>
    /// フィールド内に留める
    /// </summary>
    /// <param name="value"></param>
    /// <param name="r">半径</param>
    /// <returns></returns>
    public Vector3 ClampFreeField(Vector3 value, float r)
    {
        var ret = value;

        // 端の座標を選択
        var maxX = 0f;
        var maxY = 0f;
        var minX = 0f;
        var minY = 0f;
        // フィールド非表示時は画面端
        if (fieldWidth < 0f)
        {
            maxX = Constant.SCREEN_WIDTH / 2f - transform.position.x;
            minX = -Constant.SCREEN_WIDTH / 2f - transform.position.x;
        }
        else
        {
            maxX = fieldWidth / 2f;
            minX = -fieldWidth / 2f;
        }
        if (fieldHeight < 0f)
        {
            maxY = Constant.SCREEN_HEIGHT / 2f - transform.position.y;
            minY = Constant.SCREEN_HEIGHT / 2f - transform.position.y;
        }
        else
        {
            maxY = fieldHeight / 2f;
            minY = -fieldHeight / 2f;
        }

        // X
        if (maxX - minX < r * 2f) // 幅が大きすぎたら中央
            ret.x = (maxX + minX) / 2f;
        else if (ret.x + r > maxX)    // 超えていたらクランプ
            ret.x = maxX - r;
        else if (ret.x - r < minX)
            ret.x = minX + r;

        // Y
        if (maxY - minY < r * 2f) ret.y = (maxY + minY) / 2f;
        else if (ret.y + r > maxY) ret.y = maxY - r;
        else if (ret.y - r < minY) ret.y = minY + r;

        return ret;
    }

    /// <summary>
    /// フィールド内に留める
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public Vector2 ClampCellField(Vector2 value)
    {
        if (fieldCellX <= 0 || fieldCellY <= 0) return Vector2.zero;

        var ret = value;
        if (ret.x >= fieldCellX) ret.x = fieldCellX - 1;
        else if (ret.x < 0) ret.x = 0;
        if (ret.y >= fieldCellY) ret.y = fieldCellY - 1;
        else if (ret.y < 0) ret.y = 0;

        return ret;
    }

    /// <summary>
    /// フィールド内か判定
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool InField(Vector2 value)
    {
        if (value.x < 0 || value.y < 0) return false;
        if (value.x >= fieldCellX || value.y >= fieldCellY) return false;

        return true;
    }

    /// <summary>
    /// セルの座標を取得
    /// </summary>
    /// <param name="cell"></param>
    /// <returns></returns>
    public Vector3 GetCellPosition(Vector2 cell)
    {
        var zeroX = (fieldCellX - 1) * CELL_SIZE * -0.5f;
        var zeroY = (fieldCellY - 1) * CELL_SIZE * -0.5f;

        return new Vector3(zeroX + CELL_SIZE * cell.x, zeroY + CELL_SIZE * cell.y);
    }
}
