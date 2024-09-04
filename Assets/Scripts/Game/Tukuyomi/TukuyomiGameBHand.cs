using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// つくよみちゃん最終戦　手
/// </summary>
public class TukuyomiGameBHand : MonoBehaviour
{
    public bool IsRight;
    public Sprite sp_normal;
    public Sprite sp_shake;
    public Sprite sp_finger;

    private float posRot = 0f;
    private Vector3 basePos;
    private Vector3 nowPos;

    /// <summary>
    /// 初期化
    /// </summary>
    private void Start()
    {
        basePos = transform.localPosition;
        posRot = IsRight ? 0f : Mathf.PI / 2f;
        GetComponent<ModelUtil>().FadeOutImmediate();
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        // 常に上下している
        posRot -= Mathf.PI * Time.deltaTime;
        if (posRot < 0f) posRot += Mathf.PI * 2f;

        var addPos = Mathf.Sin(posRot) * 20f;
        nowPos = basePos + new Vector3(0, addPos);
        transform.localPosition = nowPos;
    }

    /// <summary>
    /// 手の形
    /// </summary>
    /// <param name="phase">0:通常　1:握り　2:指差し</param>
    /// <returns></returns>
    public void ShowHand(int phase)
    {
        GetComponent<SpriteRenderer>().sprite = phase switch
        {
            0 => sp_normal,
            1 => sp_shake,
            _ => sp_finger,
        };
    }

    /// <summary>
    /// 発射の指先座標を取得
    /// </summary>
    /// <returns></returns>
    public Vector3 GetShotRoot()
    {
        return nowPos + new Vector3(IsRight ? -100f : 100f, -97f);
    }
}
