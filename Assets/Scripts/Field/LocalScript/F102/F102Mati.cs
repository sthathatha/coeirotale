using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F102�@�}�`
/// </summary>
public class F102Mati : ActionEventBase
{
    /// <summary>�����J�E���g</summary>
    private int loseCount = 0;

    /// <summary>
    /// �J�n��
    /// </summary>
    public override void Start()
    {
        base.Start();

        if (Global.GetSaveData().GetGameDataInt(F102System.MATI_WIN_FLG) == 1)
        {
            // �N���A�ς݂ŋ��Ȃ��Ȃ�
            gameObject.SetActive(false);
        }
        else if (Global.GetSaveData().GetGameDataInt(F102System.MATI_MEET_FLG) == 1)
        {
            GetComponent<CharacterScript>().PlayAnim("down");
        } else
        {
            GetComponent<CharacterScript>().PlayAnim("up");
        }
    }

    /// <summary>
    /// ���s
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        var manager = ManagerSceneScript.GetInstance();
        var msg = manager.GetMessageWindow();
        var save = Global.GetSaveData();

        msg.Open();
        // �J�n��b
        if (save.GetGameDataInt(F102System.MATI_MEET_FLG) == 1)
        {
            msg.StartMessage(MessageWindow.Face.Mati0, StringFieldMessage.F102_Retry1_Mati, null);
            yield return msg.WaitForMessageEnd();
        }
        else
        {
            GetComponent<CharacterScript>().PlayAnim("down");
            save.SetGameData(F102System.MATI_MEET_FLG, 1);
            msg.StartMessage(MessageWindow.Face.Mati0, StringFieldMessage.F102_New1_Mati, null);
            yield return msg.WaitForMessageEnd();
        }
        msg.Close();

        // �퓬
        Global.GetTemporaryData().loseCount = loseCount;
        manager.StartGame("GameSceneIkusautaA");
        yield return new WaitUntil(() => manager.SceneState == ManagerSceneScript.State.Main);

        msg.Open();
        if (Global.GetTemporaryData().gameWon)
        {
            save.SetGameData(F102System.MATI_WIN_FLG, 1);
            //����
            msg.StartMessage(MessageWindow.Face.Mati0, StringFieldMessage.F102_Win1_Mati, null);
            yield return msg.WaitForMessageEnd();
            msg.Close();
            manager.LoadMainScene("Field004", 4);
        }
        else
        {
            //����
            if (loseCount < 100) loseCount++;

            msg.StartMessage(MessageWindow.Face.Mati0, StringFieldMessage.F102_Lose1_Mati, null);
            yield return msg.WaitForMessageEnd();
            msg.Close();
        }
    }
}
