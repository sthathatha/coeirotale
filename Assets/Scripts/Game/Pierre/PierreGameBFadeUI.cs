using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// �{�X���b�V���s�G�[���@�t�F�[�hUI
/// </summary>
public class PierreGameBFadeUI : MonoBehaviour
{
    public TMP_Text text;

    /// <summary>
    /// �t�F�[�h�C��
    /// </summary>
    /// <param name="time"></param>
    public IEnumerator Show(string txt, float time = -1f)
    {
        text.SetText(txt);

        var canvas = GetComponent<ModelUtil>();
        if (time <= 0f)
        {
            canvas.FadeIn(0f);
            yield break;
        }

        canvas.FadeIn(time);
        yield return new WaitWhile(() => canvas.IsFading());
    }

    /// <summary>
    /// �t�F�[�h�A�E�g
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public IEnumerator Hide(float time = -1f)
    {
        var canvas = GetComponent<ModelUtil>();
        if (time <= 0f)
        {
            canvas.FadeOut(0f);
            yield break;
        }

        canvas.FadeOut(time);
        yield return new WaitWhile(() => canvas.IsFading());
    }
}
