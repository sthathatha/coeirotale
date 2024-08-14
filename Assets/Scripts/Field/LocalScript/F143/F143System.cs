using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F143　教会内部
/// </summary>
public class F143System : MainScriptBase
{
    /// <summary>メンデルにあったフラグ</summary>
    public const string MENDERU_MEET_FLG = "BossMenderuMeet";
    /// <summary>メンデルに勝ったフラグ</summary>
    public const string MENDERU_WIN_FLG = "BossMenderuWin";

    /// <summary>開始イベント見たフラグ</summary>
    public const string F143_SHOW_FLG = "F143Show";

    public CharacterScript koob;
    public CharacterScript menderu;

    /// <summary>
    /// 開始時
    /// </summary>
    /// <returns></returns>
    public override IEnumerator BeforeInitFadeIn()
    {
        yield return base.BeforeInitFadeIn();

        if (Global.GetSaveData().GetGameDataInt(MENDERU_WIN_FLG) >= 1)
        {
            menderu.gameObject.SetActive(false);
        }

        // 3回以上ならクー居ない
        if (Global.GetSaveData().GetGameDataInt(F143_SHOW_FLG) >= 1 ||
            Global.GetSaveData().GetGameDataInt(F141System.FAIL_COUNT_SAVE) >= 3)
        {
            Global.GetSaveData().SetGameData(F143_SHOW_FLG, 1);

            koob.gameObject.SetActive(false);
        }
        else
        {
            InitPlayerPos(1);
        }
    }

    /// <summary>
    /// フェードイン後
    /// </summary>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn(bool init)
    {
        yield return base.AfterFadeIn(init);

        // 3回以下ならクーイベント開始
        if (Global.GetSaveData().GetGameDataInt(F143_SHOW_FLG) <= 0 &&
            Global.GetSaveData().GetGameDataInt(F141System.FAIL_COUNT_SAVE) < 3)
        {
            GetComponent<F143Start>().ExecEvent();
        }
    }
}
