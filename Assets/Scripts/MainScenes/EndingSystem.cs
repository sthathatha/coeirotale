using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G���f�B���O
/// </summary>
public class EndingSystem : MainScriptBase
{
    public AudioClip endingBgm;

    /// <summary>
    /// �t�F�[�h�C����
    /// </summary>
    /// <param name="init"></param>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn(bool init)
    {
        yield return base.AfterFadeIn(init);
        Global.GetSaveData().system.clearFlag = 1;
        StartCoroutine(EndingCoroutine());
    }

    /// <summary>
    /// �G���f�B���O�Đ�
    /// </summary>
    /// <returns></returns>
    private IEnumerator EndingCoroutine()
    {
        var manager = ManagerSceneScript.GetInstance();
        manager.soundMan.PlayFieldBgm(SoundManager.FieldBgmType.None, endingBgm);

        //todo:




        // ���͑҂�
        var input = InputManager.GetInstance();
        yield return new WaitUntil(() => input.GetKeyPress(InputManager.Keys.South));
        manager.LoadMainScene("TitleScene", 0);
    }
}
