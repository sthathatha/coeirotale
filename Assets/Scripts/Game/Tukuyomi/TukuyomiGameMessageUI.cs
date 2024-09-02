using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// つくよみちゃん戦　テキストUI系
/// </summary>
public class TukuyomiGameMessageUI : MonoBehaviour
{
    public TMP_Text textObj;

    /// <summary>
    /// 表示
    /// </summary>
    /// <param name="text"></param>
    /// <param name="voice"></param>
    public void Show(string text, AudioClip voice = null)
    {
        textObj.SetText(text);
        gameObject.SetActive(true);

        if (voice != null)
        {
            ManagerSceneScript.GetInstance().soundMan.PlayVoice(voice);
        }
    }

    /// <summary>
    /// 非表示
    /// </summary>
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
