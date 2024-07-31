using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// ���X�{�X�{��@�v���C���[
/// </summary>
public class BossGameBPlayer : BossGameBCharacterBase
{
    /// <summary>���Ԍo�߂Ȃ��ŕ��������</summary>
    private const int WALK_DELAY_COUNT = 3;

    /// <summary>����������</summary>
    private int walkCount = 0;

    /// <summary>
    /// �L����ID
    /// </summary>
    /// <returns></returns>
    public override BossGameSystemB.CharacterID GetCharacterID()
    {
        return BossGameSystemB.CharacterID.Player;
    }

    /// <summary>
    /// �^�[���s��
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
    /// ���ԃ��Z�b�g
    /// </summary>
    /// <param name="init"></param>
    public override void ResetTime(bool init = false)
    {
        if (walkCount > 0)
        {
            // ���������̎���
            param_wait_time = GetMaxWaitTime() / 3;
        }
        else if (init)
        {
            // �v���C���[�̏������Ԃ͏��Ȃ�
            param_wait_time = GetMaxWaitTime() / 2;
        }
        else
        {
            base.ResetTime();
        }
    }

    /// <summary>
    /// ���s�I�����̎��Ԍo�ߔ���
    /// </summary>
    /// <returns></returns>
    private bool WalkEndCheck()
    {
        ++walkCount;
        // �R���܂ł͎��Ԃ����Ȃ�
        if (walkCount <= WALK_DELAY_COUNT) return true;

        var walkWait = GetMaxWaitTime() / 3;
        if (system.CanWalkWait(this, walkWait))
        {
            // �܂����������Ȃ��Ȃ猸�炵�đ��s
            system.DecreaseAllCharacterWait(this, walkWait);
            return true;
        }

        return false;
    }
}
