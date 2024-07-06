using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// ����������@�e�L�X�g�\��UI
/// </summary>
public class BossGameATextUI : MonoBehaviour
{
    public GameObject window;
    public TMP_Text text;

    /// <summary>
    /// �\��
    /// </summary>
    /// <param name="_text"></param>
    public void Show(string _text)
    {
        window.SetActive(true);
        text.SetText(_text);
        text.gameObject.SetActive(true);
    }

    /// <summary>
    /// ����
    /// </summary>
    public void Hide()
    {
        window.SetActive(false);
        text.gameObject.SetActive(false);
    }
}
