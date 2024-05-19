using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// セーブポイント
/// </summary>
public class SavePointScript : AreaEventBase
{
    /// <summary>ロード時のGeneralPos番号</summary>
    public int loadPos;

    /// <summary>セーブ時のSE</summary>
    public AudioClip saveSe;

    /// <summary>セーブした時の表示</summary>
    public GameObject savedSprite;

    /// <summary>セーブ中</summary>
    private bool isSaving = false;

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Start()
    {
        base.Start();
        savedSprite.SetActive(false);
    }

    /// <summary>
    /// 実行
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        if (isSaving) yield break;

        var fieldScene = gameObject.scene.name;
        var save = Global.GetSaveData();
        save.SetGameData("SaveFieldScene", fieldScene);
        save.SetGameData("SaveFieldPos", loadPos);

        save.SaveGameData();

        isSaving = true;
        StartCoroutine(SaveSpriteCoroutine());
    }

    /// <summary>
    /// セーブ表示
    /// </summary>
    /// <returns></returns>
    private IEnumerator SaveSpriteCoroutine()
    {
        ManagerSceneScript.GetInstance().soundMan.PlaySE(saveSe);
        savedSprite.SetActive(true);
        yield return new WaitForSeconds(1f);
        savedSprite.SetActive(false);
        isSaving = false;
    }
}
