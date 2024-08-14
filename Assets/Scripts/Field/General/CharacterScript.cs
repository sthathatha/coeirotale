using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    protected Animator modelAnim
    {
        get { return model?.GetComponent<Animator>(); }
    }

    /// <summary>�����ړ��Ǘ��p</summary>
    private DeltaVector3 walkPosition = new DeltaVector3();

    /// <summary>�J���������L�� �f�t�H���g�̓v���C���[�̂�</summary>
    protected bool enableCamera = false;

    /// <summary>���݂̌���</summary>
    protected Constant.Direction direction = Constant.Direction.None;

    /// <summary>�㉺���E�̋��ʃA�j���[�V����������</summary>
    protected bool generalDirection = true;

    #endregion

    #region ���

    /// <summary>
    /// ������
    /// </summary>
    protected override void Start()
    {
        base.Start();
    }

    /// <summary>
    /// �X�V
    /// </summary>
    protected override void Update()
    {
        if (ManagerSceneScript.GetInstance()?.SceneState == ManagerSceneScript.State.Game)
        {
            base.Update();
            return;
        }

        UpdateCamera();
        base.Update();
    }

    /// <summary>
    /// �Q�[�����A���ȂǂɌ��������ɂȂ��Ă��܂����߃��Z�b�g����
    /// </summary>
    public void AwakeResetDirection()
    {
        SetDirection(direction);
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
    /// <param name="afterDir"></param>
    public void WalkTo(Vector3 position, float speed = 1f, string afterDir = "")
    {
        StartCoroutine(WalkToCoroutine(position, true, speed, afterDir));
    }

    /// <summary>
    /// ���̂܂ܓ���
    /// </summary>
    /// <param name="position"></param>
    /// <param name="speed"></param>
    /// <param name="afterDir"></param>
    public void SlideTo(Vector3 position,
        float speed = 8f,
        string afterDir = "",
        DeltaFloat.MoveType moveType = DeltaFloat.MoveType.LINE)
    {
        StartCoroutine(WalkToCoroutine(position, false, speed, afterDir, moveType));
    }

    /// <summary>
    /// �����r����
    /// </summary>
    /// <returns></returns>
    public bool IsWalking()
    {
        return walkPosition.IsActive();
    }

    /// <summary>
    /// �����ύX
    /// </summary>
    /// <param name="dir">����</param>
    public virtual void SetDirection(Constant.Direction dir)
    {
        direction = dir;
        if (dir == Constant.Direction.None) return;

        if (generalDirection &&
            modelAnim?.isActiveAndEnabled == true)
        {
            modelAnim.Play(dir switch
            {
                Constant.Direction.Up => "up",
                Constant.Direction.Down => "down",
                Constant.Direction.Right => "right",
                _ => "left"
            });
        }
    }

    /// <summary>
    /// �A�j���[�V�����w��
    /// </summary>
    /// <param name="anim"></param>
    public void PlayAnim(string anim)
    {
        modelAnim?.Play(anim);
    }

    #endregion

    #region �������\�b�h

    /// <summary>
    /// �����R���[�`��
    /// </summary>
    /// <param name="target"></param>
    /// <param name="playAnim">�����A�j���[�V����</param>
    /// <param name="speed"></param>
    /// <param name="afterDir"></param>
    /// <returns></returns>
    private IEnumerator WalkToCoroutine(Vector3 target,
        bool playAnim = true,
        float speed = 1f,
        string afterDir = "",
        DeltaFloat.MoveType moveType = DeltaFloat.MoveType.LINE
        )
    {
        var vec = target - transform.position;
        var time = vec.magnitude / (WALK_VELOCITY * speed);

        walkPosition.Set(transform.position);
        walkPosition.MoveTo(target, time, moveType);

        var walkSpeed = vec / time;
        if (playAnim)
        {
            WalkStartAnim(walkSpeed);
        }
        while (walkPosition.IsActive())
        {
            yield return null;

            walkPosition.Update(Time.deltaTime);
            transform.position = walkPosition.Get();
        }
        if (playAnim)
        {
            WalkStopAnim();
        }

        if (string.IsNullOrEmpty(afterDir) == false)
        {
            modelAnim?.Play(afterDir);
        }
    }

    /// <summary>
    /// �����n�߃A�j���[�V�����Đ�
    /// </summary>
    /// <param name="vec"></param>
    virtual protected void WalkStartAnim(Vector3 vec)
    {
        modelAnim.SetFloat("speedX", vec.x);
        modelAnim.SetFloat("speedY", vec.y);

        // �[���łȂ���Ό�����ݒ�
        if (vec.sqrMagnitude < 0.1f) return;
        if (Mathf.Abs(vec.x) > Mathf.Abs(vec.y))
        {
            if (vec.x > 0)
            {
                SetDirection(Constant.Direction.Right);
            }
            else
            {
                SetDirection(Constant.Direction.Left);
            }
        }
        else
        {
            if (vec.y > 0)
            {
                SetDirection(Constant.Direction.Up);
            }
            else
            {
                SetDirection(Constant.Direction.Down);
            }
        }
    }

    /// <summary>
    /// �����I���A�j���[�V�����Đ�
    /// </summary>
    virtual protected void WalkStopAnim()
    {
        modelAnim.SetFloat("speedX", 0);
        modelAnim.SetFloat("speedY", 0);
    }

    /// <summary>
    /// �J�����X�V
    /// </summary>
    protected void UpdateCamera()
    {
        if (enableCamera == false) return;

        var cam = ManagerSceneScript.GetInstance().mainCam;
        cam.SetTargetPos(gameObject);
    }

    /// <summary>
    /// �J�����L���ݒ�
    /// </summary>
    /// <param name="enable"></param>
    public void SetCameraEnable(bool enable)
    {
        enableCamera = enable;
    }

    /// <summary>
    /// �J�����L���t���O
    /// </summary>
    /// <returns></returns>
    public bool IsCameraEnable() { return enableCamera; }

    #endregion
}
