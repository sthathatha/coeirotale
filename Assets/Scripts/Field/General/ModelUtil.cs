using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 表示物の汎用機能
/// </summary>
public class ModelUtil : MonoBehaviour
{
    /// <summary>
    /// フェードアウト
    /// </summary>
    /// <param name="time"></param>
    public void FadeOut(float time)
    {
        StartCoroutine(FadeOutCoroutine(time));
    }

    /// <summary>
    /// フェードアウト
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public IEnumerator FadeOutCoroutine(float time)
    {
        // SpriteRendererがあればColorのAlpha、CanvasGroupがあればalpha
        var sprite = GetComponent<SpriteRenderer>();
        var canvas = GetComponent<CanvasGroup>();
        if (sprite == null && canvas == null) yield break;

        var alpha = new DeltaFloat();
        if (sprite != null) alpha.Set(sprite.color.a);
        else alpha.Set(canvas.alpha);

        alpha.MoveTo(0f, time, DeltaFloat.MoveType.LINE);
        while (alpha.IsActive())
        {
            yield return null;

            alpha.Update(Time.deltaTime);
            if (sprite != null) sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha.Get());
            else canvas.alpha = alpha.Get();
        }
    }
}
