using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

/// <summary>
/// F101�@�c�^
/// </summary>
public class F101Plant : ActionEventBase
{
    /// <summary>�v���C���[</summary>
    public PlayerScript player;
    /// <summary>����݂����</summary>
    public TukuyomiScript tukuyomi;
    /// <summary>�I</summary>
    public CharacterScript you;
    /// <summary>�����\��</summary>
    public ModelUtil slash;
    /// <summary>�c�^���f��</summary>
    public ModelUtil model;

    /// <summary>��������SE</summary>
    public AudioClip swordCatchSe;
    /// <summary>�؂�SE</summary>
    public AudioClip slashSe;

    public AudioClip voiceNew1;


    /// <summary>
    /// �J�n��
    /// </summary>
    public override void Start()
    {
        base.Start();

        you.gameObject.SetActive(false);
        slash.gameObject.SetActive(false);
        if (Global.GetSaveData().GetGameDataInt(F101System.PLANT_FLG) >= 3)
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
        var sound = manager.soundMan;

        if (Global.GetSaveData().GetGameDataInt(F101System.PLANT_FLG) == 0)
        {
            // ����
            msg.Open();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F101_New1_Reko);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F101_New2_Tukuyomi, voiceNew1);
            yield return msg.WaitForMessageEnd();
            msg.Close();

            Global.GetSaveData().SetGameData(F101System.PLANT_FLG, 1);
        }
        else if (Global.GetSaveData().GetGameDataInt(F101System.PLANT_FLG) == 1)
        {
            // ���łɌ���
            msg.Open();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F101_Check1_Reko);
            yield return msg.WaitForMessageEnd();
            msg.Close();
        }
        else if (Global.GetSaveData().GetGameDataInt(F101System.PLANT_FLG) == 2)
        {
            // �I�؂�
            you.gameObject.SetActive(true);
            var pos2 = fieldScript.SearchGeneralPosition(2);
            var pos3 = fieldScript.SearchGeneralPosition(3);
            yield return null;
            you.WalkTo(pos2.GetPosition());
            player.WalkTo(pos3.GetPosition());
            tukuyomi.SetMode(TukuyomiScript.TukuyomiMode.NPC);
            tukuyomi.WalkTo(pos3.GetPosition() + new Vector3(-80f, 0, 0), afterDir:"right");

            msg.Open();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F101_Slash1_Reko);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.You0, StringFieldMessage.F101_Slash2_You);
            yield return msg.WaitForMessageEnd();
            msg.Close();
            yield return new WaitWhile(() => you.IsWalking());
            you.PlayAnim("up_kamae");
            sound.PlaySE(swordCatchSe);
            yield return new WaitForSeconds(1f);
            manager.FadeOutNoWait();
            slash.gameObject.SetActive(true);
            sound.PlaySE(slashSe);
            yield return new WaitForSeconds(0.1f);
            manager.FadeInNoWait();
            slash.FadeOut(0.6f);
            model.FadeOut(2f);
            yield return new WaitForSeconds(2f);
            you.SetDirection(Constant.Direction.Up);
            yield return new WaitForSeconds(1f);

            msg.Open();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F101_Slash3_Reko);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.You0, StringFieldMessage.F101_Slash4_You);
            you.WalkTo(pos2.GetPosition() + new Vector3(0, -600, 0));
            fieldScript.StartCoroutine(YouWalkWaitCoroutine());
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F101_Slash5_Reko);
            yield return msg.WaitForMessageEnd();
            msg.Close();

            Global.GetSaveData().SetGameData(F101System.PLANT_FLG, 3);
            tukuyomi.SetMode(TukuyomiScript.TukuyomiMode.Trace);
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// �I�����I�����������R���[�`��
    /// </summary>
    /// <returns></returns>
    private IEnumerator YouWalkWaitCoroutine()
    {
        yield return new WaitWhile(() => you.IsWalking());
        you.gameObject.SetActive(false);
    }
}
