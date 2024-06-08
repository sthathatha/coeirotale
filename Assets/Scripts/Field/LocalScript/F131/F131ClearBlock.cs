using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F131　滑るブロック
/// </summary>
public class F131ClearBlock : F131Block
{
    #region 基底

    /// <summary>
    /// 滑る
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        var dir = Util.GetDirectionFromVec(GetPlayerDistance());
        var after = system.CheckSlipLocation(location, dir);
        MoveTo(after);

        yield break;
    }

    #endregion

    #region パブリック

    /// <summary>
    /// クリア穴状態にする
    /// </summary>
    public void SetClearHole()
    {
        location = new Vector2Int(-2, F131System.LOC_GOAL_Y);
        transform.localPosition = CalcLocPosition(location);
        var model = GetComponent<ObjectBase>().model;
        var modelPos = model.transform.localPosition;
        modelPos.y = 32f - BLOCK_SIZE / 2f;
        model.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
        model.transform.localPosition = modelPos;
        model.GetComponent<Collider2D>().enabled = false;
    }

    #endregion

    #region プライベート

    /// <summary>
    /// 滑る
    /// </summary>
    /// <param name="loc"></param>
    protected void MoveTo(Vector2Int loc)
    {
        if (location == loc) return;
        if (localPos.IsActive()) return;

        StartCoroutine(MoveCoroutine(loc));
    }

    /// <summary>
    /// 滑るコルーチン
    /// </summary>
    /// <param name="loc"></param>
    /// <returns></returns>
    protected IEnumerator MoveCoroutine(Vector2Int loc)
    {
        var sound = ManagerSceneScript.GetInstance().soundMan;
        sound.PlaySE(system.slipSe);

        var dist = location - loc;
        location = loc;

        var distAbs = Math.Abs(dist.x) + Math.Abs(dist.y);
        var to = CalcLocPosition(loc);
        localPos.Set(transform.localPosition);

        localPos.MoveTo(to, distAbs * SLIP_BLOCK_TIME, DeltaFloat.MoveType.LINE);
        while (localPos.IsActive())
        {
            yield return null;
            localPos.Update(Time.deltaTime);
            transform.localPosition = localPos.Get();
        }

        if (location.x >= -1)
        {
            sound.PlaySE(system.stopSe);
            yield break;
        }

        // 穴に落ちて通れるようにする
        var model = GetComponent<ObjectBase>().model;
        var modelPos = new DeltaVector3();
        modelPos.Set(model.transform.localPosition);
        modelPos.MoveTo(new Vector3(0, model.transform.localPosition.y - BLOCK_SIZE / 2f, 0f), 0.4f, DeltaFloat.MoveType.ACCEL);
        while (modelPos.IsActive())
        {
            yield return null;
            modelPos.Update(Time.deltaTime);
            model.transform.localPosition = modelPos.Get();
        }
        sound.PlaySE(system.stopSe);
        Global.GetSaveData().SetGameData(F131System.ICE_BLOCK_FLG, 1);
        system.holeObj.SetActive(false);

        SetClearHole();
    }


    #endregion
}
