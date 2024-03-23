using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PierreGamePierreA : PierreGameRoadObject
{
    #region �����o�[
    /// <summary>�r</summary>
    public SpriteRenderer armSprite = null;
    /// <summary>��</summary>
    public GameObject bodyObject = null;

    /// <summary>�㉺�A�j���[�V����</summary>
    public Animator jumpAnimator = null;
    /// <summary>�����̃{�[��</summary>
    public SpriteRenderer ballSprite = null;

    /// <summary>�茳�̃{�[��</summary>
    public SpriteRenderer ballOnArmSprite = null;

    /// <summary>�r��{�摜</summary>
    public Sprite arm0 = null;
    /// <summary>�r�グ��摜</summary>
    public Sprite arm1 = null;
    #endregion

    #region �ϐ�
    /// <summary>�㉺�ړ��R���[�`��</summary>
    private IEnumerator randomMoveCoroutine = null;
    /// <summary>�{�[�������R���[�`��</summary>
    private IEnumerator generateBallCoroutine = null;
    #endregion

    /// <summary>
    /// ������
    /// </summary>
    override public void Start()
    {
        base.Start();

        AddRenderList(bodyObject.GetComponent<SpriteRenderer>());
        AddRenderList(ballSprite);

        SetFarPosition(0f);

        armSprite.sprite = arm0;
        ballOnArmSprite.gameObject.SetActive(false);
        randomMoveCoroutine = RandomMoveCoroutine();
        generateBallCoroutine = GenerateBallCoroutine();
    }

    /// <summary>
    /// �ʒu�ݒ莞�ɘr�̕`�揇��+1
    /// </summary>
    /// <param name="_far"></param>
    public override void SetFarPosition(float _far)
    {
        base.SetFarPosition(_far);

        var order = CalcBaseSortingOrder();
        armSprite.sortingOrder = order + 1;
        ballOnArmSprite.sortingOrder = order + 2;
    }

    /// <summary>
    /// �㉺�����_���ړ�
    /// </summary>
    /// <returns></returns>
    private IEnumerator RandomMoveCoroutine()
    {
        var far = new DeltaFloat();
        far.Set(0f);
        while (true)
        {
            var target = Util.RandomFloat(-ROAD_FAR_MAX, ROAD_FAR_MAX);
            var time = Mathf.Abs(target - GetFarPosition()) / ROAD_FAR_MAX;
            far.MoveTo(target, time, DeltaFloat.MoveType.LINE);
            while (far.IsActive())
            {
                yield return null;
                SetFarPosition(far.Get());
                far.Update(Time.deltaTime);
            }

            yield return new WaitForSeconds(Util.RandomFloat(0.1f, 1f));
        }
    }

    /// <summary>
    /// �{�[������
    /// </summary>
    /// <returns></returns>
    private IEnumerator GenerateBallCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Util.RandomFloat(1f, 2f));

            var ballType = system.GetPierreLevel() switch
            {
                PierreGameSystemA.PierreLevel.L1 => PierreGameBall.BallType.Normal,
                PierreGameSystemA.PierreLevel.L2 => PierreGameBall.BallType.Drift,
                _ => (PierreGameBall.BallType)Util.RandomInt(0, 3)
            };

            // �茳�Ɏ���
            ballOnArmSprite.gameObject.SetActive(true);
            armSprite.sprite = arm1;
            ballOnArmSprite.color = PierreGameBall.CalcBallColor(ballType);

            yield return new WaitForSeconds(1f);

            // ������
            ballOnArmSprite.gameObject.SetActive(false);
            armSprite.sprite = arm0;

            system.GenerateBall(GetFarPosition(), ballType);
        }
    }

    /// <summary>
    /// �Q�[���J�n�������o��
    /// </summary>
    public void StartGame()
    {
        StartCoroutine(generateBallCoroutine);
        StartCoroutine(randomMoveCoroutine);
    }

    /// <summary>
    /// ���[�v�����̂��ߒ�~
    /// </summary>
    public void StopForWarp()
    {
        var bodyAnim = bodyObject.GetComponent<Animator>();

        StopCoroutine(generateBallCoroutine);
        StopCoroutine(randomMoveCoroutine);

        bodyAnim.Play("stop");
        jumpAnimator.Play("stop");
    }

    /// <summary>
    /// ���[�v�����̌�ĊJ
    /// </summary>
    public void RestartForWarp()
    {
        var bodyAnim = bodyObject.GetComponent<Animator>();

        StartCoroutine(generateBallCoroutine);
        StartCoroutine(randomMoveCoroutine);

        bodyAnim.Play("run");
        jumpAnimator.Play("run");
    }

    /// <summary>
    /// �Q�[���I�����̒�~
    /// </summary>
    public void StopForGameEnd()
    {
        StopCoroutine(generateBallCoroutine);
        StopCoroutine(randomMoveCoroutine);

        ballOnArmSprite.gameObject.SetActive(false);
        armSprite.sprite = arm0;
    }
}
