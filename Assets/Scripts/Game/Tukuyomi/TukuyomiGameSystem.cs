using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ����݂�����
/// </summary>
public class TukuyomiGameSystem : GameSceneScriptBase
{
    #region �萔

    private readonly bool DEBUG_SERIF = false;
    private readonly bool DEBUG_A_SKIP = false;

    /// <summary></summary>
    public const string START_FLG = "TukuGameShow";

    /// <summary>
    /// �R�}�̎��
    /// </summary>
    public enum Koma : int
    {
        None = 0,
        Ou,
        Hisya,
        Kaku,
        Kin,
        Gin,
        Kei,
        Kyou,
        Hu,
    }

    /// <summary>�R�}�I�����̈ʒu</summary>
    private const float KOMA_SELECT_INT = 160f;
    private const float KOMA_X_KEI = 0f;
    private const float KOMA_X_GIN = KOMA_X_KEI - KOMA_SELECT_INT;
    private const float KOMA_X_KIN = KOMA_X_GIN - KOMA_SELECT_INT;
    private const float KOMA_X_KYOU = KOMA_X_KEI + KOMA_SELECT_INT;
    private const float KOMA_X_HU = KOMA_X_KYOU + KOMA_SELECT_INT;
    private const float KOMA_X_HISYA = 200f;
    private const float KOMA_X_KAKU = -200f;

    /// <summary>�R�}�I�����̂���݂����ʒu</summary>
    private const float KOMA_SELECT_TUKUYOMI_POS = -500f;

    /// <summary>�t�B�[���h��Y�ʒu�@��{</summary>
    private const float AFIELD_Y_DEFAULT = -172f;
    /// <summary>�t�B�[���h��Y�ʒu�@����</summary>
    private const float AFIELD_Y_KYOU = -270f;

    #endregion

    #region �����o�[

    public AudioClip bgm_scene1;
    public AudioClip bgm_scene2;
    public TukuyomiGameResource resource;

    public TukuyomiGameTukuyomiA tukuyomiA;
    public TukuyomiGamePlayer reko;
    public TukuyomiGameTukuyomiB tukuyomiB;

    public TukuyomiGamePlayField playField;

    public TukuyomiGameKomaBig koma_hisya;
    public TukuyomiGameKomaBig koma_kaku;
    public TukuyomiGameKomaBig koma_kin;
    public TukuyomiGameKomaBig koma_gin;
    public TukuyomiGameKomaBig koma_kei;
    public TukuyomiGameKomaBig koma_kyou;
    public TukuyomiGameKomaBig koma_hu;
    public TukuyomiGameKomaSmall small_dummy;
    public TukuyomiGameKomaSmallB smallb_dummy;
    public TukuyomiGameKomaSmallC smallc_dummy;
    public TukuyomiGameAttackEffect attack_dummy;
    public TukuyomiGameShot shot_dummy;
    public TukuyomiGameLaser laser_dummy;
    public TukuyomiGameWarningEffect warning_dummy;
    public TukuyomiGameBAttackParent blast_dummy;

    public Transform objectParent;

    public TukuyomiGameMessageUI serifUI;
    public TukuyomiGameMessageUI systemUI;
    public TukuyomiGameLifeUI lifeUI;
    public TukuyomiGameLifeUI lifeUIT;
    public GameObject pressUI;

    #endregion

    #region �ϐ�

    private bool flg_beat_kin;
    private bool flg_beat_gin;
    private bool flg_beat_kei;
    private bool flg_beat_kyou;
    private bool flg_beat_hu;
    private int NokoriKoma
    {
        get
        {
            var ret = 0;
            if (!flg_beat_kin) ++ret;
            if (!flg_beat_gin) ++ret;
            if (!flg_beat_kei) ++ret;
            if (!flg_beat_kyou) ++ret;
            if (!flg_beat_hu) ++ret;
            return ret;
        }
    }

    private int reko_life = 5;
    public int tukuyomi_life { get; set; } = 5;

    /// <summary>�_���[�W�ň�U��������</summary>
    private bool reko_damage_hide_mode = false;

    #endregion

    #region ���

    /// <summary>
    /// �t�F�[�h�C�����O
    /// </summary>
    /// <returns></returns>
    public override IEnumerator Start()
    {
        yield return base.Start();

        koma_hisya.Show(false);
        koma_kaku.Show(false);
        koma_kin.Show(false);
        koma_gin.Show(false);
        koma_kei.Show(false);
        koma_kyou.Show(false);
        koma_hu.Show(false);

        small_dummy.gameObject.SetActive(false);
        smallb_dummy.gameObject.SetActive(false);
        smallc_dummy.gameObject.SetActive(false);
        attack_dummy.gameObject.SetActive(false);
        shot_dummy.gameObject.SetActive(false);
        laser_dummy.gameObject.SetActive(false);
        warning_dummy.gameObject.SetActive(false);

        flg_beat_kin = false;
        flg_beat_gin = false;
        flg_beat_kei = false;
        flg_beat_kyou = false;
        flg_beat_hu = false;
        if (DEBUG_A_SKIP)
        {
            flg_beat_kin = true;
            flg_beat_gin = true;
            flg_beat_kei = true;
            flg_beat_kyou = true;
        }

        serifUI.Hide();
        systemUI.Hide();
        lifeUI.Hide();
        lifeUIT.Hide();
        pressUI.SetActive(false);

        // �J�n���̏��
        tukuyomiA.gameObject.SetActive(true);
        tukuyomiA.PlayAnim(TukuyomiGameTukuyomiA.AnimType.Before);
        ADefaultField();
    }

    /// <summary>
    /// �t�F�[�h�C����
    /// </summary>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn()
    {
        yield return base.AfterFadeIn();
        StartCoroutine(MainACoroutine());
    }

    #endregion

    #region �_���[�W����

    /// <summary>
    /// �v���C���[�̃_���[�W
    /// </summary>
    public void PlayerDamage()
    {
        resource.PlaySE(resource.se_player_damage);
        reko_life--;
        lifeUI.ShowLife(reko_life);

        if (reko_life > 0)
        {
            if (reko_damage_hide_mode) reko.gameObject.SetActive(false);
            return;
        }

        // 0�ɂȂ�����Q�[���I��
        reko.gameObject.SetActive(false);
        StartCoroutine(LoseExitCoroutine());
    }

    /// <summary>
    /// �s�k�I��
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoseExitCoroutine()
    {
        SetGameResult(false);
        yield return new WaitForSeconds(2f);
        ManagerSceneScript.GetInstance().ExitGame();
    }

    #endregion

    #region �V���b�g�Ǘ�

    private List<TukuyomiGameShot> shotList = new List<TukuyomiGameShot>();

    /// <summary>
    /// �V���b�g�쐬
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="vec"></param>
    /// <param name="type"></param>
    public void CreateShot(Vector3 pos, Vector3 vec, TukuyomiGameShot.ShotType type)
    {
        var shot = Instantiate(shot_dummy, playField.transform);
        shotList.Add(shot);

        shot.Shoot(pos, vec, type);
    }

