using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// フィールドイベント基本処理
/// </summary>
public abstract class EventBase : MonoBehaviour
{
    /// <summary>見た回数カウントをセーブする</summary>
    public bool SaveViewFlag = false;

    /// <summary>見た回数保存名</summary>
    public string saveName = string.Empty;

    /// <summary>フィールド</summary>
    protected MainScriptBase fieldScript;

    /// <summary>見た回数</summary>
    protected int viewCount = 0;

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

        if (SaveViewFlag)
        {
            if (string.IsNullOrEmpty(saveName))
            {
                saveName = this.GetType().Name;
            }

            viewCount = Global.GetSaveData().GetGameDataInt(saveName);
        }
    }

    /// <summary>
    /// イベント実行
    /// </summary>
    /// <returns></returns>
    public void ExecEvent()
    {
        fieldScript.ActivateEvent(this);
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

        if (SaveViewFlag)
        {
            // イベント見た回数を保存
            if (viewCount < int.MaxValue)
            {
                viewCount++;
            }

            Global.GetSaveData().SetGameData(saveName, viewCount);
        }

        fieldScript.EndEvent(this);
    }

    /// <summary>
    /// 実際のイベント処理
    /// </summary>
    /// <returns></returns>
    protected abstract IEnumerator Exec();
}
