using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

/// <summary>
/// ＊＊＊＊　メテオ
/// </summary>
public class BossGameAMeteorEffect : BossGameAMagicBase
{
    private const float TOTAL_TIME = 3f;

    public BossGameAMeteorOne mateor_src;
    public AudioClip fall_se;

    /// <summary>
    /// 実行
    /// </summary>
    /// <returns></returns>
    public override IEnumerator Play()
    {
        StartCoroutine(SeCoroutine());
        StartCoroutine(FaderCoroutine());

        var interval = 0.08f;
        var cnt = TOTAL_TIME / interval;

        for (var i = 0; i < cnt; ++i)
        {
            CreateMeteor();
            yield return new WaitForSeconds(interval);
        }

        yield return new WaitForSeconds(0.2f);
    }

    /// <summary>
    /// SE再生コルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator SeCoroutine()
    {
        var sound = ManagerSceneScript.GetInstance().soundMan;

        var se_interval = 0.6f;
        var cnt = Mathf.FloorToInt(TOTAL_TIME / se_interval);

        for (var i = 0; i < cnt; ++i)
        {
            sound.PlaySE(fall_se);
            yield return new WaitForSeconds(se_interval);
        }
    }

    /// <summary>
    /// フェード制御コルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator FaderCoroutine()
    {
        var fade_interval = 1.2f;
        var cnt = Mathf.FloorToInt(TOTAL_TIME / fade_interval);

        for (var i = 0; i < cnt; ++i)
        {
            fader.FadeIn(fade_interval / 2f, new Color(1f, 0.3f, 0f, 0.7f));
            yield return new WaitWhile(() => fader.IsFading());
            fader.FadeOut(fade_interval / 2f);
            yield return new WaitWhile(() => fader.IsFading());
        }
    }

    /// <summary>
    /// 隕石1個作成
    /// </summary>
    private void CreateMeteor()
    {
        var m = Instantiate(mateor_src, transform);
        m.Fall();
    }
}
