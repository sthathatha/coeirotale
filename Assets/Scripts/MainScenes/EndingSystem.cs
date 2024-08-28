using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// �G���f�B���O
/// </summary>
public class EndingSystem : MainScriptBase
{
    public AudioClip endingBgm;
    public AudioClip voice_tukuyomi1;
    public AudioClip voice_tukuyomi2;
    public EndingPicture picture1;
    public EndingPicture picture2;
    public ModelUtil sirowani;

    public ModelUtil logo;
    public GameObject logoMask;
    public EndingText txtCenter;
    public EndingText txtDefault;
    public EndingText txtAmi;
    public EndingText txtMana;
    public EndingText txtMenderu;
    public EndingText txtMatuka;
    public EndingText txtMati;
    public EndingText txtPierre;
    public EndingText txtTukuyomi;

    public OpeningStarObject2 reko;

    public Transform starParent;
    public OpeningStarObject1 starDummy;

    private IEnumerator starCoroutine;

    /// <summary>
    /// ������
    /// </summary>
    /// <returns></returns>
    public override IEnumerator BeforeInitFadeIn()
    {
        yield return base.BeforeInitFadeIn();
        var cam = ManagerSceneScript.GetInstance().mainCam;
        cam.SetTargetPos(Vector2.zero);

        logo.FadeOutImmediate();
        picture1.transform.localPosition = Vector3.zero;
        picture2.transform.localPosition = Vector3.zero;
        picture1.gameObject.SetActive(true);
        picture2.gameObject.SetActive(true);
        sirowani.gameObject.SetActive(true);
        sirowani.FadeOutImmediate();
    }

    /// <summary>
    /// �t�F�[�h�C����
    /// </summary>
    /// <param name="init"></param>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn(bool init)
    {
        yield return base.AfterFadeIn(init);
        Global.GetSaveData().system.clearFlag = 1;
        Global.GetSaveData().SaveSystemData();
        StartCoroutine(EndingCoroutine());
    }

