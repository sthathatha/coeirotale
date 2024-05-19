using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// タイトル画面
/// </summary>
public class TitleSceneSystem : MainScriptBase
{
    #region メンバー

    /// <summary>Continue選択肢</summary>
    public TMP_Text continueSelect;
    /// <summary>カーソル</summary>
    public GameObject cursorUI;
    /// <summary>★のマスク</summary>
    public GameObject starMask;

    #endregion

    #region 変数

    /// <summary>カーソル位置</summary>
    private enum TitleSelect : int
    {
        GameStart = 0,
        Continue,
        Option,
    }
    /// <summary>カーソル位置</summary>
    private TitleSelect cursor = 0;

    #endregion

    #region 基底

    /// <summary>
    /// 初期化
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Start()
    {
        yield return base.Start();

        if (Global.GetSaveData().IsEnableGameData() == false)
        {
            continueSelect.alpha = 0.2f;
        }
        if (Global.GetSaveData().system.clearFlag != 0)
        {
            starMask.SetActive(false);
        }
    }

    /// <summary>
    /// フレーム処理
    /// </summary>
    protected override void Update()
    {
        base.Update();

        if (ManagerSceneScript.GetInstance()?.SceneState != ManagerSceneScript.State.Main) { return; }
        if (FieldState != State.Idle) { return; }

        // オプション実行時は操作不可
        var optionWindow = ManagerSceneScript.GetInstance().GetOptionWindow();
        if (optionWindow.gameObject.activeSelf) { return; }

        var input = InputManager.GetInstance();
        var saveData = Global.GetSaveData();
        var sound = ManagerSceneScript.GetInstance().soundMan;

        // カーソル上下
        if (input.GetKeyPress(InputManager.Keys.Up))
        {
            sound.PlaySE(sound.commonSeMove);
            cursor -= 1;
            if (cursor < 0) cursor = TitleSelect.Option;
            UpdateCursor();
        }
        else if (input.GetKeyPress(InputManager.Keys.Down))
        {
            sound.PlaySE(sound.commonSeMove);
            cursor += 1;
            if (cursor > TitleSelect.Option) cursor = TitleSelect.GameStart;
            UpdateCursor();
        }
        else if (input.GetKeyPress(InputManager.Keys.South))
        {
            switch (cursor)
            {
                case TitleSelect.GameStart:
                    // ゲーム開始
                    sound.PlaySE(sound.commonSeSelect);
                    Global.GetSaveData().InitGameData();
                    ManagerSceneScript.GetInstance().LoadMainScene("OpeningScene", 0);
                    break;
                case TitleSelect.Continue:
                    if (saveData.IsEnableGameData())
                    {
                        sound.PlaySE(sound.commonSeSelect);
                        // ロード
                        var save = Global.GetSaveData();
                        save.LoadGameData();
                        ManagerSceneScript.GetInstance().LoadMainScene(save.GetGameDataString("SaveFieldScene"), save.GetGameDataInt("SaveFieldPos"));
                    }
                    else
                    {
                        sound.PlaySE(sound.commonSeError);
                    }
                    break;
                case TitleSelect.Option:
                    // オプション開く
                    StartCoroutine(optionWindow.OpenDialog());
                    break;
            }
        }
    }

    #endregion

    #region プライベートメソッド

    /// <summary>
    /// カーソル位置更新
    /// </summary>
    private void UpdateCursor()
    {
        cursorUI.transform.localPosition = new Vector3(0, -100f * (int)cursor, 0);
    }

    #endregion
}
