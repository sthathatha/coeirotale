using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// マチゲーム内弱フェーダ
/// </summary>
public class IkusautaGameMiniFader : MonoBehaviour
{
    /// <summary>動作中</summary>
    private bool _active = false;

    /// <summary>
    /// 色設定
    /// </summary>
    /// <param name="color"></param>
    private void SetColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
    }

    /// <summary>
    /// 非表示
    /// </summary>
    public void Hide()
    {
        SetColor(new Color(0, 0, 0, 0));
    }

    /// <summary>
    /// 暗くする
    /// </summary>
    /// <param name="_enable">true:暗くする　false:解除</param>
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
    /// 一瞬フラッシュ
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
    /// 動作中
    /// </summary>
    /// <returns></returns>
    public bool IsActive() { return _active; }
}
