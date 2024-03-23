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
        Pierre0,
        Mati0,
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
    /// <summary></summary>
    public Sprite face_Pierre0;
    /// <summary></summary>
    public Sprite face_Mati0;
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
        ManagerSceneScript.GetInstance().SoundManager.PlayVoice(voice);
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
            face_Reko
            ,face_Tukuyomi0
            ,face_Menderu0
            ,face_Pierre0
            ,face_Mati0
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
            _=> message_default,
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
