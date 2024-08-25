using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// �G���f�B���O�e�L�X�g
/// </summary>
public class EndingText : MonoBehaviour
{
    private const float FADE_TIME = 1f;
    private TMP_Text txt;

    /// <summary>
    /// ������
    /// </summary>
    private void Start()
    {
        txt = GetComponent<TMP_Text>();
        txt.color = new Color(1, 1, 1, 0);
    }

    /// <summary>
    /// �\��
    /// </summary>
    /// <param name="text"></param>
    public void FadeIn(string text)
    {
        txt.SetText(text);
        StartCoroutine(FadeCoroutine(0f, 1f));
    }

    /// <summary>
    /// ��\��
    /// </summary>
    public void FadeOut()
    {
        StartCoroutine(FadeCoroutine(1f, 0f));
    }

    /// <summary>
    /// �t�F�[�h
    /// </summary>
    /// <param name="a1"></param>
    /// <param name="a2"></param>
    /// <returns></returns>
    private IEnumerator FadeCoroutine(float a1, float a2)
    {
        var a = new DeltaFloat();
        a.Set(a1);
        a.MoveTo(a2, FADE_TIME, DeltaFloat.MoveType.LINE);
        while (a.IsActive())
        {
            yield return null;
            a.Update(Time.deltaTime);
            txt.color = new Color(1, 1, 1, a.Get());
        }
    }
}
