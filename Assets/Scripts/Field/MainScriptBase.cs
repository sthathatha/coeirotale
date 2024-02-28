using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScriptBase : MonoBehaviour
{
    /// <summary>�X���[�v����Active=false����e�I�u�W�F�N�g</summary>
    public GameObject objectParent = null;

    /// <summary>
    /// �t�B�[���h�̏��
    /// </summary>
    public enum State : int
    {
        Idle = 0,
        Event,
    }

    /// <summary>
    /// �t�B�[���h�̏��
    /// </summary>
    public State FieldState
    {
        get; set;
    }

    // Start is called before the first frame update
    virtual protected IEnumerator Start()
    {
        // ���ڋN�����Ƀ}�l�[�W���N��
        if (!ManagerSceneScript.GetInstance())
        {
            ManagerSceneScript.isDebugLoad = true;
            SceneManager.LoadScene("_ManagerScene", LoadSceneMode.Additive);
            yield return null;
        }
        ManagerSceneScript.GetInstance().SetMainScript(this);

        FieldState = State.Idle;
    }

    // Update is called once per frame
    virtual protected void Update()
    {

    }

    /// <summary>
    /// �V�[����
    /// </summary>
    /// <returns></returns>
    virtual public string GetSceneName() { return "SampleScene"; }

    /// <summary>
    /// �Q�[���J�n�p�ɃX���[�v
    /// </summary>
    virtual public void Sleep()
    {
        objectParent?.SetActive(false);
    }

    /// <summary>
    /// �Q�[���I�����ɍĊJ
    /// </summary>
    virtual public void Awake()
    {
        objectParent?.SetActive(true);
    }

    /// <summary>
    /// �t�F�[�h�C�����O�ɂ�邱��
    /// </summary>
    /// <returns></returns>
    virtual public IEnumerator BeforeFadeIn()
    {
        yield break;
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
