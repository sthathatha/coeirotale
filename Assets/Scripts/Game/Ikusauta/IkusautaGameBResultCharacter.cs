using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

/// <summary>
/// �{�X���b�V���}�`�@���ʕ\���p�L�����N�^�[
/// </summary>
public class IkusautaGameBResultCharacter : MonoBehaviour
{
    private const float WIN_BACK_JUMP = 200f;
    private const float LOSE_BACK_JUMP = 64f;

    /// <summary>���������̈ʒu</summary>
    public float win_pos_x;
    /// <summary>��{�ʒu</summary>
    public float base_pos_x;
    /// <summary>���������̈ʒu</summary>
    public float lose_pos_x;

    /// <summary>���������̃e�N�X�`��</summary>
    public Sprite spr_win;
    /// <summary>���������̃e�N�X�`��</summary>
    public Sprite spr_lose;
    /// <summary>�߂鎞�̃e�N�X�`��</summary>
    public Sprite spr_back;

    /// <summary>�����\�������ǂ���</summary>
    private bool dispWon;

    /// <summary>
    /// ���ʂ̈ʒu�ɕ\��
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
