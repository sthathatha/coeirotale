using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ！の表示
/// </summary>
public class TukuyomiGameWarningEffect : MonoBehaviour
{
    private Color color;
    private float waitTime;

    /// <summary>
    /// 表示
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="baseColor"></param>
    /// <param name="timeOffset"></param>
    public void Show(Vector3 pos, Color baseColor, float timeOffset)
    {
        transform.position = pos;
        GetComponent<SpriteRenderer>().color = new Color(baseColor.r, baseColor.g, baseColor.b, 0);
        color = baseColor;
        waitTime = timeOffset;
        gameObject.SetActive(true);
        StartCoroutine(ShowCoroutine());
    }

    /// <summary>
    /// 表示
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowCoroutine()
    {
        yield return new WaitForSeconds(waitTime);
        // 表示切り替え
        var col1 = color;
        var col2 = new Color(1, 1, 1, 0.8f);
        var render = GetComponent<SpriteRenderer>();
        for (var i = 0; i < 10; ++i)
        {
            render.color = col1;
            yield return new WaitForSeconds(0.04f);
            render.color = col2;
            yield return new WaitForSeconds(0.04f);
        }

        // 消える
        Destroy(gameObject);
    }
}
