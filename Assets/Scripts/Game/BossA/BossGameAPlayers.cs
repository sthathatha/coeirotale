using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����������@�v���C���[�L�������ʏ���
/// </summary>
public class BossGameAPlayers : MonoBehaviour
{
    protected const float LEFT_X = -80;

    public SpriteRenderer model;
    public SpriteRenderer magicEff;

    /// <summary>���@�G�t�F�N�g�̉�</summary>
    public AudioClip magicSe;

    /// <summary>�����|�[�Y</summary>
    public Sprite image_stand;
    /// <summary>�����|�[�Y</summary>
    public Sprite image_walk;
    /// <summary>���@�|�[�Y</summary>
    public Sprite image_magic;

    /// <summary>���@�G�t�F�N�g��</summary>
    public Sprite image_spell0;
    /// <summary>���@�G�t�F�N�g�J</summary>
    public Sprite image_spell1;

    /// <summary>X�ʒu</summary>
    private DeltaFloat walkX = new DeltaFloat();

    /// <summary>
    /// ���ɏo�Ė��@�|�[�Y
    /// </summary>
    /// <returns></returns>
    public IEnumerator ToLeft()
    {
        walkX.Set(model.transform.localPosition.x);
        walkX.MoveTo(LEFT_X, 0.2f, DeltaFloat.MoveType.LINE);

        model.flipX = false;
        model.sprite = image_walk;
        while (walkX.IsActive())
        {
            yield return null;
            walkX.Update(Time.deltaTime);
            model.transform.localPosition = new Vector3(walkX.Get(), 0);
        }
        model.sprite = image_magic;
    }

    /// <summary>
    /// �E�ɖ߂��Ēʏ�|�[�Y
    /// </summary>
    /// <returns></returns>
    public IEnumerator ToRight()
    {
        walkX.Set(model.transform.localPosition.x);
        walkX.MoveTo(0, 0.2f, DeltaFloat.MoveType.LINE);

        model.flipX = true;
        model.sprite = image_walk;
        while (walkX.IsActive())
        {
            yield return null;
            walkX.Update(Time.deltaTime);
            model.transform.localPosition = new Vector3(walkX.Get(), 0);
        }
        model.flipX = false;
        model.sprite = image_stand;
    }
}
