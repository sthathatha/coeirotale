using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オープニング
/// </summary>
public class OpeningSceneSystem : MainScriptBase
{
    #region メンバー

    public Transform starParent;
    public OpeningStarObject1 dummy;
    public OpeningStarObject2 player;
    public AudioClip fallSE;
    public AudioClip fallEndSE;

    #endregion

    #region プライベート
    #endregion

    #region 基底
    /// <summary>
    /// フェードイン前
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Start()
    {
        yield return base.Start();

        StartCoroutine(CreateStarCoroutine());
    }

    /// <summary>
    /// フェードイン後
    /// </summary>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn(bool init)
    {
        yield return base.AfterFadeIn(init);

        // AfterFadeInで次ロードまでやると、ManagerSceneScriptのFieldState更新タイミングが重なって
        // 次シーンがすぐState.Mainで始まってしまうため、AfterFadeInは終了して自前コルーチンにする
        StartCoroutine(OpeningCoroutine());
    }
    #endregion

    #region コルーチン

    /// <summary>
    /// 実行コルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator OpeningCoroutine()
    {
        yield return new WaitForSeconds(2f);
        // 主人公
        player.MoveTo(Vector3.zero, 1f, DeltaFloat.MoveType.DECEL);
        yield return new WaitForSeconds(2f);
        player.MoveTo(new Vector3(0, -800f, 0), 4f, DeltaFloat.MoveType.ACCEL);
        yield return new WaitForSeconds(1f);
        ManagerSceneScript.GetInstance().soundMan.PlaySE(fallSE);
        yield return new WaitWhile(() => player.IsActive());

        // 墜落演出
        ManagerSceneScript.GetInstance().mainCam.PlayShakeOne(Shaker.ShakeSize.Middle);
        ManagerSceneScript.GetInstance().soundMan.PlaySE(fallEndSE);
        yield return new WaitForSeconds(2f);

        // フィールドに移動
        ManagerSceneScript.GetInstance().LoadMainScene("Field000", 0);
    }

    /// <summary>
    /// 背景の星生成コルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator CreateStarCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Util.RandomFloat(0.3f, 0.7f));
            CreateStarOne();
        }
    }

    #endregion

    #region プライベートメソッド
    /// <summary>
    /// 背景の星ランダムに1個生成
    /// </summary>
    private void CreateStarOne()
    {
        var create = GameObject.Instantiate(dummy.gameObject);
        create.transform.SetParent(starParent, true);
        create.SetActive(true);
    }
    #endregion
}
