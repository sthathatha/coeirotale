using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���X�{�X�{��@�g�����L�[���C�U�[�G�t�F�N�g
/// </summary>
public class BossGameBTranquiEffect : BossGameBEffectBase
{
    public Sprite sp_tranqui1;
    public Sprite sp_tranqui2;

    private Vector3 pos1;
    private Vector3 pos2;

    public void SetParam(Vector3 p1, Vector3 p2)
    {
        pos1 = p1;
        pos2 = p2;

        var rot = Util.GetRadianFromVector(p2 - p1);
        model.transform.localRotation = Util.GetRotateQuaternion(rot);
    }

    /// <summary>
    /// �Đ�
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Play()
    {
        transform.localPosition = pos1;
        model.sprite = sp_tranqui1;
        StartCoroutine(SpriteCoroutine());

        var p = new DeltaVector3();
        p.Set(pos1);
        p.MoveTo(pos2, 0.15f, DeltaFloat.MoveType.LINE);
        while (p.IsActive())
        {
            yield return null;
            p.Update(Time.deltaTime);
            transform.localPosition = p.Get();
        }
    }

    /// <summary>
    /// �X�v���C�g�؂�ւ��R���[�`��
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpriteCoroutine()
    {
        while (true)
        {
            model.sprite = sp_tranqui1;
            yield return new WaitForSeconds(0.05f);
            model.sprite = sp_tranqui2;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
