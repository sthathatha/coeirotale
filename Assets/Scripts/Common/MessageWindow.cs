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
        /// <summary>なしナレーション</summary>
        None2 = 0,
        /// <summary>レコ</summary>
        Reko,
        /// <summary>つくよみちゃん</summary>
        Tukuyomi0,
        /// <summary>メンデル</summary>
        Menderu0,
        /// <summary>ピエール</summary>
        Pierre0,
        /// <summary>軍歌マチ</summary>
        Mati0,
        /// <summary>まつかりすく</summary>
        Matuka0,
        /// <summary>MANA</summary>
        Mana0,
        /// <summary>小春音アミ</summary>
        Ami0,

        /// <summary>ドロシー</summary>
        Drows0,
        /// <summary>エラ</summary>
        Eraps0,
        /// <summary>エグザ</summary>
        Exa0,
        /// <summary>ウーラ</summary>
        Worra0,
        /// <summary>クー</summary>
        Koob0,
        /// <summary>悠</summary>
        You0,
    }
    #endregion

    #region 表示コントロール
    /// <summary>顔アイコン</summary>
    public Image faceIcon;
    /// <summary>メッセージ デフォルト</summary>
    public TMP_Text message_default;
    /// <summary>メッセージ つくよみちゃん用</summary>
    public TMP_Text message_tukuyomi;
    /// <summary>メッセージ 遠藤愛用</summary>
    public TMP_Text message_menderu;
    /// <summary>メッセージ ピエール用</summary>
    public TMP_Text message_pierre;
    /// <summary>メッセージ 軍歌マチ用</summary>
    public TMP_Text message_mati;
    /// <summary>メッセージ 小春音アミ用</summary>
    public TMP_Text message_koharune;
    /// <summary>メッセージ MANA用</summary>
    public TMP_Text message_mana;
    /// <summary>メッセージ まつかりすく用</summary>
    public TMP_Text message_matuka;
    #endregion

    #region 顔画像
    /// <summary>レコ</summary>
    public Sprite face_Reko;
    /// <summary>つくよみちゃん0</summary>
    public Sprite face_Tukuyomi0;
    /// <summary>メンデル0</summary>
    public Sprite face_Menderu0;
    /// <summary>ピエール</summary>
    public Sprite face_Pierre0;
    /// <summary>マチ</summary>
    public Sprite face_Mati0;
    /// <summary>まつかりすく</summary>
    public Sprite face_Matuka0;
    /// <summary>MANA</summary>
    public Sprite face_Mana0;
    /// <summary>アミ</summary>
    public Sprite face_Ami0;

    /// <summary>ナレーションなど</summary>
    public Sprite face_None;

    /// <summary>ドロシー</summary>
    public Sprite face_Drows0;
    /// <summary>エラ</summary>
    public Sprite face_Eraps0;
    /// <summary>エグザ</summary>
    public Sprite face_Exa0;
    /// <summary>ウーラ</summary>
    public Sprite face_Worra0;
    /// <summary>クー</summary>
    public Sprite face_Koob0;
    /// <summary>悠</summary>
    public Sprite face_You0;
    #endregion

    /// <summary>
    /// 初期化
    /// </summary>
    void Start()
    {
        gameObject.SetActive(false);
        SetTextShows(null);
    }

    #region メソッド
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
    /// <param name="face"></param>
    /// <param name="message"></param>
    /// <param name="voice"></param>
    /// <returns></returns>
    public void StartMessage(Face face, string message, AudioClip voice = null)
    {
        // 顔設定
        setFace(face);

        // テキスト設定
        var txt = GetTextByFace(face);
        SetTextShows(txt);
        txt.SetText(message);

        // 声の再生
        ManagerSceneScript.GetInstance().soundMan.PlayVoice(voice);
    }

    /// <summary>
    /// メッセージ終了待ちコルーチン
    /// </summary>
    /// <returns></returns>
    public IEnumerator WaitForMessageEnd()
    {
        while (true)
        {
            yield return null;

            // ボタンで送る
            if (InputManager.GetInstance().GetKeyPress(InputManager.Keys.South))
            {
                break;
            }
        }
    }
    #endregion

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

        //todo:顔アイコン追加時は対応
        var ary = new List<Sprite>
        {
#region アイコンリスト化
            face_None
            ,face_Reko
            ,face_Tukuyomi0
            ,face_Menderu0
            ,face_Pierre0
            ,face_Mati0
            ,face_Matuka0
            ,face_Mana0
            ,face_Ami0
            ,face_Drows0
            ,face_Eraps0
            ,face_Exa0
            ,face_Worra0
            ,face_Koob0
            ,face_You0
#endregion
        };

        faceIcon.sprite = ary[(int)face];
    }

    /// <summary>
    /// 顔アイコンに対応するテキストを取得
    /// </summary>
    /// <param name="face"></param>
    /// <returns></returns>
    private TMP_Text GetTextByFace(Face face)
    {
        //todo:顔アイコン追加時は対応
        return face switch
        {
            Face.Tukuyomi0 => message_tukuyomi,
            Face.Menderu0 => message_menderu,
            Face.Pierre0 => message_pierre,
            Face.Mati0 => message_mati,
            Face.Matuka0 => message_matuka,
            Face.Mana0 => message_mana,
            Face.Ami0 => message_koharune,
            _ => message_default,
        };
    }

    /// <summary>
    /// 特定のテキストのみ表示
    /// </summary>
    /// <param name="txt"></param>
    private void SetTextShows(TMP_Text txt)
    {
        message_default.gameObject.SetActive(txt == message_default);
        message_tukuyomi.gameObject.SetActive(txt == message_tukuyomi);
        message_menderu.gameObject.SetActive(txt == message_menderu);
        message_pierre.gameObject.SetActive(txt == message_pierre);
        message_mati.gameObject.SetActive(txt == message_mati);
        message_koharune.gameObject.SetActive(txt == message_koharune);
        message_mana.gameObject.SetActive(txt == message_mana);
        message_matuka.gameObject.SetActive(txt == message_matuka);
    }
    #endregion
}
