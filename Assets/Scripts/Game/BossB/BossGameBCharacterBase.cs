using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ラスボス本戦　キャラクター共通処理
/// </summary>
public class BossGameBCharacterBase : MonoBehaviour
{
    #region 定数

    /// <summary>キャラの向き</summary>
    public enum CharaDirection : int
    {
        LeftUp = 0,
        RightUp,
        LeftDown,
        RightDown,
    }

    #endregion

    #region メンバー

    /// <summary>ゲームシステム</summary>
    public BossGameSystemB system;

    /// <summary>モデル</summary>
    public SpriteRenderer model;

    /// <summary>右上画像</summary>
    public Sprite sp_rightup = null;
    /// <summary>左上画像</summary>
    public Sprite sp_leftup = null;
    /// <summary>右下画像</summary>
    public Sprite sp_rightdown = null;
    /// <summary>左下画像</summary>
    public Sprite sp_leftdown = null;

    /// <summary>横マス数</summary>
    public int body_width = 1;
    /// <summary>縦マス数</summary>
    public int body_height = 1;

    /// <summary>初期座標</summary>
    public int init_locx = 0;
    /// <summary>初期座標</summary>
    public int init_locy = 0;
    /// <summary>初期向き</summary>
    public int init_dir_x = 1;
    /// <summary>初期向き</summary>
    public int init_dir_y = 1;

    #endregion

    #region 変数

    /// <summary>現在の向き</summary>
    protected CharaDirection nowDirection;

    /// <summary>現在座標</summary>
    protected Vector2Int location;

    /// <summary>最大HP</summary>
    public int param_HP_max;
    /// <summary>現在HP</summary>
    protected int param_HP;
    /// <summary>基本攻撃力</summary>
    public int param_ATK_base;
    /// <summary>攻撃力変動値</summary>
    protected float param_ATK_rate;
    /// <summary>基本速度</summary>
    public int param_SPD_base;
    /// <summary>速度変動値</summary>
    protected float param_SPD_rate;
    /// <summary>行動待ち時間</summary>
    protected int param_wait_time;

    #endregion

    #region 初期化

    /// <summary>
    /// 初期化
    /// </summary>
    protected virtual void Start()
    {
        SetDirection(init_dir_x, init_dir_y);
        location = new Vector2Int(init_locx, init_locy);
        transform.localPosition = BossGameSystemB.GetCellLocation(init_locx, init_locy);
    }

    #endregion

    #region 機能

    /// <summary>
    /// キャラID　派生先で実装
    /// </summary>
    /// <returns></returns>
    public virtual BossGameSystemB.CharacterID GetCharacterID() { return BossGameSystemB.CharacterID.Player; }

    /// <summary>
    /// Vectorから向きを設定
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void SetDirection(int x, int y)
    {
        // 上下・左右をフラグ判定
        var isRight = false;
        var isUp = false;

        if (x > 0) isRight = true;
        else if (x < 0) isRight = false;
        else isRight = nowDirection == CharaDirection.RightUp || nowDirection == CharaDirection.RightDown;

        if (y > 0) isUp = true;
        else if (y < 0) isUp = false;
        else isUp = nowDirection == CharaDirection.RightUp || nowDirection == CharaDirection.LeftUp;


        // 右上
        if (isRight && isUp) SetDirection(CharaDirection.RightUp);
        // 右下
        else if (isRight && !isUp) SetDirection(CharaDirection.RightDown);
        // 左上
        else if (!isRight && isUp) SetDirection(CharaDirection.LeftUp);
        // 左下
        else SetDirection(CharaDirection.LeftDown);
    }

    /// <summary>
    /// 向きを設定
    /// </summary>
    /// <param name="dir"></param>
    public void SetDirection(CharaDirection dir)
    {
        // 画像変更
        model.sprite = dir switch
        {
            CharaDirection.LeftUp => sp_leftup,
            CharaDirection.RightUp => sp_rightup,
            CharaDirection.RightDown => sp_rightdown,
            _ => sp_leftdown,
        };

        nowDirection = dir;
    }

