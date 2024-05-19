using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Z�[�u�|�C���g
/// </summary>
public class SavePointScript : AreaEventBase
{
    /// <summary>���[�h����GeneralPos�ԍ�</summary>
    public int loadPos;

    /// <summary>�Z�[�u����SE</summary>
    public AudioClip saveSe;

    /// <summary>�Z�[�u�������̕\��</summary>
    public GameObject savedSprite;

    /// <summary>�Z�[�u��</summary>
    private bool isSaving = false;

    /// <summary>
    /// ������
    /// </summary>
    public override void Start()
    {
        base.Start();
        savedSprite.SetActive(false);
    }

    /// <summary>
    /// ���s
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        if (isSaving) yield break;

        var fieldScene = gameObject.scene.name;
        var save = Global.GetSaveData();
        save.SetGameData("SaveFieldScene", fieldScene);
        save.SetGameData("SaveFieldPos", loadPos);

        save.SaveGameData();

        isSaving = true;
        StartCoroutine(SaveSpriteCoroutine());
    }

    /// <summary>
    /// �Z�[�u�\��
    /// </summary>
    /// <returns></returns>
    private IEnumerator SaveSpriteCoroutine()
    {
        ManagerSceneScript.GetInstance().soundMan.PlaySE(saveSe);
        savedSprite.SetActive(true);
        yield return new WaitForSeconds(1f);
        savedSprite.SetActive(false);
        isSaving = false;
    }
}
