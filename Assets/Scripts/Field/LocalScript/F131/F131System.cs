using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// F131　氷のブロックパズル
/// </summary>
public class F131System : MainScriptBase
{
    #region 定数

    /// <summary>ブロッククリア</summary>
    public const string ICE_BLOCK_FLG = "F131IceBlock";

    /// <summary>悠捕まえるフラグ　0:未確認　1:確認　2:ウーラ連れて来る　3:捕獲済み</summary>
    public const string ICE_YOU_FLG = "F131YouCatch";

    /// <summary>最大X</summary>
    protected const int LOC_MAX_X = 10;
    /// <summary>最大Y</summary>
    protected const int LOC_MAX_Y = 9;
    /// <summary>クリア判定Y</summary>
    public const int LOC_GOAL_Y = 4;

    #endregion

    #region メンバー

    /// <summary>悠</summary>
    public GameObject you;
    /// <summary>ウーラ</summary>
    public GameObject worra;

    /// <summary>ブロック</summary>
    public GameObject blockParent;

    /// <summary>穴</summary>
    public GameObject holeObj;

    /// <summary>滑るSE</summary>
    public AudioClip slipSe;
    /// <summary>止まるSE</summary>
    public AudioClip stopSe;

    #endregion

    #region 変数

    /// <summary>ブロックリスト</summary>
    private List<F131Block> blocks = new List<F131Block>();

    #endregion

    #region 基底

    /// <summary>
    /// 開始時
    /// </summary>
    /// <returns></returns>
    public override IEnumerator BeforeInitFadeIn()
    {
        yield return base.BeforeInitFadeIn();

        blocks.AddRange(blockParent.GetComponentsInChildren<F131Block>());

        you.SetActive(false);
        var clearSyori = false;
        var youFlg = Global.GetSaveData().GetGameDataInt(ICE_YOU_FLG);
        if (Global.GetSaveData().GetGameDataInt(ICE_BLOCK_FLG)  >= 1)
        {
            // 悠捕獲前＋右から入った場合は初期状態
            if (youFlg >= 3 || ManagerSceneScript.GetInstance().GetInitId() != 0)
            {
                clearSyori = true;
            }
            else
            {
                // ウーラ連れてきた場合のみ配置
                if (youFlg != 2)
                {
                    worra.SetActive(false);
                }
            }
        }
        else
        {
            worra.SetActive(false);
        }

        if (clearSyori)
        {
            worra.SetActive(false);
            foreach (var b in blocks)
            {
                var clear = b as F131ClearBlock;
                if (clear != null)
                {
                    clear.SetClearHole();
                    break;
                }
            }
            holeObj.SetActive(false);
        }
    }

    /// <summary>
    /// フェードイン後
    /// </summary>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn()
    {
        yield return base.AfterFadeIn();

        if (Global.GetSaveData().GetGameDataInt(ICE_BLOCK_FLG) >= 1)
        {
            if (Global.GetSaveData().GetGameDataInt(ICE_YOU_FLG) < 3 &&
                ManagerSceneScript.GetInstance().GetInitId() == 0)
            {
                GetComponent<F131Start>().ExecEvent();
            }
        }
    }

    /// <summary>
    /// BGM
    /// </summary>
    /// <returns></returns>
    public override Tuple<SoundManager.FieldBgmType, AudioClip> GetBgm()
    {
        return new Tuple<SoundManager.FieldBgmType, AudioClip>(SoundManager.FieldBgmType.Common1, null);
    }

    #endregion

    #region パブリック

    /// <summary>
    /// 滑る先の座標を判定
    /// </summary>
    /// <param name="before"></param>
    /// <param name="dir"></param>
    /// <returns></returns>
    public Vector2Int CheckSlipLocation(Vector2Int before, Constant.Direction dir)
    {
        // ぶつかる石を検索
        F131Block hitBlock = null;
        foreach (var block in blocks)
        {
            var l = block.GetLocation();
            switch (dir)
            {
                case Constant.Direction.Up:
                    if (l.x == before.x && l.y > before.y &&
                        (hitBlock == null || hitBlock.GetLocation().y > l.y))
                    {
                        hitBlock = block;
                    }
                    break;
                case Constant.Direction.Down:
                    if (l.x == before.x && l.y < before.y &&
                        (hitBlock == null || hitBlock.GetLocation().y < l.y))
                    {
                        hitBlock = block;
                    }
                    break;
                case Constant.Direction.Right:
                    if (l.y == before.y && l.x > before.x &&
                        (hitBlock == null || hitBlock.GetLocation().x > l.x))
                    {
                        hitBlock = block;
                    }
                    break;
                case Constant.Direction.Left:
                    if (l.y == before.y && l.x < before.x &&
                        (hitBlock == null || hitBlock.GetLocation().x < l.x))
                    {
                        hitBlock = block;
                    }
                    break;
            }
        }

        var after = before;
        if (hitBlock == null)
        {
            // ぶつかる石が無い場合端まで
            if (dir == Constant.Direction.Left && before.y == LOC_GOAL_Y)
            {
                after.x = -2;
            }
            else
            {
                switch (dir)
                {
                    case Constant.Direction.Up: after.y = LOC_MAX_Y; break;
                    case Constant.Direction.Down: after.y = 0; break;
                    case Constant.Direction.Right: after.x = LOC_MAX_X; break;
                    case Constant.Direction.Left: after.x = 0; break;
                }
            }
        }
        else
        {
            // ぶつかる石の1個前
            var l = hitBlock.GetLocation();
            switch (dir)
            {
                case Constant.Direction.Up: after.y = l.y - 1; break;
                case Constant.Direction.Down: after.y = l.y + 1; break;
                case Constant.Direction.Right: after.x = l.x - 1; break;
                case Constant.Direction.Left: after.x = l.x + 1; break;
            }
        }

        return after;
    }

    #endregion

    #region プライベート



    #endregion
}
