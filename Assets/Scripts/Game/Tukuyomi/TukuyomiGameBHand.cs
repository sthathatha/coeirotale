using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����݂����ŏI��@��
/// </summary>
public class TukuyomiGameBHand : MonoBehaviour
{
    public bool IsRight;
    public Sprite sp_normal;
    public Sprite sp_shake;
    public Sprite sp_finger;

    private float posRot = 0f;
    private Vector3 basePos;
    private Vector3 nowPos;

    private bool idling = true;

    /// <summary>
    /// ������
    /// </summary>
    private void Start()
    {
        basePos = transform.localPosition;
        posRot = IsRight ? 0f : Mathf.PI / 2f;
        GetComponent<ModelUtil>().FadeOutImmediate();
    }

    /// <summary>
    /// �X�V
    /// </summary>
    void Update()
    {
        if (!idling) return;
        // ��ɏ㉺���Ă���
        posRot -= Mathf.PI * Time.deltaTime;
        if (posRot < 0f) posRot += Mathf.PI * 2f;

        var addPos = Mathf.Sin(posRot) * 20f;
        nowPos = basePos + new Vector3(0, addPos);
        transform.localPosition = nowPos;
    }

    /// <summary>
    /// ��̌`
    /// </summary>
    /// <param name="phase">0:�ʏ�@1:����@2:�w����</param>
    /// <returns></returns>
    public void ShowHand(int phase)
    {
        GetComponent<SpriteRenderer>().sprite = phase switch
        {
            0 => sp_normal,
            1 => sp_shake,
            _ => sp_finger,
        };

        idling = phase == 0;
    }

    /// <summary>
    /// ���˂̎w����W���擾�i���[���h���W�j
    /// </summary>
    /// <returns></returns>
    public Vector3 GetShotRoot()
    {
        return transform.parent.position + nowPos + new Vector3(IsRight ? -100f : 100f, -97f);
    }
}
