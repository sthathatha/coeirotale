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

    /// <summary>一番上のときのY</summary>
    private const float CURSOR_TOP_Y = 140f;
    /// <summary>選択肢１個ごとのY</summary>
    private const float CURSOR_INTERVAL_Y = 56f;
    /// <summary>１画面の表示件数</summary>
    private const int DISP_COUNT = 6;

    #endregion

    #region メンバー

    public GameObject detailParent;
    public TMP_Text detailText;

    public GameObject commandParent;
    public RectTransform cursor;
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
    public BossGameBDataBase.SkillID SelectSkill { get; private set; }

    /// <summary>表示するスキル</summary>
    public List<BossGameBDataBase.SkillID> SkillList { get; set; }

    #endregion

    #region 変数

    private int topIndex;
    private int selectIndex;

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
    /// <param name="reset">選択初期化</param>
    /// <returns></returns>
    public IEnumerator Open(bool reset)
    {
        var input = InputManager.GetInstance();
        var sound = ManagerSceneScript.GetInstance().soundMan;

        if (reset)
        {
            topIndex = 0;
            selectIndex = 0;

            UpdateDisplay();
        }

        detailParent.SetActive(true);
        commandParent.SetActive(true);

        while (true)
        {
            yield return null;
            if (input.GetKeyPress(InputManager.Keys.East))
            {
                // キャンセル
                Result = CommandResult.Cancel;
                break;
            }
            else if (input.GetKeyPress(InputManager.Keys.South))
            {
                // 選択
                Result = CommandResult.SkillSelect;
                SelectSkill = SkillList[selectIndex];
                break;
            }

            if (input.GetKeyPress(InputManager.Keys.Up))
            {
                sound.PlaySE(sound.commonSeMove);
                // 上へ
                selectIndex--;
                if (selectIndex < 0)
                {
                    // 上の端から下へ
                    selectIndex = SkillList.Count - 1;
                    topIndex = SkillList.Count - DISP_COUNT;
                    if (topIndex < 0) topIndex = 0;
                }
                else if (selectIndex <= topIndex)
                {
                    topIndex = selectIndex - 1;
                    if (topIndex < 0) topIndex = 0;
                }
                UpdateDisplay();
            }
            else if (input.GetKeyPress(InputManager.Keys.Down))
            {
                sound.PlaySE(sound.commonSeMove);
                // 下へ
                selectIndex++;
                if (selectIndex >= SkillList.Count)
                {
                    // 下の端からトップへ
                    topIndex = 0;
                    selectIndex = 0;
                }
                else if (selectIndex >= topIndex + DISP_COUNT - 1)
                {
                    // 下の端に行こうとするとスクロール
                    topIndex = selectIndex - 4;
                    if (topIndex < 0) topIndex = 0;
                    else if (topIndex >= SkillList.Count - DISP_COUNT)
                        topIndex = SkillList.Count - DISP_COUNT;
                }
                UpdateDisplay();
            }
        }

        Close();
    }

    #endregion

    #region 内部メソッド

    /// <summary>
    /// 現在の選択状態を表示更新する
    /// </summary>
    private void UpdateDisplay()
    {
        // リスト
        cmd1.SetText(BossGameBDataBase.SkillList[SkillList[topIndex]].Name);
        cmd2.SetText(BossGameBDataBase.SkillList[SkillList[topIndex + 1]].Name);
        cmd3.SetText(BossGameBDataBase.SkillList[SkillList[topIndex + 2]].Name);
        cmd4.SetText(BossGameBDataBase.SkillList[SkillList[topIndex + 3]].Name);
        cmd5.SetText(BossGameBDataBase.SkillList[SkillList[topIndex + 4]].Name);
        cmd6.SetText(BossGameBDataBase.SkillList[SkillList[topIndex + 5]].Name);

        // カーソル
        var cursorIndex = selectIndex - topIndex;
        var cursorY = CURSOR_TOP_Y - (cursorIndex * CURSOR_INTERVAL_Y);
        cursor.anchoredPosition = new Vector2(cursor.anchoredPosition.x, cursorY);

        // 詳細文
        detailText.SetText(BossGameBDataBase.SkillList[SkillList[selectIndex]].Detail);
    }

    #endregion
}
