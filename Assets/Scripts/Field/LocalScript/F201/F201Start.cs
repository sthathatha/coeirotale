using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// F201�@���X�g�_���W���������j��C�x���g
/// </summary>
public class F201Start : EventBase
{
    #region �����o�[

    public CharacterScript ami;
    public CharacterScript mati;
    public CharacterScript menderu;
    public CharacterScript matuka;
    public CharacterScript mana;
    public CharacterScript pierre;

    public CharacterScript drows;
    public CharacterScript eraps;
    public CharacterScript exa;
    public CharacterScript worra;
    public CharacterScript koob;
    public CharacterScript you;

    public AudioClip se_drows_hit;
    public AudioClip se_drows_berserk;
    public AudioClip se_drows_roll;
    public AudioClip se_eraps_catch;

    #endregion

    /// <summary>
    /// ���s
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        var manager = ManagerSceneScript.GetInstance();
        var msg = manager.GetMessageWindow();
        var cam = manager.mainCam;
        var player = fieldScript.playerObj.GetComponent<PlayerScript>();
        var tukuyomi = fieldScript.tukuyomiObj.GetComponent<TukuyomiScript>();
        var system = fieldScript as F201System;
        var sound = manager.soundMan;

        var posBottom = fieldScript.SearchGeneralPosition(0);
        var posUp = fieldScript.SearchGeneralPosition(1);
        var posWall = fieldScript.SearchGeneralPosition(3);
        var posCenter = fieldScript.SearchGeneralPosition(6);
        var posRoll = fieldScript.SearchGeneralPosition(7);

        // �����܂ŕ���
        var pp = player.transform.position;
        var tp = tukuyomi.transform.position;
        yield return new WaitWhile(() => player.IsWalking());

