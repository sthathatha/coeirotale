using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// フィールドイベント基本処理
/// </summary>
public abstract class EventBase : MonoBehaviour
{
    /// <summary>フィールド</summary>
    public MainScriptBase fieldScript;

    public virtual void Start()
    {
        if (fieldScript == null)
        {
            // 設定が面倒なので基本的な構造なら取得
            var objects = gameObject.scene.GetRootGameObjects();
            foreach (var obj in objects)
            {
                var sys = obj.GetComponent<MainScriptBase>();
                if (sys != null)
                {
                    fieldScript = sys;
                    break;
                }
            }
        }
    }

    /// <summary>
    /// イベント実行
    /// </summary>
    /// <returns></returns>
    public void ExecEvent()
    {
        fieldScript.FieldState = MainScriptBase.State.Event;
        fieldScript.StartCoroutine(ExecEventCoroutine());
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
        fieldScript.FieldState = MainScriptBase.State.Idle;
    }

    /// <summary>
    /// 実際のイベント処理
    /// </summary>
    /// <returns></returns>
    protected abstract IEnumerator Exec();
}
