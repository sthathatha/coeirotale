using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F203 ラストフロアキャラクター
/// </summary>
public class F203Characters : SimpleMessageEvent
{
    public int characterId;
    public AudioClip voice1;

    /// <summary>
    /// 開始時
    /// </summary>
    public override void Start()
    {
        base.Start();

        // 最後の壁を開くと消える
        if (Global.GetSaveData().GetGameDataInt(F204System.WALL_OPEN_FLG) >= 1)
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 会話
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    protected override bool ShowMessage(MessageWindow msg, int index)
    {
        if (index == 0)
        {
            switch (characterId)
            {
                case 0: // アミ
                    msg.StartMessage(MessageWindow.Face.Ami0, StringFieldMessage.F203_Ami_1, voice1);
                    break;
                case 1: // MANA
                    msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.F203_Mana_1, voice1);
                    break;
            }
            return true;
        }

        return false;
    }
}
