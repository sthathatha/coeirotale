using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// ラスボス本戦　プレイヤー
/// </summary>
public class BossGameBPlayer : BossGameBCharacterBase
{
    /// <summary>時間経過なしで歩ける歩数</summary>
    private const int WALK_DELAY_COUNT = 3;

    /// <summary>歩いた歩数</summary>
    private int walkCount = 0;

    /// <summary>
    /// キャラID
    /// </summary>
    /// <returns></returns>
    public override BossGameSystemB.CharacterID GetCharacterID()
    {
        return BossGameSystemB.CharacterID.Player;
    }

    /// <summary>
    /// ターン行動
    /// </summary>
    /// <returns></returns>
    public override IEnumerator TurnProcess()
    {
        var input = InputManager.GetInstance();

        while (true)
        {
            yield return null;



            if (input.GetKeyPress(InputManager.Keys.East))
            {
                //todo:
                walkCount = 0;
                break;
            }
            else
            {
                var walked = false;
                if (input.GetKey(InputManager.Keys.Up))
                {
                    walked = true;
                    yield return Walk(0, 1);
                }
                else if (input.GetKey(InputManager.Keys.Down))
                {
                    walked = true;
                    yield return Walk(0, -1);
                }
                else if (input.GetKey(InputManager.Keys.Left))
                {
                    walked = true;
                    yield return Walk(-1, 0);
                }
                else if (input.GetKey(InputManager.Keys.Right))
                {
                    walked = true;
                    yield return Walk(1, 0);
                }

                if (walked)
                {
                    if (WalkEndCheck() == false) break;
                }
            }
        }


    }

    /// <summary>
    /// 時間リセット
    /// </summary>
    /// <param name="init"></param>
    public override void ResetTime(bool init = false)
    {
        if (walkCount > 0)
        {
            // 歩いた時の時間
            param_wait_time = GetMaxWaitTime() / 3;
        }
        else if (init)
        {
            // プレイヤーの初期時間は少ない
            param_wait_time = GetMaxWaitTime() / 2;
        }
        else
        {
            base.ResetTime();
        }
    }

    /// <summary>
    /// 歩行終了時の時間経過判定
    /// </summary>
    /// <returns></returns>
    private bool WalkEndCheck()
    {
        ++walkCount;
        // ３歩までは時間たたない
        if (walkCount <= WALK_DELAY_COUNT) return true;

        var walkWait = GetMaxWaitTime() / 3;
        if (system.CanWalkWait(this, walkWait))
        {
            // まだ他が動かないなら減らして続行
            system.DecreaseAllCharacterWait(this, walkWait);
            return true;
        }

        return false;
    }
}
