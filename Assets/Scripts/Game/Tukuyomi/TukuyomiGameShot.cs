using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// つくよみちゃん戦　弾
/// </summary>
public class TukuyomiGameShot : MonoBehaviour
{
    public enum ShotType
    {
        /// <summary>プレイヤーの弾　攻撃判定扱い</summary>
        Player = 0,
        /// <summary>香車の弾　くらい判定扱い</summary>
        Enemy,
    }

    /// <summary>削除予約</summary>
    public bool DestroyWait { get; set; } = false;

    /// <summary>飛んでいく向き</summary>
    private Vector3 shotVec;

    /// <summary>タイプ</summary>
    private ShotType shotType;

    /// <summary>システムアクセス用</summary>
    public TukuyomiGameSystem system;

    /// <summary>
    ///  発射
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="vec"></param>
    /// <param name="enemy"></param>
    public void Shoot(Vector3 pos, Vector3 vec, ShotType type)
    {
        shotType = type;
        gameObject.SetActive(true);

        // タイプ設定
        if (type == ShotType.Player)
        {
            // プレイヤーの弾はisTrigger攻撃判定
            GetComponent<Collider2D>().isTrigger = true;
        }
        else
        {
            // 敵の弾は通常コリジョン
            GetComponent<Collider2D>().isTrigger = false;
        }

        // 角度
        transform.localRotation = Util.GetRotateQuaternion(Util.GetRadianFromVector(vec));
        shotVec = vec;
        // 位置
        transform.localPosition = pos;
        StartCoroutine(ShootCoroutine());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShootCoroutine()
    {
        const float MaxX = Constant.SCREEN_WIDTH / 2f + 64f;
        const float MinX = -MaxX;
        const float MaxY = Constant.SCREEN_HEIGHT / 2f + 64f;
        const float MinY = -MaxY;

        while (true)
        {
            yield return null;
            var p = transform.localPosition + shotVec * Time.deltaTime;
            transform.localPosition = p;

            var wp = transform.position;
            if (wp.x > MaxX || wp.x < MinX || wp.y > MaxY || wp.y < MinY || DestroyWait) break;

            if (shotType == ShotType.Enemy)
            {
                // 主人公の位置に到達
                if (system.reko.gameObject.activeSelf && p.y < 20f)
                {
                    system.PlayerDamage();
                    DestroyWait = true;
                }
            }
        }

        system.ShotRemove(this);
        // 外に行ったら消える
        Destroy(gameObject);
    }

    /// <summary>
    /// 攻撃うけたらお互い消える
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var shotParam = collision.GetComponent<TukuyomiGameShot>();
        if (shotParam == null) return; // ショット以外（無いと思う）
        shotParam.DestroyWait = true;

        DestroyWait = true;
    }
}
