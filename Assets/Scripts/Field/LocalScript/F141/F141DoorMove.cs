using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F141　ドアに入った移動
/// </summary>
public class F141DoorMove : AreaEventBase
{
    /// <summary>
    /// 実行
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        var scr = GetComponent<F141Door>();

        if (scr.GetDoorType() == F141Door.DoorType.Success)
        {
            Global.GetSaveData().SetGameData(F141System.CLEAR_FLG, 1);

            ManagerSceneScript.GetInstance().LoadMainScene("Field142", 0);
        }
        else
        {
            var failed = Global.GetSaveData().GetGameDataInt(F141System.FAIL_COUNT_SAVE);
            if (failed < 10) failed++;
            Global.GetSaveData().SetGameData(F141System.FAIL_COUNT_SAVE, failed);

            ManagerSceneScript.GetInstance().LoadMainScene("Field141", 0);
        }

        yield break;
    }
}