    /// <summary>
    /// �G���f�B���O�Đ�
    /// </summary>
    /// <returns></returns>
    private IEnumerator EndingCoroutine()
    {
        var manager = ManagerSceneScript.GetInstance();
        manager.soundMan.PlayFieldBgm(SoundManager.FieldBgmType.None, endingBgm, false);
        var input = InputManager.GetInstance();
        var msg = manager.GetMessageWindow();

        var pos1 = SearchGeneralPosition(1);
        var pos2 = SearchGeneralPosition(2);

        yield return new WaitForSeconds(2f);
        //0:00	�����̎���Ł@�m��Ȃ��������̒n��
        //	    ���ꗎ�@���̍��̒�
        logo.FadeIn(1f);
        yield return new WaitWhile(() => logo.IsFading());
        yield return new WaitForSeconds(7f);
        logo.FadeOut(1f);
        yield return new WaitWhile(() => logo.IsFading());
        yield return new WaitForSeconds(4f);

        //0:13	�a�񂾐j�̉����@���ޗ��j�̉~��
        //  	�N�̐��@�W�����ɉ���
        picture1.ShowSyugoPic();
        yield return new WaitForSeconds(10f);
        picture1.HideSyugoPic();
        yield return new WaitForSeconds(4f);

        //0:27	�J��Ԃ��������@�ǂ�قǉi�������̂�
        //	�U��̊y�����甭�Ƃ��@�ނ̒n��
        picture1.SetPos(EndingPicture.Pic_Type.Ami);
        picture1.ShowPic(EndingPicture.Pic_Type.Ami);
        yield return new WaitForSeconds(4f);
        txtAmi.FadeIn(StringFieldMessage.End_Ami);
        yield return new WaitForSeconds(8f);
        picture1.HidePic();
        txtAmi.FadeOut();
        yield return new WaitForSeconds(5f);

        //0:43	�����肵��߁@����̈�ИA���
        //	�X�̖�ɂ��������@�����􂩂���䂭
        picture1.SetPos(EndingPicture.Pic_Type.Mati);
        picture1.ShowPic(EndingPicture.Pic_Type.Mati);
        yield return new WaitForSeconds(3f);
        txtMati.FadeIn(StringFieldMessage.End_Mati);
        yield return new WaitForSeconds(7f);
        picture1.HidePic();
        txtMati.FadeOut();
        yield return new WaitForSeconds(3f);

        //0:57	���r��镗���@�^���������z����
        //	�ʂĂɖ����������Ƃ��@���ɍs����Ȃ��
        picture1.SetPos(EndingPicture.Pic_Type.Menderu);
        picture1.ShowPic(EndingPicture.Pic_Type.Menderu);
        yield return new WaitForSeconds(3f);
        txtMenderu.FadeIn(StringFieldMessage.End_Menderu);
        yield return new WaitForSeconds(7f);
        picture1.HidePic();
        txtMenderu.FadeOut();
        yield return new WaitForSeconds(4f);

        //1:10	�ԑt�i�Z�j
        picture1.SetPos(EndingPicture.Pic_Type.Matuka);
        picture1.ShowPic(EndingPicture.Pic_Type.Matuka);
        yield return new WaitForSeconds(3f);
        txtMatuka.FadeIn(StringFieldMessage.End_Matuka);
        yield return new WaitForSeconds(7f);
        picture1.HidePic();
        txtMatuka.FadeOut();
        yield return new WaitForSeconds(3.5f);

        //1:25	���񂾓��͊J���ā@����_�f����
        //	���̍��@�ޕ������߂�
        picture2.ShowSyugoPic();
        yield return new WaitForSeconds(10.5f);
        picture2.HideSyugoPic();
        yield return new WaitForSeconds(4f);

        //1:38	��]�Ɛ�]���@�D�萬���n���삯�o��
        //	�S�̒��@�N�̎p������
        picture2.SetPos(EndingPicture.Pic_Type.Mana);
        picture2.ShowPic(EndingPicture.Pic_Type.Mana);
        yield return new WaitForSeconds(3f);
        txtMana.FadeIn(StringFieldMessage.End_Mana);
        yield return new WaitForSeconds(7f);
        picture2.HidePic();
        txtMana.FadeOut();
        yield return new WaitForSeconds(3f);

        //1:52	�k���Ă��Ɉ���@�|����������
        //	�U�����̐����́@����U�蕥��
        picture2.SetPos(EndingPicture.Pic_Type.Tukuyomi);
        picture2.ShowPic(EndingPicture.Pic_Type.Tukuyomi);
        yield return new WaitForSeconds(3f);
        txtTukuyomi.FadeIn(StringFieldMessage.End_Tukuyomi);
        yield return new WaitForSeconds(8f);
        picture2.HidePic();
        txtTukuyomi.FadeOut();
        yield return new WaitForSeconds(5f);

        //2:08	�₦�ԂȂ������@��̖���w�ɂ���
        //	�������������Ȃ��@�ʂ�ꂽ���E
        picture2.SetPos(EndingPicture.Pic_Type.Pierre);
        picture2.ShowPic(EndingPicture.Pic_Type.Pierre);
        yield return new WaitForSeconds(3f);
        txtPierre.FadeIn(StringFieldMessage.End_Pierre);
        yield return new WaitForSeconds(7f);
        picture2.HidePic();
        txtPierre.FadeOut();
        yield return new WaitForSeconds(3f);

        //2:21	���̍Ő�ց@������Ӗ���T����
        //	�I���ʒn���̉ʂĂ��@�N�Ɩa���O��
        sirowani.FadeIn(1f);
        yield return new WaitForSeconds(3f);
        txtCenter.FadeIn(StringFieldMessage.End_Sirowani);
        yield return new WaitForSeconds(8f);
        sirowani.FadeOut(1f);
        txtCenter.FadeOut();
        yield return new WaitForSeconds(4f);

        //2:36	�ԑt�i���j
        starCoroutine = CreateStarCoroutine();
        StartCoroutine(starCoroutine);
        txtCenter.FadeIn(StringFieldMessage.End_BgmSe);
        yield return new WaitForSeconds(12f);
        txtCenter.FadeOut();
        yield return new WaitForSeconds(3f);

        //2:50	�����ꂽ�������́@�H�΂����Ȃ�����
        //	���̑��������鐢�E���@����ł䂭
        txtCenter.FadeIn(StringFieldMessage.End_Song);
        yield return new WaitForSeconds(14f);
        txtCenter.FadeOut();
        yield return new WaitForSeconds(3f);

        //3:08	�₦�ԂȂ������@��̖���w�ɂ���
        //	�������������Ȃ��@�ʂ�ꂽ���E
        //3:22	���̍Ő�ց@������Ӗ���T����
        //	�I���ʒn���̉ʂĂ��@�N�Ɩa���O��
        reko.MoveTo(pos1.GetPosition(), 3f, DeltaFloat.MoveType.DECEL);
        yield return new WaitForSeconds(6f);
        txtDefault.FadeIn(Global.GetTemporaryData().ending_select_voice switch
        {
            F210System.VOICE_AMI => StringFieldMessage.End_Reko_Ami,
            F210System.VOICE_MANA => StringFieldMessage.End_Reko_Mana,
            F210System.VOICE_MATI => StringFieldMessage.End_Reko_Mati,
            F210System.VOICE_MATUKA => StringFieldMessage.End_Reko_Matuka,
            F210System.VOICE_MENDERU => StringFieldMessage.End_Reko_Menderu,
            F210System.VOICE_PIERRE => StringFieldMessage.End_Reko_Pierre,
            F210System.VOICE_TUKUYOMI => StringFieldMessage.End_Reko_Tukuyomi,
            _ => StringFieldMessage.End_Reko_Mycoe,
        });
        yield return new WaitForSeconds(9.5f);
        txtDefault.FadeOut();

        yield return MoveReko(200, 400, 0, 50f, 2f);
        yield return new WaitForSeconds(2f);
        yield return MoveReko(-100, -300, 100f, 150f, 2f);
        yield return new WaitForSeconds(3f);
        reko.MoveTo(new Vector3(0f, 400f), 2f, DeltaFloat.MoveType.DECEL);
        yield return new WaitForSeconds(3.2f);

        //3:35	�N�Ɩa���O��
        StopCoroutine(starCoroutine);
        starCoroutine = null;
        logo.FadeIn(2f);
        yield return new WaitForSeconds(2f);
        reko.MoveTo(pos2.GetPosition(), 3f, DeltaFloat.MoveType.DECEL);
        yield return new WaitForSeconds(3f);

        //3:41
        reko.gameObject.SetActive(false);
        logoMask.SetActive(false);

        yield return new WaitForSeconds(6f);
        msg.Open();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.End_Last_Tuku1, voice_tukuyomi1);
        var timer = new DeltaFloat();
        timer.Set(0);
        timer.MoveTo(1f, 180f, DeltaFloat.MoveType.LINE);
        while (timer.IsActive())
        {
            yield return null;
            timer.Update(Time.deltaTime);

            // ���͂�����I��
            if (input.GetKeyPress(InputManager.Keys.South))
            {
                msg.Close();
                manager.LoadMainScene("TitleScene", 0);
                yield break;
            }
        }

