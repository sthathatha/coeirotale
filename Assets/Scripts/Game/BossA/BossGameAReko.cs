using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

/// <summary>
/// ���������C�x���g ���R
/// </summary>
public class BossGameAReko : BossGameAPlayers
{
    public GameObject magicParent;
    public BossGameAWhiteBlock magicSrc;

    /// <summary>
    /// ���ɖ��@�G�t�F�N�g�o���ė����|�[�Y�ɖ߂�
    /// </summary>
    /// <returns></returns>
    public IEnumerator PlayMagicEffect()
    {
        var sound = ManagerSceneScript.GetInstance().soundMan;
        sound.PlaySE(magicSe);

        for(var i =0; i<8; ++i)
        {
            var rot = i * Mathf.PI / 4f;
            var block = Instantiate(magicSrc, magicParent.transform);
            block.gameObject.SetActive(true);
            block.Show(rot);
        }
        yield return new WaitForSeconds(1f);

        model.sprite = image_stand;
    }

}
