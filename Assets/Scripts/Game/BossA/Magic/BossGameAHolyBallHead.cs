using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGameAHolyBallHead : MonoBehaviour
{
    private readonly Vector3 ENEMY_CENTER = new Vector3(-256, -180, 0);

    public Transform centerParent;
    public BossGameAHolyBall ballSrc;

    public AudioClip headSe;

    private DeltaVector3 basePosition = new DeltaVector3();
    private float rotate = 0;
    private float distance;
    private int addPriority;
    private int headMode;

    private int headPhase;

    /// <summary>
    /// �X�V����
    /// </summary>
    private void Update()
    {
        // �p�x�͏�ɍX�V
        if (headMode == 0)
        {
            rotate += Mathf.PI * 16f * Time.deltaTime;
            if (rotate > Mathf.PI * 2f) rotate -= Mathf.PI * 2f;
        }
        else
        {
            rotate -= Mathf.PI * 16f * Time.deltaTime;
            if (rotate < 0f) rotate += Mathf.PI * 2f;
        }
    }

    /// <summary>
    /// �\���R���[�`��
    /// </summary>
    /// <returns></returns>
    public IEnumerator ShowCoroutine(int mode)
    {
        distance = 40f;
        rotate = 0;
        addPriority = 0;
        headMode = mode;
        headPhase = 0;
        if (headMode == 0)
        {
            StartCoroutine(SeCoroutine());
        }

        basePosition.Set(new Vector3(headMode == 0 ? 640f : -640f, 0f, 0f));
        basePosition.MoveTo(new Vector3(headMode == 0 ? 10f : -10f, 0f, 0f), 0.3f, DeltaFloat.MoveType.LINE);

        while (basePosition.IsActive())
        {
            yield return new WaitForSeconds(0.04f);
            basePosition.Update(Time.deltaTime);
            CreateBall(0);
        }
        headPhase = 1;

        var tmp = new DeltaFloat();
        tmp.Set(0);
        tmp.MoveTo(1f, 0.35f, DeltaFloat.MoveType.LINE);
        while (tmp.IsActive())
        {
            yield return new WaitForSeconds(0.04f);
            tmp.Update(Time.deltaTime);
            CreateBall(1);
        }
        headPhase = 2;

        // �G�Ɍ������̂͂P����
        if (headMode != 0)
        {
            yield break;
        }

        basePosition.MoveTo(ENEMY_CENTER, 0.1f, DeltaFloat.MoveType.LINE);
        while (basePosition.IsActive())
        {
            yield return new WaitForSeconds(0.04f);
            basePosition.Update(Time.deltaTime);
            CreateBall(2);
        }
    }

    /// <summary>
    /// �{�[������
    /// </summary>
    /// <param name="phase">0:�����ɂ��@1:�����ł��邮��@2:�G�Ɍ�����</param>
    private void CreateBall(int phase)
    {
        var p = basePosition.Get();
        if (phase == 0)
        {
            p.y += distance * Mathf.Sin(rotate);
        }
        else if (phase == 1)
        {
            p.y += distance * Mathf.Sin(rotate) * Util.RandomFloat(0.95f, 1.03f);
            p.x += distance * Mathf.Cos(rotate) * Util.RandomFloat(0.95f, 1.03f);
        }

        addPriority++;
        var ball = Instantiate(ballSrc, centerParent);
        ball.Show(p, 0.4f, addPriority);
    }

    /// <summary>
    /// SE�A���Đ��R���[�`��
    /// </summary>
    /// <returns></returns>
    private IEnumerator SeCoroutine()
    {
        var sound = ManagerSceneScript.GetInstance().soundMan;

        while (headPhase == 0 || headPhase == 1)
        {
            sound.PlaySE(headSe);
            yield return new WaitForSeconds(0.25f);
        }
    }
}
