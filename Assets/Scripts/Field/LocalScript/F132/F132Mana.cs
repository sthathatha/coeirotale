using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F132MANA
/// </summary>
public class F132Mana : ActionEventBase
{
    /// <summary>�����J�E���g</summary>
    private int loseCount = 0;

    /// <summary>
    /// �J�n��
    /// </summary>
    public override void Start()
    {
        base.Start();

        if (Global.GetSaveData().GetGameDataInt(F132System.MANA_WIN_FLG) == 1)
        {
            // �N���A�ς݂ŋ��Ȃ��Ȃ�
            gameObject.SetActive(false);
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
        if (save.GetGameDataInt(F132System.MANA_MEET_FLG) == 1)
        {
            msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.F132_Retry1_Mana, null);
            yield return msg.WaitForMessageEnd();
        }
        else
        {
            save.SetGameData(F132System.MANA_MEET_FLG, 1);
            msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.F132_New1_Mana, null);
            yield return msg.WaitForMessageEnd();
        }
        msg.Close();

        // �퓬
        Global.GetTemporaryData().loseCount = loseCount;
        manager.StartGame("GameSceneManaA");
        yield return new WaitUntil(() => manager.SceneState == ManagerSceneScript.State.Main);

        msg.Open();
        if (Global.GetTemporaryData().gameWon)
        {
            save.SetGameData(F132System.MANA_WIN_FLG, 1);
            //����
            msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.F132_Win1_Mana, null);
            yield return msg.WaitForMessageEnd();
            msg.Close();
            manager.LoadMainScene("Field004", 4);
        }
        else
        {
            //����
            if (loseCount < 100) loseCount++;

            msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.F132_Lose1_Mana, null);
            yield return msg.WaitForMessageEnd();
            msg.Close();
        }
    }
}