    /// <summary>
    /// ���X�g����V���b�g���폜
    /// </summary>
    /// <param name="s"></param>
    public void ShotRemove(TukuyomiGameShot s)
    {
        shotList.Remove(s);
    }

    /// <summary>
    /// ���[�U�[����
    /// </summary>
    /// <param name="root"></param>
    /// <param name="rot"></param>
    public void CreateLaser(Vector3 root, float rot)
    {
        var laser = Instantiate(laser_dummy, objectParent);
        laser.Shoot(root, rot);
    }

    #endregion

    #region ���̑��G�t�F�N�g�Ǘ�

    /// <summary>
    /// �I�\��
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="col"></param>
    /// <param name="waitTime"></param>
    public void CreateWarning(Vector3 pos, Color col, float waitTime = 0f)
    {
        var eff = Instantiate(warning_dummy, objectParent);
        eff.Show(pos, col, waitTime);
    }

    /// <summary>
    /// ������W�ɍU��
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="isLocal"></param>
    public void CreateAttackBlast(Vector3 pos, bool isLocal = true)
    {
        var atk = Instantiate(attack_dummy);
        atk.transform.SetParent(playField.transform);
        if (!isLocal) { pos -= playField.transform.position; }
        atk.Attack(pos);
    }

    /// <summary>
    /// �Ռ��g�쐬
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="rot"></param>
    public void CreateBlast(Vector3 pos, float rot)
    {
        var blast = Instantiate(blast_dummy, objectParent);
        blast.StartBlast(pos, rot);
    }

    #endregion

    #region �O��

    /// <summary>
    /// �ҋ@���̕\��
    /// </summary>
    private void ADefaultField()
    {
        playField.Show(240f, 240f);
        reko.moveMode = TukuyomiGamePlayer.MoveMode.Free;
        reko.gameObject.SetActive(true);
        reko.transform.localPosition = Vector3.zero;
    }

    private Koma _ASelectKoma;

    /// <summary>
    /// ���C���i�s
    /// </summary>
    /// <returns></returns>
    private IEnumerator MainACoroutine()
    {
        var manager = ManagerSceneScript.GetInstance();

        // �J�n��b
        yield return AStartCoroutine();
        if (reko_life <= 0) yield break;

        while (true)
        {
            // �R�}�I��
            yield return APlayerSelectCoroutine();
            reko_damage_hide_mode = true;
            if (_ASelectKoma == Koma.Kin)
            {
                yield return AKinCoroutine();
                if (reko_life <= 0) yield break;
                ADefaultField();
                if (flg_beat_kin) yield return APlaySerif();
            }
            else if (_ASelectKoma == Koma.Gin)
            {
                yield return AGinCoroutine();
                if (reko_life <= 0) yield break;
                ADefaultField();
                if (flg_beat_gin) yield return APlaySerif();
            }
            else if (_ASelectKoma == Koma.Kei)
            {
                yield return AKeiCoroutine();
                if (reko_life <= 0) yield break;
                ADefaultField();
                if (flg_beat_kei) yield return APlaySerif();
            }
            else if (_ASelectKoma == Koma.Kyou)
            {
                yield return AKyouCoroutine();
                if (reko_life <= 0) yield break;
                ADefaultField();
                if (flg_beat_kyou) yield return APlaySerif();
            }
            else if (_ASelectKoma == Koma.Hu)
            {
                yield return AHuCoroutine();
                if (reko_life <= 0) yield break;
                ADefaultField();
                if (flg_beat_hu) yield return APlaySerif();
            }
            reko_damage_hide_mode = false;

            if (NokoriKoma == 0)
            {
                break;
            }

            // ����݂����̍U��
            yield return ATukuTurnCoroutine(false);
            if (reko_life <= 0) yield break;
        }

        // �S���I�������㔼��
        StartCoroutine(MainBCoroutine());
    }

    /// <summary>
    /// �Z���t�Đ�
    /// </summary>
    /// <returns></returns>
    private IEnumerator APlaySerif()
    {
        if (DEBUG_SERIF)
        {
            yield return new WaitForSeconds(1f);
            if (NokoriKoma == 0)
            {
                StartCoroutine(ManagerSceneScript.GetInstance().soundMan.FadeOutGameBgm());
                tukuyomiA.GetComponent<ModelUtil>().FadeOut(1.5f);
                yield return new WaitForSeconds(2.5f);
            }

            yield break;
        }
        if (NokoriKoma == 4)
        {
            serifUI.Show(StringMinigameMessage.Tuku_Game_1_1, resource.voice_game_1_1);
            yield return new WaitForSeconds(2.3f);
            serifUI.Show(StringMinigameMessage.Tuku_Game_1_2, resource.voice_game_1_2);
            yield return new WaitForSeconds(4.5f);
        }
        else if (NokoriKoma == 3)
        {
            serifUI.Show(StringMinigameMessage.Tuku_Game_2_1, resource.voice_game_2_1);
            yield return new WaitForSeconds(6.5f);
            serifUI.Show(StringMinigameMessage.Tuku_Game_2_2, resource.voice_game_2_2);
            yield return new WaitForSeconds(3f);
        }
        else if (NokoriKoma == 2)
        {
            serifUI.Show(StringMinigameMessage.Tuku_Game_3_1, resource.voice_game_3_1);
            yield return new WaitForSeconds(4f);
            serifUI.Show(StringMinigameMessage.Tuku_Game_3_2, resource.voice_game_3_2);
            yield return new WaitForSeconds(3f);
        }
        else if (NokoriKoma == 1)
        {
            serifUI.Show(StringMinigameMessage.Tuku_Game_4_1, resource.voice_game_4_1);
            yield return new WaitForSeconds(2f);
            serifUI.Show(StringMinigameMessage.Tuku_Game_4_2, resource.voice_game_4_2);
            yield return new WaitForSeconds(4f);
        }
        else if (NokoriKoma == 0)
        {
            StartCoroutine(ManagerSceneScript.GetInstance().soundMan.FadeOutGameBgm()); // ���}���u �~�܂�I����܂ł̎��Ԃ͏[������͂�
            tukuyomiA.GetComponent<ModelUtil>().FadeOut(1.5f);
            serifUI.Show(StringMinigameMessage.Tuku_Game_5_1, resource.voice_game_5_1);
            yield return new WaitForSeconds(2.5f);
            serifUI.Show(StringMinigameMessage.Tuku_Game_5_2, resource.voice_game_5_2);
            yield return new WaitForSeconds(4f);
        }
        serifUI.Hide();
    }

