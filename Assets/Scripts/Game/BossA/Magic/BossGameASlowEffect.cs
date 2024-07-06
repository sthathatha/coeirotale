using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ＊＊＊＊　スロウ
/// </summary>
public class BossGameASlowEffect : BossGameAMagicBase
{
    private const float FAST_TIME = 0.4f;
    private const float SLOW_TIME = 0.7f;

    public GameObject slow1;
    public GameObject slow2;
    public GameObject slow3;
    public GameObject slow4;

    public AudioClip se1;
    public AudioClip se2;

    /// <summary>
    /// 実行
    /// </summary>
    /// <returns></returns>
    public override IEnumerator Play()
    {
        StartCoroutine(SeCoroutine());

        ShowEffect(1);
        yield return new WaitForSeconds(FAST_TIME / 4f);
        ShowEffect(2);
        yield return new WaitForSeconds(FAST_TIME / 4f);
        ShowEffect(3);
        yield return new WaitForSeconds(FAST_TIME / 4f);
        ShowEffect(4);
        yield return new WaitForSeconds(FAST_TIME / 4f);
        ShowEffect(1);
        yield return new WaitForSeconds(SLOW_TIME / 4f);
        ShowEffect(2);
        yield return new WaitForSeconds(SLOW_TIME / 4f);
        ShowEffect(3);
        yield return new WaitForSeconds(SLOW_TIME / 4f);
        ShowEffect(4);
        yield return new WaitForSeconds(SLOW_TIME / 4f);
        ShowEffect(0);
    }

    /// <summary>
    /// エフェクト表示
    /// </summary>
    /// <param name="id"></param>
    private void ShowEffect(int id)
    {
        slow1.SetActive(id == 1);
        slow2.SetActive(id == 2);
        slow3.SetActive(id == 3);
        slow4.SetActive(id == 4);
    }

    /// <summary>
    /// SE再生コルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator SeCoroutine()
    {
        var sound = ManagerSceneScript.GetInstance().soundMan;

        sound.PlaySE(se1);
        yield return new WaitForSeconds(FAST_TIME / 3f);
        sound.PlaySE(se1);
        yield return new WaitForSeconds(FAST_TIME / 3f);
        sound.PlaySE(se1);
        yield return new WaitForSeconds(FAST_TIME / 3f);
        sound.PlaySE(se2);
        yield return new WaitForSeconds(SLOW_TIME / 3f);
        sound.PlaySE(se2);
        yield return new WaitForSeconds(SLOW_TIME / 3f);
        sound.PlaySE(se2);
    }
}
