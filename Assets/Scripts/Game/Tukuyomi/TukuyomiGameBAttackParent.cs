using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// つくよみちゃん本戦　衝撃波を直線上に生成
/// </summary>
public class TukuyomiGameBAttackParent : MonoBehaviour
{
    public TukuyomiGameSystem system;
    private float blastRot;

    /// <summary>
    /// 攻撃開始
    /// </summary>
    /// <param name="root"></param>
    /// <param name="rot"></param>
    public void StartBlast(Vector3 root, float rot)
    {
        transform.position = root;
        blastRot = rot;
        StartCoroutine(BlastCoroutine());
    }

    /// <summary>
    /// 攻撃コルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator BlastCoroutine()
    {
        var moveVec = Util.GetVector3IdentityFromRot(blastRot) * TukuyomiGameKomaSmallB.KOMAB_SIZE;

        // 画面外まで
        while (transform.position.x > Constant.SCREEN_WIDTH * -0.5f &&
            transform.position.x < Constant.SCREEN_WIDTH * 0.5f &&
            transform.position.y > Constant.SCREEN_HEIGHT * -0.5f &&
            transform.position.y < Constant.SCREEN_HEIGHT * 0.5f)
        {
            yield return new WaitForSeconds(0.022f);
            transform.position += moveVec;
            system.CreateAttackBlast(transform.position, false);
        }
    }
}
