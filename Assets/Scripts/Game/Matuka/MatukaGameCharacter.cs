using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �܂��肷���Q�[���L����
/// </summary>
public class MatukaGameCharacter : MonoBehaviour
{
    public GameObject faceObj;
    public GameObject waitObj;
    public GameObject poseObj;
    public GameObject downObj;

    /// <summary>
    /// �O���t�B�b�N�\��
    /// </summary>
    /// <param name="wait">�ҋ@�摜</param>
    /// <param name="pose">�|�[�Y�摜</param>
    /// <param name="face">��</param>
    /// <param name="down">�_�E��</param>
    public void ShowObject(bool wait, bool pose, bool face, bool down = false)
    {
        waitObj.SetActive(wait);
        poseObj.SetActive(pose);
        faceObj.SetActive(face);
        if (downObj != null)
        {
            downObj.SetActive(down);
        }
    }

    /// <summary>
    /// �`�揇�ݒ�
    /// </summary>
    /// <param name="priority"></param>
    public void SetRenderPriority(int priority)
    {
        waitObj.GetComponent<SpriteRenderer>().sortingOrder = priority;
        poseObj.GetComponent<SpriteRenderer>().sortingOrder = priority;
        faceObj.GetComponent<SpriteRenderer>().sortingOrder = priority + 1;
        if (downObj != null)
        {
            downObj.GetComponent<SpriteRenderer>().sortingOrder = priority;
        }
    }
}
