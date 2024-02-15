using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageWindow : MonoBehaviour
{
    #region �萔
    /// <summary>
    /// ��A�C�R��
    /// </summary>
    public enum Face : int
    {
        /// <summary>�Ȃ�</summary>
        None = -1,
        /// <summary>���R</summary>
        Reko = 0,
        /// <summary>����݂����</summary>
        Tukuyomi0,
        /// <summary>�����f��</summary>
        Menderu0,

    }
    #endregion

    /// <summary>��A�C�R��</summary>
    public Image faceIcon;
    /// <summary>���b�Z�[�W</summary>
    public TMP_Text messageText;

    #region ��摜
    /// <summary>���R</summary>
    public Sprite face_Reko;
    /// <summary>����݂����0</summary>
    public Sprite face_Tukuyomi0;
    /// <summary>�����f��0</summary>
    public Sprite face_Menderu0;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        messageText.SetText("");
    }

    /// <summary>
    /// �\��
    /// </summary>
    public void Open()
    {
        gameObject.SetActive(true);
    }
    /// <summary>
    /// ����
    /// </summary>
    public void Close()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// ���b�Z�[�W�\��
    /// </summary>
    /// <returns></returns>
    public void StartMessage(Face face, string message)
    {
        // ��ݒ�
        setFace(face);

        // �e�L�X�g�ݒ�
        messageText.SetText(message);

        //todo: ���̍Đ�
    }

    /// <summary>
    /// ���b�Z�[�W�I���҂��R���[�`��
    /// </summary>
    /// <returns></returns>
    public IEnumerator WaitForMessageEnd()
    {
        while (true)
        {
            // �{�^���ő���
            if (InputManager.GetInstance().GetKeyPress(InputManager.Keys.South))
            {
                break;
            }

            yield return null;
        }
    }

    #region ��A�C�R���ݒ�
    /// <summary>
    /// ��A�C�R���ݒ�
    /// </summary>
    /// <param name="face"></param>
    /// <returns></returns>
    private void setFace(Face face)
    {
        if (face == Face.None)
        {
            faceIcon.gameObject.SetActive(false);
            return;
        }
        faceIcon.gameObject.SetActive(true);

        var ary = new List<Sprite>
        {
#region �A�C�R�����X�g��
            face_Reko
            ,face_Tukuyomi0
            ,face_Menderu0
#endregion
        };

        faceIcon.sprite = ary[(int)face];
    }
    #endregion
}
