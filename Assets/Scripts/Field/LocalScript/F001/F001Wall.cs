using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F001��
/// </summary>
public class F001Wall : ObjectBase
{
    public AudioClip moveSE;

    /// <summary>�J�������̍��W</summary>
    private const float OPEN_Y = 56;
    /// <summary>�������̍��W</summary>
    private const float CLOSE_Y = -232;

    /// <summary>�J���Ă�</summary>
    private bool opened = false;
    /// <summary>���W�A�j���[�V�����p</summary>
    private DeltaVector3 pos;

    /// <summary>
    /// �R���X�g���N�^
    /// </summary>
    public F001Wall()
    {
        pos = new DeltaVector3();
    }

    /// <summary>
    /// �J�n��
    /// </summary>
    protected override void Start()
    {
        base.Start();

        pos.Set(new Vector3(transform.localPosition.x, CLOSE_Y, 0));
        transform.localPosition = pos.Get();
    }

    /// <summary>
    /// �J���Ă�
    /// </summary>
    /// <returns></returns>
    public bool IsOpened() { return opened; }

    /// <summary>
    /// �J��
    /// </summary>
    public void Open()
    {
        if (opened) return;
        opened = true;

        StartCoroutine(OpenCoroutine());
    }

    /// <summary>
    /// �J���R���[�`��
    /// </summary>
    /// <returns></returns>
    private IEnumerator OpenCoroutine()
    {
        yield return new WaitForSeconds(0.5f);

        var sound = ManagerSceneScript.GetInstance().soundMan;

        // ���[�vSE�Đ�
        var se = sound.PlaySELoop(moveSE);
        // �ړ�
        pos.MoveTo(new Vector3(transform.localPosition.x, OPEN_Y, 0), 3f, DeltaFloat.MoveType.LINE);
        while (pos.IsActive())
        {
            yield return null;
            pos.Update(Time.deltaTime);
            transform.localPosition = pos.Get();
        }

        sound.StopLoopSE(se, 0.5f);
    }
}
