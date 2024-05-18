using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// �t�B�[���h�L�����N�^�[���ʏ���
/// </summary>
public class CharacterScript : ObjectBase
{
    #region �萔

    /// <summary>�ړ����x</summary>
    protected const float WALK_VELOCITY = 200f;

    #endregion

    #region �ϐ�

    /// <summary>���f���A�j���[�V����</summary>
    protected Animator modelAnim;

    /// <summary>�����ړ��Ǘ��p</summary>
    private DeltaVector3 walkPosition = new DeltaVector3();

    #endregion

    #region ���

    /// <summary>
    /// ������
    /// </summary>
    protected override void Start()
    {
        base.Start();

        modelAnim = model.GetComponent<Animator>();
    }

    #endregion

    #region ���\�b�h

    /// <summary>
    /// �u�Ԉʒu�ݒ�
    /// </summary>
    /// <param name="position"></param>
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="position"></param>
    /// <param name="speed"></param>
    public void WalkTo(Vector3 position, float speed = 1f)
    {
        StartCoroutine(WalkToCoroutine(position, speed));
    }

    /// <summary>
    /// �����r����
    /// </summary>
    /// <returns></returns>
    public bool IsWalking()
    {
        return walkPosition.IsActive();
    }

    #endregion

    #region �������\�b�h

    /// <summary>
    /// �����R���[�`��
    /// </summary>
    /// <param name="target"></param>
    /// <param name="speed"></param>
    /// <returns></returns>
    private IEnumerator WalkToCoroutine(Vector3 target, float speed = 1f)
    {
        var vec = target - transform.position;
        var time = vec.magnitude / (WALK_VELOCITY * speed);

        walkPosition.Set(transform.position);
        walkPosition.MoveTo(target, time, DeltaFloat.MoveType.LINE);

        var walkSpeed = vec / time;
        WalkStartAnim(walkSpeed);
        while (walkPosition.IsActive())
        {
            yield return null;

            walkPosition.Update(Time.deltaTime);
            transform.position = walkPosition.Get();
        }
        WalkStopAnim();
    }

    /// <summary>
    /// �����n�߃A�j���[�V�����Đ�
    /// </summary>
    /// <param name="vec"></param>
    virtual protected void WalkStartAnim(Vector3 vec)
    {
        modelAnim.SetFloat("speedX", vec.x);
        modelAnim.SetFloat("speedY", vec.y);
    }

    /// <summary>
    /// �����I���A�j���[�V�����Đ�
    /// </summary>
    virtual protected void WalkStopAnim()
    {
        modelAnim.SetFloat("speedX", 0);
        modelAnim.SetFloat("speedY", 0);
    }

    #endregion
}
