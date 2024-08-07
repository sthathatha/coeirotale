using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;

/// <summary>
/// ダメージ表示
/// </summary>
public class BossGameBUIDamage : MonoBehaviour
{
    public TMP_Text num4;
    public TMP_Text num3;
    public TMP_Text num2;
    public TMP_Text num1;

    private bool effecting = true;

    /// <summary>
    /// ダメージ表示
    /// </summary>
    /// <param name="num"></param>
    /// <param name="col"></param>
    public IEnumerator ShowDamage(int num)
    {
        if (num < 0)
        {
            num = -num;
            num4.color = Color.green;
            num3.color = Color.green;
            num2.color = Color.green;
            num1.color = Color.green;
        }
        var str = num.ToString();
        if (str.Length == 4)
        {
            num4.SetText(str.Substring(0, 1));
            num3.SetText(str.Substring(1, 1));
            num2.SetText(str.Substring(2, 1));
            num1.SetText(str.Substring(3, 1));
        }
        else if (str.Length == 3)
        {
            num4.SetText("");
            num3.SetText(str.Substring(0, 1));
            num2.SetText(str.Substring(1, 1));
            num1.SetText(str.Substring(2, 1));
        }
        else if (str.Length == 2)
        {
            num4.SetText("");
            num3.SetText("");
            num2.SetText(str.Substring(0, 1));
            num1.SetText(str.Substring(1, 1));
        }
        else
        {
            num4.SetText("");
            num3.SetText("");
            num2.SetText("");
            num1.SetText(str.Substring(0, 1));
        }
        gameObject.SetActive(true);

        StartCoroutine(NumOne(num4.rectTransform));
        yield return new WaitForSeconds(0.03f);
        StartCoroutine(NumOne(num3.rectTransform));
        yield return new WaitForSeconds(0.03f);
        StartCoroutine(NumOne(num2.rectTransform));
        yield return new WaitForSeconds(0.03f);
        StartCoroutine(NumOne(num1.rectTransform));
        yield return new WaitForSeconds(1f);
        effecting = false;
    }

    /// <summary>
    /// 数字1個アニメーション
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    private IEnumerator NumOne(RectTransform num)
    {
        var r = new DeltaFloat();
        r.Set(Mathf.PI / 8f);
        r.MoveTo(Mathf.PI, 0.2f, DeltaFloat.MoveType.LINE);
        while (r.IsActive())
        {
            yield return null;
            r.Update(Time.deltaTime);

            var p = num.localPosition;
            p.y = Mathf.Sin(r.Get()) * 20f;
            num.localPosition = p;
        }
    }

    /// <summary>
    /// 表示中
    /// </summary>
    /// <returns></returns>
    public bool IsEffecting() { return effecting; }
}
