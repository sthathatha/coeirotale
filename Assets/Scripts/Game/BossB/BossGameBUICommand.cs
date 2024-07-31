using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// ���X�{�X�{��@�R�}���h�I��UI
/// </summary>
public class BossGameBUICommand : MonoBehaviour
{
    #region �萔

    /// <summary>
    /// �R�}���h�I������
    /// </summary>
    public enum CommandResult
    {
        SkillSelect = 0,
        Cancel,
    }

    #endregion

    #region �����o�[

    public GameObject detailParent;
    public TMP_Text detailText;

    public GameObject commandParent;
    public Transform cursor;
    public TMP_Text cmd1;
    public TMP_Text cmd2;
    public TMP_Text cmd3;
    public TMP_Text cmd4;
    public TMP_Text cmd5;
    public TMP_Text cmd6;

    #endregion

    #region �R�}���h�I��

    /// <summary>����</summary>
    public CommandResult Result { get; private set; }
    /// <summary>�I���X�L��</summary>
    public int SelectSkill { get; private set; }

    #endregion

    #region �@�\

    /// <summary>
    /// ����
    /// </summary>
    public void Close()
    {
        detailParent.SetActive(false);
        commandParent.SetActive(false);
    }

    /// <summary>
    /// �J���đI������
    /// </summary>
    /// <returns></returns>
    public IEnumerator Open()
    {
        detailParent.SetActive(false);
        commandParent.SetActive(false);

        yield break;
    }

    #endregion

    #region �R���[�`��



    #endregion
}
