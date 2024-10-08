using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;

/// <summary>
/// 表示物の汎用機能
/// </summary>
public class ModelUtil : MonoBehaviour
{
    /// <summary>
    /// フェード用
    /// </summary>
    private DeltaFloat alpha = new DeltaFloat();

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
        var sprites = GetComponentsInChildren<SpriteRenderer>();
        var canvas = GetComponent<CanvasGroup>();
        if (sprites.Length == 0 && canvas == null)
        {
            alpha.Set(0f);
            yield break;
        }

        var spriteA = sprites.Select(s => s.color.a).ToArray();
        var canvasA = canvas == null ? 0f : canvas.alpha;
        alpha.Set(1);
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
    /// フェードイン
    /// </summary>
    /// <param name="time"></param>
    /// <param name="color">色指定</param>
    /// <param name="alpha">アルファ指定</param>
    public void FadeIn(float time, Color? color = null, float alpha = 1f)
    {
        var sprites = GetComponentsInChildren<SpriteRenderer>();
        if (color.HasValue)
        {
            foreach (var s in sprites)
            {
                s.color = new Color(color.Value.r, color.Value.g, color.Value.b, s.color.a);
            }
        }

        StartCoroutine(FadeInCoroutine(time, alpha));
    }

    /// <summary>
    /// 表示状態にする
    /// </summary>
    public void FadeInImmediate()
    {
        var sprites = GetComponentsInChildren<SpriteRenderer>();
        var canvas = GetComponent<CanvasGroup>();
        foreach (var s in sprites)
        {
            s.color = Color.white;
        }
        if (canvas != null)
        {
            canvas.alpha = 1f;
        }
    }

    /// <summary>
    /// フェードイン
    /// </summary>
    /// <param name="time"></param>
    /// <param name="targetAlpha"></param>
    /// <returns></returns>
    public IEnumerator FadeInCoroutine(float time, float targetAlpha)
    {
        // SpriteRendererがあればColorのAlpha、CanvasGroupがあればalpha
        var sprites = GetComponentsInChildren<SpriteRenderer>();
        var canvas = GetComponent<CanvasGroup>();
        if (sprites.Length == 0 && canvas == null)
        {
            alpha.Set(1f);
            yield break;
        }
        var nowAlpha = sprites.Length > 0 ? sprites[0].color.a : canvas.alpha;
        alpha.Set(nowAlpha);
        alpha.MoveTo(targetAlpha, time, DeltaFloat.MoveType.LINE);
        while (alpha.IsActive())
        {
            yield return null;
            alpha.Update(Time.deltaTime);

            foreach (var itm in sprites.Select((s, idx) => new { s, idx }))
            {
                itm.s.color = new Color(itm.s.color.r, itm.s.color.g, itm.s.color.b, alpha.Get());
            }
            if (canvas != null)
            {
                canvas.alpha = alpha.Get();
            }
        }
    }

    /// <summary>
    /// 非表示状態にする
    /// </summary>
    public void FadeOutImmediate()
    {
        var sprites = GetComponentsInChildren<SpriteRenderer>();
        var canvas = GetComponent<CanvasGroup>();
        foreach (var s in sprites)
        {
            s.color = new Color(1, 1, 1, 0);
        }
        if (canvas != null)
        {
            canvas.alpha = 0f;
        }
    }

    /// <summary>
    /// フェード途中
    /// </summary>
    /// <returns></returns>
    public bool IsFading()
    {
        return alpha.IsActive();
    }
}
