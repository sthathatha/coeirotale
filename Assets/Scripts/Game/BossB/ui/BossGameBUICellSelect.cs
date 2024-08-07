using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ラスボス本戦　マス選択UI
/// </summary>
public class BossGameBUICellSelect : MonoBehaviour
{
    #region 定数

    /// <summary>
    /// セル選択結果
    /// </summary>
    public enum CellSelectResult
    {
        CellSelect = 0,
        Cancel,
    }

    #endregion

    #region メンバー

    public BossGameSystemB system;

    public GameObject grayMaskParent;
    public GameObject maskListParent;

    public Transform cursorParent;
    public BossGameBUICellCursor cursorDummy;

    #endregion

    #region 位置選択機能

    /// <summary>選択位置</summary>
    public Vector2Int SelectCell { get; set; }

    /// <summary>選択結果</summary>
    public CellSelectResult Result { get; set; }

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
        grayMaskParent.SetActive(true);
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
        foreach (var p in cursorList)
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
                cursor.transform.SetParent(cursorParent);
            }

            // 位置設定
            cursor.Show(p.location, p.width, p.height, p.enable);
            newList.Add(cursor);
        }

        // 使わなかった分削除
        foreach (var old in activeCursorList)
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

    #region 選択機能

    /// <summary>
    /// 表示と選択
    /// </summary>
    /// <param name="skillID"></param>
    /// <param name="chara"></param>
    /// <returns></returns>
    public IEnumerator ShowSelect(BossGameBDataBase.SkillID skillID, BossGameBCharacterBase chara)
    {
        var sound = ManagerSceneScript.GetInstance().soundMan;
        var input = InputManager.GetInstance();
        var skill = BossGameBDataBase.SkillList[skillID];

        // 選択可能リスト作成
        var cellList = BossGameSystemB.CreateEnableCellList(skillID, chara);

        // 初期位置を選択
        SelectCell = cellList.Count > 0 ? cellList[0] : chara.GetLocation();
        // 選択対象タイプ
        BossGameBCharacterBase.CharaType targetType = BossGameBCharacterBase.CharaType.Player;
        if (skill.TargetType == BossGameBDataBase.TargetTypeEnum.Fellow &&
            chara.CharacterType == BossGameBCharacterBase.CharaType.Player ||
            skill.TargetType == BossGameBDataBase.TargetTypeEnum.Enemy &&
            chara.CharacterType == BossGameBCharacterBase.CharaType.Enemy)
        {
            targetType = BossGameBCharacterBase.CharaType.Player;
        }
        else
        {
            targetType = BossGameBCharacterBase.CharaType.Enemy;
        }

        // ターゲットが選択位置内に見つかったら選択、無ければ最初の位置
        foreach (var cell in cellList)
        {
            var cellChara = system.GetCellCharacter(cell);
            // キャラが居ない
            if (cellChara == null) continue;

            if (skill.TargetType != BossGameBDataBase.TargetTypeEnum.All)
            {
                // ターゲットでない
                if (cellChara.CharacterType != targetType) continue;
            }

            // ここに決定
            SelectCell = cell;
        }

        // 選択カーソルパラメータ 初期表示
        var cursorParam = new CellUIParam(SelectCell, skill.EffectRange * 2 + 1, skill.EffectRange * 2 + 1, true);
        Show(cellList, new List<CellUIParam>() { cursorParam });

        while (true)
        {
            yield return null;

            if (input.GetKeyPress(InputManager.Keys.South))
            {
                Hide();
                Result = CellSelectResult.CellSelect;
                break;
            }
            else if (input.GetKeyPress(InputManager.Keys.East))
            {
                Hide();
                Result = CellSelectResult.Cancel;
                break;
            }

            if (input.GetKeyPress(InputManager.Keys.Up) ||
                input.GetKeyPress(InputManager.Keys.Down) ||
                input.GetKeyPress(InputManager.Keys.Right) ||
                input.GetKeyPress(InputManager.Keys.Left))
            {
                // 移動
                sound.PlaySE(sound.commonSeMove);

                if (input.GetKeyPress(InputManager.Keys.Up))
                    SelectCell = SelectMoveCell(cellList, SelectCell, InputManager.Keys.Up);
                else if (input.GetKeyPress(InputManager.Keys.Down))
                    SelectCell = SelectMoveCell(cellList, SelectCell, InputManager.Keys.Down);
                else if (input.GetKeyPress(InputManager.Keys.Right))
                    SelectCell = SelectMoveCell(cellList, SelectCell, InputManager.Keys.Right);
                else if (input.GetKeyPress(InputManager.Keys.Left))
                    SelectCell = SelectMoveCell(cellList, SelectCell, InputManager.Keys.Left);

                cursorParam.location = SelectCell;
                ShowCursor(new List<CellUIParam>() { cursorParam });
            }
        }
    }

    #endregion

    #region 表示パラメータクラス

    /// <summary>
    /// 表示指定パラメータ
    /// </summary>
    public class CellUIParam
    {
        /// <summary>座標</summary>
        public Vector2Int location;
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
        public CellUIParam(Vector2Int _location, int _width, int _height, bool _enable)
        {
            location = _location;
            width = _width;
            height = _height;
            enable = _enable;
        }
    }

    #endregion

    #region 機能メソッド

    /// <summary>
    /// 移動先セルを選択
    /// </summary>
    /// <param name="cellList"></param>
    /// <param name="now"></param>
    /// <param name="moveDir"></param>
    /// <returns></returns>
    private Vector2Int SelectMoveCell(List<Vector2Int> cellList, Vector2Int now, InputManager.Keys moveDir)
    {
        if (!cellList.Any()) return now;
        var minAbs = 0;
        var targetLoc = 0;

        if (moveDir == InputManager.Keys.Up)
        {
            // 真上を探す
            var nextList = cellList.Where(c => c.y > now.y && c.x == now.x);
            if (nextList.Any())
            {
                return new Vector2Int(now.x, nextList.Min(c => c.y));
            }

            // 無ければ真下
            nextList = cellList.Where(c => c.y < now.y && c.x == now.x);
            if (nextList.Any())
            {
                return new Vector2Int(now.x, nextList.Min(c => c.y));
            }

            // 無ければ一番近いもの
            nextList = cellList.Where(c => c.y > now.y);
            if (nextList.Any())
            {
                targetLoc = nextList.Min(c => c.y);
                nextList = nextList.Where(c => c.y == targetLoc);
                minAbs = nextList.Min(c => Mathf.Abs(c.x - now.x));
                foreach (var c in nextList)
                {
                    if (Mathf.Abs(c.x - now.x) == minAbs) return c;
                }
            }

            // 無ければ一番下にあるもの
            targetLoc = cellList.Min(c => c.y);
            nextList = cellList.Where(c => c.y == targetLoc);
            minAbs = nextList.Min(c => Mathf.Abs(c.x - now.x));
            foreach (var c in nextList)
            {
                if (Mathf.Abs(c.x - now.x) == minAbs) return c;
            }
        }
        else if (moveDir == InputManager.Keys.Down)
        {
            // 真下を探す
            var nextList = cellList.Where(c => c.y < now.y && c.x == now.x);
            if (nextList.Any())
            {
                return new Vector2Int(now.x, nextList.Max(c => c.y));
            }

            // 無ければ真上
            nextList = cellList.Where(c => c.y > now.y && c.x == now.x);
            if (nextList.Any())
            {
                return new Vector2Int(now.x, nextList.Max(c => c.y));
            }

            // 無ければ一番近いもの
            nextList = cellList.Where(c => c.y < now.y);
            if (nextList.Any())
            {
                targetLoc = nextList.Max(c => c.y);
                nextList = nextList.Where(c => c.y == targetLoc);
                minAbs = nextList.Min(c => Mathf.Abs(c.x - now.x));
                foreach (var c in nextList)
                {
                    if (Mathf.Abs(c.x - now.x) == minAbs) return c;
                }
            }

            // 無ければ一番上にあるもの
            targetLoc = cellList.Max(c => c.y);
            nextList = cellList.Where(c => c.y == targetLoc);
            minAbs = nextList.Min(c => Mathf.Abs(c.x - now.x));
            foreach (var c in nextList)
            {
                if (Mathf.Abs(c.x - now.x) == minAbs) return c;
            }
        }
        else if (moveDir == InputManager.Keys.Right)
        {
            // 真右を探す
            var nextList = cellList.Where(c => c.x > now.x && c.y == now.y);
            if (nextList.Any())
            {
                return new Vector2Int(nextList.Min(c => c.x), now.y);
            }

            // 無ければ真左
            nextList = cellList.Where(c => c.x < now.x && c.y == now.y);
            if (nextList.Any())
            {
                return new Vector2Int(nextList.Min(c => c.x), now.y);
            }

            // 無ければ一番近いもの
            nextList = cellList.Where(c => c.x > now.x);
            if (nextList.Any())
            {
                targetLoc = nextList.Min(c => c.x);
                nextList = nextList.Where(c => c.x == targetLoc);
                minAbs = nextList.Min(c => Mathf.Abs(c.y - now.y));
                foreach (var c in nextList)
                {
                    if (Mathf.Abs(c.y - now.y) == minAbs) return c;
                }
            }

            // 無ければ一番左にあるもの
            targetLoc = cellList.Min(c => c.x);
            nextList = cellList.Where(c => c.x == targetLoc);
            minAbs = nextList.Min(c => Mathf.Abs(c.y - now.y));
            foreach (var c in nextList)
            {
                if (Mathf.Abs(c.y - now.y) == minAbs) return c;
            }
        }
        else if (moveDir == InputManager.Keys.Left)
        {
            // 真左を探す
            var nextList = cellList.Where(c => c.x < now.x && c.y == now.y);
            if (nextList.Any())
            {
                return new Vector2Int(nextList.Max(c => c.x), now.y);
            }

            // 無ければ真右
            nextList = cellList.Where(c => c.x > now.x && c.y == now.y);
            if (nextList.Any())
            {
                return new Vector2Int(nextList.Max(c => c.x), now.y);
            }

            // 無ければ一番近いもの
            nextList = cellList.Where(c => c.x < now.x);
            if (nextList.Any())
            {
                targetLoc = nextList.Max(c => c.x);
                nextList = nextList.Where(c => c.x == targetLoc);
                minAbs = nextList.Min(c => Mathf.Abs(c.y - now.y));
                foreach (var c in nextList)
                {
                    if (Mathf.Abs(c.y - now.y) == minAbs) return c;
                }
            }

            // 無ければ一番左にあるもの
            targetLoc = cellList.Max(c => c.x);
            nextList = cellList.Where(c => c.x == targetLoc);
            minAbs = nextList.Min(c => Mathf.Abs(c.y - now.y));
            foreach (var c in nextList)
            {
                if (Mathf.Abs(c.y - now.y) == minAbs) return c;
            }
        }

        return now;
    }

    #endregion
}