    /// <summary>
    /// 現在の向き
    /// </summary>
    /// <returns></returns>
    public CharaDirection GetDirection() { return nowDirection; }

    /// <summary>
    /// 現在位置
    /// </summary>
    /// <returns></returns>
    public Vector2Int GetLocation() { return location; }

    #region パラメータ管理

    /// <summary>
    /// パラメータ初期化
    /// </summary>
    public void InitParameter()
    {
        param_ATK_rate = 1f;
        param_SPD_rate = 1f;
        param_HP = param_HP_max;

        param_wait_time = CalcWaitTime(param_SPD_base);
    }

    /// <summary>
    /// 基本の行動待機時間
    /// </summary>
    /// <returns></returns>
    public int GetMaxWaitTime() { return CalcWaitTime(Mathf.FloorToInt(param_SPD_base * param_SPD_rate)); }
    /// <summary>
    /// 現在の待ち時間
    /// </summary>
    /// <returns></returns>
    public int GetWaitTime() { return param_wait_time; }
    /// <summary>
    /// 待ち時間を減らす
    /// </summary>
    /// <param name="t"></param>
    public void DecreaseTime(int t) { param_wait_time -= t; }
    /// <summary>
    /// 待ち時間を再設定
    /// </summary>
    /// <param name="init"></param>
    public virtual void ResetTime(bool init = false) { param_wait_time = GetMaxWaitTime(); }
    /// <summary>
    /// 速度変化
    /// </summary>
    /// <param name="mul"></param>
    public void ChangeSpeed(float mul)
    {
        //todo:待ち時間を変動

        param_SPD_rate *= mul;
    }

    /// <summary>
    /// バフ・デバフをリセット
    /// </summary>
    public void ResetParam()
    {
        //todo:待ち時間を変動

        param_ATK_rate = 1f;
        param_SPD_rate = 1f;
    }

    #endregion

    #endregion

    #region

    /// <summary>
    /// SPDによりかかる標準待機時間
    /// </summary>
    /// <param name="spd"></param>
    /// <returns></returns>
    protected static int CalcWaitTime(int spd)
    {
        var tmp = 100 - spd;
        if (tmp < 10) tmp = 10;
        return tmp;
    }

    #endregion

    #region コルーチン

    /// <summary>
    /// 歩き移動
    /// </summary>
    /// <param name="x">移動距離</param>
    /// <param name="y">移動距離</param>
    /// <returns></returns>
    public IEnumerator Walk(int x, int y)
    {
        /// 画像変更
        SetDirection(x, y);

        location.x += x;
        location.y += y;

        var deltaPos = new DeltaVector3();
        deltaPos.Set(transform.localPosition);
        deltaPos.MoveTo(BossGameSystemB.GetCellLocation(location.x, location.y), 0.2f, DeltaFloat.MoveType.LINE);
        while (deltaPos.IsActive())
        {
            yield return null;
            deltaPos.Update(Time.deltaTime);
            transform.localPosition = deltaPos.Get();
        }
    }

    /// <summary>
    /// ふっとばされ
    /// </summary>
    /// <param name="x">移動距離</param>
    /// <param name="y">移動距離</param>
    /// <returns></returns>
    public IEnumerator Slide(int x, int y)
    {
        location.x += x;
        location.y += y;

        var deltaPos = new DeltaVector3();
        deltaPos.Set(transform.localPosition);
        deltaPos.MoveTo(BossGameSystemB.GetCellLocation(location.x, location.y), 0.05f, DeltaFloat.MoveType.LINE);
        while (deltaPos.IsActive())
        {
            yield return null;
            deltaPos.Update(Time.deltaTime);
            transform.localPosition = deltaPos.Get();
        }
    }

    /// <summary>
    /// ターン行動
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator TurnProcess()
    {
        // 派生先で実装
        yield return new WaitForSeconds(1f);
    }

    #endregion
}
