using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// フィールドキャラクター共通処理
/// </summary>
public class CharacterScript : ObjectBase
{
    #region 定数

    /// <summary>移動速度</summary>
    protected const float WALK_VELOCITY = 200f;

    #endregion

    #region 変数

    /// <summary>モデルアニメーション</summary>
    protected Animator modelAnim
    {
        get { return model?.GetComponent<Animator>(); }
    }

    /// <summary>歩き移動管理用</summary>
    private DeltaVector3 walkPosition = new DeltaVector3();

    /// <summary>カメラ処理有効 デフォルトはプレイヤーのみ</summary>
    protected bool enableCamera = false;

    /// <summary>現在の向き</summary>
    protected Constant.Direction direction = Constant.Direction.None;

    /// <summary>上下左右の共通アニメーションを持つか</summary>
    protected bool generalDirection = true;

    #endregion

    #region 基底

    /// <summary>
    /// 初期化
    /// </summary>
    protected override void Start()
    {
        base.Start();
    }

    /// <summary>
    /// 更新
    /// </summary>
    protected override void Update()
    {
        if (ManagerSceneScript.GetInstance()?.SceneState == ManagerSceneScript.State.Game)
        {
            base.Update();
            return;
        }

        UpdateCamera();
        base.Update();
    }

    /// <summary>
    /// ゲーム復帰時などに向きが下になってしまうためリセットする
    /// </summary>
    public void AwakeResetDirection()
    {
        SetDirection(direction);
    }

    #endregion

    #region メソッド

    /// <summary>
    /// 瞬間位置設定
    /// </summary>
    /// <param name="position"></param>
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    /// <summary>
    /// 歩く
    /// </summary>
    /// <param name="position"></param>
    /// <param name="speed"></param>
    /// <param name="afterDir"></param>
    public void WalkTo(Vector3 position, float speed = 1f, string afterDir = "")
    {
        StartCoroutine(WalkToCoroutine(position, true, speed, afterDir));
    }

    /// <summary>
    /// そのまま動く
    /// </summary>
    /// <param name="position"></param>
    /// <param name="speed"></param>
    /// <param name="afterDir"></param>
    public void SlideTo(Vector3 position,
        float speed = 8f,
        string afterDir = "",
        DeltaFloat.MoveType moveType = DeltaFloat.MoveType.LINE)
    {
        StartCoroutine(WalkToCoroutine(position, false, speed, afterDir, moveType));
    }

    /// <summary>
    /// 歩き途中か
    /// </summary>
    /// <returns></returns>
    public bool IsWalking()
    {
        return walkPosition.IsActive();
    }

    /// <summary>
    /// 向き変更
    /// </summary>
    /// <param name="dir">向き</param>
    public virtual void SetDirection(Constant.Direction dir)
    {
        direction = dir;
        if (dir == Constant.Direction.None) return;

        if (generalDirection &&
            modelAnim?.isActiveAndEnabled == true)
        {
            modelAnim.Play(dir switch
            {
                Constant.Direction.Up => "up",
                Constant.Direction.Down => "down",
                Constant.Direction.Right => "right",
                _ => "left"
            });
        }
    }

    /// <summary>
    /// アニメーション指定
    /// </summary>
    /// <param name="anim"></param>
    public void PlayAnim(string anim)
    {
        modelAnim?.Play(anim);
    }

    #endregion

    #region 内部メソッド

    /// <summary>
    /// 歩くコルーチン
    /// </summary>
    /// <param name="target"></param>
    /// <param name="playAnim">歩くアニメーション</param>
    /// <param name="speed"></param>
    /// <param name="afterDir"></param>
    /// <returns></returns>
    private IEnumerator WalkToCoroutine(Vector3 target,
        bool playAnim = true,
        float speed = 1f,
        string afterDir = "",
        DeltaFloat.MoveType moveType = DeltaFloat.MoveType.LINE
        )
    {
        var vec = target - transform.position;
        var time = vec.magnitude / (WALK_VELOCITY * speed);

        walkPosition.Set(transform.position);
        walkPosition.MoveTo(target, time, moveType);

        var walkSpeed = vec / time;
        if (playAnim)
        {
            WalkStartAnim(walkSpeed);
        }
        while (walkPosition.IsActive())
        {
            yield return null;

            walkPosition.Update(Time.deltaTime);
            transform.position = walkPosition.Get();
        }
        if (playAnim)
        {
            WalkStopAnim();
        }

        if (string.IsNullOrEmpty(afterDir) == false)
        {
            modelAnim?.Play(afterDir);
        }
    }

    /// <summary>
    /// 歩き始めアニメーション再生
    /// </summary>
    /// <param name="vec"></param>
    virtual protected void WalkStartAnim(Vector3 vec)
    {
        modelAnim.SetFloat("speedX", vec.x);
        modelAnim.SetFloat("speedY", vec.y);

        // ゼロでなければ向きを設定
        if (vec.sqrMagnitude < 0.1f) return;
        if (Mathf.Abs(vec.x) > Mathf.Abs(vec.y))
        {
            if (vec.x > 0)
            {
                SetDirection(Constant.Direction.Right);
            }
            else
            {
                SetDirection(Constant.Direction.Left);
            }
        }
        else
        {
            if (vec.y > 0)
            {
                SetDirection(Constant.Direction.Up);
            }
            else
            {
                SetDirection(Constant.Direction.Down);
            }
        }
    }

    /// <summary>
    /// 歩き終わりアニメーション再生
    /// </summary>
    virtual protected void WalkStopAnim()
    {
        modelAnim.SetFloat("speedX", 0);
        modelAnim.SetFloat("speedY", 0);
    }

    /// <summary>
    /// カメラ更新
    /// </summary>
    protected void UpdateCamera()
    {
        if (enableCamera == false) return;

        var cam = ManagerSceneScript.GetInstance().mainCam;
        cam.SetTargetPos(gameObject);
    }

    /// <summary>
    /// カメラ有効設定
    /// </summary>
    /// <param name="enable"></param>
    public void SetCameraEnable(bool enable)
    {
        enableCamera = enable;
    }

    /// <summary>
    /// カメラ有効フラグ
    /// </summary>
    /// <returns></returns>
    public bool IsCameraEnable() { return enableCamera; }

    #endregion
}
