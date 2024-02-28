using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SeedScript : MonoBehaviour
{
    #region �萔
    /// <summary>���Ȃ��Ȃ鐔��</summary>
    public const int SEED_MAX_NUM = 10;
    #endregion

    #region �����o�[
    /// <summary>�摜</summary>
    public SpriteRenderer image;
    /// <summary>��������</summary>
    public TMP_Text valueText = null;

    /// <summary>0�̎�</summary>
    public Sprite image0 = null;
    /// <summary>1�̎�</summary>
    public Sprite image1 = null;
    /// <summary>2�̎�</summary>
    public Sprite image2 = null;
    /// <summary>3�̎�</summary>
    public Sprite image3 = null;
    /// <summary>4�̎�</summary>
    public Sprite image4 = null;
    /// <summary>5�̎�</summary>
    public Sprite image5 = null;
    #endregion

    #region �v���C�x�[�g
    /// <summary>���ݒl</summary>
    private int nowValue = 0;
    #endregion

    /// <summary>
    /// ���
    /// </summary>
    /// <param name="num"></param>
    public void GrowNum(int num)
    {
        if (num == 0) { return; }

        var grow = nowValue + num;
        if (grow > SEED_MAX_NUM)
        {
            grow = SEED_MAX_NUM;
        }

        SetNum(grow);
    }

    /// <summary>
    /// ���l�ݒ�
    /// </summary>
    /// <param name="num"></param>
    public void SetNum(int num)
    {
        nowValue = num;
        valueText.SetText(num.ToString());

        var rate = num * 5 / SEED_MAX_NUM;
        switch (rate)
        {
            case 0:
                image.sprite = image0;
                break;
            case 1:
                image.sprite = image1;
                break;
            case 2:
                image.sprite = image2;
                break;
            case 3:
                image.sprite = image3;
                break;
            case 4:
                image.sprite = image4;
                break;
            case 5:
                image.sprite = image5;
                break;
        }
    }

    /// <summary>���ݒl</summary>
    /// <returns></returns>
    public int GetNum() { return nowValue; }

    /// <summary>
    /// ��菜��
    /// </summary>
    public void Pick()
    {
        nowValue = -1;
        image.gameObject.SetActive(false);
        valueText.gameObject.SetActive(false);
    }

    /// <summary>
    /// �I���\���ǂ���
    /// </summary>
    public bool IsEnable()
    {
        return nowValue >= 0 && nowValue < SEED_MAX_NUM;
    }
}
