using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// ラスボス本戦　マス選択UI
/// </summary>
public class BossGameBUICellSelect : MonoBehaviour
{
    #region メンバー

    public GameObject grayMaskParent;
    public GameObject maskListParent;

    public Transform cursorParent;
    public BossGameBUICellCursor cursorDummy;

    #endregion

    #region 変数

    /// <summary>マスク</summary>
    private List<GameObject> maskList;

    /// <summary>表示中カーソル</summary>
    private List<BossGameBUICellCursor> activeCursorList;

    #endregion

    #region 基底

    /// <summary>
    /// 開始時
    /// </summary>
    private void Start()
    {
        maskList = new List<GameObject>();
        activeCursorList = new List<BossGameBUICellCursor>();

        // マスク取得
        for (var i = 0; i < maskListParent.transform.childCount; ++i)
        {
            maskList.Add(maskListParent.transform.GetChild(i).gameObject);
        }

        // 初期非表示
        cursorDummy.gameObject.SetActive(false);
    }

    #endregion

    #region 機能

    /// <summary>
    /// 表示
    /// </summary>
    /// <param name="maskEnableList">明るい場所のリスト</param>
    /// <param name="cursorList">カーソル表示リスト</param>
    public void Show(List<Vector2Int> maskEnableList, List<CellUIParam> cursorList)
    {
        ShowMask(maskEnableList);
        ShowCursor(cursorList);
    }

    /// <summary>
    /// マスク表示
    /// </summary>
    /// <param name="maskEnableList"></param>
    public void ShowMask(List<Vector2Int> maskEnableList)
    {
        var enableNameList = maskEnableList.Select(p => $"mask{p.x}_{p.y}").ToList();

        foreach (var mask in maskList)
        {
            mask.SetActive(enableNameList.Contains(mask.name));
        }
    }

    /// <summary>
    /// カーソル表示
    /// </summary>
    /// <param name="cursorList"></param>
    public void ShowCursor(List<CellUIParam> cursorList)
    {
        var newList = new List<BossGameBUICellCursor>();
        foreach(var p in cursorList)
        {
            BossGameBUICellCursor cursor;
            if (activeCursorList.Count > 0)
            {
                // あるぶん再利用
                cursor = activeCursorList[0];
                activeCursorList.RemoveAt(0);
            }
            else
            {
                // 無ければ新規作成
                cursor = Instantiate(cursorDummy);
            }

            // 位置設定
            cursor.Show(p.x, p.y, p.width, p.height, p.enable);
            newList.Add(cursor);
        }

        // 使わなかった分削除
        foreach(var old in activeCursorList)
        {
            Destroy(old.gameObject);
        }
        activeCursorList.Clear();
        activeCursorList = newList;
    }

    /// <summary>
    /// 非表示
    /// </summary>
    public void Hide()
    {
        grayMaskParent.SetActive(false);
        ShowCursor(new List<CellUIParam>());
    }

    #endregion

    #region 表示パラメータ

    /// <summary>
    /// 表示指定パラメータ
    /// </summary>
    public struct CellUIParam
    {
        /// <summary>座標</summary>
        public int x;
        /// <summary>座標</summary>
        public int y;
        /// <summary>幅：カーソルのみ</summary>
        public int width;
        /// <summary>高さ：カーソルのみ</summary>
        public int height;
        /// <summary>★表示：カーソルのみ</summary>
        public bool enable;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="_x"></param>
        /// <param name="_y"></param>
        /// <param name="_width"></param>
        /// <param name="_height"></param>
        /// <param name="_enable"></param>
        public CellUIParam(int _x, int _y, int _width, int _height, bool _enable)
        {
            x = _x;
            y = _y;
            width = _width;
            height = _height;
            enable = _enable;
        }
    }

    #endregion
}
