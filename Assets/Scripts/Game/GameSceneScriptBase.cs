using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneScriptBase : MonoBehaviour
{
    // Start is called before the first frame update
    virtual public IEnumerator Start()
    {
        ManagerSceneScript.GetInstance().SetGameScript(this);
        yield break;
    }

    // Update is called once per frame
    virtual public void Update()
    {

    }

    /// <summary>
    /// �t�F�[�h�C���I��������邱��
    /// </summary>
    /// <returns></returns>
    virtual public IEnumerator AfterFadeIn()
    {
        yield break;
    }
}
