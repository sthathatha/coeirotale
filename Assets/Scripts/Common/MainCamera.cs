using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

/// <summary>
/// カメラクラス
/// </summary>
public class MainCamera : MonoBehaviour
{
    /// <summary>即設定距離</summary>
    private const float IMMEDIATE_DISTANCE = 5f;

    /// <summary>現在位置</summary>
    private Vector2 basePos;
    /// <summary>目標位置</summary>
    private Vector2 targetPos;

    /// <summary>シェイク管理</summary>
    private Shaker shaker;

    /// <summary>
    /// 初期化
    /// </summary>
    void Start()
    {
        basePos = new Vector2(0, 0);
        targetPos = new Vector2(0, 0);
        shaker = new Shaker();
    }

    /// <summary>
    /// フレーム処理
    /// </summary>
    void Update()
    {
        // 目標位置が離れている場合
        var distance = targetPos - basePos;
        var length = distance.magnitude;
        if (length < IMMEDIATE_DISTANCE)
        {
            basePos = targetPos;
        } else
        {
            var deltaLen = length * Time.deltaTime * 2f;
            if (deltaLen < IMMEDIATE_DISTANCE) deltaLen = IMMEDIATE_DISTANCE;
            basePos += distance.normalized * deltaLen;
        }

        // シェイク制御
        var shakeY = 0f;
        if (shaker.IsActive())
        {
            shaker.Update();
            shakeY = shaker.GetShakeValue();
        }

        // 位置設定
        transform.position = new Vector3(basePos.x, basePos.y + shakeY, -10);
    }

    /// <summary>
    /// 位置設定
    /// </summary>
    /// <param name="pos"></param>
    public void SetTargetPos(Vector2 pos)
    {
        targetPos = pos;

        var distance = targetPos - basePos;
        if (distance.magnitude < IMMEDIATE_DISTANCE)
        {
            Immediate();
        }
    }
    /// <summary>
    /// 位置設定
    /// </summary>
    /// <param name="_object"></param>
    public void SetTargetPos(GameObject _object)
    {
        SetTargetPos(_object.transform.position);
    }
    /// <summary>
    /// 即設定
    /// </summary>
    public void Immediate()
    {
        basePos = targetPos;
    }

    /// <summary>
    /// シェイク1回
    /// </summary>
    /// <param name="size"></param>
    /// <param name="time"></param>
    public void PlayShakeOne(Shaker.ShakeSize size, float time = 1f)
    {
        shaker.PlayOne(size, time);
    }
    /// <summary>
    /// シェイク永久
    /// </summary>
    /// <param name="size"></param>
    public void PlayShake(Shaker.ShakeSize size)
    {
        shaker.Play(size);
    }
    /// <summary>
    /// シェイク停止
    /// </summary>
    public void StopShake()
    {
        shaker.Stop();
    }
}
