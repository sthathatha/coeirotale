using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// フィールドイベント基本処理
/// </summary>
public abstract class EventBase : MonoBehaviour
{
    /// <summary>フィールド</summary>
    protected MainScriptBase fieldScript;

    /// <summary>イベント見たフラグの保存 "Event"で終わるもののみ</summary>
    private string saveName = string.Empty;

    /// <summary>
    /// 開始時
    /// </summary>
    public virtual void Start()
    {
        // フィールドスクリプト　設定が面倒なのでprotectedにして基本的な構造なら取得
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

        // クラス名を見たフラグ保存用にする
        var name = GetType().Name;
        if (name.EndsWith("Event"))
        {
            saveName = name.Replace("Event", "");
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

        if (string.IsNullOrEmpty(saveName) == false)
        {
            // イベントフラグを保存
            Global.GetSaveData().SetGameData(saveName, 1);
        }

        if (ManagerSceneScript.GetInstance().SceneState != ManagerSceneScript.State.Loading)
        {
            fieldScript.FieldState = MainScriptBase.State.Idle;
        }
    }

    /// <summary>
    /// 見たことあるか
    /// </summary>
    /// <returns></returns>
    public bool IsShowed()
    {
        if (string.IsNullOrEmpty(saveName)) return false;

        return Global.GetSaveData().GetGameDataString(saveName) == "1";
    }

    /// <summary>
    /// 実際のイベント処理
    /// </summary>
    /// <returns></returns>
    protected abstract IEnumerator Exec();
}
