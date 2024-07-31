using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// ラスボス本戦　コマンド選択UI
/// </summary>
public class BossGameBUICommand : MonoBehaviour
{
    #region 定数

    /// <summary>
    /// コマンド選択結果
    /// </summary>
    public enum CommandResult
    {
        SkillSelect = 0,
        Cancel,
    }

    #endregion

    #region メンバー

    public GameObject detailParent;
    public TMP_Text detailText;

    public GameObject commandParent;
    public Transform cursor;
    public TMP_Text cmd1;
    public TMP_Text cmd2;
    public TMP_Text cmd3;
    public TMP_Text cmd4;
    public TMP_Text cmd5;
    public TMP_Text cmd6;

    #endregion

    #region コマンド選択

    /// <summary>結果</summary>
    public CommandResult Result { get; private set; }
    /// <summary>選択スキル</summary>
    public int SelectSkill { get; private set; }

    #endregion

    #region 機能

    /// <summary>
    /// 閉じる
    /// </summary>
    public void Close()
    {
        detailParent.SetActive(false);
        commandParent.SetActive(false);
    }

    /// <summary>
    /// 開いて選択処理
    /// </summary>
    /// <returns></returns>
    public IEnumerator Open()
    {
        detailParent.SetActive(false);
        commandParent.SetActive(false);

        yield break;
    }

    #endregion

    #region コルーチン



    #endregion
}
