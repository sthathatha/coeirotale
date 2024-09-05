using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����݂����{��@�Ռ��g�𒼐���ɐ���
/// </summary>
public class TukuyomiGameBAttackParent : MonoBehaviour
{
    public TukuyomiGameSystem system;
    private float blastRot;

    /// <summary>
    /// �U���J�n
    /// </summary>
    /// <param name="root"></param>
    /// <param name="rot"></param>
    public void StartBlast(Vector3 root, float rot)
    {
        transform.position = root;
        blastRot = rot;
        StartCoroutine(BlastCoroutine());
    }

    /// <summary>
    /// �U���R���[�`��
    /// </summary>
    /// <returns></returns>
    private IEnumerator BlastCoroutine()
    {
        var moveVec = Util.GetVector3IdentityFromRot(blastRot) * TukuyomiGameKomaSmallB.KOMAB_SIZE;

        // ��ʊO�܂�
        while (transform.position.x > Constant.SCREEN_WIDTH * -0.5f &&
            transform.position.x < Constant.SCREEN_WIDTH * 0.5f &&
            transform.position.y > Constant.SCREEN_HEIGHT * -0.5f &&
            transform.position.y < Constant.SCREEN_HEIGHT * 0.5f)
        {
            yield return new WaitForSeconds(0.022f);
            transform.position += moveVec;
            system.CreateAttackBlast(transform.position, false);
        }
    }
}
