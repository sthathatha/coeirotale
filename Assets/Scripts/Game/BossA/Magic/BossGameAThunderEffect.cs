using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ＊＊＊＊戦　サンダガエフェクト
/// </summary>
public class BossGameAThunderEffect : BossGameAMagicBase
{
    private const float SPRITE_INTERVAL = 0.03f;

    public SpriteRenderer pirror;
    public SpriteRenderer splash;

    public Sprite image_pirror_y0;
    public Sprite image_pirror_y1;
    public Sprite image_pirror_b0;
    public Sprite image_splash;
    public Sprite image_splash_y0;
    public Sprite image_splash_y1;
    public Sprite image_splash_y2;
    public Sprite image_splash_b0;
    public Sprite image_splash_b1;
    public Sprite image_splash_b2;

    public AudioClip thunderSe;

    /// <summary>
    /// 実行
    /// </summary>
    /// <returns></returns>
    public override IEnumerator Play()
    {
        pirror.gameObject.SetActive(true);
        splash.gameObject.SetActive(false);

        fader.FadeIn(SPRITE_INTERVAL, new Color(1f, 1f, 0f));
        pirror.flipX = false;
        pirror.sprite = image_pirror_y0;
        yield return new WaitForSeconds(SPRITE_INTERVAL);
        fader.FadeOut(0.5f);
        pirror.sprite = image_pirror_y1;
        yield return new WaitForSeconds(SPRITE_INTERVAL * 2);
        pirror.sprite = image_pirror_b0;
        yield return new WaitForSeconds(SPRITE_INTERVAL);
        pirror.flipX = true;
        yield return new WaitForSeconds(SPRITE_INTERVAL);
        pirror.flipX = false;
        yield return new WaitForSeconds(SPRITE_INTERVAL);

        pirror.gameObject.SetActive(false);
        splash.gameObject.SetActive(true);
        splash.sprite = image_splash;
        ManagerSceneScript.GetInstance().soundMan.PlaySE(thunderSe);
        yield return new WaitForSeconds(SPRITE_INTERVAL);
        splash.sprite = image_splash_y0;
        yield return new WaitForSeconds(SPRITE_INTERVAL);
        splash.sprite = image_splash_y1;
        yield return new WaitForSeconds(SPRITE_INTERVAL);
        splash.sprite = image_splash_y2;
        yield return new WaitForSeconds(SPRITE_INTERVAL);
        splash.sprite = image_splash_b0;
        yield return new WaitForSeconds(SPRITE_INTERVAL);
        splash.sprite = image_splash_b1;
        yield return new WaitForSeconds(SPRITE_INTERVAL);
        splash.sprite = image_splash_b2;
        yield return new WaitForSeconds(SPRITE_INTERVAL);
        splash.gameObject.SetActive(false);
        yield return new WaitForSeconds(SPRITE_INTERVAL);
    }
}
