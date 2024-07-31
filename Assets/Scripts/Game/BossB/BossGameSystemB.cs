using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ボス　＊＊＊＊戦
/// </summary>
public class BossGameSystemB : GameSceneScriptBase
{
    #region 定数

    /// <summary>
    /// キャラID
    /// </summary>
    public enum CharacterID : int
    {
        Player = 0,
        Boss,
        Ami,
        Mana,
        Matuka,
        Menderu,
        Mati,
        Pierre,
    }

    /// <summary>1マスの幅</summary>
    public const float CELL_WIDTH = 130f;
    /// <summary>1マスの高さ</summary>
    public const float CELL_HEIGHT = 86f;


    #endregion

    #region メンバー

    public BossGameBPlayer player;
    public BossGameBEnemy ami;
    public BossGameBEnemy mana;
    public BossGameBEnemy matuka;
    public BossGameBEnemy menderu;
    public BossGameBEnemy mati;
    public BossGameBEnemy pierre;
    public BossGameBEnemy boss;

    public BossGameBUICommand commandUI;
    public BossGameBUISkillName skillNameUI;
    public BossGameBUITurnShow turnUI;
    public BossGameBUICellSelect cellUI;

    #endregion

    private List<BossGameBCharacterBase> characterList = new List<BossGameBCharacterBase>();

    #region 基底

    /// <summary>
    /// 開始前
    /// </summary>
    /// <returns></returns>
    public override IEnumerator Start()
    {
        yield return base.Start();

        //todo:
        characterList.Add(player);
        characterList.Add(boss);
        characterList.Add(ami);
        characterList.Add(mana);
        characterList.Add(matuka);
        characterList.Add(menderu);
        characterList.Add(mati);
        characterList.Add(pierre);

        foreach (var chara in characterList)
        {
            chara.InitParameter();
            chara.ResetTime(true);
        }

        commandUI.Close();
        skillNameUI.Hide();
        turnUI.Show(new List<CharacterID>());
    }



    /// <summary>
    /// フェードイン後
    /// </summary>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn()
    {
        yield return base.AfterFadeIn();

        StartCoroutine(MainCoroutine());
    }

    #endregion

    #region 動作

    /// <summary>
    /// メインコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator MainCoroutine()
    {
        var input = InputManager.GetInstance();

        while (true)
        {
            yield return null;
            //
            BossGameBCharacterBase turnChara = null;
            var nextTime = int.MaxValue;
            foreach (var chara in characterList)
            {
                if (chara.GetWaitTime() < nextTime)
                {
                    turnChara = chara;
                    nextTime = chara.GetWaitTime();
                }
            }

            //
            foreach (var chara in characterList)
            {
                chara.DecreaseTime(nextTime);
            }

            UpdateTurnUI();

            yield return turnChara.TurnProcess();

            //todo:
            if (!characterList.Contains(boss))
            {
                break;
            }

            //todo:
            if (!characterList.Contains(player))
            {
                break;
            }

            //todo:
            turnChara.ResetTime();
        }

        ManagerSceneScript.GetInstance().ExitGame();
    }

    #endregion

    #region システム機能

    /// <summary>
    /// セルの座標計算　左下(0,0)　右上(6,6)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static Vector3 GetCellLocation(int x, int y)
    {
        return new Vector3(x * CELL_WIDTH, y * CELL_HEIGHT);
    }

    /// <summary>
    /// ターン表示更新
    /// </summary>
    /// <param name="deleteTop">先頭のアイコンを確実に消すか</param>
    private void UpdateTurnUI(bool deleteTop = true)
    {
        var turnList = new List<CharacterID>();
        var tmpWaitTime = new List<int>();
        var tmpMaxWaitTime = new List<int>();
        foreach (var chara in characterList)
        {
            tmpWaitTime.Add(chara.GetWaitTime());
            tmpMaxWaitTime.Add(chara.GetMaxWaitTime());
        }

        for (var i = 0; i < 6; ++i)
        {
            var min = tmpWaitTime.Min();
            var idx = tmpWaitTime.IndexOf(min);
            turnList.Add(characterList[idx].GetCharacterID());

            for (var j = 0; j < tmpWaitTime.Count; ++j)
            {
                tmpWaitTime[j] -= min;
            }
            tmpWaitTime[idx] = tmpMaxWaitTime[idx];
        }

        turnUI.Show(turnList, deleteTop);
    }

    /// <summary>
    /// 歩き行動時間の続行チェック
    /// </summary>
    /// <param name="excludeCharacter">自身を判定から除く</param>
    /// <param name="time">経過する時間</param>
    /// <returns></returns>
    public bool CanWalkWait(BossGameBCharacterBase excludeCharacter, int time)
    {
        foreach (var character in characterList)
        {
            if (character == excludeCharacter) continue;
            if (character.GetWaitTime() < time) return false;
        }

        return true;
    }

    /// <summary>
    /// 全体の待ち時間を減らす
    /// </summary>
    /// <param name="excludeCharacter">自身を除く</param>
    /// <param name="time">経過時間</param>
    public void DecreaseAllCharacterWait(BossGameBCharacterBase excludeCharacter, int time)
    {
        foreach (var character in characterList)
        {
            if (character == excludeCharacter) continue;
            character.DecreaseTime(time);
        }

        UpdateTurnUI(false);
    }

    #endregion
}
