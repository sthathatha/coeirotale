using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// ＊＊＊＊戦　テキスト表示UI
/// </summary>
public class BossGameATextUI : MonoBehaviour
{
    public GameObject window;
    public TMP_Text text;

    /// <summary>
    /// 表示
    /// </summary>
    /// <param name="_text"></param>
    public void Show(string _text)
    {
        window.SetActive(true);
        text.SetText(_text);
        text.gameObject.SetActive(true);
    }

    /// <summary>
    /// 消す
    /// </summary>
    public void Hide()
    {
        window.SetActive(false);
        text.gameObject.SetActive(false);
    }
}
