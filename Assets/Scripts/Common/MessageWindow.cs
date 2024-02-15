using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageWindow : MonoBehaviour
{
    #region 定数
    /// <summary>
    /// 顔アイコン
    /// </summary>
    public enum Face : int
    {
        /// <summary>なし</summary>
        None = -1,
        /// <summary>レコ</summary>
        Reko = 0,
        /// <summary>つくよみちゃん</summary>
        Tukuyomi0,
        /// <summary>メンデル</summary>
        Menderu0,

    }
    #endregion

    /// <summary>顔アイコン</summary>
    public Image faceIcon;
    /// <summary>メッセージ</summary>
    public TMP_Text messageText;

    #region 顔画像
    /// <summary>レコ</summary>
    public Sprite face_Reko;
    /// <summary>つくよみちゃん0</summary>
    public Sprite face_Tukuyomi0;
    /// <summary>メンデル0</summary>
    public Sprite face_Menderu0;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        messageText.SetText("");
    }

    /// <summary>
    /// 表示
    /// </summary>
    public void Open()
    {
        gameObject.SetActive(true);
    }
    /// <summary>
    /// 閉じる
    /// </summary>
    public void Close()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// メッセージ表示
    /// </summary>
    /// <returns></returns>
    public void StartMessage(Face face, string message)
    {
        // 顔設定
        setFace(face);

        // テキスト設定
        messageText.SetText(message);

        //todo: 声の再生
    }

    /// <summary>
    /// メッセージ終了待ちコルーチン
    /// </summary>
    /// <returns></returns>
    public IEnumerator WaitForMessageEnd()
    {
        while (true)
        {
            // ボタンで送る
            if (InputManager.GetInstance().GetKeyPress(InputManager.Keys.South))
            {
                break;
            }

            yield return null;
        }
    }

    #region 顔アイコン設定
    /// <summary>
    /// 顔アイコン設定
    /// </summary>
    /// <param name="face"></param>
    /// <returns></returns>
    private void setFace(Face face)
    {
        if (face == Face.None)
        {
            faceIcon.gameObject.SetActive(false);
            return;
        }
        faceIcon.gameObject.SetActive(true);

        var ary = new List<Sprite>
        {
#region アイコンリスト化
            face_Reko
            ,face_Tukuyomi0
            ,face_Menderu0
#endregion
        };

        faceIcon.sprite = ary[(int)face];
    }
    #endregion
}
