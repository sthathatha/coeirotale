using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �}�`�Q�[������t�F�[�_
/// </summary>
public class IkusautaGameMiniFader : MonoBehaviour
{
    /// <summary>���쒆</summary>
    private bool _active = false;

    /// <summary>
    /// �F�ݒ�
    /// </summary>
    /// <param name="color"></param>
    private void SetColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
    }

    /// <summary>
    /// ��\��
    /// </summary>
    public void Hide()
    {
        SetColor(new Color(0, 0, 0, 0));
    }

    /// <summary>
    /// �Â�����
    /// </summary>
    /// <param name="_enable">true:�Â�����@false:����</param>
    /// <returns></returns>
    public IEnumerator FadeOutDark(bool _enable = true)
    {
        _active = true;
        var alpha = new DeltaFloat();
        alpha.Set(_enable ? 0 : 0.6f);
        alpha.MoveTo(_enable ? 0.6f : 0, 0.8f, DeltaFloat.MoveType.LINE);

        while (alpha.IsActive())
        {
            SetColor(new Color(0, 0, 0, alpha.Get()));
            alpha.Update(Time.deltaTime);

            yield return null;
        }
        SetColor(new Color(0, 0, 0, alpha.Get()));

        _active = false;
    }

    /// <summary>
    /// ��u�t���b�V��
    /// </summary>
    /// <returns></returns>
    public IEnumerator Flash()
    {
        _active = true;

        var alpha = new DeltaFloat();
        alpha.Set(1);
        alpha.MoveTo(0, 0.06f, DeltaFloat.MoveType.LINE);

        while (alpha.IsActive())
        {
            SetColor(new Color(1, 1, 1, alpha.Get()));
            alpha.Update(Time.deltaTime);

            yield return null;
        }
        SetColor(new Color(1, 1, 1, alpha.Get()));

        _active = false;
    }

    /// <summary>
    /// ���쒆
    /// </summary>
    /// <returns></returns>
    public bool IsActive() { return _active; }
}
