using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���������C�x���g ����݂����
/// </summary>
public class BossGameATukuyomi : BossGameAPlayers
{
    /// <summary>
    /// ���ɖ��@�G�t�F�N�g�o���ė����|�[�Y�ɖ߂�
    /// </summary>
    /// <returns></returns>
    public IEnumerator PlayMagicEffect()
    {
        var sound = ManagerSceneScript.GetInstance().soundMan;
        sound.PlaySE(magicSe);

        magicEff.gameObject.SetActive(true);
        for (var i = 0; i < 3; ++i)
        {
            magicEff.sprite = image_spell0;
            yield return new WaitForSeconds(0.07f);
            magicEff.sprite = image_spell1;
            yield return new WaitForSeconds(0.13f);
        }
        magicEff.gameObject.SetActive(false);

        model.sprite = image_stand;
    }

    /// <summary>
    /// �\���邾��
    /// </summary>
    public void KamaePose()
    {
        model.sprite = image_magic;
    }
}
