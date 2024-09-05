using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// つくよみちゃん本戦　攻撃のみの大駒
/// </summary>
public class TukuyomiGameKomaSmallC : MonoBehaviour
{
    /// <summary>
    /// 種類
    /// </summary>
    public enum KomaCType : int
    {
        NormalHisya = 0,
        NormalKaku,
        Dmg1Kaku1,
        Dmg1Kaku2,
        Dmg2Hisya1,
        Dmg2Hisya2,
        Dmg2Hisya3,
        Dmg3Hisya1,
        Dmg3Hisya2,
        Dmg3Kaku,
        Dmg4Kaku,
        Dmg4Kyou,
    }

    private const float MISO_RIGHT_X = 250f;
    private const float MISO_LEFT_X = -250f;
    private const float MISO_TOP_Y = 3f;
    private readonly Vector3 FIELD_CENTER = new Vector3(0, -172f);
    private const float FIELD_R = 192f;

    public TukuyomiGameSystem system;

    private KomaCType type;
    private TukuyomiGameSystem.Koma komaKind;
    private List<BlastNode> nodes = new List<BlastNode>();
    private float headRot;

    /// <summary>
    /// ものによる動作パラメータを作成
    /// </summary>
    /// <param name="ctype"></param>
    /// <param name="rootPos">開始座標</param>
    /// <param name="rot">角度</param>
    public void CreateParams(KomaCType ctype, Vector3 rootPos, float rot = Mathf.PI * 1.5f)
    {
        gameObject.SetActive(true);
        GetComponent<ModelUtil>().FadeOutImmediate();
        type = ctype;
        nodes.Clear();

        var tmp = rootPos;
        var tmpTime = 0f;
        switch (type)
        {
            case KomaCType.NormalHisya:
                komaKind = TukuyomiGameSystem.Koma.Hisya;
                // 通常の飛車　開始座標から下に動きつつ左右に衝撃波
                while (tmp.y > -Constant.SCREEN_HEIGHT * 0.5f - 50f)
                {
                    tmpTime += 0.1f;
                    tmp.y -= TukuyomiGameKomaSmallB.KOMAB_SIZE * 2f;
                    nodes.Add(new BlastNode(tmp, tmpTime, true));
                }
                break;
            case KomaCType.NormalKaku:
                komaKind = TukuyomiGameSystem.Koma.Kaku;
                // 通常の角　開始座標から右下に動きつつ右上、左下に衝撃波
                var first = true;
                while (tmp.y > -Constant.SCREEN_HEIGHT * 0.5f - 50f)
                {
                    tmpTime += 0.1f;
                    tmp.x += TukuyomiGameKomaSmallB.KOMAB_SIZE * 1.4f;
                    tmp.y -= TukuyomiGameKomaSmallB.KOMAB_SIZE * 1.4f;
                    nodes.Add(new BlastNode(tmp, tmpTime, true, first ? 0 : 1));
                    first = false;
                }
                break;
            case KomaCType.Dmg1Kaku1:
            case KomaCType.Dmg1Kaku2:   // ダメージ1の角1 時計回り・反時計回り
                {
                    komaKind = TukuyomiGameSystem.Koma.Kaku;
                    var hugo = (type == KomaCType.Dmg1Kaku1) ? 1 : -1;
                    rootPos = new Vector3(FIELD_CENTER.x - hugo * FIELD_R, FIELD_CENTER.y + FIELD_R * 2f);
                    rootPos.x += Util.RandomFloat(-40f, 40f);
                    tmp = rootPos;
                    // 右下へ
                    for (var i = 0; i < 3; ++i)
                    {
                        tmpTime += 0.08f;
                        tmp.x += hugo * FIELD_R / 3f;
                        tmp.y -= FIELD_R / 3f;
                        nodes.Add(new BlastNode(tmp, tmpTime, false));
                    }
                    for (var i = 0; i < 3; ++i)
                    {
                        tmpTime += 0.08f;
                        tmp.x += hugo * FIELD_R / 3f;
                        tmp.y -= FIELD_R / 3f;
                        nodes.Add(new BlastNode(tmp, tmpTime, true, hugo));
                    }
                    // 左下へ
                    for (var i = 0; i < 3; ++i)
                    {
                        tmpTime += 0.08f;
                        tmp.x -= hugo * FIELD_R / 3f;
                        tmp.y -= FIELD_R / 3f;
                        nodes.Add(new BlastNode(tmp, tmpTime, true, -hugo));
                    }
                    // 左上へ
                    for (var i = 0; i < 3; ++i)
                    {
                        tmpTime += 0.08f;
                        tmp.x -= hugo * FIELD_R / 3f;
                        tmp.y += FIELD_R / 3f;
                        nodes.Add(new BlastNode(tmp, tmpTime, true, hugo));
                    }
                    // 右上へ
                    for (var i = 0; i < 3; ++i)
                    {
                        tmpTime += 0.08f;
                        tmp.x += hugo * FIELD_R / 3f;
                        tmp.y += FIELD_R / 3f;
                        nodes.Add(new BlastNode(tmp, tmpTime, true, -hugo));
                    }
                }
                break;
            case KomaCType.Dmg2Hisya1:
            case KomaCType.Dmg2Hisya2:
            case KomaCType.Dmg2Hisya3:  // ダメージ2の飛車　←　→　←
                {
                    komaKind = TukuyomiGameSystem.Koma.Hisya;
                    var hugo = (type == KomaCType.Dmg2Hisya2) ? -1 : 1;
                    rootPos.y = MISO_TOP_Y;
                    if (type == KomaCType.Dmg2Hisya2) rootPos.y += 40f;
                    else if (type == KomaCType.Dmg2Hisya3) rootPos.y += 80f;
                    rootPos.x = hugo * MISO_RIGHT_X;
                    tmp = rootPos;
                    for (var i = 0; i < 7; ++i)
                    {
                        tmpTime += 0.1f;
                        tmp.x += -hugo * 50f;
                        nodes.Add(new BlastNode(tmp, tmpTime, true));
                    }
                }
                break;
            case KomaCType.Dmg3Hisya1:
            case KomaCType.Dmg3Hisya2:
                {
                    komaKind = TukuyomiGameSystem.Koma.Hisya;
                    var hugo = (type == KomaCType.Dmg3Hisya1) ? 1 : -1;
                    rootPos.x = hugo * MISO_RIGHT_X + 100f;
                    rootPos.y = FIELD_CENTER.y + Util.RandomFloat(-110f, 110f);
                    tmp = rootPos;
                    tmp.x = hugo * MISO_RIGHT_X;
                    tmpTime += 0.2f;
                    nodes.Add(new BlastNode(tmp, tmpTime, true, hugo));
                    nodes.Add(new BlastNode(tmp, tmpTime + 0.5f, false));
                }
                break;
            case KomaCType.Dmg3Kaku:
                {
                    komaKind = TukuyomiGameSystem.Koma.Kaku;
                    rootPos.x = Util.RandomFloat(-200f, 200f);
                    rootPos.y = MISO_TOP_Y + 100f;
                    tmp = rootPos;
                    tmp.y = MISO_TOP_Y;
                    tmpTime += 0.2f;
                    nodes.Add(new BlastNode(tmp, tmpTime, true));
                    nodes.Add(new BlastNode(tmp, tmpTime + 0.5f, false));
                }
                break;
            case KomaCType.Dmg4Kaku:
                // 上から来て中央に押し込む
                {
                    komaKind = TukuyomiGameSystem.Koma.Kaku;
                    rootPos.x = 0f;
                    rootPos.y = FIELD_CENTER.y + 540f;
                    tmp = rootPos;
                    for (var i = 0; i < 9; ++i)
                    {
                        tmp.y -= 60f;
                        tmpTime += 0.2f;
                        nodes.Add(new BlastNode(tmp, tmpTime, true));
                    }
                }
                break;
            case KomaCType.Dmg4Kyou:
                // 角度で現れて1発
                {
                    komaKind = TukuyomiGameSystem.Koma.Kyou;
                    var identity = Util.GetVector3IdentityFromRot(rot);
                    rootPos = FIELD_CENTER - identity * FIELD_R * 1.5f;
                    tmp = FIELD_CENTER - identity * FIELD_R;
                    nodes.Add(new BlastNode(tmp, 0.2f, false));
                    nodes.Add(new BlastNode(tmp, 0.6f, true));
                }
                break;
        }

        transform.rotation = Util.GetRotateQuaternion(rot - Mathf.PI * 0.5f);
        transform.position = rootPos;
    }

