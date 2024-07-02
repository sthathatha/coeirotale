using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F153���t���a�@�N�[
/// </summary>
public class F153Koob : ActionEventBase
{
    private List<int> helpList = new List<int>();
    private int helpIndex;

    /// <summary>
    /// �J�n��
    /// </summary>
    public override void Start()
    {
        base.Start();
        var save = Global.GetSaveData();

        if (save.GetGameDataInt(F143System.F143_SHOW_FLG) < 1 ||
            Global.GetSaveData().GetGameDataInt(F201System.DUNGEON_OPEN_FLG) >= 1)
        {
            gameObject.SetActive(false);
            return;
        }

        // �T�[�J�X�̌�
        if (save.GetGameDataInt(F121System.KEY_FLG) == 1)
        {
            helpList.Add(1);
        }

        // �X�̗I
        if (save.GetGameDataInt(F131System.ICE_YOU_FLG) == 1)
        {
            helpList.Add(2);
        }

        // �A��
        if (save.GetGameDataInt(F101System.PLANT_FLG) == 1)
        {
            helpList.Add(3);
        }

        helpIndex = 0;
    }

    /// <summary>
    /// ���s
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        var manager = ManagerSceneScript.GetInstance();
        var msg = manager.GetMessageWindow();
        var dlg = manager.GetDialogWindow();

        if (helpList.Count == 0)
        {
            // �w���v�K�v�Ȃ̂�����
            msg.Open();
            msg.StartMessage(MessageWindow.Face.Koob0, StringFieldMessage.F153_Koob_Def1_Koob);
            yield return msg.WaitForMessageEnd();
            msg.Close();
            yield break;
        }

        // ����
        msg.Open();
        msg.StartMessage(MessageWindow.Face.Koob0, StringFieldMessage.F153_Koob_HelpQ_Koob);
        yield return msg.WaitForMessageEnd();
        yield return dlg.OpenDialog();
        if (dlg.GetResult() == DialogWindow.Result.No)
        {
            msg.Close();
            yield break;
        }

        // �w���v�\��
        if (helpList[helpIndex] == 1)
        {
            // �T�[�J�X�̌�
            msg.StartMessage(MessageWindow.Face.Koob0, StringFieldMessage.F153_Koob_Key1_Koob);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Koob0, StringFieldMessage.F153_Koob_Key2_Koob);
            yield return msg.WaitForMessageEnd();
        }
        else if (helpList[helpIndex] == 2)
        {
            // �X�̗I
            msg.StartMessage(MessageWindow.Face.Koob0, StringFieldMessage.F153_Koob_You1_Koob);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Koob0, StringFieldMessage.F153_Koob_You2_Koob);
            yield return msg.WaitForMessageEnd();
        }
        else if (helpList[helpIndex] == 3)
        {
            // �A��
            msg.StartMessage(MessageWindow.Face.Koob0, StringFieldMessage.F153_Koob_Plant1_Koob);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Koob0, StringFieldMessage.F153_Koob_Plant2_Koob);
            yield return msg.WaitForMessageEnd();
            if (Global.GetSaveData().GetGameDataInt(F131System.ICE_YOU_FLG) < 3)
            {
                msg.StartMessage(MessageWindow.Face.Koob0, StringFieldMessage.F153_Koob_PlantEx3_Koob);
                yield return msg.WaitForMessageEnd();
            }
        }
        msg.Close();

        helpIndex++;
        if (helpIndex >= helpList.Count) helpIndex = 0;
    }
}
