using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���X�{�X�{��@�X�L�����\��
/// </summary>
public class BossGameBUISkillName : MonoBehaviour
{
    public Image bg;
    public TMP_Text text;

    public void Show(string txt)
    {
        bg.gameObject.SetActive(true);
        text.gameObject.SetActive(true);
        text.SetText(txt);

        // �������
        if (txt.Length > 12)
        {
            text.fontSize = 32;
        }
        else
        {
            text.fontSize = 64;
        }
    }

    public void Hide()
    {
        bg.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
    }
}
