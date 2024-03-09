using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// �w�i��TextMeshPro�����̃V���v��UI
/// </summary>
public class SimpleMessageWindow : MonoBehaviour
{
    /// <summary>�e�I�u�W�F�N�g</summary>
    public GameObject parent;

    /// <summary>�e�L�X�g</summary>
    public TMP_Text message;

    /// <summary>
    /// �J��
    /// </summary>
    public void Open()
    {
        parent.SetActive(true);
    }

    /// <summary>
    /// ����
    /// </summary>
    public void Close()
    {
        parent.SetActive(false);
    }

    /// <summary>
    /// ���b�Z�[�W�ݒ�
    /// </summary>
    /// <param name="mes"></param>
    public void SetMessage(string mes)
    {
        message.SetText(mes);
    }
}
