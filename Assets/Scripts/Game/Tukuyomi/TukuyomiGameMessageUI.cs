using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// ����݂�����@�e�L�X�gUI�n
/// </summary>
public class TukuyomiGameMessageUI : MonoBehaviour
{
    public TMP_Text textObj;

    /// <summary>
    /// �\��
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
    /// ��\��
    /// </summary>
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
