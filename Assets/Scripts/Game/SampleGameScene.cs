using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleGameScene : GameSceneScriptBase
{
    /// <summary>
    /// ������
    /// </summary>
    /// <returns></returns>
    override public IEnumerator Start()
    {
        yield return base.Start();
    }

    /// <summary>
    /// �t���[������
    /// </summary>
    override public void Update()
    {
        base.Update();

        if (ManagerSceneScript.GetInstance().SceneState != ManagerSceneScript.State.Game) { return; }

        if (InputManager.GetInstance().GetKeyPress(InputManager.Keys.South))
        {
            // �I�����ăt�B�[���h�ɖ߂�
            SetGameResult(false);
            ManagerSceneScript.GetInstance().ExitGame();
        }
    }
}
