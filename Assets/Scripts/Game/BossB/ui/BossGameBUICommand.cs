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

    /// <summary>��ԏ�̂Ƃ���Y</summary>
    private const float CURSOR_TOP_Y = 140f;
    /// <summary>�I�����P���Ƃ�Y</summary>
    private const float CURSOR_INTERVAL_Y = 56f;
    /// <summary>�P��ʂ̕\������</summary>
    private const int DISP_COUNT = 6;

    #endregion

    #region �����o�[

    public GameObject detailParent;
    public TMP_Text detailText;

    public GameObject commandParent;
    public RectTransform cursor;
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
    public BossGameBDataBase.SkillID SelectSkill { get; private set; }

    /// <summary>�\������X�L��</summary>
    public List<BossGameBDataBase.SkillID> SkillList { get; set; }

    #endregion

    #region �ϐ�

    private int topIndex;
    private int selectIndex;

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
    /// <param name="reset">�I��������</param>
    /// <returns></returns>
    public IEnumerator Open(bool reset)
    {
        var input = InputManager.GetInstance();
        var sound = ManagerSceneScript.GetInstance().soundMan;

        if (reset)
        {
            topIndex = 0;
            selectIndex = 0;

            UpdateDisplay();
        }

        detailParent.SetActive(true);
        commandParent.SetActive(true);

        while (true)
        {
            yield return null;
            if (input.GetKeyPress(InputManager.Keys.East))
            {
                // �L�����Z��
                Result = CommandResult.Cancel;
                break;
            }
            else if (input.GetKeyPress(InputManager.Keys.South))
            {
                // �I��
                Result = CommandResult.SkillSelect;
                SelectSkill = SkillList[selectIndex];
                break;
            }

            if (input.GetKeyPress(InputManager.Keys.Up))
            {
                sound.PlaySE(sound.commonSeMove);
                // ���
                selectIndex--;
                if (selectIndex < 0)
                {
                    // ��̒[���牺��
                    selectIndex = SkillList.Count - 1;
                    topIndex = SkillList.Count - DISP_COUNT;
                    if (topIndex < 0) topIndex = 0;
                }
                else if (selectIndex <= topIndex)
                {
                    topIndex = selectIndex - 1;
                    if (topIndex < 0) topIndex = 0;
                }
                UpdateDisplay();
            }
            else if (input.GetKeyPress(InputManager.Keys.Down))
            {
                sound.PlaySE(sound.commonSeMove);
                // ����
                selectIndex++;
                if (selectIndex >= SkillList.Count)
                {
                    // ���̒[����g�b�v��
                    topIndex = 0;
                    selectIndex = 0;
                }
                else if (selectIndex >= topIndex + DISP_COUNT - 1)
                {
                    // ���̒[�ɍs�����Ƃ���ƃX�N���[��
                    topIndex = selectIndex - 4;
                    if (topIndex < 0) topIndex = 0;
                    else if (topIndex >= SkillList.Count - DISP_COUNT)
                        topIndex = SkillList.Count - DISP_COUNT;
                }
                UpdateDisplay();
            }
        }

        Close();
    }

    #endregion

    #region �������\�b�h

    /// <summary>
    /// ���݂̑I����Ԃ�\���X�V����
    /// </summary>
    private void UpdateDisplay()
    {
        // ���X�g
        cmd1.SetText(BossGameBDataBase.SkillList[SkillList[topIndex]].Name);
        cmd2.SetText(BossGameBDataBase.SkillList[SkillList[topIndex + 1]].Name);
        cmd3.SetText(BossGameBDataBase.SkillList[SkillList[topIndex + 2]].Name);
        cmd4.SetText(BossGameBDataBase.SkillList[SkillList[topIndex + 3]].Name);
        cmd5.SetText(BossGameBDataBase.SkillList[SkillList[topIndex + 4]].Name);
        cmd6.SetText(BossGameBDataBase.SkillList[SkillList[topIndex + 5]].Name);

        // �J�[�\��
        var cursorIndex = selectIndex - topIndex;
        var cursorY = CURSOR_TOP_Y - (cursorIndex * CURSOR_INTERVAL_Y);
        cursor.anchoredPosition = new Vector2(cursor.anchoredPosition.x, cursorY);

        // �ڍו�
        detailText.SetText(BossGameBDataBase.SkillList[SkillList[selectIndex]].Detail);
    }

    #endregion
}
