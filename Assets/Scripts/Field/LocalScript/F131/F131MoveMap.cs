using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// F131氷　右から出ようとしたとき
/// </summary>
public class F131MoveMap : AreaEventBase
{
    public PlayerScript player;
    public CharacterScript worra;
    public CharacterScript you;
    public GameObject holeObj;

    public AudioClip catchSe;

    /// <summary>
    /// 実行
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        // ウーラ連れてきてて穴を塞いである場合以外はただの移動
        if (holeObj.activeInHierarchy == true ||
            Global.GetSaveData().GetGameDataInt(F131System.ICE_YOU_FLG) != 2)
        {
            ManagerSceneScript.GetInstance().LoadMainScene("Field010", 1);
            yield break;
        }

        var posEnd = fieldScript.SearchGeneralPosition(2).GetPosition();
        var posD = fieldScript.SearchGeneralPosition(3).GetPosition();
        var posU = fieldScript.SearchGeneralPosition(4).GetPosition();
        var manager = ManagerSceneScript.GetInstance();

        yield return manager.FadeOut();
        player.SetCameraEnable(false);
        manager.mainCam.SetTargetPos(posD);
        manager.mainCam.Immediate();
        yield return new WaitForSeconds(0.5f);
        yield return manager.FadeIn();

        // 捕獲
        const float WORRA_SPD = 8f;
        var posEndW = posEnd;
        posEndW.y += 30f;
        var posStartW = posEnd;
        posStartW.y += (posD.y - posEnd.y) * WORRA_SPD;
        worra.transform.position = posStartW;
        you.model.GetComponent<SpriteRenderer>().sortingLayerName = "Under";
        you.gameObject.SetActive(true);
        worra.gameObject.SetActive(true);

        // 陰から出てくる
        you.SlideTo(posD, speed: 0.3f);
        yield return new WaitWhile(() => you.IsWalking());
        yield return new WaitForSeconds(1f);
        you.SlideTo(posU, speed: 1.8f, moveType: DeltaFloat.MoveType.DECEL);
        yield return new WaitWhile(() => you.IsWalking());
        you.model.GetComponent<SpriteRenderer>().sortingLayerName = "FieldObject";
        you.SlideTo(posD, speed: 1.8f, moveType: DeltaFloat.MoveType.ACCEL);
        yield return new WaitWhile(() => you.IsWalking());
        yield return new WaitForSeconds(1f);

        // 歩き出す
        StartCoroutine(CameraYouCoroutine());
        you.WalkTo(posEnd, afterDir: "up");
        worra.SlideTo(posEndW, WORRA_SPD);
        yield return new WaitWhile(() => you.IsWalking());
        manager.soundMan.PlaySE(catchSe);
        yield return new WaitForSeconds(0.5f);

        // 会話
        var msg = manager.GetMessageWindow();
        msg.Open();
        msg.StartMessage(MessageWindow.Face.Worra0, StringFieldMessage.F131_Catch3_Worra);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.You0, StringFieldMessage.F131_Catch4_You);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Worra0, StringFieldMessage.F131_Catch5_Worra);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.You0, StringFieldMessage.F131_Catch6_You);
        yield return msg.WaitForMessageEnd();
        msg.Close();

        Global.GetSaveData().SetGameData(F131System.ICE_YOU_FLG, 3);
        ManagerSceneScript.GetInstance().LoadMainScene("Field010", 1);
    }

    /// <summary>
    /// 悠を追尾するカメラ
    /// </summary>
    /// <returns></returns>
    private IEnumerator CameraYouCoroutine()
    {
        var cam = ManagerSceneScript.GetInstance().mainCam;
        while (true)
        {
            cam.SetTargetPos(you.transform.position);
            yield return null;
        }
    }
}
