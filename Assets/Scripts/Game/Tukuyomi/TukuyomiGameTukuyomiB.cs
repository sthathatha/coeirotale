using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// つくよみちゃん戦　後半本体制御
/// </summary>
public class TukuyomiGameTukuyomiB : MonoBehaviour
{
    public TukuyomiGameSystem system;
    public ModelUtil body;
    public TukuyomiGameBHand handR;
    public TukuyomiGameBHand handL;
    public ModelUtil kingKomaBig;

    /// <summary>
    /// 初期化
    /// </summary>
    private void Start()
    {
        body.FadeOutImmediate();
        kingKomaBig.FadeOutImmediate();
    }

    /// <summary>
    /// 出現演出
    /// </summary>
    /// <returns></returns>
    public IEnumerator AppearAnimation()
    {
        body.FadeIn(3f);
        kingKomaBig.FadeIn(3f, TukuyomiGameKomaSmall.COLOR_OU);
        yield return new WaitForSeconds(1f);
        handR.ShowHand(0);
        handR.GetComponent<ModelUtil>().FadeIn(2f);
        yield return new WaitForSeconds(0.5f);
        handL.ShowHand(0);
        handL.GetComponent<ModelUtil>().FadeIn(2f);
        kingKomaBig.FadeOut(1f);
        yield return new WaitForSeconds(2f);
    }

    /// <summary>
    /// 消滅演出
    /// </summary>
    /// <returns></returns>
    public IEnumerator DisappearAnimation()
    {
        var se = system.resource.PlaySELoop(system.resource.se_defeat_loop);
        StartCoroutine(ShakeCoroutine());
        handR.GetComponent<ModelUtil>().FadeOut(2f);
        yield return new WaitForSeconds(2f);
        handL.GetComponent<ModelUtil>().FadeOut(2f);
        yield return new WaitForSeconds(2f);
        body.FadeOut(5f);
        yield return new WaitForSeconds(5f);
        system.resource.StopLoopSE(se);
    }

    /// <summary>
    /// 左右に震え続ける
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShakeCoroutine()
    {
        var y = transform.position.y;
        var r = Mathf.PI * 2f;
        while (true)
        {
            yield return null;
            r -= Mathf.PI * 12f * Time.deltaTime;
            if (r < 0f) r += Mathf.PI * 2f;

            transform.position = new Vector3(Mathf.Sin(r) * 8f, y);
        }
    }
}
