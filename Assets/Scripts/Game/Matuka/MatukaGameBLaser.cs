using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �܂��肷�����[�U�[
/// </summary>
public class MatukaGameBLaser : MonoBehaviour
{
    /// <summary>��[����w�b�h�̒��S�܂ł̋���</summary>
    private const float HEAD_OFFSET = 128f;
    /// <summary>�{�̑f�ނ̕��s�N�Z��</summary>
    private const float BODY_WIDTH = 64f;

    /// <summary>���W�Ɋ|���Z���Ďg���@�v���C���[�F1 �G�l�~�[�F-1</summary>
    public int player_pos_mul;

    public GameObject head;
    public SpriteRenderer body;

    /// <summary>
    /// �\��
    /// </summary>
    /// <param name="show"></param>
    public void Show(bool show)
    {
        head.SetActive(show);
        body.gameObject.SetActive(show);
    }

    /// <summary>
    /// �ʒu�w��
    /// </summary>
    /// <param name="x">��[�̈ʒu�i���[���h���W�j</param>
    public void SetPos(float x)
    {
        var baseX = transform.localPosition.x;
        ///���̈ʒu
        var locHead = x - baseX + HEAD_OFFSET * player_pos_mul;
        head.transform.localPosition = new Vector3(locHead, 0, 0);

        //�{�f�B�̕��ƈʒu
        body.transform.localPosition = new Vector3(locHead / 2f, 0, 0);
        float widthP = Mathf.Abs(locHead);
        float widthScale = widthP / BODY_WIDTH * 100f;
        body.transform.localScale = new Vector3(widthScale, 100f, 0);
    }
}