    /// <summary>
    /// �J�n��
    /// </summary>
    /// <returns></returns>
    private IEnumerator AStartCoroutine()
    {
        var manager = ManagerSceneScript.GetInstance();
        var save = Global.GetSaveData();

        // ��b����
        if (DEBUG_SERIF)
        {
            yield return new WaitForSeconds(1f);
        }
        else
        {
            if (save.GetGameDataInt(START_FLG) != 1)
            {
                serifUI.Show(StringMinigameMessage.Tuku_Start_First_0, resource.voice_start_first_0);
                yield return new WaitForSeconds(2f);
                serifUI.Show(StringMinigameMessage.Tuku_Start_First_1, resource.voice_start_first_1);
                yield return new WaitForSeconds(5.5f);
                serifUI.Show(StringMinigameMessage.Tuku_Start_First_2, resource.voice_start_first_2);
                yield return new WaitForSeconds(4f);
            }
            else
            {
                serifUI.Show(StringMinigameMessage.Tuku_Start_Second, resource.voice_start_second);
                yield return new WaitForSeconds(2.5f);
            }
        }
        // ��ʍ�
        serifUI.Hide();
        manager.FadeOutNoWait();
        manager.soundMan.PlaySE(manager.soundMan.commonSeMove);
        yield return new WaitForSeconds(0.4f);

        // �U���O�̉�ʐݒ肵�ĉ�ʕ��A
        playField.ShowCellField(5, 5);
        reko.moveMode = TukuyomiGamePlayer.MoveMode.Cell;
        reko.CellLocation = new Vector2(2, 2);
        tukuyomiA.PlayAnim(TukuyomiGameTukuyomiA.AnimType.Stop);
        reko_life = 5;
        lifeUI.ShowLife(reko_life);
        manager.FadeInNoWait();
        manager.soundMan.PlaySE(manager.soundMan.commonSeMove);
        yield return new WaitForSeconds(1f);

        // �Z���t
        if (save.GetGameDataInt(START_FLG) != 1 && !DEBUG_SERIF)
        {
            serifUI.Show(StringMinigameMessage.Tuku_Start_First_3, resource.voice_start_first_3);
            yield return new WaitForSeconds(3f);
            serifUI.Hide();
        }
        else
        {
            yield return new WaitForSeconds(1f);
        }

        // �U��
        yield return ATukuTurnCoroutine(true);

        // �U����̃Z���t
        if (DEBUG_SERIF)
        {
            yield return new WaitForSeconds(1f);
        }
        else
        {
            if (save.GetGameDataInt(START_FLG) != 1)
            {
                serifUI.Show(StringMinigameMessage.Tuku_Start2_First_0, resource.voice_start2_first_0);
                yield return new WaitForSeconds(2.5f);
                serifUI.Show(StringMinigameMessage.Tuku_Start2_First_1, resource.voice_start2_first_1);
                yield return new WaitForSeconds(4f);
            }
            else
            {
                serifUI.Show(StringMinigameMessage.Tuku_Start2_Second, resource.voice_start2_second);
                yield return new WaitForSeconds(2.5f);
            }
            serifUI.Hide();
        }

        // �t���O�ݒ肵�ďI��
        save.SetGameData(START_FLG, 1);
        ManagerSceneScript.GetInstance().soundMan.StartGameBgm(bgm_scene1);
        tukuyomiA.PlayAnim(TukuyomiGameTukuyomiA.AnimType.Idle);
    }