    /// <summary>
    /// ！を表示
    /// </summary>
    public IEnumerator ShowWarning()
    {
        var color = TukuyomiGameKomaSmall.GetKomaColor(komaKind);
        var maxTime = 0f;
        foreach (var node in nodes)
        {
            if (!node.blast) continue;

            system.CreateWarning(node.pos, color, node.time);
            if (node.time > maxTime) maxTime = node.time;
        }
        yield return new WaitForSeconds(maxTime + 0.7f);
    }

    /// <summary>
    /// 攻撃実行
    /// </summary>
    /// <returns></returns>
    public IEnumerator ExecAttackCoroutine()
    {
        var model = GetComponent<ModelUtil>();
        model.FadeIn(nodes[0].time, TukuyomiGameKomaSmall.GetKomaColor(komaKind));
        // ノードごとに移動しながら攻撃する
        var nowTime = 0f;
        var p = new DeltaVector3();
        p.Set(transform.position);
        foreach (var node in nodes)
        {
            // 移動
            p.MoveTo(node.pos, node.time - nowTime, DeltaFloat.MoveType.LINE);
            while (p.IsActive())
            {
                yield return null;
                p.Update(Time.deltaTime);
                transform.position = p.Get();
            }
            nowTime = node.time;

            if (!node.blast) continue;
            // 攻撃
            NodeAttack(node);
        }

        if (type == KomaCType.Dmg4Kyou)
        {
            // 香車は少しバック KomaSmallBの香車処理と同じ
            var backRot = headRot - Mathf.PI;
            var backPos = transform.position + Util.GetVector3IdentityFromRot(backRot) * 10f;
            p.Set(transform.position);
            p.MoveTo(backPos, 0.2f, DeltaFloat.MoveType.DECEL);
            while (p.IsActive())
            {
                yield return null;
                p.Update(Time.deltaTime);
                transform.position = p.Get();
            }
        }

        model.FadeOut(0.5f);
        yield return new WaitWhile(() => model.IsFading());
        Destroy(gameObject);
    }

