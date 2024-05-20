using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F007チュートリアル
/// </summary>
public class F007Tutorial : EventBase
{
    public TukuyomiScript tukuyomi;
    public PlayerScript player;

    #region ボイス

    #endregion

    /// <summary>
    /// 実行
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        var posCenter = fieldScript.SearchGeneralPosition(2);
        var posUp = fieldScript.SearchGeneralPosition(1).GetPosition() + new Vector3(0, 1000, 0);

        tukuyomi.WalkTo(posCenter.GetPosition());
        yield return new WaitForSeconds(0.4f);
        player.WalkTo(posCenter.GetPosition());
        yield return new WaitWhile(() => tukuyomi.IsWalking());
        tukuyomi.WalkTo(posUp);
        yield return player.IsWalking();
        player.WalkTo(posUp);
        yield return new WaitForSeconds(1f);

        // 自動で次のマップへ
        ManagerSceneScript.GetInstance().LoadMainScene("Field008", 0);
    }
}
