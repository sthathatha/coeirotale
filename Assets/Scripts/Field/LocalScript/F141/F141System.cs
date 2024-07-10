using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F141�@�����e�B�z�[�����ǂ�
/// </summary>
public class F141System : MainScriptBase
{
    #region �萔

    /// <summary>�t�B�[���h���</summary>
    public enum F141Phase : int
    {
        Init = 0,
        Knocked,
        Cleared,
    }

    /// <summary>���s�񐔃Z�[�u</summary>
    public const string FAIL_COUNT_SAVE = "F141Failed";
    /// <summary>�����t���O</summary>
    public const string CLEAR_FLG = "F141Clear";

    #endregion

    #region �����o�[

    /// <summary>�Q�[�g�Q�̐e</summary>
    public Transform gateParent;

    /// <summary>�N�[</summary>
    public GameObject koob;

    /// <summary>�m�b�N��</summary>
    public AudioClip se_knock;
    /// <summary>�J����</summary>
    public AudioClip se_open;

    #endregion

    #region �ϐ�

    /// <summary>�Q�[�g�Q</summary>
    private List<F141Door> gates = new List<F141Door>();

    private F141Phase phase;
    /// <summary>�t�F�[�Y</summary>
    public F141Phase GetPhase() { return phase; }

    #endregion

    #region ���

    /// <summary>
    /// BGM
    /// </summary>
    /// <returns></returns>
    public override Tuple<SoundManager.FieldBgmType, AudioClip> GetBgm()
    {
        return new Tuple<SoundManager.FieldBgmType, AudioClip>(SoundManager.FieldBgmType.Common1, null);
    }

    /// <summary>
    /// �J�n��
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Start()
    {
        yield return base.Start();

        gates.AddRange(gateParent.GetComponentsInChildren<F141Door>());

        // �N���A���̔�\��
        if (Global.GetSaveData().GetGameDataInt(CLEAR_FLG) >= 1)
        {
            foreach (var g in gates)
            {
                if (gates.IndexOf(g) == 0)
                {
                    g.SetDoorType(F141Door.DoorType.Success);
                    g.Open(true);
                    continue;
                }

                g.gameObject.SetActive(false);
            }
            koob.SetActive(false);
            phase = F141Phase.Cleared;
            yield break;
        }

        phase = F141Phase.Init;
        // ���N���A�̂Ƃ�
        if (Global.GetSaveData().GetGameDataInt(FAIL_COUNT_SAVE) < 3)
        {
            // �R�񎸔s�ŃN�[���o�Ă���
            koob.SetActive(false);
        }

        // �����_���ō��E�ݒ�
        var mirrorIdx = Util.RandomUniqueIntList(0, gates.Count - 1, gates.Count / 2);
        foreach (var i in mirrorIdx)
        {
            gates[i].SetMirror(true);
        }
    }

    #endregion

    #region �p�u���b�N���\�b�h

    /// <summary>
    /// �m�b�N����
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public IEnumerator Knock(F141Door target)
    {
        if (phase == F141Phase.Cleared) { yield break; }
        var sound = ManagerSceneScript.GetInstance().soundMan;

        if (phase == F141Phase.Init)
        {
            // �m�b�N����
            sound.PlaySE(se_knock);
            yield return new WaitForSeconds(1f);

            // �c�����Ɠ�����̔���I��
            var targetIdx = gates.IndexOf(target);
            var uniq = Util.RandomUniqueIntList(0, gates.Count - 2, 2);
            // �c����
            var nokoIndex = uniq[0];
            if (nokoIndex >= targetIdx) nokoIndex++;
            // ������̔�
            var atariIndex = uniq[1];
            if (atariIndex >= targetIdx) atariIndex++;

            sound.PlaySE(se_open);
            // �ݒ�
            for (var i = 0; i < gates.Count; ++i)
            {
                gates[i].SetDoorType(i == atariIndex ? F141Door.DoorType.Success : F141Door.DoorType.Failed);
                if (i != targetIdx && i != nokoIndex)
                {
                    gates[i].Open();
                    yield return new WaitForSeconds(0.2f);
                }
            }

            phase = F141Phase.Knocked;
        }
        else if (phase == F141Phase.Knocked)
        {
            // �I�񂾂̂��J��
            sound.PlaySE(se_open);
            target.Open();

            phase = F141Phase.Cleared;
        }
    }

    #endregion
}
