using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F210　クリア前マップ
/// </summary>
public class F210System : MainScriptBase
{
    public const int VOICE_MYCOE = -1;
    public const int VOICE_TUKUYOMI = 0;
    public const int VOICE_AMI = 1;
    public const int VOICE_MENDERU = 2;
    public const int VOICE_MATUKA = 3;
    public const int VOICE_MATI = 4;
    public const int VOICE_PIERRE = 5;
    public const int VOICE_MANA = 6;

    public const string FIRST_EVENT_FLG = "F210FirstShow";

    /// <summary>
    /// 最初フェードイン前
    /// </summary>
    /// <returns></returns>
    public override IEnumerator BeforeInitFadeIn()
    {
        yield return base.BeforeInitFadeIn();

        // つくよみちゃん固定位置
        var tukuyomi = tukuyomiObj.GetComponent<TukuyomiScript>();
        tukuyomi.SetMode(TukuyomiScript.TukuyomiMode.NPC);

        var gp = SearchGeneralPosition(3);
        tukuyomi.SetPosition(gp.GetPosition());
        tukuyomi.SetDirection(Constant.Direction.Down);

        // 最初カメラを出口に
        if (Global.GetSaveData().GetGameDataInt(FIRST_EVENT_FLG) != 1)
        {
            playerObj.GetComponent<PlayerScript>().SetCameraEnable(false);
            var cam = ManagerSceneScript.GetInstance().mainCam;
            var gp1 = SearchGeneralPosition(1);
            cam.SetTargetPos(gp1.GetPosition());
            cam.Immediate();
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

        if (Global.GetSaveData().GetGameDataInt(FIRST_EVENT_FLG) != 1)
        {
            GetComponent<F210Start>().ExecEvent();
        }
    }
}