    /// <summary>
    /// 攻撃
    /// </summary>
    /// <param name="node"></param>
    private void NodeAttack(BlastNode node)
    {
        system.resource.PlaySE(system.resource.se_attack_effect_A);
        switch (type)
        {
            case KomaCType.NormalHisya:
                // 左右
                system.CreateBlast(node.pos, 0f);
                system.CreateBlast(node.pos, Mathf.PI);
                break;
            case KomaCType.NormalKaku:
                // 右上、左下
                if (node.generalParam == 0)
                {
                    // 最初に右下に撃っておく
                    system.CreateBlast(node.pos, Mathf.PI * 1.75f);
                }
                else
                {
                    system.CreateBlast(node.pos, Mathf.PI * 0.25f);
                    system.CreateBlast(node.pos, Mathf.PI * 1.25f);
                }
                break;
            case KomaCType.Dmg1Kaku1:
            case KomaCType.Dmg1Kaku2:
                // 1の場合は右上左下、-1は右下左上
                if (node.generalParam == 1)
                {
                    system.CreateBlast(node.pos, Mathf.PI * 0.25f);
                    system.CreateBlast(node.pos, Mathf.PI * 1.25f);
                }
                else
                {
                    system.CreateBlast(node.pos, Mathf.PI * 0.75f);
                    system.CreateBlast(node.pos, Mathf.PI * 1.75f);
                }
                break;
            case KomaCType.Dmg2Hisya1:
            case KomaCType.Dmg2Hisya2:
            case KomaCType.Dmg2Hisya3:
                // 縦
                system.CreateBlast(node.pos, Mathf.PI * 1.5f);
                break;
            case KomaCType.Dmg3Hisya1:
                // 左へ
                system.CreateBlast(node.pos, Mathf.PI);
                break;
            case KomaCType.Dmg3Hisya2:
                // 右へ
                system.CreateBlast(node.pos, 0f);
                break;
            case KomaCType.Dmg3Kaku:
            case KomaCType.Dmg4Kaku:
                // 下2方向
                system.CreateBlast(node.pos, Mathf.PI * 1.25f);
                system.CreateBlast(node.pos, Mathf.PI * 1.75f);
                break;
            case KomaCType.Dmg4Kyou:
                {
                    var laserRot = Mathf.Deg2Rad * transform.rotation.eulerAngles.z + Mathf.PI / 2f;
                    system.CreateLaser(transform.position, laserRot);
                }
                break;
        }
    }

    /// <summary>
    /// 衝撃波を出す場所パラメータ
    /// </summary>
    private class BlastNode
    {
        public Vector3 pos;
        public float time;
        public bool blast;
        public int generalParam;

        public BlastNode(Vector3 _pos, float _time, bool _blast, int _param = 0)
        {
            pos = _pos;
            time = _time;
            blast = _blast;
            generalParam = _param;
        }
    }
}
