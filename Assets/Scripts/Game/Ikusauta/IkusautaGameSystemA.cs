using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �R�̃}�`�~�j�Q�[���Ǘ��X�N���v�g
/// </summary>
public class IkusautaGameSystemA : GameSceneScriptBase
{
    #region �萔
    /// <summary>
    /// �L�����\���p�^�[��
    /// </summary>
    private enum CharacterPattern : int
    {
        Waiting = 0,
        TukuyomiWin,
        MatiWin,
    }

    /// <summary>
    /// �D�\���p�^�[��
    /// </summary>
    private enum HudaPattern : int
    {
        None = 0,
        TukuyomiWin,
        MatiWin,
        Sikiri,
    }
    #endregion

    #region �����o�[
    /// <summary>�ҋ@���\���L����</summary>
    public GameObject waitingCharacter = null;
    /// <summary>����݂���񏟗����\���L����</summary>
    public GameObject tukuyomiWinCharacter = null;
    /// <summary>�}�`�������\���L����</summary>
    public GameObject matiWinCharacter = null;

    /// <summary>�~�j�t�F�[�_</summary>
    public IkusautaGameMiniFader miniFader = null;

    /// <summary>�r�b�N���}�[�N</summary>
    public GameObject ui_bikkuri = null;

    /// <summary>����݂�����</summary>
    public IkusautaGameZoomFace ui_zoom_tukuyomi = null;
    /// <summary>�}�`��</summary>
    public IkusautaGameZoomFace ui_zoom_mati = null;

    /// <summary>����</summary>
    public GameObject ui_huda_winner = null;
    /// <summary>�d�؂蒼��</summary>
    public GameObject ui_huda_sikiri = null;
    /// <summary>����݂����</summary>
    public GameObject ui_huda_tukuyomi = null;
    /// <summary>�}�`</summary>
    public GameObject ui_huda_mati = null;
    /// <summary>�D�}�X�N</summary>
    public IkusautaGameHudaMask ui_huda_mask = null;

    /// <summary>����݂����}��</summary>
    public GameObject ui_maru_tukuyomi = null;
    /// <summary>�}�`�}��</summary>
    public GameObject ui_maru_mati = null;
    /// <summary>����݂����o�c</summary>
    public GameObject ui_batu_tukuyomi = null;
    /// <summary>����݂����o�c�@�L������</summary>
    public GameObject ui_batu_tukuyomi_chara = null;

    /// <summary>��</summary>
    public GameObject grasses = null;

    /// <summary>�I���o�鉹</summary>
    public AudioClip se_bikkuri = null;
    /// <summary></summary>
    public AudioClip se_fault = null;
    /// <summary>�}�`����SE</summary>
    public AudioClip se_mati_win = null;
    /// <summary>����݂���񏟗�SE</summary>
    public AudioClip se_tukuyomi_win = null;
    /// <summary>�ڔ�SE</summary>
    public AudioClip se_syakuhati = null;

    #endregion

    #region �v���C�x�[�g�ϐ�
    /// <summary>�d�؂蒼��</summary>
    private int faultCount;
    /// <summary>����݂���񏟗��_</summary>
    private int tukuyomiWinCount;
    /// <summary>�}�`�����_</summary>
    private int matiWinCount;
    #endregion

    #region ��ꏈ��
    /// <summary>
    /// ������
    /// </summary>
    /// <returns></returns>
    override public IEnumerator Start()
    {
        faultCount = 0;
        tukuyomiWinCount = 0;
        matiWinCount = 0;
        UpdateScoreUI();
        ShowCharacter(CharacterPattern.Waiting);
        miniFader.Hide();
        ui_zoom_tukuyomi.gameObject.SetActive(false);
        ui_zoom_mati.gameObject.SetActive(false);
        ui_huda_tukuyomi.SetActive(false);
        ui_huda_mati.SetActive(false);
        ui_huda_winner.SetActive(false);
        ui_huda_sikiri.SetActive(false);
        ui_maru_tukuyomi.SetActive(false);
        ui_maru_mati.SetActive(false);
        ui_batu_tukuyomi.SetActive(false);
        ui_batu_tukuyomi_chara.SetActive(false);

        for (var i = 0; i < grasses.transform.childCount; ++i)
        {
            var grassAnim = grasses.transform.GetChild(i).gameObject.GetComponent<Animator>();

            grassAnim.PlayInFixedTime("obj_grass", 0, Util.RandomFloat(0, 1));
        }

        yield return base.Start();
    }
    #endregion

    #region �R���[�`��
    /// <summary>
    /// �t�F�[�h�C����̏�������
    /// </summary>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn()
    {
        var tutorial = ManagerSceneScript.GetInstance().GetMinigameTutorialWindow();
        var input = InputManager.GetInstance();

        yield return base.AfterFadeIn();
        yield return new WaitForSeconds(1f);

        // �`���[�g���A���\��
        tutorial.SetTitle(StringMinigameMessage.MatiA_Title);
        tutorial.SetText(StringMinigameMessage.MatiA_Tutorial);
        yield return tutorial.Open();
        yield return new WaitUntil(() => input.GetKeyPress(InputManager.Keys.South));
        yield return tutorial.Close();

        yield return new WaitForSeconds(0.5f);

        // �J�n
        StartCoroutine(GameStart());
    }

    /// <summary>
    /// �����J�n�̗���
    /// </summary>
    /// <returns></returns>
    private IEnumerator GameStart()
    {
        var input = InputManager.GetInstance();

        // �Â�����
        PlaySE(se_syakuhati);
        yield return miniFader.FadeOutDark(true);

        // �㉺�̊�A�j���[�V����
        ui_zoom_tukuyomi.gameObject.SetActive(true);
        ui_zoom_mati.gameObject.SetActive(true);
        StartCoroutine(ui_zoom_tukuyomi.PlayAnim());
        StartCoroutine(ui_zoom_mati.PlayAnim());
        yield return new WaitWhile(() => ui_zoom_tukuyomi.IsActive());
        yield return new WaitForSeconds(1f);
        ui_zoom_tukuyomi.gameObject.SetActive(false);
        ui_zoom_mati.gameObject.SetActive(false);

        // ���邭����
        yield return miniFader.FadeOutDark(false);

        // �I���o�鎞��
        var waitTime = Util.RandomFloat(2f, 7f);

        while (waitTime > 0f)
        {
            if (input.GetKeyPress(InputManager.Keys.South))
            {
                // �o��O�ɉ������玸�s�{
                StartCoroutine(FaultCoroutine());
                yield break;
            }

            yield return null;
            waitTime -= Time.deltaTime;
        }

        // �I�o��
        yield return new WaitUntil(() => se_bikkuri.loadState == AudioDataLoadState.Loaded);
        PlaySE(se_bikkuri, 0.04f);
        ui_bikkuri.SetActive(true);
        // �}�`�̔������x
        var matiTime = CalcMatiTime();

        while (matiTime > 0f)
        {
            yield return null;
            
            if (input.GetKeyPress(InputManager.Keys.South))
            {
                // �}�`��葁�������珟��
                StartCoroutine(TukuyomiWinCoroutine());
                yield break;
            }
            
            matiTime -= Time.deltaTime;
        }

        // �}�`�̏���
        StartCoroutine(MatiWinCoroutine());
    }

    /// <summary>
    /// �t���C���O�Ŏd�؂蒼��
    /// </summary>
    /// <returns></returns>
    private IEnumerator FaultCoroutine()
    {
        var manager = ManagerSceneScript.GetInstance();

        PlaySE(se_fault);
        // �L�����Ƀo�c�\��
        ui_batu_tukuyomi_chara.SetActive(true);

        // �D�\��
        ShowWinnerHuda(HudaPattern.Sikiri);
        yield return HudaMaskShowCoroutine();

        // �X�R�A
        AddFault();

        if (matiWinCount >= 2)
        {
            // �I�����ăt�B�[���h�ɖ߂�
            SetGameResult(false);
            ManagerSceneScript.GetInstance().ExitGame();
            yield break;
        }
        else
        {
            // �t�F�[�h�A�E�g���ĕ\���X�V�A���̃Q�[���J�n
            yield return manager.FadeOut();
            UpdateScoreUI();
            ui_batu_tukuyomi_chara.SetActive(false);
            yield return manager.FadeIn();
            StartCoroutine(GameStart());
        }
    }

    /// <summary>
    /// ����݂���񏟗�
    /// </summary>
    /// <returns></returns>
    private IEnumerator TukuyomiWinCoroutine()
    {
        var manager = ManagerSceneScript.GetInstance();
        PlaySE(se_tukuyomi_win);

        // �r�b�N���}�[�N����
        ui_bikkuri.SetActive(false);

        // ��
        StartCoroutine(miniFader.Flash());

        // �L�����ƎD�\��
        ShowCharacter(CharacterPattern.TukuyomiWin);
        ShowWinnerHuda(HudaPattern.TukuyomiWin);
        yield return HudaMaskShowCoroutine();

        // �X�R�A
        AddTukuyomiWin();

        if (tukuyomiWinCount >= 2)
        {
            // �I�����ăt�B�[���h�ɖ߂�
            SetGameResult(true);
            ManagerSceneScript.GetInstance().ExitGame();
            yield break;
        }
        else
        {
            // �t�F�[�h�A�E�g���ĕ\���X�V�A���̃Q�[���J�n
            yield return manager.FadeOut();
            UpdateScoreUI();
            ShowCharacter(CharacterPattern.Waiting);
            yield return manager.FadeIn();
            StartCoroutine(GameStart());
        }
    }

    /// <summary>
    /// �}�`����
    /// </summary>
    /// <returns></returns>
    private IEnumerator MatiWinCoroutine()
    {
        var manager = ManagerSceneScript.GetInstance();
        PlaySE(se_mati_win);

        // �r�b�N���}�[�N����
        ui_bikkuri.SetActive(false);

        // ��
        StartCoroutine(miniFader.Flash());

        // �L�����ƎD�\��
        ShowCharacter(CharacterPattern.MatiWin);
        ShowWinnerHuda(HudaPattern.MatiWin);
        yield return HudaMaskShowCoroutine();

        // �X�R�A
        AddMatiWin();

        if (matiWinCount >= 2)
        {
            // �I�����ăt�B�[���h�ɖ߂�
            SetGameResult(false);
            ManagerSceneScript.GetInstance().ExitGame();
            yield break;
        }
        else
        {
            // �t�F�[�h�A�E�g���ĕ\���X�V�A���̃Q�[���J�n
            yield return manager.FadeOut();
            UpdateScoreUI();
            ShowCharacter(CharacterPattern.Waiting);
            yield return manager.FadeIn();
            StartCoroutine(GameStart());
        }
    }

    /// <summary>
    /// �D�̕\��
    /// </summary>
    /// <returns></returns>
    private IEnumerator HudaMaskShowCoroutine()
    {
        yield return ui_huda_mask.ShowHuda(true);
        yield return new WaitForSeconds(2f);
        yield return ui_huda_mask.ShowHuda(false);
        yield return new WaitForSeconds(1f);
    }

    #endregion

    #region �v���C�x�[�g
    /// <summary>
    /// �}�`�̔������x����
    /// </summary>
    /// <returns></returns>
    private float CalcMatiTime()
    {
        var rand = Util.RandomFloat(0.2f, 0.25f);
        if (GetLoseCount() >= 3)
        {
            rand += (GetLoseCount() - 2) * 0.02f;
        }

        return rand;
    }

    /// <summary>
    /// �t���C���O�񐔉��Z
    /// </summary>
    private void AddFault()
    {
        faultCount++;
        if (faultCount >= 2)
        {
            AddMatiWin();
        }
    }

    /// <summary>
    /// �}�`�����񐔉��Z
    /// </summary>
    private void AddMatiWin()
    {
        matiWinCount++;
        faultCount = 0;
    }

    /// <summary>
    /// ����݂���񏟗��񐔉��Z
    /// </summary>
    private void AddTukuyomiWin()
    {
        tukuyomiWinCount++;
        faultCount = 0;
    }

    /// <summary>
    /// �����񐔂̕\���X�V
    /// </summary>
    private void UpdateScoreUI()
    {
        ui_maru_tukuyomi.SetActive(tukuyomiWinCount == 1);
        ui_maru_mati.SetActive(matiWinCount == 1);
        ui_batu_tukuyomi.SetActive(faultCount == 1);
    }

    /// <summary>
    /// �L�����N�^�[�\���X�V
    /// </summary>
    /// <param name="scene"></param>
    private void ShowCharacter(CharacterPattern scene)
    {
        waitingCharacter.SetActive(scene == CharacterPattern.Waiting);
        tukuyomiWinCharacter.SetActive(scene == CharacterPattern.TukuyomiWin);
        matiWinCharacter.SetActive(scene == CharacterPattern.MatiWin);
    }

    /// <summary>
    /// �D�\���X�V
    /// </summary>
    /// <param name="scene"></param>
    private void ShowWinnerHuda(HudaPattern scene)
    {
        ui_huda_mati.SetActive(scene == HudaPattern.MatiWin);
        ui_huda_tukuyomi.SetActive(scene == HudaPattern.TukuyomiWin);
        ui_huda_sikiri.SetActive(scene == HudaPattern.Sikiri);
        ui_huda_winner.SetActive(scene == HudaPattern.TukuyomiWin || scene == HudaPattern.MatiWin);
    }

    /// <summary>
    /// SE�Đ�
    /// </summary>
    /// <param name="se"></param>
    /// <param name="startTime"></param>
    private void PlaySE(AudioClip se, float startTime = 0f)
    {
        ManagerSceneScript.GetInstance().soundMan.PlaySE(se, startTime);
    }
    #endregion
}
