using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アクションイベント用
/// </summary>
public abstract class ActionEventBase : MonoBehaviour
{
    /// <summary>フィールド</summary>
    public MainScriptBase field;

    public virtual void Start() { }

    /// <summary>
    /// イベント実行
    /// </summary>
    /// <returns></returns>
    public void ExecEvent()
    {
        field.FieldState = MainScriptBase.State.Event;
        field.StartCoroutine(ExecEventCoroutine());
    }

    /// <summary>
    /// イベント実行コルーチン　MainScriptから呼び出してもらう
    /// </summary>
    /// <returns></returns>
    public IEnumerator ExecEventCoroutine()
    {
        yield return null; //入力Aボタンが残ってるので１フレ待つ

        yield return Exec();

        yield return null;
        field.FieldState = MainScriptBase.State.Idle;
    }

    /// <summary>
    /// 実際のイベント処理
    /// </summary>
    /// <returns></returns>
    abstract protected IEnumerator Exec();
}
