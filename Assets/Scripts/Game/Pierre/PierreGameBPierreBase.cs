using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �s�G�[���Q�l�̋��ʏ���
/// </summary>
public class PierreGameBPierreBase : MonoBehaviour
{
    /// <summary>�V�X�e��</summary>
    public PierreGameSystemB system;

    /// <summary>�r</summary>
    public SpriteRenderer spr_arm;
    /// <summary>��</summary>
    public SpriteRenderer spr_body;
    /// <summary>�����̃{�[��</summary>
    public SpriteRenderer spr_underBall;

    /// <summary>
    /// �J�n��
    /// </summary>
    virtual protected void Start()
    {
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    virtual protected void Update()
    {
        // ���݂�Y���W�ŕ`�揇���X�V
        var so = Mathf.FloorToInt(100 - transform.position.y);

        spr_underBall.sortingOrder = so;
        spr_body.sortingOrder = so;
        spr_arm.sortingOrder = so + 1;
    }

    /// <summary>
    /// �����蔻��
    /// </summary>
    /// <param name="collision"></param>
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (system.State != PierreGameSystemB.GameState.PLAY) return;

        var ball = collision.GetComponent<PierreGameBallB>();
        OnBallHit(ball);
    }

    /// <summary>
    /// �h����p
    /// </summary>
    /// <param name="ball"></param>
    protected virtual void OnBallHit(PierreGameBallB ball) { }

    /// <summary>
    /// �ʒu���擾
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPos() { return transform.position; }
}
