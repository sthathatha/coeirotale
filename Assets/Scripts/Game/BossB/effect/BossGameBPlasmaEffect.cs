using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

/// <summary>
/// ���X�{�X�@�v���Y�}�t�B�[���h�G�t�F�N�g
/// </summary>
public class BossGameBPlasmaEffect : BossGameBEffectBase
{
    public Sprite sp_plasma1;
    public Sprite sp_plasma2;

    /// <summary>
    /// �Đ�
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Play()
    {
        model.color = Color.yellow;
        model.sprite = sp_plasma1;
        yield return new WaitForSeconds(0.04f);

        model.sprite = sp_plasma2;
        yield return new WaitForSeconds(0.04f);
        model.flipX = true;
        yield return new WaitForSeconds(0.04f);

        model.flipX = false;
        yield return new WaitForSeconds(0.04f);
        model.color = new Color(0.5f, 0.5f, 1f);
        model.flipX = true;
        yield return new WaitForSeconds(0.04f);

        model.color = Color.yellow;
        model.flipX = false;
        yield return new WaitForSeconds(0.04f);
        model.flipX = true;
        yield return new WaitForSeconds(0.04f);
    }
}
