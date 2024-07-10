using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F141　モンティホールもどき
/// </summary>
public class F141System : MainScriptBase
{
    #region 定数

    /// <summary>フィールド状態</summary>
    public enum F141Phase : int
    {
        Init = 0,
        Knocked,
        Cleared,
    }

    /// <summary>失敗回数セーブ</summary>
    public const string FAIL_COUNT_SAVE = "F141Failed";
    /// <summary>成功フラグ</summary>
    public const string CLEAR_FLG = "F141Clear";

    #endregion

    #region メンバー

    /// <summary>ゲート群の親</summary>
    public Transform gateParent;

    /// <summary>クー</summary>
    public GameObject koob;

    /// <summary>ノック音</summary>
    public AudioClip se_knock;
    /// <summary>開く音</summary>
    public AudioClip se_open;

    #endregion

    #region 変数

    /// <summary>ゲート群</summary>
    private List<F141Door> gates = new List<F141Door>();

    private F141Phase phase;
    /// <summary>フェーズ</summary>
    public F141Phase GetPhase() { return phase; }

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
    /// 開始時
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Start()
    {
        yield return base.Start();

        gates.AddRange(gateParent.GetComponentsInChildren<F141Door>());

        // クリア時の非表示
        if (Global.GetSaveData().GetGameDataInt(CLEAR_FLG) >= 1)
        {
            foreach (var g in gates)
            {
                if (gates.IndexOf(g) == 0)
                {
                    g.SetDoorType(F141Door.DoorType.Success);
                    g.Open(true);
                    continue;
                }

                g.gameObject.SetActive(false);
            }
            koob.SetActive(false);
            phase = F141Phase.Cleared;
            yield break;
        }

        phase = F141Phase.Init;
        // 未クリアのとき
        if (Global.GetSaveData().GetGameDataInt(FAIL_COUNT_SAVE) < 3)
        {
            // ３回失敗でクーが出てくる
            koob.SetActive(false);
        }

        // ランダムで左右設定
        var mirrorIdx = Util.RandomUniqueIntList(0, gates.Count - 1, gates.Count / 2);
        foreach (var i in mirrorIdx)
        {
            gates[i].SetMirror(true);
        }
    }

    #endregion

    #region パブリックメソッド

    /// <summary>
    /// ノックする
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public IEnumerator Knock(F141Door target)
    {
        if (phase == F141Phase.Cleared) { yield break; }
        var sound = ManagerSceneScript.GetInstance().soundMan;

        if (phase == F141Phase.Init)
        {
            // ノックする
            sound.PlaySE(se_knock);
            yield return new WaitForSeconds(1f);

            // 残す扉と当たりの扉を選択
            var targetIdx = gates.IndexOf(target);
            var uniq = Util.RandomUniqueIntList(0, gates.Count - 2, 2);
            // 残す扉
            var nokoIndex = uniq[0];
            if (nokoIndex >= targetIdx) nokoIndex++;
            // あたりの扉
            var atariIndex = uniq[1];
            if (atariIndex >= targetIdx) atariIndex++;

            sound.PlaySE(se_open);
            // 設定
            for (var i = 0; i < gates.Count; ++i)
            {
                gates[i].SetDoorType(i == atariIndex ? F141Door.DoorType.Success : F141Door.DoorType.Failed);
                if (i != targetIdx && i != nokoIndex)
                {
                    gates[i].Open();
                    yield return new WaitForSeconds(0.2f);
                }
            }

            phase = F141Phase.Knocked;
        }
        else if (phase == F141Phase.Knocked)
        {
            // 選んだのを開く
            sound.PlaySE(se_open);
            target.Open();

            phase = F141Phase.Cleared;
        }
    }

    #endregion
}
