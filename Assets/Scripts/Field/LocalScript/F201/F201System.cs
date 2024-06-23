using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

/// <summary>
/// F201　ラストダンジョン入口
/// </summary>
public class F201System : MainScriptBase
{
    #region 定数

    public const string DUNGEON_OPEN_FLG = "F201Open";

    #endregion

    #region メンバー

    public GameObject towerBefore;
    public GameObject towerAfter;

    public GameObject ami;
    public GameObject mana;
    public GameObject mati;
    public GameObject menderu;
    public GameObject matuka;
    public GameObject pierre;

    #endregion

    #region 基底

    /// <summary>
    /// 開始時
    /// </summary>
    /// <returns></returns>
    public override IEnumerator BeforeInitFadeIn()
    {
        yield return base.BeforeInitFadeIn();

        var save = Global.GetSaveData();

        if (save.GetGameDataInt(DUNGEON_OPEN_FLG) < 1)
        {
            towerBefore.SetActive(true);
            towerAfter.SetActive(false);

            ami.SetActive(true);
            mana.SetActive(true);
            mati.SetActive(true);
            matuka.SetActive(true);
            menderu.SetActive(true);
            pierre.SetActive(true);

            var pos4 = SearchGeneralPosition(4);
            var pos5 = SearchGeneralPosition(5);
            var pp = pos4.GetPosition();
            var tp = pos5.GetPosition();
            var player = playerObj.GetComponent<PlayerScript>();
            var tukuyomi = tukuyomiObj.GetComponent<TukuyomiScript>();
            player.SetCameraEnable(false);
            player.SetPosition(pp);
            tukuyomi.SetMode(TukuyomiScript.TukuyomiMode.NPC);
            tukuyomi.SetPosition(tp);

            var posCenter = SearchGeneralPosition(6);
            pp.y = posCenter.GetPosition().y;
            tp.y = posCenter.GetPosition().y;
            player.WalkTo(pp);
            tukuyomi.WalkTo(tp);

            var cam = ManagerSceneScript.GetInstance().mainCam;
            cam.SetTargetPos(posCenter.gameObject);
            cam.Immediate();
        }
        else
        {
            towerBefore.SetActive(false);
            towerAfter.SetActive(true);
        }
    }

    /// <summary>
    /// フェードイン後
    /// </summary>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn()
    {
        yield return base.AfterFadeIn();

        if (Global.GetSaveData().GetGameDataInt(DUNGEON_OPEN_FLG) < 1)
        {
            GetComponent<F201Start>().ExecEvent();
        }
    }

    /// <summary>
    /// BGM
    /// </summary>
    /// <returns></returns>
    public override Tuple<SoundManager.FieldBgmType, AudioClip> GetBgm()
    {
        return new Tuple<SoundManager.FieldBgmType, AudioClip>(SoundManager.FieldBgmType.Common1, null);
    }

    #endregion
}
