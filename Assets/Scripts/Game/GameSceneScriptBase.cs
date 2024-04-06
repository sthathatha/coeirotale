using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneScriptBase : MonoBehaviour
{
    public AudioClip bgmClip = null;

    // Start is called before the first frame update
    virtual public IEnumerator Start()
    {
        SetGameResult(false);
        ManagerSceneScript.GetInstance().SetGameScript(this);

        var cam = ManagerSceneScript.GetInstance().mainCam;
        cam.SetTargetPos(new Vector2(0, 0));
        cam.Immediate();

        yield break;
    }

    // Update is called once per frame
    virtual public void Update()
    {

    }

    /// <summary>
    /// フェードイン終わったらやること
    /// </summary>
    /// <returns></returns>
    virtual public IEnumerator AfterFadeIn()
    {
        yield break;
    }

    /// <summary>
    /// 負けた回数
    /// </summary>
    /// <returns></returns>
    protected int GetLoseCount()
    {
        return Global.GetTemporaryData().loseCount;
    }

    /// <summary>
    /// BGMClip
    /// </summary>
    /// <returns></returns>
    virtual public AudioClip GetBgmClip() { return  bgmClip; }

    /// <summary>
    /// 結果設定
    /// </summary>
    /// <param name="_win"></param>
    protected void SetGameResult(bool _win)
    {
        Global.GetTemporaryData().gameWon = _win;
    }
}
