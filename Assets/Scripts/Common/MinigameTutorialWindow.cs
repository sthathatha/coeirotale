using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// �~�j�Q�[���`���[�g���A���p�E�B���h�E
/// </summary>
public class MinigameTutorialWindow : MonoBehaviour
{
    #region �萔
    /// <summary>�t�F�[�h����</summary>
    private const float FADE_TIME = 0.3f;
    #endregion

    #region �����o�ϐ�
    /// <summary>�^�C�g��</summary>
    public TMP_Text titleText;
    /// <summary>����</summary>
    public TMP_Text tutorialText;
    #endregion

    #region �v���C�x�[�g
    /// <summary>�A���t�@</summary>
    private DeltaFloat alpha;
    #endregion

    /// <summary>
    /// �R���X�g���N�^
    /// </summary>
    public MinigameTutorialWindow()
    {
        alpha = new DeltaFloat();
        alpha.Set(0);
    }

    #region �g�p���\�b�h
    /// <summary>
    /// �J��
    /// �t�F�[�h����̂ŊJ���O�Ƀe�L�X�g�ݒ肵�Ă���
    /// </summary>
    /// <returns></returns>
    public IEnumerator Open()
    {
        gameObject.SetActive(true);
        alpha.Set(0);
        alpha.MoveTo(1, FADE_TIME, DeltaFloat.MoveType.LINE);
        while (alpha.IsActive())
        {
            alpha.Update(Time.deltaTime);
            gameObject.GetComponent<CanvasGroup>().alpha = alpha.Get();
            yield return null;
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <returns></returns>
    public IEnumerator Close()
    {
        alpha.Set(1);
        alpha.MoveTo(0, FADE_TIME, DeltaFloat.MoveType.LINE);
        while (alpha.IsActive())
        {
            alpha.Update(Time.deltaTime);
            gameObject.GetComponent<CanvasGroup>().alpha = alpha.Get();
            yield return null;
        }
        gameObject.SetActive(false);
    }

    /// <summary>
    /// �^�C�g���ݒ�
    /// </summary>
    /// <param name="title"></param>
    public void SetTitle(string title) 
    {
        titleText.SetText(title);
    }

    /// <summary>
    /// ���e�ݒ�
    /// </summary>
    /// <param name="text"></param>
    public void SetText(string text)
    {
        tutorialText.SetText(text);
    }
    #endregion
}
