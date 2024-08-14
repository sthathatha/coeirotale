using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 204　月の中心核
/// </summary>
public class F204System : MainScriptBase
{
    /// <summary>最後の壁開いている</summary>
    public const string WALL_OPEN_FLG = "F204Opened";

    #region メンバー

    public F204IceWall iceWall;

    public F204Characters ami;
    public F204Characters mana;
    public F204Characters menderu;
    public F204Characters mati;
    public F204Characters matuka;
    public F204Characters pierre;

    #endregion

    #region 基底

    /// <summary>
    /// BGM
    /// </summary>
    /// <returns></returns>
    public override Tuple<SoundManager.FieldBgmType, AudioClip> GetBgm()
    {
        return new Tuple<SoundManager.FieldBgmType, AudioClip>(SoundManager.FieldBgmType.Common2, null);
    }

    /// <summary>
    /// 初期化フェードイン前
    /// </summary>
    /// <returns></returns>
    public override IEnumerator BeforeInitFadeIn()
    {
        yield return base.BeforeInitFadeIn();

        tukuyomiObj.SetActive(true);
        var tukuyomi = tukuyomiObj.GetComponent<TukuyomiScript>();
        var pos3 = SearchGeneralPosition(3);

        if (Global.GetSaveData().GetGameDataInt(WALL_OPEN_FLG) == 1)
        {
            // 開いていたら全員集合
            tukuyomi.SetMode(TukuyomiScript.TukuyomiMode.NPC);
            tukuyomi.SetPosition(pos3.GetPosition());
            tukuyomi.SetDirection(Constant.Direction.Down);

            // 壁も開く
            iceWall.SetRight();
        }
        else
        {
            // 開く前はマチのみ
            ami.gameObject.SetActive(false);
            mana.gameObject.SetActive(false);
            menderu.gameObject.SetActive(false);
            matuka.gameObject.SetActive(false);
            pierre.gameObject.SetActive(false);

            iceWall.SetLeft();
        }
    }

    /// <summary>
    /// フェードイン後
    /// </summary>
    /// <param name="init"></param>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn(bool init)
    {
        yield return base.AfterFadeIn(init);

        // ラスボスに負けてきた場合
        if (Global.GetTemporaryData().lastBossLost)
        {
            var msg = ManagerSceneScript.GetInstance().GetMessageWindow();
            msg.Open();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F204_Lose_Reko);
            yield return msg.WaitForMessageEnd();
            msg.Close();
            Global.GetTemporaryData().lastBossLost = false;
        }
    }

    #endregion
}