    /// <summary>
    /// ����݂����̍U���R���[�`��
    /// </summary>
    /// <returns></returns>
    private IEnumerator ATukuTurnCoroutine(bool firstAttack = false)
    {
        tukuyomiA.PlayAnim(TukuyomiGameTukuyomiA.AnimType.Down);
        koma_hisya.Appear(KOMA_X_HISYA);
        koma_kaku.Appear(KOMA_X_KAKU);

        if (!firstAttack)
        {
            // 5x5�̒����ɔz�u
            playField.ShowCellField(5, 5);
            reko.moveMode = TukuyomiGamePlayer.MoveMode.Cell;
            reko.CellLocation = new Vector2(2, 2);
            reko.gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(1f);

        if (DEBUG_A_SKIP) yield return new WaitForSeconds(1f);
        else
        {
            // ��ԂƊp�������_������
            var cnt = firstAttack ? 10 : 6;
            for (var i = 0; i < cnt; ++i)
            {
                StartCoroutine(AttackHisyaKaku(Util.RandomCheck(50)));

                yield return new WaitForSeconds(firstAttack ? 0.4f : 0.6f);
            }
            yield return new WaitForSeconds(1.5f);
        }
        koma_hisya.Disappear();
        koma_kaku.Disappear();
    }

    /// <summary>
    /// ��ԂƊp�̍U��1��
    /// </summary>
    /// <returns></returns>
    private IEnumerator AttackHisyaKaku(bool hisya)
    {
        var small = Instantiate(small_dummy);
        small.transform.SetParent(playField.transform);
        small.transform.position = hisya ? koma_hisya.transform.position : koma_kaku.transform.position;
        small.SetKoma(hisya ? Koma.Hisya : Koma.Kaku);
        small.gameObject.SetActive(true);

        var p = new DeltaVector3();
        p.Set(small.transform.localPosition);

        // �łꏊ��I��
        var atkCell = new Vector2(Util.RandomInt(0, 4), Util.RandomInt(0, 4));
        p.MoveTo(playField.GetCellPosition(atkCell), 0.3f, DeltaFloat.MoveType.LINE);
        while (p.IsActive())
        {
            yield return null;
            p.Update(Time.deltaTime);
            small.transform.localPosition = p.Get();
        }
        resource.PlaySE(resource.se_koma_set);
        yield return new WaitForSeconds(0.5f);

        // �U��
        Destroy(small.gameObject);
        if (hisya)
        {
            // ��Ԃ͏c��
            for (var i = 0; i <= 4; ++i)
            {
                var yoko = new Vector2(i, atkCell.y);
                var tate = new Vector2(atkCell.x, i);
                if (yoko != atkCell) CreateAttackCell(yoko);
                if (tate != atkCell) CreateAttackCell(tate);
            }
        }
        else
        {
            //�p�͎΂�
            for (var i = 1; i <= 4; ++i)
            {
                var ru = atkCell + new Vector2(i, i);
                var rd = atkCell + new Vector2(i, -i);
                var lu = atkCell + new Vector2(-i, i);
                var ld = atkCell + new Vector2(-i, -i);
                if (playField.InField(ru)) CreateAttackCell(ru);
                if (playField.InField(rd)) CreateAttackCell(rd);
                if (playField.InField(lu)) CreateAttackCell(lu);
                if (playField.InField(ld)) CreateAttackCell(ld);
            }
        }
        // �R�}���g�̈ʒu
        CreateAttackCell(atkCell);
        resource.PlaySE(resource.se_attack_effect_A);
    }

    /// <summary>
    /// �Z���ɍU��
    /// </summary>
    /// <param name="cell"></param>
    private void CreateAttackCell(Vector2 cell)
    {
        CreateAttackBlast(playField.GetCellPosition(cell));
    }

    /// <summary>
    /// �R�}�I���R���[�`��
    /// </summary>
    /// <returns></returns>
    private IEnumerator APlayerSelectCoroutine()
    {
        _ASelectKoma = Koma.None;
        playField.Hide();
        reko.gameObject.SetActive(false);

        // ����݂���񂪔����A�R�}���o�Ă���
        tukuyomiA.PlayAnim(TukuyomiGameTukuyomiA.AnimType.Idle);
        tukuyomiA.Move(new Vector3(KOMA_SELECT_TUKUYOMI_POS, tukuyomiA.transform.localPosition.y), 0.5f);
        var komaList = new List<Koma>();

        if (!flg_beat_kin) { koma_kin.Appear(KOMA_X_KIN); komaList.Add(Koma.Kin); }
        if (!flg_beat_gin) { koma_gin.Appear(KOMA_X_GIN); komaList.Add(Koma.Gin); }
        if (!flg_beat_kei) { koma_kei.Appear(KOMA_X_KEI); komaList.Add(Koma.Kei); }
        if (!flg_beat_kyou) { koma_kyou.Appear(KOMA_X_KYOU); komaList.Add(Koma.Kyou); }
        if (!flg_beat_hu) { koma_hu.Appear(KOMA_X_HU); komaList.Add(Koma.Hu); }
        yield return new WaitForSeconds(0.8f);

        // �V�X�e�����b�Z�[�W
        systemUI.Show(NokoriKoma switch
        {
            5 => StringMinigameMessage.Tuku_System_1,
            4 => StringMinigameMessage.Tuku_System_2,
            3 => StringMinigameMessage.Tuku_System_3,
            2 => StringMinigameMessage.Tuku_System_4,
            _ => StringMinigameMessage.Tuku_System_5,
        });

        // �J�[�\��
        var sel = 0;
        APlayerSelectShowCursor(komaList[sel]);

        var input = InputManager.GetInstance();
        while (true)
        {
            yield return null;

            if (input.GetKeyPress(InputManager.Keys.South)) break;

            if (input.GetKeyPress(InputManager.Keys.Right))
            {
                sel++;
                if (sel >= komaList.Count) sel = 0;
                APlayerSelectShowCursor(komaList[sel]);
            }
            else if (input.GetKeyPress(InputManager.Keys.Left))
            {
                sel--;
                if (sel < 0) sel = komaList.Count - 1;
                APlayerSelectShowCursor(komaList[sel]);
            }
        }

        // ����
        systemUI.Hide();
        _ASelectKoma = komaList[sel];
        reko.gameObject.SetActive(false);

        // �I�����Ȃ������R�}��������
        if (!flg_beat_kin && _ASelectKoma != Koma.Kin) koma_kin.Disappear();
        if (!flg_beat_gin && _ASelectKoma != Koma.Gin) koma_gin.Disappear();
        if (!flg_beat_kei && _ASelectKoma != Koma.Kei) koma_kei.Disappear();
        if (!flg_beat_kyou && _ASelectKoma != Koma.Kyou) koma_kyou.Disappear();
        if (!flg_beat_hu && _ASelectKoma != Koma.Hu) koma_hu.Disappear();
        // �I�������R�}�������ɕ\��
        switch (_ASelectKoma)
        {
            case Koma.Kin: koma_kin.Move(Vector3.zero, 0.5f); break;
            case Koma.Gin: koma_gin.Move(Vector3.zero, 0.5f); break;
            case Koma.Kei: koma_kei.Move(Vector3.zero, 0.5f); break;
            case Koma.Kyou: koma_kyou.Move(Vector3.zero, 0.5f); break;
            case Koma.Hu: koma_hu.Move(Vector3.zero, 0.5f); break;
        }
        yield return new WaitForSeconds(0.5f);
    }

    /// <summary>
    /// �J�[�\���\��
    /// </summary>
    /// <param name="koma"></param>
    private void APlayerSelectShowCursor(Koma koma)
    {
        const float cursor_Y = 80f;
        reko.gameObject.SetActive(true);
        reko.moveMode = TukuyomiGamePlayer.MoveMode.Disable;

        reko.transform.position = new Vector3(koma switch
        {
            Koma.Kin => KOMA_X_KIN,
            Koma.Gin => KOMA_X_GIN,
            Koma.Kei => KOMA_X_KEI,
            Koma.Kyou => KOMA_X_KYOU,
            _ => KOMA_X_HU
        }, cursor_Y);
    }

    #region ��

    /// <summary>
    /// ���̃S�[���}�X
    /// </summary>
    public Vector2 AKinGoal { get; set; }
    /// <summary>���̐퓬��</summary>
    public bool AKinPlayMode { get; set; } = false;
    private bool leachKinGoal = false;

    /// <summary>
    /// �v���C���[�����̃S�[���ɓ��B
    /// </summary>
    public void ALeachKinGoal()
    {
        AKinPlayMode = false;
        resource.PlaySE(resource.se_koma_damage);
        leachKinGoal = true;
    }

    /// <summary>
    /// ��
    /// </summary>
    /// <returns></returns>
    private IEnumerator AKinCoroutine()
    {
        // �t�B�[���h�\��
        AKinPlayMode = true;
        playField.ShowCellField(7, 5);
        reko.moveMode = TukuyomiGamePlayer.MoveMode.Cell;
        reko.CellLocation = new Vector2(3, 0);
        reko.gameObject.SetActive(true);

        // �S�[���}�X
        AKinGoal = new Vector2(Util.RandomInt(2, 4), 4);
        playField.ShowTargetCell(AKinGoal);

        // ���𐶐�
        var kin = Instantiate(small_dummy, playField.transform);
        kin.SetKoma(Koma.Kin);
        var kinPos = new Vector2(3, 4);
        kin.transform.localPosition = playField.GetCellPosition(kinPos);
        kin.transform.localRotation = Util.GetRotateQuaternion(Mathf.PI);
        kin.gameObject.SetActive(true);
        kin.CellLocation = kinPos;
        enemyKomaList.Add(kin);

        // �����o���܂őҋ@
        yield return new WaitWhile(() => reko.CellLocation == new Vector2(3, 0));

        while (true)
        {
            // �v���C���[�߂����ē���
            var moveVec = Vector2.zero;
            var dist = reko.CellLocation - kinPos;
            if (dist.x > 0 && dist.y < 0) moveVec = new Vector2(1, -1); // �E��
            else if (dist.x < 0 && dist.y < 0) moveVec = new Vector2(-1, -1); // ����
            else if (dist.x == 0) moveVec.y = dist.y > 0 ? 1 : -1; //��A��
            else if (dist.y == 0) moveVec.x = dist.x > 0 ? 1 : -1; //�E�A��
            else
            {
                // �E��A����͏c���ŉ�������I��
                if (Mathf.Abs(dist.x) >= Mathf.Abs(dist.y)) moveVec.x = dist.x > 0 ? 1 : -1;
                else moveVec.y = 1;
            }
            kinPos += moveVec;
            kin.CellLocation = kinPos;
            kin.transform.localPosition = playField.GetCellPosition(kinPos);
            resource.PlaySE(resource.se_koma_set);
            // �����Ƀv���C���[��������_���[�W
            if (reko.CellLocation == kinPos)
            {
                PlayerDamage();
            }

            yield return new WaitForSeconds(0.35f);

            if (!reko.gameObject.activeSelf || leachKinGoal) break;
        }
        AKinPlayMode = false;
        yield return new WaitForSeconds(0.6f);

        enemyKomaList.Clear();
        Destroy(kin.gameObject);

        // �v���C���[�������Ă��琬��
        if (leachKinGoal)
        {
            flg_beat_kin = true;
        }

        koma_kin.Disappear();
        tukuyomiA.Move(new Vector3(0, tukuyomiA.transform.localPosition.y), 0.5f);
        yield return new WaitForSeconds(1f);
    }

    #endregion

    #region ��

    /// <summary>
    /// ��
    /// </summary>
    /// <returns></returns>
    private IEnumerator AGinCoroutine()
    {
        // �t�B�[���h�\��
        playField.ShowCellField(3, 3);
        reko.moveMode = TukuyomiGamePlayer.MoveMode.Cell;
        reko.CellLocation = new Vector2(1, 0);
        reko.gameObject.SetActive(true);

        // ��𐶐�
        var gin = Instantiate(small_dummy, playField.transform);
        gin.SetKoma(Koma.Gin);
        gin.transform.localPosition = playField.GetCellPosition(new Vector2(1, 1));
        var rot = Mathf.PI;
        gin.transform.localRotation = Util.GetRotateQuaternion(rot);
        gin.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);

        var timer = new DeltaFloat();
        timer.Set(0);
        for (var i = 0; i < 5; ++i)
        {
            gin.gameObject.SetActive(true);
            // ����������
            var muki = Util.RandomInt(0, 3);
            var mukiRot = muki switch
            {
                0 => 0f,                //0:��
                1 => Mathf.PI * 0.5f,   //1:��
                2 => Mathf.PI,          //2:��
                _ => Mathf.PI * 1.5f    //3:�E
            };
            var atkCell = muki switch
            {
                0 => new Vector2(1, 2),
                1 => new Vector2(0, 1),
                2 => new Vector2(1, 0),
                _ => new Vector2(2, 1),
            };
            // ��]
            timer.MoveTo(0, Util.RandomFloat(2f, 3f), DeltaFloat.MoveType.LINE);
            while (timer.IsActive())
            {
                yield return null;
                timer.Update(Time.deltaTime);
                rot -= Mathf.PI * 16f * Time.deltaTime;
                if (rot < 0f) rot += Mathf.PI * 2f;
                gin.transform.localRotation = Util.GetRotateQuaternion(rot);
            }
            // 1��]����܂�
            while (rot > 0f)
            {
                yield return null;
                rot -= Mathf.PI * 16f * Time.deltaTime;
                gin.transform.localRotation = Util.GetRotateQuaternion(rot);
            }
            // ���܂��������Ŏ~�߂�
            while (rot > mukiRot)
            {
                yield return null;
                rot -= Mathf.PI * 16f * Time.deltaTime;
                gin.transform.localRotation = Util.GetRotateQuaternion(rot);
            }
            gin.transform.localRotation = Util.GetRotateQuaternion(mukiRot);
            resource.PlaySE(resource.se_koma_set);
            // �U��
            yield return new WaitForSeconds(0.6f);
            resource.PlaySE(resource.se_attack_effect_A);
            CreateAttackCell(new Vector2(0, 0));
            CreateAttackCell(new Vector2(2, 0));
            CreateAttackCell(new Vector2(0, 2));
            CreateAttackCell(new Vector2(2, 2));
            CreateAttackCell(new Vector2(1, 1));
            CreateAttackCell(atkCell);
            yield return new WaitForSeconds(1f);
            if (!reko.gameObject.activeSelf) break;
        }

        Destroy(gin.gameObject);

        // �v���C���[�������Ă��琬��
        if (reko.gameObject.activeSelf)
        {
            flg_beat_gin = true;
        }

        koma_gin.Disappear();
        tukuyomiA.Move(new Vector3(0, tukuyomiA.transform.localPosition.y), 0.5f);
        yield return new WaitForSeconds(1f);
    }

