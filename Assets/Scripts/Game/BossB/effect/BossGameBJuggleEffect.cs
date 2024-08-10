using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ラスボス本戦　ジャグリングヒット用
/// </summary>
public class BossGameBJuggleEffect : BossGameBEffectBase
{
    private const float THROW_TIME = 0.6f;
    private const float THROW2_TIME = 0.1f;
    public const float JUGGLE_TIME = THROW2_TIME + THROW_TIME;

    public Sprite sp_ball;
    public Sprite sp_knife;

    private Vector3 p1;
    private Vector3 p2;
    private Vector3 p3;

    private float rot = 0f;

    /// <summary>
    /// 常に回転
    /// </summary>
    private void Update()
    {
        rot += Mathf.PI * 8f * Time.deltaTime;
        if (rot > Mathf.PI * 2) rot -= Mathf.PI * 2;

        transform.localRotation = Util.GetRotateQuaternion(rot);
    }

    /// <summary>
    /// 再生開始
    /// </summary>
    /// <param name="pos1"></param>
    /// <param name="pos2"></param>
    /// <param name="pos3"></param>
    /// <param name="type">0:ボール 1:ナイフ</param>
    public void SetParam(Vector3 pos1, Vector3 pos2, Vector3 pos3, int type)
    {
        p1 = pos1; p2 = pos2; p3 = pos3;
        transform.localPosition = pos1;
        model.sprite = type switch
        {
            0 => sp_ball,
            _ => sp_knife,
        };
    }

    /// <summary>
    /// 再生
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Play()
    {
        var dp = new DeltaVector3();
        dp.Set(p1);
        dp.MoveTo(p2, THROW_TIME, DeltaFloat.MoveType.LINE);
        var dr = new DeltaFloat();
        dr.Set(0);
        dr.MoveTo(Mathf.PI, THROW_TIME, DeltaFloat.MoveType.LINE);

        while (dp.IsActive())
        {
            yield return null;
            dp.Update(Time.deltaTime);
            dr.Update(Time.deltaTime);

            var p = dp.Get();
            p.y += Mathf.Sin(dr.Get()) * 60f;
            transform.localPosition = p;
        }

        dp.MoveTo(p3, THROW2_TIME, DeltaFloat.MoveType.LINE);
        while (dp.IsActive())
        {
            yield return null;
            dp.Update(Time.deltaTime);
            transform.localPosition = dp.Get();
        }

        ManagerSceneScript.GetInstance().soundMan.PlaySE(system.dataObj.se_skill_juggling);
    }
}
