using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

/// <summary>
/// つくよみちゃん戦　プレイヤー
/// </summary>
public class TukuyomiGamePlayer : MonoBehaviour
{
    #region 定数

    public const float PLAYER_SIZE = 30f;
    private const float FREE_MOVE_SPEED = 200f;

    #endregion

    #region メンバー

    public TukuyomiGameSystem system;
    public TukuyomiGamePlayField playField;

    #endregion

    #region 変数

    /// <summary>
    /// 移動モード
    /// </summary>
    public enum MoveMode
    {
        /// <summary>移動不可</summary>
        Disable = 0,
        /// <summary>自由移動</summary>
        Free,
        /// <summary>マス目移動</summary>
        Cell,
    }
    /// <summary>移動モード</summary>
    public MoveMode moveMode { get; set; } = MoveMode.Disable;

    /// <summary>射撃モード</summary>
    public bool ShotEnable { get; set; } = false;

    private Vector2 _cellLocation;
    /// <summary>
    /// セル位置
    /// </summary>
    public Vector2 CellLocation
    {
        get { return _cellLocation; }
        set
        {
            _cellLocation = value;
            transform.localPosition = playField.GetCellPosition(_cellLocation);
        }
    }

    #endregion

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        var input = InputManager.GetInstance();

        if (moveMode == MoveMode.Free)
        {
            // フリー移動の場合
            var moveVec = Vector3.zero;
            var ymove = false;
            var xmove = false;
            if (input.GetKey(InputManager.Keys.Up))
            {
                moveVec.y = FREE_MOVE_SPEED;
                ymove = true;
            }
            else if (input.GetKey(InputManager.Keys.Down))
            {
                moveVec.y = -FREE_MOVE_SPEED;
                ymove = true;
            }
            if (input.GetKey(InputManager.Keys.Right))
            {
                moveVec.x = FREE_MOVE_SPEED;
                xmove = true;
            }
            else if (input.GetKey(InputManager.Keys.Left))
            {
                moveVec.x = -FREE_MOVE_SPEED;
                xmove = true;
            }
            if (xmove && ymove) moveVec *= 0.8f;
            moveVec *= Time.deltaTime;

            transform.localPosition = playField.ClampFreeField(transform.localPosition + moveVec, PLAYER_SIZE / 2f);
        }
        else if (moveMode == MoveMode.Cell)
        {
            // セル移動の場合
            var nextPos = CellLocation;
            if (input.GetKeyPress(InputManager.Keys.Up)) nextPos.y++;
            else if (input.GetKeyPress(InputManager.Keys.Down)) nextPos.y--;
            if (input.GetKeyPress(InputManager.Keys.Right)) nextPos.x++;
            else if (input.GetKeyPress(InputManager.Keys.Left)) nextPos.x--;

            if (nextPos != CellLocation)
            {
                CellLocation = playField.ClampCellField(nextPos);
                if (system.AEnemyIsInField(CellLocation))
                {
                    // 歩に突っ込んだら自分を消す
                    system.PlayerDamage();
                    gameObject.SetActive(false);
                }
                if (system.AKinPlayMode)
                {
                    // 金の時ゴールを通知
                    if (system.AKinGoal == CellLocation)
                    {
                        system.ALeachKinGoal();
                    }
                }
            }
        }

        if (ShotEnable && input.GetKeyPress(InputManager.Keys.South))
        {
            // ショット作成
            system.CreateShot(transform.localPosition, new Vector3(0, 500f), TukuyomiGameShot.ShotType.Player);
        }
    }

    /// <summary>
    /// 攻撃ヒット
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 自分のショットは無視
        var shot = collision.GetComponent<TukuyomiGameShot>();
        if (shot != null) { return; }

        // 当たった攻撃は消す
        collision.enabled = false;

        // システムに通知
        system.PlayerDamage();
    }
}