    #endregion

    #region �j�n

    /// <summary>
    /// �j�n
    /// </summary>
    /// <returns></returns>
    private IEnumerator AKeiCoroutine()
    {
        // �t�B�[���h�\��
        enemyKomaList.Clear();
        playField.ShowCellField(5, 5);
        reko.moveMode = TukuyomiGamePlayer.MoveMode.Cell;
        reko.CellLocation = new Vector2(2, 2);
        reko.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);

        // �j�n�������_������
        for (var i = 0; i < 8; ++i)
        {
            if (!reko.gameObject.activeSelf) break;

            StartCoroutine(AttackKei());

            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(1.5f);

        // �v���C���[�������Ă��琬��
        if (reko.gameObject.activeSelf)
        {
            flg_beat_kei = true;
        }

        koma_kei.Disappear();
        tukuyomiA.Move(new Vector3(0, tukuyomiA.transform.localPosition.y), 0.5f);
        yield return new WaitForSeconds(1f);
    }

    /// <summary>
    /// �j�n�U��2��
    /// </summary>
    /// <returns></returns>
    private IEnumerator AttackKei()
    {
        var small1 = Instantiate(small_dummy, playField.transform);
        small1.transform.position = koma_kei.transform.position;
        small1.SetKoma(Koma.Kei);
        small1.gameObject.SetActive(true);
        var small2 = Instantiate(small_dummy, playField.transform);
        small2.transform.position = koma_kei.transform.position;
        small2.SetKoma(Koma.Kei);
        small2.gameObject.SetActive(true);

        var p1 = new DeltaVector3();
        p1.Set(small1.transform.localPosition);
        var p2 = new DeltaVector3();
        p2.Set(small2.transform.localPosition);

        // �łꏊ��I��
        var atkCell1 = new Vector2(Util.RandomInt(0, 4), Util.RandomInt(0, 4));
        var atkCell2 = new Vector2((atkCell1.x + Util.RandomInt(1, 4)) % 5, (atkCell1.y + Util.RandomInt(1, 4)) % 5);
        p1.MoveTo(playField.GetCellPosition(atkCell1), 0.3f, DeltaFloat.MoveType.LINE);
        p2.MoveTo(playField.GetCellPosition(atkCell2), 0.3f, DeltaFloat.MoveType.LINE);
        while (p1.IsActive() || p2.IsActive())
        {
            yield return null;
            p1.Update(Time.deltaTime);
            p2.Update(Time.deltaTime);
            small1.transform.localPosition = p1.Get();
            small2.transform.localPosition = p2.Get();
        }
        resource.PlaySE(resource.se_koma_set);
        yield return new WaitForSeconds(0.7f);

        // �U��
        Destroy(small1.gameObject);
        Destroy(small2.gameObject);
        var atkList = new List<Vector2>();
        var distList = new List<Vector2>(){
            new Vector2(2, 1),
            new Vector2(1, 2),
            new Vector2(-1, 2),
            new Vector2(-2, 1),
            new Vector2(-2, -1),
            new Vector2(-1, -2),
            new Vector2(1, -2),
            new Vector2(2, -1)
        };
        foreach (var dist in distList)
        {
            var atk1 = atkCell1 + dist;
            var atk2 = atkCell2 + dist;
            if (!atkList.Contains(atk1)) atkList.Add(atk1);
            if (!atkList.Contains(atk2)) atkList.Add(atk2);
        }
        // �R�}���g�̈ʒu
        if (!atkList.Contains(atkCell1)) atkList.Add(atkCell1);
        if (!atkList.Contains(atkCell2)) atkList.Add(atkCell2);

        foreach (var a in atkList)
            CreateAttackCell(a);

        resource.PlaySE(resource.se_attack_effect_A);
    }

