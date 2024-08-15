using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����݂�����
/// </summary>
public class TukuyomiGameSystem : GameSceneScriptBase
{
    public AudioClip bgm_scene1;

    /// <summary>
    /// �t�F�[�h�C����
    /// </summary>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn()
    {
        yield return base.AfterFadeIn();
        StartCoroutine(MainCoroutine());
    }

    /// <summary>
    /// ���C���i�s
    /// </summary>
    /// <returns></returns>
    private IEnumerator MainCoroutine()
    {
        var manager = ManagerSceneScript.GetInstance();
        manager.soundMan.StartGameBgm(bgm_scene1);

        //todo:�e�X�g
        var input = InputManager.GetInstance();
        while (true)
        {
            yield return null;
            if (input.GetKeyPress(InputManager.Keys.South))
            {
                Global.GetTemporaryData().gameWon = true;
                break;
            }
            else if(input.GetKeyPress(InputManager.Keys.East))
            {
                Global.GetTemporaryData().gameWon = false;
                break;
            }
        }
        ManagerSceneScript.GetInstance().ExitGame();

    }
}
