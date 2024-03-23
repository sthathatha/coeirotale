using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// はい・いいえウィンドウ
/// </summary>
public class DialogWindow : MonoBehaviour
{
    #region 定数
    /// <summary>はい・いいえ</summary>
    public enum Result : int
    {
        Yes = 0,
        No,
    }

    /// <summary>カーソル位置</summary>
    private const float CURSOR_POS_Y = 28f;
    #endregion

    /// <summary>カーソル</summary>
    public RectTransform cursor = null;

    /// <summary>選択カーソル</summary>
    private Result selectResult;

    #region 使用メソッド
    /// <summary>
    /// 開くコルーチン
    /// </summary>
    /// <returns></returns>
    public IEnumerator OpenDialog()
    {
        var input = InputManager.GetInstance();
        var sound = ManagerSceneScript.GetInstance().soundManager;

        UpdateCursor(Result.Yes);
        gameObject.SetActive(true);

        yield return null;
        while (!input.GetKeyPress(InputManager.Keys.South))
        {
            yield return null;

            if (input.GetKeyPress(InputManager.Keys.Up) || input.GetKeyPress(InputManager.Keys.Down))
            {
                sound.PlaySE(sound.commonSeMove);
                UpdateCursor(selectResult == Result.Yes ? Result.No : Result.Yes);
            }
        }

        sound.PlaySE(sound.commonSeSelect);
        gameObject.SetActive(false);
    }
    #endregion

    /// <summary>
    /// 結果を取得
    /// </summary>
    /// <returns></returns>
    public Result GetResult() { return selectResult; }

    #region プライベート
    /// <summary>
    /// カーソル位置更新
    /// </summary>
    /// <param name="result"></param>
    private void UpdateCursor(Result result)
    {
        selectResult = result;

        cursor.localPosition = new Vector3(0, result == Result.Yes ? CURSOR_POS_Y : -CURSOR_POS_Y, 0);
    }
    #endregion
}