        // ��b�P
        msg.Open();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F201_Break1_1_Reko);
        yield return msg.WaitForMessageEnd();

        // �h���V�[�o��E��b�Q
        cam.SetTargetPos(drows.gameObject);
        drows.gameObject.SetActive(true);
        drows.WalkTo(posUp.GetPosition());
        drows.SetCameraEnable(true);
        msg.StartMessage(MessageWindow.Face.Drows0, StringFieldMessage.F201_Break2_1_Drows);
        yield return msg.WaitForMessageEnd();
        if (drows.IsWalking())
        {
            msg.Close();
            yield return new WaitWhile(() => drows.IsWalking());
            msg.Open();
        }
        msg.StartMessage(MessageWindow.Face.Drows0, StringFieldMessage.F201_Break2_2_Drows);
        yield return msg.WaitForMessageEnd();
        msg.Close();

        // �j��
        drows.SetCameraEnable(false);
        drows.PlayAnim("up_kamae");
        yield return new WaitForSeconds(0.5f);
        drows.SlideTo(posWall.GetPosition(), 3f);
        yield return new WaitForSeconds(0.3f);
        sound.PlaySE(se_drows_hit);
        // �z���C�g�A�E�g
        yield return manager.FadeOut(2f, Color.white);
        system.towerBefore.SetActive(false);
        system.towerAfter.SetActive(true);
        drows.SetPosition(posRoll.GetPosition());
        yield return new WaitForSeconds(1.5f);
        yield return manager.FadeIn(2f);
        yield return new WaitForSeconds(1f);

        // �h���E��b�R
        var shake = DrowsShake();

        msg.Open();
        msg.StartMessage(MessageWindow.Face.Mati0, StringFieldMessage.F201_Break3_1_Mati);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Drows0, StringFieldMessage.F201_Break3_2_Drows);
        StartCoroutine(shake);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.F201_Break3_3_Mana);
        yield return msg.WaitForMessageEnd();

        // �G�O�U�@�E�[���@�N�[�@�I�o��E��b�S
        cam.SetTargetPos(posBottom.gameObject);
        var plusY = new Vector3(0, posBottom.GetPosition().y - koob.transform.position.y, 0);
        koob.gameObject.SetActive(true);
        koob.WalkTo(koob.transform.position + plusY);
        msg.StartMessage(MessageWindow.Face.Koob0, StringFieldMessage.F201_Break4_1_Koob);
        yield return msg.WaitForMessageEnd();
        exa.gameObject.SetActive(true);
        exa.WalkTo(exa.transform.position + plusY);
        msg.StartMessage(MessageWindow.Face.Exa0, StringFieldMessage.F201_Break4_2_Exa);
        yield return msg.WaitForMessageEnd();
        worra.gameObject.SetActive(true);
        worra.WalkTo(worra.transform.position + plusY);
        msg.StartMessage(MessageWindow.Face.Worra0, StringFieldMessage.F201_Break4_3_Worra);
        yield return msg.WaitForMessageEnd();
        StopCoroutine(shake);
        you.gameObject.SetActive(true);
        you.WalkTo(you.transform.position + plusY);
        msg.StartMessage(MessageWindow.Face.You0, StringFieldMessage.F201_Break4_4_You);
        yield return msg.WaitForMessageEnd();

        // �]����E�G���o��E��b�T
        msg.StartMessage(MessageWindow.Face.Drows0, StringFieldMessage.F201_Break5_1_Drows);
        yield return msg.WaitForMessageEnd();
        msg.Close();
        var rollSe = sound.PlaySELoop(se_drows_roll);
        drows.SetCameraEnable(true);
        drows.WalkTo(posBottom.GetPosition(), 2f);
        yield return new WaitForSeconds(0.5f);
        eraps.gameObject.SetActive(true);
        eraps.WalkTo(eraps.transform.position + plusY);
        yield return new WaitWhile(() => drows.IsWalking());
        sound.StopLoopSE(rollSe);
        sound.PlaySE(se_eraps_catch);
        cam.PlayShakeOne(Shaker.ShakeSize.Weak, 0f);
        yield return new WaitForSeconds(1f);
        drows.SetCameraEnable(false);

        // ��b�U
        msg.Open();
        msg.StartMessage(MessageWindow.Face.Eraps0, StringFieldMessage.F201_Break6_1_Eraps);
        yield return msg.WaitForMessageEnd();
        msg.Close();
        cam.SetTargetPos(posCenter.GetPosition());

        // �S������
        mati.WalkTo(posRoll.GetPosition());
        yield return new WaitForSeconds(0.2f);
        matuka.WalkTo(posRoll.GetPosition());
        yield return new WaitForSeconds(0.2f);
        menderu.WalkTo(posRoll.GetPosition());
        yield return new WaitForSeconds(0.2f);
        pierre.WalkTo(posRoll.GetPosition());
        yield return new WaitForSeconds(0.2f);
        ami.WalkTo(posRoll.GetPosition());
        yield return new WaitForSeconds(0.2f);
        mana.WalkTo(posRoll.GetPosition());

        msg.Open();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F201_Break6_2_Tukuyomi);
        yield return msg.WaitForMessageEnd();
        msg.Close();
        player.WalkTo(posRoll.GetPosition());
        tukuyomi.WalkTo(posRoll.GetPosition());
        yield return new WaitForSeconds(2f);

        // ��b�V
        msg.Open();
        msg.StartMessage(MessageWindow.Face.Drows0, StringFieldMessage.F201_Break7_1_Drows);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Exa0, StringFieldMessage.F201_Break7_2_Exa);
        yield return msg.WaitForMessageEnd();
        msg.Close();

        // �}�b�v�ړ�
        Global.GetSaveData().SetGameData(F201System.DUNGEON_OPEN_FLG, 1);
        manager.LoadMainScene("Field007", 0);//todo:Field202
    }

    /// <summary>
    /// �J��Ԃ��h�炷
    /// </summary>
    /// <returns></returns>
    private IEnumerator DrowsShake()
    {
        var sound = ManagerSceneScript.GetInstance().soundMan;
        var cam = ManagerSceneScript.GetInstance().mainCam;
        while (true)
        {
            sound.PlaySE(se_drows_berserk);
            cam.PlayShakeOne(Shaker.ShakeSize.Strong, 0f);
            yield return new WaitForSeconds(1.5f);
        }
    }
}