    #endregion

    #region ����

    /// <summary>
    /// ����
    /// </summary>
    /// <returns></returns>
    private IEnumerator AKyouCoroutine()
    {
        // �t�B�[���h�\��
        playField.transform.localPosition = new Vector3(0, AFIELD_Y_KYOU);
        playField.ShowCellField(1, 1);
        reko.moveMode = TukuyomiGamePlayer.MoveMode.Disable;
        reko.CellLocation = new Vector2(0, 0);
        reko.gameObject.SetActive(true);

        // ���Ԃ𐶐�
        var kyou = Instantiate(small_dummy, playField.transform);
        kyou.transform.localPosition = playField.GetCellPosition(new Vector2(0, 7));
        kyou.SetKoma(Koma.Kyou);
        kyou.EnableDamageMode();
        kyou.transform.localRotation = Util.GetRotateQuaternion(Mathf.PI);
        kyou.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        reko.ShotEnable = true;
        pressUI.SetActive(true);
        yield return new WaitUntil(() => InputManager.GetInstance().GetKeyPress(InputManager.Keys.South));
        pressUI.SetActive(false);

        // ���Ԃ̃V���b�g�J�n
        shotList.Clear();
        while (true)
        {
            if (!kyou.gameObject.activeSelf || !reko.gameObject.activeSelf)
            {
                // ���Ԍ��j�܂��̓v���C���[���łŏI��
                break;
            }

            // �����Ă��獁�ԃV���b�g����
            CreateShot(kyou.transform.localPosition, new Vector3(0, -500f), TukuyomiGameShot.ShotType.Enemy);

            yield return new WaitForSeconds(Util.RandomFloat(0.1f, 0.16f));
        }

        reko.ShotEnable = false;
        // �V���b�g������܂őҋ@
        yield return new WaitWhile(() => shotList.Any());

        // �v���C���[�������Ă��琬��
        if (reko.gameObject.activeSelf)
        {
            flg_beat_kyou = true;
        }
        Destroy(kyou.gameObject);
        playField.transform.localPosition = new Vector3(0, AFIELD_Y_DEFAULT);

        koma_kyou.Disappear();
        tukuyomiA.Move(new Vector3(0, tukuyomiA.transform.localPosition.y), 0.5f);
        yield return new WaitForSeconds(1f);
    }

    #endregion

    #region ��

    private List<TukuyomiGameKomaSmall> enemyKomaList = new List<TukuyomiGameKomaSmall>();

    /// <summary>
    /// ���̍��W�ɕ������邩����
    /// </summary>
    /// <param name="cell"></param>
    /// <returns></returns>
    public bool AEnemyIsInField(Vector2 cell)
    {
        return enemyKomaList.Any(h => h.CellLocation == cell && h.gameObject.activeSelf);
    }

    /// <summary>
    /// ��
    /// </summary>
    /// <returns></returns>
    private IEnumerator AHuCoroutine()
    {
        // �t�B�[���h�\��
        enemyKomaList.Clear();
        playField.ShowCellField(5, 5);
        reko.moveMode = TukuyomiGamePlayer.MoveMode.Cell;
        reko.CellLocation = new Vector2(2, 2);
        reko.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);

        // �����쐬
        const int Offset = 8;
        var tmp0 = 2;
        var tmp1 = 2;
        var tmp2 = 2;
        var add0 = Util.RandomInt(1, 3);
        var add1 = Util.RandomInt(1, 3);
        var add2 = Util.RandomInt(1, 3);
        for (var i = 0; i < 5; ++i)
        {
            // �ォ�牺
            enemyKomaList.Add(ACreateHu(0, i, tmp0 + Offset));
            // �E���獶
            enemyKomaList.Add(ACreateHu(1, tmp1 + Offset, i));
            // ������E
            enemyKomaList.Add(ACreateHu(2, tmp2 - Offset, i));

            var hugo = i % 2 == 0 ? 1 : -1;
            tmp0 += add0 * hugo;
            tmp1 += add1 * hugo;
            tmp2 += add2 * hugo;
            add0 = (add0 == 1) ? 2 : Util.RandomInt(1, 3);
            add1 = (add1 == 1) ? 2 : Util.RandomInt(1, 3);
            add2 = (add2 == 1) ? 2 : Util.RandomInt(1, 3);
        }

        // �S��������܂ő҂�
        yield return new WaitWhile(() => enemyKomaList.Any(hu => hu.gameObject.activeSelf));
        yield return new WaitForSeconds(1f);
        foreach (var hu in enemyKomaList) Destroy(hu);
        enemyKomaList.Clear();

        // �v���C���[�������Ă��琬��
        if (reko.gameObject.activeSelf)
        {
            flg_beat_hu = true;
        }

