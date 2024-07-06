using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

/// <summary>
/// ＊＊＊＊　ホーリー
/// </summary>
public class BossGameAHolyEffect : BossGameAMagicBase
{
    private const float BOMB_TIME = 0.8f;

    public BossGameAHolyBallHead head1;
    public BossGameAHolyBallHead head2;

    public Transform bombParent;
    public BossGameAHolyBomb bombSrc;

    public AudioClip bombSe;

    private int addPriority;

    /// <summary>
    /// 実行
    /// </summary>
    /// <returns></returns>
    public override IEnumerator Play()
    {
        StartCoroutine(FaderCoroutine());

        // ヘッドを動かす
        StartCoroutine(head2.ShowCoroutine(1));
        yield return head1.ShowCoroutine(0);

        // 爆発
        StartCoroutine(BombSeCoroutine());
        addPriority = 0;
        var timer = new DeltaFloat();
        timer.Set(0);
        timer.MoveTo(1, BOMB_TIME, DeltaFloat.MoveType.LINE);
        while (timer.IsActive())
        {
            yield return new WaitForSeconds(0.04f);
            timer.Update(Time.deltaTime);
            CreateBombOne();
        }
    }

    /// <summary>
    /// 爆発作成
    /// </summary>
    private void CreateBombOne()
    {
        addPriority++;

        var bomb = Instantiate(bombSrc, bombParent);
        // おおよそ4エリアを順番に
        var p = (addPriority % 4) switch
        {
            0 => new Vector3(-40, -40),
            1 => new Vector3(40, 40),
            2 => new Vector3(40, -40),
            _ => new Vector3(-40, 40),
        };
        // ランダム位置
        p.x += Util.RandomFloat(-50f, 50f);
        p.y += Util.RandomFloat(-50f, 50f);

        bomb.Show(p, addPriority);
    }

    /// <summary>
    /// フェード制御コルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator FaderCoroutine()
    {
        fader.FadeIn(0.8f, new Color(0.2f, 0.2f, 1f, 0.6f));
        yield return new WaitWhile(() => fader.IsFading());
        fader.FadeOut(0.8f);
    }

    /// <summary>
    /// 爆発SEコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator BombSeCoroutine()
    {
        var sound = ManagerSceneScript.GetInstance().soundMan;

        for (var i = 0; i < 5; ++i)
        {
            sound.PlaySE(bombSe);
            yield return new WaitForSeconds(BOMB_TIME / 5f);
        }
    }
}