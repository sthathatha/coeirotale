using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �͂��E�������E�B���h�E
/// </summary>
public class DialogWindow : MonoBehaviour
{
    #region �萔
    /// <summary>�͂��E������</summary>
    public enum Result : int
    {
        Yes = 0,
        No,
    }

    /// <summary>�J�[�\���ʒu</summary>
    private const float CURSOR_POS_Y = 28f;
    #endregion

    /// <summary>�J�[�\��</summary>
    public RectTransform cursor = null;

    /// <summary>�I���J�[�\��</summary>
    private Result selectResult;

    #region �g�p���\�b�h
    /// <summary>
    /// �J���R���[�`��
    /// </summary>
    /// <returns></returns>
    public IEnumerator OpenDialog()
    {
        var input = InputManager.GetInstance();
        var sound = ManagerSceneScript.GetInstance().soundManager;

        UpdateCursor(Result.Yes);
        gameObject.SetActive(true);

        yield return null;
        while (!input.GetKeyPress(InputManager.Keys.South))
        {
            yield return null;

            if (input.GetKeyPress(InputManager.Keys.Up) || input.GetKeyPress(InputManager.Keys.Down))
            {
                sound.PlaySE(sound.commonSeMove);
                UpdateCursor(selectResult == Result.Yes ? Result.No : Result.Yes);
            }
        }

        sound.PlaySE(sound.commonSeSelect);
        gameObject.SetActive(false);
    }
    #endregion

    /// <summary>
    /// ���ʂ��擾
    /// </summary>
    /// <returns></returns>
    public Result GetResult() { return selectResult; }

    #region �v���C�x�[�g
    /// <summary>
    /// �J�[�\���ʒu�X�V
    /// </summary>
    /// <param name="result"></param>
    private void UpdateCursor(Result result)
    {
        selectResult = result;

        cursor.localPosition = new Vector3(0, result == Result.Yes ? CURSOR_POS_Y : -CURSOR_POS_Y, 0);
    }
    #endregion
}
