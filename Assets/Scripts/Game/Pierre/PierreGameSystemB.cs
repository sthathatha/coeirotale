using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PierreGameSystemB : GameSceneScriptBase
{
    #region �萔

    /// <summary>���I�u�W�F�N�g�����ʒu</summary>
    private const float OBJ_INIT_X = (Constant.SCREEN_WIDTH + PierreGameBGObject.OBJECT_WIDTH_MAX) / (-2f);

    #endregion

    #region �����o�[

    /// <summary>�I�u�W�F�N�g�e</summary>
    public GameObject objectParent = null;
    /// <summary>�n�ʃe���v��</summary>
    public GameObject ground_dummy = null;
    /// <summary>�{�[��0�̃e���v���[�g</summary>
    public GameObject ball_dummy = null;

    /// <summary>�s�G�[��A</summary>
    public PierreGameBPlayer pierreA = null;
    /// <summary>�s�G�[��B</summary>
    public PierreGameBEnemy pierreB = null;

    #endregion

    #region �v���C�x�[�g

    /// <summary>�ŐV�̒n��</summary>
    private GameObject newBG = null;

    #endregion

    /// <summary>
    /// ������
    /// </summary>
    /// <returns></returns>
    override public IEnumerator Start()
    {
        yield return base.Start();

        GenerateInitObjects();

        StartCoroutine(GenerateGroundCoroutine());
    }

    /// <summary>
    /// �t�F�[�h�C����Q�[���J�n
    /// </summary>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn()
    {
        var tutorial = ManagerSceneScript.GetInstance().GetMinigameTutorialWindow();
        var input = InputManager.GetInstance();

        yield return base.AfterFadeIn();

        tutorial.SetTitle(StringMinigameMessage.PierreB_Title);
        tutorial.SetText(StringMinigameMessage.PierreB_Tutorial);
        yield return tutorial.Open();
        yield return new WaitUntil(() => input.GetKeyPress(InputManager.Keys.South));
        yield return tutorial.Close();

        //pierreA.StartGame();
    }

    #region �@�\�Ăяo��
    /// <summary>
    /// �{�[������
    /// </summary>
    /// <param name="farPosition"></param>
    /// <param name="ballType"></param>
    public void GenerateBall(float farPosition, PierreGameBall.BallType ballType)
    {
        var ball = Instantiate(ball_dummy);
        ball.transform.SetParent(objectParent.transform, false);
        ball.transform.localPosition = new Vector3(-500, 0, 0);
        var scr = ball.GetComponent<PierreGameBall>();
        scr.SetFarPosition(farPosition);
        scr.SetBallType(ballType);
    }

    #endregion

    #region �Q�[�����ꏈ��

    #endregion

    #region �w�i�I�u�W�F�N�g�܂��

    /// <summary>
    /// �����I�u�W�F�N�g�쐬
    /// </summary>
    private void GenerateInitObjects()
    {
        // BG�@�E�[�������΂��������
        newBG = Instantiate(ground_dummy);
        newBG.transform.SetParent(objectParent.transform);
        newBG.transform.localPosition = new Vector3(0, 0);
    }

    /// <summary>
    /// �n�ʂ���ɐ���
    /// </summary>
    /// <returns></returns>
    private IEnumerator GenerateGroundCoroutine()
    {
        while (true)
        {
            yield return new WaitUntil(() => newBG.transform.localPosition.x > 0f);

            var bg = Instantiate(ground_dummy);
            bg.transform.SetParent(objectParent.transform, false);
            bg.transform.localPosition = new Vector3(newBG.transform.localPosition.x - Constant.SCREEN_WIDTH, 0);

            newBG = bg;
        }
    }
    #endregion
}
