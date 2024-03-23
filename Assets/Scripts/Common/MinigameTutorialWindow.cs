using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// ミニゲームチュートリアル用ウィンドウ
/// </summary>
public class MinigameTutorialWindow : MonoBehaviour
{
    #region 定数
    /// <summary>フェード時間</summary>
    private const float FADE_TIME = 0.3f;
    #endregion

    #region メンバ変数
    /// <summary>タイトル</summary>
    public TMP_Text titleText;
    /// <summary>説明</summary>
    public TMP_Text tutorialText;
    #endregion

    #region プライベート
    /// <summary>アルファ</summary>
    private DeltaFloat alpha;
    #endregion

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public MinigameTutorialWindow()
    {
        alpha = new DeltaFloat();
        alpha.Set(0);
    }

    #region 使用メソッド
    /// <summary>
    /// 開く
    /// フェードするので開く前にテキスト設定しておく
    /// </summary>
    /// <returns></returns>
    public IEnumerator Open()
    {
        gameObject.SetActive(true);
        alpha.Set(0);
        alpha.MoveTo(1, FADE_TIME, DeltaFloat.MoveType.LINE);
        while (alpha.IsActive())
        {
            alpha.Update(Time.deltaTime);
            gameObject.GetComponent<CanvasGroup>().alpha = alpha.Get();
            yield return null;
        }
    }

    /// <summary>
    /// 閉じる
    /// </summary>
    /// <returns></returns>
    public IEnumerator Close()
    {
        alpha.Set(1);
        alpha.MoveTo(0, FADE_TIME, DeltaFloat.MoveType.LINE);
        while (alpha.IsActive())
        {
            alpha.Update(Time.deltaTime);
            gameObject.GetComponent<CanvasGroup>().alpha = alpha.Get();
            yield return null;
        }
        gameObject.SetActive(false);
    }

    /// <summary>
    /// タイトル設定
    /// </summary>
    /// <param name="title"></param>
    public void SetTitle(string title) 
    {
        titleText.SetText(title);
    }

    /// <summary>
    /// 内容設定
    /// </summary>
    /// <param name="text"></param>
    public void SetText(string text)
    {
        tutorialText.SetText(text);
    }
    #endregion
}