        // �R���ŃZ���t�ύX
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.End_Last_Tuku2, voice_tukuyomi2);
        yield return new WaitUntil(() => input.GetKeyPress(InputManager.Keys.South));
        msg.Close();
        manager.LoadMainScene("TitleScene", 0);
    }

    /// <summary>
    /// ���R�����_���ړ�
    /// </summary>
    /// <param name="x1"></param>
    /// <param name="x2"></param>
    /// <param name="y1"></param>
    /// <param name="y2"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator MoveReko(float x1, float x2, float y1, float y2, float time)
    {
        if (x1 > x2)
        {
            var t = x1;
            x1 = x2;
            x2 = t;
        }
        if (y1 > y2)
        {
            var t = y1;
            y1 = t;
            y2 = t;
        }
        var rand = new Vector3(Util.RandomFloat(x1, x2), Util.RandomFloat(y1, y2));
        reko.MoveTo(rand, time, DeltaFloat.MoveType.DECEL);
        yield return new WaitWhile(() => reko.IsActive());
    }

    /// <summary>
    /// �w�i�̐������R���[�`��
    /// </summary>
    /// <returns></returns>
    private IEnumerator CreateStarCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Util.RandomFloat(0.3f, 0.7f));
            CreateStarOne();
        }
    }

    /// <summary>
    /// �_�~�[���P�쐬
    /// </summary>
    private void CreateStarOne()
    {
        var create = Instantiate(starDummy);
        create.transform.SetParent(starParent, true);
        create.gameObject.SetActive(true);
    }
}
