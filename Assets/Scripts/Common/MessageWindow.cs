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
        /// <summary>�Ȃ��i���[�V����</summary>
        None2 = 0,
        /// <summary>���R</summary>
        Reko,
        /// <summary>����݂����</summary>
        Tukuyomi0,
        /// <summary>�����f��</summary>
        Menderu0,
        /// <summary>�s�G�[��</summary>
        Pierre0,
        /// <summary>�R�̃}�`</summary>
        Mati0,
        /// <summary>�܂��肷��</summary>
        Matuka0,
        /// <summary>MANA</summary>
        Mana0,
        /// <summary>���t���A�~</summary>
        Ami0,

        /// <summary>�h���V�[</summary>
        Drows0,
        /// <summary>�G��</summary>
        Eraps0,
        /// <summary>�G�O�U</summary>
        Exa0,
        /// <summary>�E�[��</summary>
        Worra0,
        /// <summary>�N�[</summary>
        Koob0,
        /// <summary>�I</summary>
        You0,
    }
    #endregion

    #region �\���R���g���[��
    /// <summary>��A�C�R��</summary>
    public Image faceIcon;
    /// <summary>���b�Z�[�W �f�t�H���g</summary>
    public TMP_Text message_default;
    /// <summary>���b�Z�[�W ����݂����p</summary>
    public TMP_Text message_tukuyomi;
    /// <summary>���b�Z�[�W �������p</summary>
    public TMP_Text message_menderu;
    /// <summary>���b�Z�[�W �s�G�[���p</summary>
    public TMP_Text message_pierre;
    /// <summary>���b�Z�[�W �R�̃}�`�p</summary>
    public TMP_Text message_mati;
    /// <summary>���b�Z�[�W ���t���A�~�p</summary>
    public TMP_Text message_koharune;
    /// <summary>���b�Z�[�W MANA�p</summary>
    public TMP_Text message_mana;
    /// <summary>���b�Z�[�W �܂��肷���p</summary>
    public TMP_Text message_matuka;
    #endregion

    #region ��摜
    /// <summary>���R</summary>
    public Sprite face_Reko;
    /// <summary>����݂����0</summary>
    public Sprite face_Tukuyomi0;
    /// <summary>�����f��0</summary>
    public Sprite face_Menderu0;
    /// <summary>�s�G�[��</summary>
    public Sprite face_Pierre0;
    /// <summary>�}�`</summary>
    public Sprite face_Mati0;
    /// <summary>�܂��肷��</summary>
    public Sprite face_Matuka0;
    /// <summary>MANA</summary>
    public Sprite face_Mana0;
    /// <summary>�A�~</summary>
    public Sprite face_Ami0;

    /// <summary>�i���[�V�����Ȃ�</summary>
    public Sprite face_None;

    /// <summary>�h���V�[</summary>
    public Sprite face_Drows0;
    /// <summary>�G��</summary>
    public Sprite face_Eraps0;
    /// <summary>�G�O�U</summary>
    public Sprite face_Exa0;
    /// <summary>�E�[��</summary>
    public Sprite face_Worra0;
    /// <summary>�N�[</summary>
    public Sprite face_Koob0;
    /// <summary>�I</summary>
    public Sprite face_You0;
    #endregion

    /// <summary>
    /// ������
    /// </summary>
    void Start()
    {
        gameObject.SetActive(false);
        SetTextShows(null);
    }

    #region ���\�b�h
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
    /// <param name="face"></param>
    /// <param name="message"></param>
    /// <param name="voice"></param>
    /// <returns></returns>
    public void StartMessage(Face face, string message, AudioClip voice = null)
    {
        // ��ݒ�
        setFace(face);

        // �e�L�X�g�ݒ�
        var txt = GetTextByFace(face);
        SetTextShows(txt);
        txt.SetText(message);

        // ���̍Đ�
        ManagerSceneScript.GetInstance().soundMan.PlayVoice(voice);
    }

    /// <summary>
    /// ���b�Z�[�W�I���҂��R���[�`��
    /// </summary>
    /// <returns></returns>
    public IEnumerator WaitForMessageEnd()
    {
        while (true)
        {
            yield return null;

            // �{�^���ő���
            if (InputManager.GetInstance().GetKeyPress(InputManager.Keys.South))
            {
                break;
            }
        }
    }
    #endregion

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

        //todo:��A�C�R���ǉ����͑Ή�
        var ary = new List<Sprite>
        {
#region �A�C�R�����X�g��
            face_None
            ,face_Reko
            ,face_Tukuyomi0
            ,face_Menderu0
            ,face_Pierre0
            ,face_Mati0
            ,face_Matuka0
            ,face_Mana0
            ,face_Ami0
            ,face_Drows0
            ,face_Eraps0
            ,face_Exa0
            ,face_Worra0
            ,face_Koob0
            ,face_You0
#endregion
        };

        faceIcon.sprite = ary[(int)face];
    }

    /// <summary>
    /// ��A�C�R���ɑΉ�����e�L�X�g���擾
    /// </summary>
    /// <param name="face"></param>
    /// <returns></returns>
    private TMP_Text GetTextByFace(Face face)
    {
        //todo:��A�C�R���ǉ����͑Ή�
        return face switch
        {
            Face.Tukuyomi0 => message_tukuyomi,
            Face.Menderu0 => message_menderu,
            Face.Pierre0 => message_pierre,
            Face.Mati0 => message_mati,
            Face.Matuka0 => message_matuka,
            Face.Mana0 => message_mana,
            Face.Ami0 => message_koharune,
            _ => message_default,
        };
    }

    /// <summary>
    /// ����̃e�L�X�g�̂ݕ\��
    /// </summary>
    /// <param name="txt"></param>
    private void SetTextShows(TMP_Text txt)
    {
        message_default.gameObject.SetActive(txt == message_default);
        message_tukuyomi.gameObject.SetActive(txt == message_tukuyomi);
        message_menderu.gameObject.SetActive(txt == message_menderu);
        message_pierre.gameObject.SetActive(txt == message_pierre);
        message_mati.gameObject.SetActive(txt == message_mati);
        message_koharune.gameObject.SetActive(txt == message_koharune);
        message_mana.gameObject.SetActive(txt == message_mana);
        message_matuka.gameObject.SetActive(txt == message_matuka);
    }
    #endregion
}
