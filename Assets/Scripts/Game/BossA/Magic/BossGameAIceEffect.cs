using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

/// <summary>
/// ＊＊＊＊戦　ブリザガエフェクト
/// </summary>
public class BossGameAIceEffect : BossGameAMagicBase
{
    #region 素材

    public GameObject splash1;
    public GameObject splash2;
    public GameObject splash3;
    public GameObject splash4;
    public GameObject splash11;
    public GameObject splash12;

    public GameObject pirrorParent;
    public GameObject pirror1src;
    public GameObject pirror2src;
    public GameObject pirror3src;

    public AudioClip fallSe;
    public AudioClip splashSe;

    #endregion

    /// <summary>
    /// 実行
    /// </summary>
    /// <returns></returns>
    public override IEnumerator Play()
    {
        var sound = ManagerSceneScript.GetInstance().soundMan;

        StartCoroutine(PirrorCoroutine());
        StartCoroutine(SplashCoroutine());
        StartCoroutine(FadeCoroutine());

        //SE
        sound.PlaySE(fallSe);
        yield return new WaitForSeconds(0.1f);

        sound.PlaySE(splashSe);
        yield return new WaitForSeconds(0.06f);
        sound.PlaySE(splashSe);
        yield return new WaitForSeconds(0.14f);
        sound.PlaySE(splashSe);
        yield return new WaitForSeconds(0.06f);
        sound.PlaySE(splashSe);
        yield return new WaitForSeconds(0.14f);
        sound.PlaySE(splashSe);

        yield return new WaitForSeconds(0.1f);
    }

    /// <summary>
    /// つらら制御コルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator PirrorCoroutine()
    {
        CreatePirror(1);
        yield return new WaitForSeconds(0.04f);
        CreatePirror(2);
        yield return new WaitForSeconds(0.04f);
        CreatePirror(3);
        yield return new WaitForSeconds(0.3f);
        CreatePirror(1);
        yield return new WaitForSeconds(0.04f);
        CreatePirror(2);
        yield return new WaitForSeconds(0.04f);
        CreatePirror(3);
        yield return new WaitForSeconds(0.3f);
    }
    /// <summary>
    /// つらら作成
    /// </summary>
    /// <param name="no"></param>
    private void CreatePirror(int no)
    {
        var pirror = Instantiate(no switch
        {
            1 => pirror1src,
            2 => pirror2src,
            _ => pirror3src,
        }, pirrorParent.transform);

        pirror.SetActive(true);
    }

    /// <summary>
    /// しぶき制御コルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator SplashCoroutine()
    {
        ShowSplash(1);
        yield return new WaitForSeconds(0.05f);
        ShowSplash(2);
        yield return new WaitForSeconds(0.05f);
        ShowSplash(3);
        yield return new WaitForSeconds(0.05f);
        ShowSplash(4);
        yield return new WaitForSeconds(0.05f);
        ShowSplash(1);
        yield return new WaitForSeconds(0.05f);
        ShowSplash(2);
        yield return new WaitForSeconds(0.05f);
        ShowSplash(3);
        yield return new WaitForSeconds(0.05f);
        ShowSplash(4);
        yield return new WaitForSeconds(0.05f);
        ShowSplash(11);
        yield return new WaitForSeconds(0.05f);
        ShowSplash(12);
        yield return new WaitForSeconds(0.05f);
        ShowSplash(11);
        yield return new WaitForSeconds(0.05f);
        ShowSplash(12);
        yield return new WaitForSeconds(0.05f);
        ShowSplash(0);
    }

    /// <summary>
    /// しぶき表示切り替え
    /// </summary>
    /// <param name="no"></param>
    private void ShowSplash(int no)
    {
        splash1.SetActive(no == 1);
        splash2.SetActive(no == 2);
        splash3.SetActive(no == 3);
        splash4.SetActive(no == 4);
        splash11.SetActive(no == 11);
        splash12.SetActive(no == 12);
    }

    /// <summary>
    /// フェード制御コルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeCoroutine()
    {
        fader.FadeIn(0.02f, new Color(0.2f, 0.4f, 1f, 0.7f));
        yield return new WaitWhile(() => fader.IsFading());
        fader.FadeOut(0.3f);
    }
}
