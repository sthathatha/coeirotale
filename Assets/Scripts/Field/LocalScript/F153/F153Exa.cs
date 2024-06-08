using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���t���a�@�G�O�U
/// </summary>
public class F153Exa : ActionEventBase
{
    /// <summary>
    /// �J�n��
    /// </summary>
    public override void Start()
    {
        base.Start();

        if (Global.GetSaveData().GetGameDataInt(F111System.BRIDGE_FLG) < 2)
        {
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

        msg.Open();

        if (Global.GetSaveData().GetGameDataInt(F101System.PLANT_FLG) == 1)
        {
            // �A������
            msg.StartMessage(MessageWindow.Face.Exa0, StringFieldMessage.F153_Exa_Plant1_Exa);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Exa0, StringFieldMessage.F153_Exa_Plant2_Exa);
            yield return msg.WaitForMessageEnd();
        }
        else
        {
            // �ʏ�
            msg.StartMessage(MessageWindow.Face.Exa0, StringFieldMessage.F153_Exa_Def1_Exa);
            yield return msg.WaitForMessageEnd();
        }


        msg.Close();

        yield break;
    }
}
