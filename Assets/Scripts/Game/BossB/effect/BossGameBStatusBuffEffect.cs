using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �X�e�[�^�X�A�b�v�E�_�E���G�t�F�N�g
/// </summary>
public class BossGameBStatusBuffEffect : BossGameBEffectBase
{
    /// <summary>
    /// �o�t���
    /// </summary>
    public enum Type
    {
        Strength = 0,
        Speed,
    }

    public Sprite sp_strength;
    public Sprite sp_speed;

    private bool isUp;

    /// <summary>
    /// �\����ސݒ�
    /// </summary>
    /// <param name="up"></param>
    /// <param name="type"></param>
    public void SetParam(bool up, Type type)
    {
        model.sprite = type switch { Type.Strength => sp_strength, _ => sp_speed };
        model.color = up ? Color.yellow : Color.cyan;

        isUp = up;
    }

    /// <summary>
    /// �\��
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Play()
    {
        var y = new DeltaFloat();
        y.Set(isUp ? 0f : 80f);
        y.MoveTo(isUp ? 80f : 0f, 0.4f, DeltaFloat.MoveType.DECEL);
        transform.localPosition = basePosition + new Vector3(0, y.Get());

        while (y.IsActive())
        {
            yield return null;
            y.Update(Time.deltaTime);

            transform.localPosition = basePosition + new Vector3(0, y.Get());
        }

        yield return new WaitForSeconds(0.1f);
    }
}
