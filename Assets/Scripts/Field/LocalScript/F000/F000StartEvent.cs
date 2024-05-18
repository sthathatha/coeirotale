using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F000開始時オープニング会話会話
/// </summary>
public class F000StartEvent : EventBase
{
    #region ボイス

    public AudioClip serif0;
    public AudioClip serif1;
    public AudioClip serif2;
    public AudioClip serif3;
    public AudioClip serif4;
    public AudioClip serif5;
    public AudioClip serif6;
    public AudioClip serif7;
    public AudioClip serif8;
    public AudioClip serif9;
    public AudioClip serif10;
    public AudioClip serif11;

    #endregion

    protected override IEnumerator Exec()
    {
        var msg = ManagerSceneScript.GetInstance().GetMessageWindow();

        yield return new WaitForSeconds(1f);

        msg.Open();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, "test", serif0);
        yield return msg.WaitForMessageEnd();


        msg.Close();
    }
}
