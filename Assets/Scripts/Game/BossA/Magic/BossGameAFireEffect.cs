using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �t�@�C�K�G�t�F�N�g
/// </summary>
public class BossGameAFireEffect : BossGameAMagicBase
{
    /// <summary>������SE</summary>
    public AudioClip bombSe;
    /// <summary>��юU��SE</summary>
    public AudioClip splashSe;

    /// <summary>�����e���v��</summary>
    public BossGameAFireBomb bomb_src;
    /// <summary>���Ԃ��e���v��</summary>
    public BossGameAFireSplash splash_src;

    /// <summary>
    /// �\��
    /// </summary>
    /// <returns></returns>
    public override IEnumerator Play()
    {
        var sound = ManagerSceneScript.GetInstance().soundMan;

        // �����쐬�ʒu4��
        var p1 = new Vector3(-45, -20);
        var p2 = new Vector3(45, 20);
        var p3 = new Vector3(45, -20);
        var p4 = new Vector3(-45, 20);

        // ���Ԃ��쐬�ʒu�U��
        var sp1 = new Vector3(10, 30);
        var sp2 = new Vector3(30, 10);
        var sp3 = new Vector3(30, -10);
        var sp4 = new Vector3(-10, 30);
        var sp5 = new Vector3(-30, 10);
        var sp6 = new Vector3(-30, -10);
        var speed1 = new Vector3(140, 370);
        var speed2 = new Vector3(110, 300);
        var speed3 = new Vector3(100, 270);
        var speed4 = new Vector3(-140, 370);
        var speed5 = new Vector3(-110, 300);
        var speed6 = new Vector3(-100, 270);

        StartCoroutine(PlayBombSe());
        StartCoroutine(PlayFlashCoroutine());
        CreateBomb(p1, 1);
        yield return new WaitForSeconds(0.15f);
        CreateBomb(p2, 2);
        yield return new WaitForSeconds(0.15f);
        CreateBomb(p3, 3);
        yield return new WaitForSeconds(0.15f);
        CreateBomb(p4, 4);
        yield return new WaitForSeconds(0.15f);
        CreateBomb(p1, 5);
        yield return new WaitForSeconds(0.15f);
        CreateBomb(p2, 6);
        yield return new WaitForSeconds(0.15f);
        CreateBomb(p3, 7);
        yield return new WaitForSeconds(0.15f);
        CreateBomb(p4, 8);
        yield return new WaitForSeconds(0.15f);

        sound.PlaySE(splashSe);
        CreateSplash(sp1, speed1);
        CreateSplash(sp2, speed2);
        CreateSplash(sp3, speed3);
        CreateSplash(sp4, speed4);
        CreateSplash(sp5, speed5);
        CreateSplash(sp6, speed6);

        yield return new WaitForSeconds(0.75f);
    }

    /// <summary>
    /// �����쐬
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="renderPriority"></param>
    private void CreateBomb(Vector3 pos, int renderPriority)
    {
        var bomb = GameObject.Instantiate(bomb_src);
        bomb.transform.SetParent(transform, false);
        bomb.transform.localPosition = pos;
        bomb.PlayAndDestroy(renderPriority);
    }

    /// <summary>
    /// ����SE5��Đ�
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayBombSe()
    {
        var sound = ManagerSceneScript.GetInstance().soundMan;

        for(var i=0; i<5; ++i)
        {
            sound.PlaySE(bombSe);
            yield return new WaitForSeconds(0.24f);
        }
    }

    /// <summary>
    /// ��ʂ�����R���[�`��
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayFlashCoroutine()
    {
        var red = new Color(1, 0.1f, 0.1f, 0.6f);
        fader.FadeIn(0.02f, red);
        yield return new WaitWhile(() => fader.IsFading());
        fader.FadeOut(0.02f);
        yield return new WaitForSeconds(0.4f);
        fader.FadeIn(0.02f, red);
        yield return new WaitWhile(() => fader.IsFading());
        fader.FadeOut(0.02f);
        yield return new WaitForSeconds(0.4f);
        fader.FadeIn(0.02f, red);
        yield return new WaitWhile(() => fader.IsFading());
        fader.FadeOut(0.02f);
    }

    /// <summary>
    /// ���Ԃ��쐬
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="initSpeed"></param>
    private void CreateSplash(Vector3 pos, Vector3 initSpeed)
    {
        var sp = GameObject.Instantiate(splash_src);
        sp.transform.SetParent(transform, false);
        sp.PlayAndDestroy(pos, initSpeed);
    }
}
