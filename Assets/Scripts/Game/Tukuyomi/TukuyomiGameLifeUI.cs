using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ����݂�����@�̗�UI
/// </summary>
public class TukuyomiGameLifeUI : MonoBehaviour
{
    public Sprite sp_on;
    public Sprite sp_off;

    public Image im1;
    public Image im2;
    public Image im3;
    public Image im4;
    public Image im5;

    /// <summary>
    /// ���C�t�\��
    /// </summary>
    /// <param name="life"></param>
    public void ShowLife(int life)
    {
        gameObject.SetActive(true);

        im1.sprite = life >= 1 ? sp_on : sp_off;
        im2.sprite = life >= 2 ? sp_on : sp_off;
        im3.sprite = life >= 3 ? sp_on : sp_off;
        im4.sprite = life >= 4 ? sp_on : sp_off;
        im5.sprite = life >= 5 ? sp_on : sp_off;
    }

    /// <summary>
    /// ��\��
    /// </summary>
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
