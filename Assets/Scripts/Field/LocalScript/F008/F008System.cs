using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F008
/// </summary>
public class F008System : MainScriptBase
{
    #region 定数

    /// <summary>バリア解除イベント　0:まだ　1:解除済み</summary>
    public const string BARRIER_PHASE = "F008Barrier";

    #endregion

    #region メンバー

    public GameObject barrier;
    public GameObject DrowsA;
    public GameObject DrowsB;
    public CharacterScript Exa;
    public CharacterScript Worra;
    public CharacterScript Koob;
    public CharacterScript You;
    public CharacterScript Ami;
    public CharacterScript Mati;
    public CharacterScript Matuka;
    public CharacterScript Pierre;
    public CharacterScript Mana;
    public CharacterScript Menderu;

    #endregion

    #region 基底

    /// <summary>
    /// BGM
    /// </summary>
    /// <returns></returns>
    public override Tuple<SoundManager.FieldBgmType, AudioClip> GetBgm()
    {
        return new Tuple<SoundManager.FieldBgmType, AudioClip>(SoundManager.FieldBgmType.Common1, null);
    }

    /// <summary>
    /// フェードイン直前処理
    /// </summary>
    /// <returns></returns>
    public override IEnumerator BeforeInitFadeIn()
    {
        yield return base.BeforeInitFadeIn();

        var save = Global.GetSaveData();

        if (save.GetGameDataInt("Tutorial") < 6)
        {
            var tukuyomi = tukuyomiObj.GetComponent<TukuyomiScript>();
            tukuyomi.SetMode(TukuyomiScript.TukuyomiMode.NPC);

            var pos = SearchGeneralPosition(2);
            tukuyomi.SetPosition(pos.GetPosition() + new Vector3(100, 0, 0));
            tukuyomi.SetDirection(Constant.Direction.Up);
        }

        if (save.GetGameDataInt(BARRIER_PHASE) == 0)
        {
            // まだ消えてない場合各ボスのクリアを見る
            var matiWin = save.GetGameDataInt(F102System.MATI_WIN_FLG) == 1;
            var matukaWin = save.GetGameDataInt(F112System.MATUKA_WIN_FLG) == 1;
            var pierreWin = save.GetGameDataInt(F122System.F122_PIERRE_PHASE) >= 2;
            var manaWin = save.GetGameDataInt(F132System.MANA_WIN_FLG) == 1;
            var menderuWin = save.GetGameDataInt(F143System.MENDERU_WIN_FLG) == 1;
            var amiWin = save.GetGameDataInt(F153System.AMI_WIN_FLG) == 1;

            Ami.PlayAnim("down");
            Mati.PlayAnim("down");
            Matuka.PlayAnim("down");
            Pierre.PlayAnim("down");
            Mana.PlayAnim("down");
            Menderu.PlayAnim("down");
            Ami.gameObject.SetActive(amiWin);
            Mati.gameObject.SetActive(matiWin);
            Matuka.gameObject.SetActive(matukaWin);
            Pierre.gameObject.SetActive(pierreWin);
            Mana.gameObject.gameObject.SetActive(manaWin);
            Menderu.gameObject.SetActive(menderuWin);

            if (amiWin && matiWin && matukaWin && pierreWin && manaWin && menderuWin)
            {
                DrowsA.SetActive(false);
                DrowsB.SetActive(true);
                Exa.gameObject.SetActive(true);
                Worra.gameObject.SetActive(true);
                Koob.gameObject.SetActive(true);
                You.gameObject.SetActive(true);
                Exa.PlayAnim("down");
                Worra.PlayAnim("down");
                Koob.PlayAnim("up");
                You.PlayAnim("up");
            }
            else
            {
                DrowsA.SetActive(true);
                DrowsB.SetActive(false);
                Exa.gameObject.SetActive(false);
                Worra.gameObject.SetActive(false);
                Koob.gameObject.SetActive(false);
                You.gameObject.SetActive(false);
            }
        }
        else
        {
            // イベント終了後は全部居ない
            barrier.SetActive(false);
            DrowsA.SetActive(false);
            DrowsB.SetActive(false);
            Exa.gameObject.SetActive(false);
            Worra.gameObject.SetActive(false);
            Koob.gameObject.SetActive(false);
            You.gameObject.SetActive(false);

            Ami.gameObject.SetActive(false);
            Mati.gameObject.SetActive(false);
            Matuka.gameObject.SetActive(false);
            Pierre.gameObject.SetActive(false);
            Mana.gameObject.SetActive(false);
            Menderu.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// フェードイン後処理
    /// </summary>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn(bool init)
    {
        yield return base.AfterFadeIn(init);

        if (Global.GetSaveData().GetGameDataInt("Tutorial") < 6)
        {
            Global.GetSaveData().SetGameData("Tutorial", 1000);

            var ev = GetComponent<F008Tutorial>();
            ev.ExecEvent();
        }
    }

    #endregion
}
