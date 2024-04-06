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

    /// <summary>
    /// �O���t�B�b�N�\��
    /// </summary>
    /// <param name="wait">�ҋ@�摜</param>
    /// <param name="pose">�|�[�Y�摜</param>
    /// <param name="face">��</param>
    public void ShowObject(bool wait, bool pose, bool face)
    {
        waitObj.SetActive(wait);
        poseObj.SetActive(pose);
        faceObj.SetActive(face);
    }
}
