using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �f�U�C�i��̃Z�b�g���ڂ���ʂɂȂ邽�ߕʃI�u�W�F�N�g�Ɏ���
/// </summary>
public class BossGameBDataObject : MonoBehaviour
{
    #region �ėp�G�t�F�N�g�p�X�v���C�g

    public Sprite sp_eff_invincible0;
    public Sprite sp_eff_invincible1;
    public Sprite sp_eff_invincible2;
    public Sprite sp_eff_invincible3;

    public Sprite sp_eff_heal0;
    public Sprite sp_eff_heal1;

    public Sprite sp_eff_hit0;
    public Sprite sp_eff_hit1;

    public Sprite sp_eff_slash0;
    public Sprite sp_eff_slash1;
    public Sprite sp_eff_slash2;

    public Sprite sp_eff_ignition0;
    public Sprite sp_eff_ignition1;
    public Sprite sp_eff_ignition2;
    public Sprite sp_eff_ignition3;

    public Sprite sp_eff_cycrone0;
    public Sprite sp_eff_cycrone1;
    public Sprite sp_eff_cycrone2;

    public Sprite sp_eff_mantrap0;
    public Sprite sp_eff_mantrap1;
    public Sprite sp_eff_mantrap2;

    public enum EffectKind
    {
        Hit = 0,
        Heal,
        Invincible,
        Slash,
        Ignition,
        Cycrone,
        Mantrap,
    }

    /// <summary>
    /// GeneralEffect�p�z��Ŏ擾
    /// </summary>
    /// <returns></returns>
    public Sprite[] GetGeneralEffectList(EffectKind kind)
    {
        return kind switch
        {
            EffectKind.Hit => new[] { sp_eff_hit0, sp_eff_hit1 },
            EffectKind.Heal => new[] { sp_eff_heal0, sp_eff_heal1 },
            EffectKind.Invincible => new[] { sp_eff_invincible0, sp_eff_invincible1, sp_eff_invincible2, sp_eff_invincible3 },
            EffectKind.Slash => new[] { sp_eff_slash0, sp_eff_slash1, sp_eff_slash2 },
            EffectKind.Ignition => new[] { sp_eff_ignition0, sp_eff_ignition1, sp_eff_ignition2, sp_eff_ignition3 },
            EffectKind.Cycrone => new[] { sp_eff_cycrone0, sp_eff_cycrone1, sp_eff_cycrone2 },
            EffectKind.Mantrap => new[] { sp_eff_mantrap0, sp_eff_mantrap1, sp_eff_mantrap2, sp_eff_mantrap2, sp_eff_mantrap2, sp_eff_mantrap2 },
            _ => new[] { sp_eff_heal0 },
        };
    }

    #endregion

    #region �ėp�X�L���pSE

    public AudioClip se_skill_harunotonari;
    public AudioClip se_skill_setuna;
    public AudioClip se_skill_juggling;
    public AudioClip se_skill_katu;
    public AudioClip se_skill_mantrap;
    public AudioClip se_skill_showdown_init;
    public AudioClip se_skill_showdown_heal;
    public AudioClip se_skill_showdown_fail;

    #endregion

    #region �n�`

    public enum FieldEffect
    {
        /// <summary>�Ȃ�</summary>
        None = 0,
        /// <summary>�}���g���b�v</summary>
        Mantrap,
        /// <summary>�v���Y�}</summary>
        Plasma,
    }

    public Sprite sp_field_mantrap;
    public Sprite sp_field_plasma;
    public AudioClip se_field_plasma;

    #endregion
}
