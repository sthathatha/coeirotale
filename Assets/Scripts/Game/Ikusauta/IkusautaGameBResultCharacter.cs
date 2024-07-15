using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

/// <summary>
/// ボスラッシュマチ　結果表示用キャラクター
/// </summary>
public class IkusautaGameBResultCharacter : MonoBehaviour
{
    private const float WIN_BACK_JUMP = 200f;
    private const float LOSE_BACK_JUMP = 64f;

    /// <summary>勝った時の位置</summary>
    public float win_pos_x;
    /// <summary>基本位置</summary>
    public float base_pos_x;
    /// <summary>負けた時の位置</summary>
    public float lose_pos_x;

    /// <summary>勝った時のテクスチャ</summary>
    public Sprite spr_win;
    /// <summary>負けた時のテクスチャ</summary>
    public Sprite spr_lose;
    /// <summary>戻る時のテクスチャ</summary>
    public Sprite spr_back;

    /// <summary>勝利表示中かどうか</summary>
    private bool dispWon;

    /// <summary>
    /// 結果の位置に表示
    /// </summary>
    /// <param name="isWin"></param>
    public void SetResultDisp(bool isWin = true)
    {
        var render = GetComponent<SpriteRenderer>();

        if (isWin)
        {
            render.sprite = spr_win;
            transform.localPosition = new Vector3(win_pos_x, 0);
        }
        else
        {
            render.sprite = spr_lose;
            transform.localPosition = new Vector3(lose_pos_x, 0);
        }

        dispWon = isWin;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public IEnumerator BackToBase(float time)
    {
        var render = GetComponent<SpriteRenderer>();
        render.sprite = spr_back;

        var x = new DeltaFloat();
        var yRad = new DeltaFloat();
        x.Set(transform.localPosition.x);
        x.MoveTo(base_pos_x, time, DeltaFloat.MoveType.LINE);
        yRad.Set(0f);
        yRad.MoveTo(Mathf.PI, time, DeltaFloat.MoveType.LINE);

        while (yRad.IsActive())
        {
            x.Update(Time.deltaTime);
            yRad.Update(Time.deltaTime);

            transform.localPosition = new Vector3(x.Get(), Mathf.Sin(yRad.Get()) * (dispWon ? WIN_BACK_JUMP : LOSE_BACK_JUMP));

            yield return null;
        }
    }
}
