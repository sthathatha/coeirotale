using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 背景とTextMeshProだけのシンプルUI
/// </summary>
public class SimpleMessageWindow : MonoBehaviour
{
    /// <summary>親オブジェクト</summary>
    public GameObject parent;

    /// <summary>テキスト</summary>
    public TMP_Text message;

    /// <summary>
    /// 開く
    /// </summary>
    public void Open()
    {
        parent.SetActive(true);
    }

    /// <summary>
    /// 閉じる
    /// </summary>
    public void Close()
    {
        parent.SetActive(false);
    }

    /// <summary>
    /// メッセージ設定
    /// </summary>
    /// <param name="mes"></param>
    public void SetMessage(string mes)
    {
        message.SetText(mes);
    }
}
