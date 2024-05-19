using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// �^�C�g�����
/// </summary>
public class TitleSceneSystem : MainScriptBase
{
    #region �����o�[

    /// <summary>Continue�I����</summary>
    public TMP_Text continueSelect;
    /// <summary>�J�[�\��</summary>
    public GameObject cursorUI;
    /// <summary>���̃}�X�N</summary>
    public GameObject starMask;

    #endregion

    #region �ϐ�

    /// <summary>�J�[�\���ʒu</summary>
    private enum TitleSelect : int
    {
        GameStart = 0,
        Continue,
        Option,
    }
    /// <summary>�J�[�\���ʒu</summary>
    private TitleSelect cursor = 0;

    #endregion

    #region ���

    /// <summary>
    /// ������
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Start()
    {
        yield return base.Start();

        if (Global.GetSaveData().IsEnableGameData() == false)
        {
            continueSelect.alpha = 0.2f;
        }
        if (Global.GetSaveData().system.clearFlag != 0)
        {
            starMask.SetActive(false);
        }
    }

    /// <summary>
    /// �t���[������
    /// </summary>
    protected override void Update()
    {
        base.Update();

        if (ManagerSceneScript.GetInstance()?.SceneState != ManagerSceneScript.State.Main) { return; }
        if (FieldState != State.Idle) { return; }

        // �I�v�V�������s���͑���s��
        var optionWindow = ManagerSceneScript.GetInstance().GetOptionWindow();
        if (optionWindow.gameObject.activeSelf) { return; }

        var input = InputManager.GetInstance();
        var saveData = Global.GetSaveData();
        var sound = ManagerSceneScript.GetInstance().soundMan;

        // �J�[�\���㉺
        if (input.GetKeyPress(InputManager.Keys.Up))
        {
            sound.PlaySE(sound.commonSeMove);
            cursor -= 1;
            if (cursor < 0) cursor = TitleSelect.Option;
            UpdateCursor();
        }
        else if (input.GetKeyPress(InputManager.Keys.Down))
        {
            sound.PlaySE(sound.commonSeMove);
            cursor += 1;
            if (cursor > TitleSelect.Option) cursor = TitleSelect.GameStart;
            UpdateCursor();
        }
        else if (input.GetKeyPress(InputManager.Keys.South))
        {
            switch (cursor)
            {
                case TitleSelect.GameStart:
                    // �Q�[���J�n
                    sound.PlaySE(sound.commonSeSelect);
                    Global.GetSaveData().InitGameData();
                    ManagerSceneScript.GetInstance().LoadMainScene("OpeningScene", 0);
                    break;
                case TitleSelect.Continue:
                    if (saveData.IsEnableGameData())
                    {
                        sound.PlaySE(sound.commonSeSelect);
                        // ���[�h
                        var save = Global.GetSaveData();
                        save.LoadGameData();
                        ManagerSceneScript.GetInstance().LoadMainScene(save.GetGameDataString("SaveFieldScene"), save.GetGameDataInt("SaveFieldPos"));
                    }
                    else
                    {
                        sound.PlaySE(sound.commonSeError);
                    }
                    break;
                case TitleSelect.Option:
                    // �I�v�V�����J��
                    StartCoroutine(optionWindow.OpenDialog());
                    break;
            }
        }
    }

    #endregion

    #region �v���C�x�[�g���\�b�h

    /// <summary>
    /// �J�[�\���ʒu�X�V
    /// </summary>
    private void UpdateCursor()
    {
        cursorUI.transform.localPosition = new Vector3(0, -100f * (int)cursor, 0);
    }

    #endregion
}
