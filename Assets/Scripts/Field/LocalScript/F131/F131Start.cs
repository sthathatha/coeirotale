using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F131�J�n�C�x���g
/// </summary>
public class F131Start : EventBase
{
    /// <summary>�E�[��</summary>
    public CharacterScript worra;

    public AudioClip jumpSe;

    public AudioClip voice1;
    public AudioClip voice2;

    /// <summary>
    /// ���s
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        var msg = ManagerSceneScript.GetInstance().GetMessageWindow();
        var youFlg = Global.GetSaveData().GetGameDataInt(F131System.ICE_YOU_FLG);

        if (youFlg == 0)
        {
            msg.Open();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F131_BackNew1_Reko);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F131_BackNew2_Tukuyomi, voice1);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F131_BackNew3_Reko);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F131_BackNew4_Tukuyomi, voice2);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F131_BackNew5_Reko);
            yield return msg.WaitForMessageEnd();
            msg.Close();
            Global.GetSaveData().SetGameData(F131System.ICE_YOU_FLG, 1);
        }
        else if (youFlg == 1)
        {
            msg.Open();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F131_Back1_Reko);
            yield return msg.WaitForMessageEnd();
            msg.Close();
        }
        else if (youFlg == 2)
        {
            msg.Open();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F131_Catch1_Reko);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Worra0, StringFieldMessage.F131_Catch2_Worra);
            yield return msg.WaitForMessageEnd();
            msg.Close();
            StartCoroutine(WorraJumpCoroutine());
        }
    }

    /// <summary>
    /// �E�[�������ł����ď�����
    /// </summary>
    /// <returns></returns>
    private IEnumerator WorraJumpCoroutine()
    {
        ManagerSceneScript.GetInstance().soundMan.PlaySE(jumpSe);
        var pos = worra.transform.position;
        pos.y += 2000f;
        worra.SlideTo(pos);
        yield return new WaitWhile(() => worra.IsWalking());
        worra.gameObject.SetActive(false);
    }
}
