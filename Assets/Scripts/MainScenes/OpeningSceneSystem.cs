using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �I�[�v�j���O
/// </summary>
public class OpeningSceneSystem : MainScriptBase
{
    #region �����o�[

    public Transform starParent;
    public OpeningStarObject1 dummy;
    public OpeningStarObject2 player;
    public AudioClip fallSE;
    public AudioClip fallEndSE;

    #endregion

    #region �v���C�x�[�g
    #endregion

    #region ���
    /// <summary>
    /// �t�F�[�h�C���O
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Start()
    {
        yield return base.Start();

        StartCoroutine(CreateStarCoroutine());
    }

    /// <summary>
    /// �t�F�[�h�C����
    /// </summary>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn()
    {
        yield return base.AfterFadeIn();
        yield return new WaitForSeconds(2f);
        // ��l��
        player.MoveTo(Vector3.zero, 1f, DeltaFloat.MoveType.DECEL);
        yield return new WaitForSeconds(2f);
        player.MoveTo(new Vector3(0, -800f, 0), 4f, DeltaFloat.MoveType.ACCEL);
        yield return new WaitForSeconds(1f);
        ManagerSceneScript.GetInstance().soundMan.PlaySE(fallSE);
        yield return new WaitWhile(() => player.IsActive());

        // �ė����o
        ManagerSceneScript.GetInstance().mainCam.PlayShakeOne(Shaker.ShakeSize.Middle);
        ManagerSceneScript.GetInstance().soundMan.PlaySE(fallEndSE);
        yield return new WaitForSeconds(2f);

        // �t�B�[���h�Ɉړ�
        ManagerSceneScript.GetInstance().LoadMainScene("Field000", 0);
    }
    #endregion

    #region �R���[�`��

    /// <summary>
    /// �w�i�̐������R���[�`��
    /// </summary>
    /// <returns></returns>
    private IEnumerator CreateStarCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Util.RandomFloat(0.3f, 0.7f));
            CreateStarOne();
        }
    }

    #endregion

    #region �v���C�x�[�g���\�b�h
    /// <summary>
    /// �w�i�̐������_����1����
    /// </summary>
    private void CreateStarOne()
    {
        var create = GameObject.Instantiate(dummy.gameObject);
        create.transform.SetParent(starParent, true);
        create.SetActive(true);
    }
    #endregion
}
