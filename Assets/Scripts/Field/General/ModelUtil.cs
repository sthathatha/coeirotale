using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;

/// <summary>
/// �\�����̔ėp�@�\
/// </summary>
public class ModelUtil : MonoBehaviour
{
    /// <summary>
    /// �t�F�[�h�p
    /// </summary>
    private DeltaFloat alpha = new DeltaFloat();

    /// <summary>
    /// �t�F�[�h�A�E�g
    /// </summary>
    /// <param name="time"></param>
    public void FadeOut(float time)
    {
        StartCoroutine(FadeOutCoroutine(time));
    }

    /// <summary>
    /// �t�F�[�h�A�E�g
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public IEnumerator FadeOutCoroutine(float time)
    {
        // SpriteRenderer�������Color��Alpha�ACanvasGroup�������alpha
        var sprites = GetComponentsInChildren<SpriteRenderer>();
        var canvas = GetComponent<CanvasGroup>();
        if (sprites.Length == 0 && canvas == null)
        {
            alpha.Set(0f);
            yield break;
        }

        var spriteA = sprites.Select(s => s.color.a).ToArray();
        var canvasA = canvas == null ? 0f : canvas.alpha;
        alpha.Set(1f);
        alpha.MoveTo(0f, time, DeltaFloat.MoveType.LINE);
        while (alpha.IsActive())
        {
            yield return null;
            alpha.Update(Time.deltaTime);

            foreach (var itm in sprites.Select((s, idx) => new { s, idx }))
            {
                itm.s.color = new Color(itm.s.color.r, itm.s.color.g, itm.s.color.b, spriteA[itm.idx] * alpha.Get());
            }
            if (canvas != null)
            {
                canvas.alpha = canvasA * alpha.Get();
            }
        }
    }

    /// <summary>
    /// �t�F�[�h�r��
    /// </summary>
    /// <returns></returns>
    public bool IsFading()
    {
        return alpha.IsActive();
    }
}