        koma_hu.Disappear();
        tukuyomiA.Move(new Vector3(0, tukuyomiA.transform.localPosition.y), 0.5f);
        yield return new WaitForSeconds(1f);
    }

    /// <summary>
    /// �����쐬
    /// </summary>
    /// <param name="type"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private TukuyomiGameKomaSmall ACreateHu(int type, int x, int y)
    {
        var hu = Instantiate(small_dummy, playField.transform);
        hu.SetKoma(Koma.Hu);
        hu.CellLocation = new Vector2(x, y);
        hu.transform.localPosition = playField.GetCellPosition(hu.CellLocation);
        hu.transform.localRotation = Util.GetRotateQuaternion(
            type switch
            {
                0 => Mathf.PI,
                1 => Mathf.PI / 2f,
                _ => -Mathf.PI / 2f,
            });
        hu.gameObject.SetActive(true);
        StartCoroutine(AHuAttackCoroutine(hu, type));

        return hu;
    }

    /// <summary>
    /// �����i�ރR���[�`��
    /// </summary>
    /// <param name="hu"></param>
    /// <param name="type">0:�ォ�牺�@1:�E���獶�@2:������E</param>
    /// <returns></returns>
    private IEnumerator AHuAttackCoroutine(TukuyomiGameKomaSmall hu, int type)
    {
        var cell = hu.CellLocation;
        while (true)
        {
            yield return new WaitForSeconds(0.6f);

            if (type == 0)
            {
                cell.y--;
                if (cell.y < 0) break;
            }
            else if (type == 1)
            {
                cell.x--;
                if (cell.x < 0) break;
            }
            else if (type == 2)
            {
                cell.x++;
                if (cell.x >= 0 && !playField.InField(cell)) break;
            }
            hu.CellLocation = cell;
            hu.transform.localPosition = playField.GetCellPosition(cell);

            // �ړ���Ɂ���������_���[�W�^���ď���
            if (reko.CellLocation == cell && reko.gameObject.activeSelf)
            {
                reko.gameObject.SetActive(false);
                PlayerDamage();
            }
        }

        // �o���������
        hu.gameObject.SetActive(false);
    }

    #endregion

    #endregion

    #region �㔼

    private List<TukuyomiGameKomaSmallB> bKomaList = new List<TukuyomiGameKomaSmallB>();
    /// <summary>����݂���񂪃_���[�W�󂯂���</summary>
    public bool bTukuyomiDamaged { get; set; } = false;

    /// <summary>
    /// ����݂���񂪃_���[�W�󂯂�
    /// </summary>
    public void TukuyomiDamage()
    {
        tukuyomi_life--;
        lifeUIT.ShowLife(tukuyomi_life);
        bTukuyomiDamaged = true;
    }

    /// <summary>
    /// B�̃R�}�폜
    /// </summary>
    /// <param name="koma"></param>
    public void BRemoveKoma(TukuyomiGameKomaSmallB koma)
    {
        bKomaList.Remove(koma);
    }

    /// <summary>
    /// B�̃R�}�쐬
    /// </summary>
    /// <param name="idx"></param>
    /// <param name="kind"></param>
    private void BCreateNormalKoma(int idx, Koma kind)
    {
        var root = new Vector3(0, 76f);
        var p = new Vector3((idx - 4) * TukuyomiGameKomaSmallB.KOMAB_SIZE, 0f);
        var k = Instantiate(smallb_dummy, objectParent);
        bKomaList.Add(k);
        k.WorkStart(root, p, kind);
    }

    /// <summary>
    /// �R�}9�쐬
    /// </summary>
    /// <param name="includeKing">�����쐬</param>
    /// <returns></returns>
    private List<Koma> BCreateRandomKomaList(bool includeKing)
    {
        // ���ȊO�͑S��2�܂�
        var kyouCnt = 0;
        var keiCnt = 0;
        var ginCnt = 0;
        var kinCnt = includeKing ? 1 : 0;

        var ret = new List<Koma>();
        for (var i = 0; i < 9; ++i)
        {
            var rand = 12;
            // ����݂����̗̑͂ɂ���Ď�ނ�������
            if (tukuyomi_life <= 1 && kinCnt < 2)
            {
                if (Util.RandomCheck(rand))
                {
                    ret.Add(Koma.Kin);
                    kinCnt++;
                    continue;
                }
                else rand += 12;
            }
            if (tukuyomi_life <= 2 && ginCnt < 2)
            {
                if (Util.RandomCheck(rand))
                {
                    ret.Add(Koma.Gin);
                    ginCnt++;
                    continue;
                }
                else rand += 12;
            }
            if (tukuyomi_life <= 3 && keiCnt < 2)
            {
                if (Util.RandomCheck(rand))
                {
                    ret.Add(Koma.Kei);
                    keiCnt++;
                    continue;
                }
                else rand += 12;
            }
            if (tukuyomi_life <= 4 && kyouCnt < 2)
            {
                if (Util.RandomCheck(rand))
                {
                    ret.Add(Koma.Kyou);
                    kyouCnt++;
                    continue;
                }
                else rand += 12;
            }
            ret.Add(Koma.Hu);
        }

        // �����܂ގ�
        if (includeKing)
        {
            var kingIdx = Util.RandomInt(0, ret.Count - 1);
            if (ret[kingIdx] == Koma.Kin) kinCnt--;
            ret[kingIdx] = Koma.Ou;

            // ���������ɂ���
            if (kingIdx < 5) ret[kingIdx + 1] = Koma.Kin;
            else ret[kingIdx - 1] = Koma.Kin;
        }

        return ret;
    }

    /// <summary>
    /// �㔼��R���[�`��
    /// </summary>
    /// <returns></returns>
    private IEnumerator MainBCoroutine()
    {
        var manager = ManagerSceneScript.GetInstance();
        var input = InputManager.GetInstance();

        // �t�B�[���h�쐬
        playField.Show(400f, 250f);
        yield return tukuyomiB.AppearAnimation();
        manager.soundMan.StartGameBgm(bgm_scene2);
        tukuyomi_life = 5;
        lifeUIT.ShowLife(tukuyomi_life);
        reko.moveMode = TukuyomiGamePlayer.MoveMode.Free;
        reko.ShotEnable = true;
        pressUI.SetActive(true);
        yield return new WaitUntil(() => input.GetKeyPress(InputManager.Keys.South));
        pressUI.SetActive(false);

        //����݂����U���J�n
        StartCoroutine(BHandNormalCoroutine());
        var bTukuyomiAttackCount = 0;
        while (true)
        {
            bTukuyomiAttackCount++;
            if (bTukuyomiAttackCount >= 3) bTukuyomiAttackCount = 0;
            // 9�쐬 3�񂲂Ƃɉ�������
            var list = BCreateRandomKomaList(bTukuyomiAttackCount == 0);

            // ���ׂ̗ɋ�����������ݒ�
            var ouIdx = list.FindIndex(k => k == Koma.Ou);
            var kinIdx = ouIdx < 5 ? ouIdx + 1 : ouIdx - 1;
            foreach (var itm in list.Select((koma, idx) => new { koma, idx }))
            {
                BCreateNormalKoma(itm.idx, itm.koma);

                if (ouIdx >= 0 && ouIdx <= itm.idx && kinIdx <= itm.idx)
                {
                    // ���
                    bKomaList[kinIdx].SetDefenceKing(bKomaList[ouIdx]);
                }
                yield return new WaitForSeconds(0.15f);
            }

            // �R�}���S��������܂ő҂�
            yield return new WaitWhile(() => bKomaList.Any());

            // �|���Ă���I��
            if (tukuyomi_life <= 0) break;

            // �����Ă���I���҂�
            if (reko_life <= 0) yield break;

            // �����|���ꂽ��
            if (bTukuyomiDamaged)
            {
                yield return new WaitWhile(() => bTukuyomiHandWorking);
                bTukuyomiAttackCount = 0;
                // ���E�̎�̓���U�����s��
                yield return BHandDamageCoroutine();

                bTukuyomiDamaged = false;
            }
        }

        // �|�����珟��
        yield return tukuyomiB.DisappearAnimation();

        SetGameResult(true);
        manager.ExitGame();
    }

    private bool bTukuyomiHandWorking = false;
    /// <summary>
    /// ���肩��̒ʏ�U���R���[�`��
    /// </summary>
    /// <returns></returns>
    private IEnumerator BHandNormalCoroutine()
    {
        var waitTime = Util.RandomFloat(5f, 7f);
        while (tukuyomi_life > 0)
        {
            bTukuyomiHandWorking = false;
            yield return null;
            if (bTukuyomiDamaged) continue; //�_���[�W�󂯂����͈�U��~
            waitTime -= Time.deltaTime;
            if (waitTime > 0f) continue;
            waitTime = tukuyomi_life + Util.RandomFloat(1f, 3f);

            bTukuyomiHandWorking = true;
            if (Util.RandomCheck(50))
            {
                // �E������
                tukuyomiB.handR.ShowHand(1);
                var hisya = Instantiate(smallc_dummy, objectParent);
                hisya.CreateParams(TukuyomiGameKomaSmallC.KomaCType.NormalHisya, tukuyomiB.handR.GetShotRoot());
                resource.PlaySE(resource.se_warning);
                yield return hisya.ShowWarning();
                if (tukuyomi_life <= 0) yield break;
                tukuyomiB.handR.ShowHand(2);
                yield return hisya.ExecAttackCoroutine();
                tukuyomiB.handR.ShowHand(0);
            }
            else
            {
                // ������p
                tukuyomiB.handL.ShowHand(1);
                var kaku = Instantiate(smallc_dummy, objectParent);
                kaku.CreateParams(TukuyomiGameKomaSmallC.KomaCType.NormalKaku, tukuyomiB.handL.GetShotRoot());
                resource.PlaySE(resource.se_warning);
                yield return kaku.ShowWarning();
                if (tukuyomi_life <= 0) yield break;
                tukuyomiB.handL.ShowHand(2);
                yield return kaku.ExecAttackCoroutine();
                tukuyomiB.handL.ShowHand(0);
            }
        }
    }

    /// <summary>
    /// �_���[�W�󂯂����̎肩��̍U��
    /// </summary>
    /// <returns></returns>
    private IEnumerator BHandDamageCoroutine()
    {
        if (tukuyomi_life == 4)
        {
            // �p���������
            tukuyomiB.handL.ShowHand(1);
            var kaku = Instantiate(smallc_dummy, objectParent);
            kaku.CreateParams(TukuyomiGameKomaSmallC.KomaCType.Dmg1Kaku1, tukuyomiB.handL.GetShotRoot());
            resource.PlaySE(resource.se_warning);
            yield return kaku.ShowWarning();
            tukuyomiB.handL.ShowHand(2);
            yield return kaku.ExecAttackCoroutine();
            tukuyomiB.handL.ShowHand(0);
            // �E����
            tukuyomiB.handR.ShowHand(1);
            kaku = Instantiate(smallc_dummy, objectParent);
            kaku.CreateParams(TukuyomiGameKomaSmallC.KomaCType.Dmg1Kaku2, tukuyomiB.handR.GetShotRoot());
            resource.PlaySE(resource.se_warning);
            yield return kaku.ShowWarning();
            tukuyomiB.handR.ShowHand(2);
            yield return kaku.ExecAttackCoroutine();
            tukuyomiB.handR.ShowHand(0);
        }
        else if (tukuyomi_life == 3)
        {
            // �E������
            tukuyomiB.handR.ShowHand(1);
            var koma = Instantiate(smallc_dummy, objectParent);
            koma.CreateParams(TukuyomiGameKomaSmallC.KomaCType.Dmg2Hisya1, Vector3.zero);
            resource.PlaySE(resource.se_warning);
            yield return koma.ShowWarning();
            tukuyomiB.handR.ShowHand(2);
            yield return koma.ExecAttackCoroutine();
            tukuyomiB.handR.ShowHand(0);
            // ��������
            tukuyomiB.handL.ShowHand(1);
            koma = Instantiate(smallc_dummy, objectParent);
            koma.CreateParams(TukuyomiGameKomaSmallC.KomaCType.Dmg2Hisya2, Vector3.zero);
            resource.PlaySE(resource.se_warning);
            yield return koma.ShowWarning();
            tukuyomiB.handL.ShowHand(2);
            yield return koma.ExecAttackCoroutine();
            tukuyomiB.handL.ShowHand(0);
            // �E������
            tukuyomiB.handR.ShowHand(1);
            koma = Instantiate(smallc_dummy, objectParent);
            koma.CreateParams(TukuyomiGameKomaSmallC.KomaCType.Dmg2Hisya3, Vector3.zero);
            resource.PlaySE(resource.se_warning);
            yield return koma.ShowWarning();
            tukuyomiB.handR.ShowHand(2);
            yield return koma.ExecAttackCoroutine();
            tukuyomiB.handR.ShowHand(0);
        }
        else if (tukuyomi_life == 2)
        {
            for (var i = 0; i < 3; ++i)
            {
                // ���E�ɔ��1���A��Ɋp1��
                tukuyomiB.handR.ShowHand(1);
                tukuyomiB.handL.ShowHand(1);
                var koma1 = Instantiate(smallc_dummy, objectParent);
                var koma2 = Instantiate(smallc_dummy, objectParent);
                var koma3 = Instantiate(smallc_dummy, objectParent);
                koma1.CreateParams(TukuyomiGameKomaSmallC.KomaCType.Dmg3Hisya1, Vector3.zero);
                koma2.CreateParams(TukuyomiGameKomaSmallC.KomaCType.Dmg3Hisya2, Vector3.zero);
                koma3.CreateParams(TukuyomiGameKomaSmallC.KomaCType.Dmg3Kaku, Vector3.zero);
                resource.PlaySE(resource.se_warning);
                StartCoroutine(koma2.ShowWarning());
                StartCoroutine(koma3.ShowWarning());
                yield return koma1.ShowWarning();
                tukuyomiB.handR.ShowHand(2);
                tukuyomiB.handL.ShowHand(2);
                StartCoroutine(koma2.ExecAttackCoroutine());
                StartCoroutine(koma3.ExecAttackCoroutine());
                yield return koma1.ExecAttackCoroutine();
                tukuyomiB.handR.ShowHand(0);
                tukuyomiB.handL.ShowHand(0);

            }
        }
        else //1
        {
            // �ォ��p���o�Ă��ĉ�������
            tukuyomiB.handR.ShowHand(1);
            tukuyomiB.handL.ShowHand(1);
            var koma = Instantiate(smallc_dummy, objectParent);
            koma.CreateParams(TukuyomiGameKomaSmallC.KomaCType.Dmg4Kaku, Vector3.zero);
            resource.PlaySE(resource.se_warning);
            yield return koma.ShowWarning();
            tukuyomiB.handR.ShowHand(2);
            tukuyomiB.handL.ShowHand(2);
            yield return koma.ExecAttackCoroutine();

            // ���Ԃ��邮��
            var kyoRotList = new List<float>();
            var startRot = Mathf.PI * 1.5f;
            for (var i = 0; i < 36f; ++i)
            {
                kyoRotList.Add(startRot);
                startRot += Mathf.PI / 18f;
                if (startRot > Mathf.PI * 2f) startRot -= Mathf.PI * 2f;
            }

            // �x���\��
            resource.PlaySE(resource.se_warning);
            foreach (var r in kyoRotList)
            {
                var p = playField.transform.position - Util.GetVector3IdentityFromRot(r) * 192f;
                CreateWarning(p, TukuyomiGameKomaSmall.GetKomaColor(Koma.Kyou));
                yield return new WaitForSeconds(0.03f);
            }
            yield return new WaitForSeconds(1f);

            // 5��
            for (var i = 0; i < 5; ++i)
            {
                foreach (var r in kyoRotList)
                {
                    var kyo = Instantiate(smallc_dummy, objectParent);
                    kyo.CreateParams(TukuyomiGameKomaSmallC.KomaCType.Dmg4Kyou, Vector3.zero, r);
                    StartCoroutine(kyo.ExecAttackCoroutine());

                    yield return new WaitForSeconds(0.06f);
                }
            }

            yield return new WaitForSeconds(1f);
            tukuyomiB.handR.ShowHand(0);
            tukuyomiB.handL.ShowHand(0);
        }
    }

    #endregion
}
