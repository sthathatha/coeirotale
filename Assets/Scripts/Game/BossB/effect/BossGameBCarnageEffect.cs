using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// ラスボス本戦　カーネイジエフェクト
/// </summary>
public class BossGameBCarnageEffect : BossGameBEffectBase
{
    public const float CARNAGE_TIME = 4f;
    private const float X_RIGHT = Constant.SCREEN_WIDTH / 2f;
    private const float X_LEFT = -Constant.SCREEN_WIDTH / 2f;
    private const float Y_TOP = Constant.SCREEN_HEIGHT / 2f;
    private const float Y_BOTTOM = -Constant.SCREEN_HEIGHT / 2f;

    /// <summary>
    /// 
    /// </summary>
    private enum HeadPos
    {
        Right = 0,
        Left = 1,
        Top = 2,
        Bottom = 3,
    }

    public AudioClip se_shot;
    public BossGameBCarnageLine lineDummy;

    private List<BossGameBCarnageLine> lineList;

    /// <summary>
    /// 再生
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Play()
    {
        lineDummy.gameObject.SetActive(false);

        var sound = ManagerSceneScript.GetInstance().soundMan;
        var time = CARNAGE_TIME;
        var headPos = transform.position;
        transform.position = Vector3.zero;
        lineList = new List<BossGameBCarnageLine>();
        var beforeHead = headPos;
        headPos = CreateHead(beforeHead);
        var line = Instantiate(lineDummy, transform);
        line.StartLine(beforeHead, headPos);
        lineList.Add(line);
        sound.PlaySE(se_shot);

        while (time > 0f)
        {
            yield return null;
            time -= Time.deltaTime;

            if (!lineList[lineList.Count - 1].IsHeadMoving())
            {
                // 先頭がもう動いてない
                beforeHead = headPos;
                headPos = CreateHead(beforeHead);
                line = Instantiate(lineDummy, transform);
                line.StartLine(beforeHead, headPos);
                lineList.Add(line);
                sound.PlaySE(se_shot);
            }
        }

        while (lineList.Any())
        {
            yield return null;
            if (!lineList[0].IsTailMoving()) Destroy(lineList[0].gameObject);
            lineList.RemoveAt(0);
        }
    }

    /// <summary>
    /// ライン作成
    /// </summary>
    /// <param name="beforeHead">前回の先端位置</param>
    /// <returns></returns>
    private Vector3 CreateHead(Vector3 beforeHead)
    {
        const float LIMIT_RIGHT = X_RIGHT * 0.9f;
        const float LIMIT_LEFT = X_LEFT * 0.9f;
        const float LIMIT_TOP = Y_TOP * 0.9f;
        const float LIMIT_BOTTOM = Y_BOTTOM * 0.9f;

        var nextLine = HeadPos.Right;
        var rand = Util.RandomInt(0, 99);

        if (beforeHead.x > LIMIT_RIGHT)
        {
            // 右なら次は上左下
            if (rand < 35) nextLine = HeadPos.Top;
            else if (rand < 70) nextLine = HeadPos.Bottom;
            else nextLine = HeadPos.Left;
        }
        else if (beforeHead.x < LIMIT_LEFT)
        {
            // 左なら次は上右下
            if (rand < 35) nextLine = HeadPos.Top;
            else if (rand < 70) nextLine = HeadPos.Bottom;
            else nextLine = HeadPos.Right;
        }
        else if (beforeHead.y > LIMIT_TOP)
        {
            // 上なら次は左右下
            if (rand < 35) nextLine = HeadPos.Right;
            else if (rand < 70) nextLine = HeadPos.Left;
            else nextLine = HeadPos.Bottom;
        }
        else if (beforeHead.y < LIMIT_BOTTOM)
        {
            // 下なら次は左右上
            if (rand < 35) nextLine = HeadPos.Right;
            else if (rand < 70) nextLine = HeadPos.Left;
            else nextLine = HeadPos.Top;
        }
        else
        {
            // 初期位置は上
            nextLine = HeadPos.Top;
        }

        switch (nextLine)
        {
            case HeadPos.Top: return new Vector3(Util.RandomFloat(LIMIT_LEFT, LIMIT_RIGHT), Y_TOP);
            case HeadPos.Bottom: return new Vector3(Util.RandomFloat(LIMIT_LEFT, LIMIT_RIGHT), Y_BOTTOM);
            case HeadPos.Right: return new Vector3(X_RIGHT, Util.RandomFloat(LIMIT_BOTTOM, LIMIT_TOP));
            default: return new Vector3(X_LEFT, Util.RandomFloat(LIMIT_BOTTOM, LIMIT_TOP));
        }
    }
}
