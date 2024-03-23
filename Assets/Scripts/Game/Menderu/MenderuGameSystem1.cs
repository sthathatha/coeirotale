using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// メンデルゲーム１
/// </summary>
public class MenderuGameSystem1 : GameSceneScriptBase
{
    #region 定数
    /// <summary>1ターンに取れる数</summary>
    private const int TURN_PICK_LIMIT = 3;

    /// <summary>プレイヤーカーソル色</summary>
    private readonly Color COLOR_PLAYER = Color.cyan;
    /// <summary>敵カーソル色</summary>
    private readonly Color COLOR_ENEMY = Color.magenta;

    /// <summary>
    /// セリフ送り待ちタイプ
    /// </summary>
    private enum TalkWaitType : int
    {
        None = 0,
        Button,
        Time,
    }
    #endregion

    #region メンバー
    /// <summary>１戦目親オブジェクト</summary>
    public GameObject battle1Parent = null;
    /// <summary>２戦目親オブジェクト</summary>
    public GameObject battle2Parent = null;

    /// <summary>吹き出し</summary>
    public GameObject talkWindow = null;
    /// <summary>セリフ</summary>
    public TMP_Text talkMessage = null;

    /// <summary>メンデル口</summary>
    public Animator menderuMouth = null;

    /// <summary>得点左</summary>
    public TMP_Text pointL = null;
    /// <summary>得点右</summary>
    public TMP_Text pointR = null;

    /// <summary>カーソル</summary>
    public GameObject cursorMain = null;
    /// <summary>終了ボタンカーソル</summary>
    public GameObject cursorEnd = null;

    /// <summary>種オブジェクトの親</summary>
    public GameObject seeds = null;

    /// <summary>カーソル移動SE</summary>
    public AudioClip se_cursor_move = null;
    /// <summary>種取得SE</summary>
    public AudioClip se_get = null;
    /// <summary>ターンエンドSE</summary>
    public AudioClip se_end = null;
    #endregion

    #region プライベート
    private Dictionary<int, Dictionary<int, SeedScript>> seedScripts;

    /// <summary>ゲーム進行コルーチン</summary>
    private Coroutine gameCoroutine = null;

    /// <summary>カーソル位置X</summary>
    private int cursorRow = 2;
    /// <summary>カーソル位置Y</summary>
    private int cursorCol = 2;

    /// <summary>敵ポイント</summary>
    private int pointLNum = 0;
    /// <summary>味方ポイント</summary>
    private int pointRNum = 0;

    /// <summary>このターンに取った個数</summary>
    private int pickCount = 0;
    /// <summary>敵が取るリスト</summary>
    private List<Vector2Int> enemyPickList = new List<Vector2Int>();

    /// <summary>口の動き止めるタイマー用</summary>
    private DeltaFloat mouthTimer = new DeltaFloat();
    #endregion

    #region 基底処理
    /// <summary>
    /// 初期化
    /// </summary>
    override public IEnumerator Start()
    {
        yield return base.Start();

        seedScripts = new Dictionary<int, Dictionary<int, SeedScript>>();
        for (int row = 0; row < 5; ++row)
        {
            var rowDictionary = new Dictionary<int, SeedScript>();
            for (int col = 0; col < 5; ++col)
            {
                var s = seeds.transform.Find($"seed{row}_{col}");
                rowDictionary.Add(col, s.GetComponent<SeedScript>());
            }
            seedScripts[row] = rowDictionary;
        }

        //メッセージ非表示
        talkWindow.SetActive(false);
        talkMessage.SetText("");

        //todo:フラグによる
        if (true)
        {
            battle1Parent.SetActive(true);
            battle2Parent.SetActive(false);
        }
        else
        {
            //battle1Parent.SetActive(false);
            //battle2Parent.SetActive(true);
        }

        // 
        cursorMain.GetComponent<SpriteRenderer>().color = COLOR_PLAYER;
        cursorEnd.GetComponent<SpriteRenderer>().color = COLOR_PLAYER;
        UpdateCursorLocation();
    }

    /// <summary>
    /// フェードイン後の初期処理
    /// </summary>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn()
    {
        var input = InputManager.GetInstance();
        var tutorial = ManagerSceneScript.GetInstance().GetMinigameTutorialWindow();

        yield return base.AfterFadeIn();

        yield return MenderuTalk("「メンデルの種」で勝負よ！");
        MenderuTalkStop();
        yield return null;

        // チュートリアル表示
        tutorial.SetTitle("メンデルの種");
        tutorial.SetText($"25個の種を交互に1～{TURN_PICK_LIMIT}個取り合います。\n" +
            $"種はターンが移る時に上下左右にある空欄の数だけ成長し、{SeedScript.SEED_MAX_NUM}まで成長すると取れなくなります。\n" +
            $"自分のターンに1個も取れなかった場合、負けとなります。");
        yield return tutorial.Open();
        yield return new WaitUntil(() => input.GetKeyPress(InputManager.Keys.South));
        yield return tutorial.Close();

        // 先攻
        yield return PlayerTurnStartCoroutine(true);
    }

    /// <summary>
    /// 処理
    /// </summary>
    override public void Update()
    {
        base.Update();
        mouthTimer.Update(Time.deltaTime);

        if (ManagerSceneScript.GetInstance().SceneState != ManagerSceneScript.State.Game) { return; }
        if (gameCoroutine != null) { return; }

        if (!mouthTimer.IsActive())
        {
            menderuMouth.SetBool("talking", false);
        }

        var sound = ManagerSceneScript.GetInstance().SoundManager;
        var input = InputManager.GetInstance();

        if (input.GetKeyPress(InputManager.Keys.Up))
        {
            if (cursorCol > 4) { return; }
            // 上
            if (cursorRow <= 0)
            {
                cursorRow = 4;
            }
            else
            {
                --cursorRow;
            }
            sound.PlaySE(se_cursor_move);
            UpdateCursorLocation();
        }
        else if (input.GetKeyPress(InputManager.Keys.Down))
        {
            if (cursorCol > 4) { return; }
            // 下
            if (cursorRow >= 4)
            {
                cursorRow = 0;
            }
            else
            {
                ++cursorRow;
            }
            sound.PlaySE(se_cursor_move);
            UpdateCursorLocation();
        }
        else if (input.GetKeyPress(InputManager.Keys.Right))
        {
            // 右
            if (cursorCol >= 5)
            {
                cursorCol = 0;
                cursorRow = 2;
            }
            else
            {
                ++cursorCol;
            }
            sound.PlaySE(se_cursor_move);
            UpdateCursorLocation();
        }
        else if (input.GetKeyPress(InputManager.Keys.Left))
        {
            // 左
            if (cursorCol >= 5)
            {
                cursorCol = 4;
                cursorRow = 2;
            }
            else if (cursorCol <= 0)
            {
                cursorCol = 5;
            }
            else
            {
                --cursorCol;
            }
            sound.PlaySE(se_cursor_move);
            UpdateCursorLocation();
        }
        else if (input.GetKeyPress(InputManager.Keys.East))
        {
            // 終了ボタンに移動
            cursorRow = 2;
            cursorCol = 5;
            sound.PlaySE(se_cursor_move);
            UpdateCursorLocation();
        }
        else if (input.GetKeyPress(InputManager.Keys.South))
        {
            // 現在位置選択のコルーチン
            gameCoroutine = StartCoroutine(SelectCoroutine());
        }
    }
    #endregion

    /// <summary>
    /// カーソル表示更新
    /// </summary>
    private void UpdateCursorLocation()
    {
        if (cursorCol >= 5)
        {
            cursorEnd.SetActive(true);
            cursorMain.SetActive(false);
        }
        else
        {
            cursorEnd.SetActive(false);
            cursorMain.SetActive(true);

            cursorMain.transform.localPosition = new Vector3(
                (cursorCol - 2) * 100,
                (cursorRow - 2) * (-100),
                0);
        }
    }

    /// <summary>
    /// カーソル位置の選択する処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator SelectCoroutine()
    {
        var manager = ManagerSceneScript.GetInstance();
        var sound = manager.SoundManager;

        if (cursorCol >= 5)
        {
            // ターン終了ボタン
            if (pickCount == 0)
            {
                //todo: 1個も取ってない場合ブブー
            }
            else
            {
                sound.PlaySE(se_end);
                yield return EnemyTurnCoroutine();
                if (manager.SceneState != ManagerSceneScript.State.Game) yield break;
            }
        }
        else
        {
            // 選択位置の種をとる
            var seed = seedScripts[cursorRow][cursorCol];
            if (seed.IsEnable())
            {
                sound.PlaySE(se_get);
                pointRNum += seed.GetNum();
                pointR.SetText(pointRNum.ToString());
                seed.Pick();

                pickCount++;
                if (pickCount >= TURN_PICK_LIMIT)
                {
                    // 上限取ったらターン終了
                    yield return EnemyTurnCoroutine();
                    if (manager.SceneState != ManagerSceneScript.State.Game) yield break;
                }
            }
            else
            {
                //todo: 取れない場合ブブー
            }
        }

        yield return null;
        gameCoroutine = null;
    }

    /// <summary>
    /// メンデル喋る処理
    /// </summary>
    /// <param name="text"></param>
    /// <param name="waitType"></param>
    /// <param name="waitTime"></param>
    private IEnumerator MenderuTalk(string text, TalkWaitType waitType = TalkWaitType.Button, float waitTime = 1f)
    {
        talkWindow.SetActive(true);
        talkMessage.SetText(text);
        menderuMouth.SetBool("talking", true);

        if (waitType == TalkWaitType.Button)
        {
            yield return new WaitUntil(() => InputManager.GetInstance().GetKeyPress(InputManager.Keys.South));
            yield return null;
        }
        else if (waitType == TalkWaitType.Time)
        {
            yield return new WaitForSeconds(waitTime);
        }
    }

    /// <summary>
    /// 喋りやめる処理
    /// </summary>
    private void MenderuTalkStop()
    {
        talkWindow.SetActive(false);
        menderuMouth.SetBool("talking", false);
    }

    /// <summary>
    /// 敵ターンのコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator EnemyTurnCoroutine()
    {
        var sound = ManagerSceneScript.GetInstance().SoundManager;

        GrowUp();
        yield return new WaitForSeconds(1f);

        cursorMain.GetComponent<SpriteRenderer>().color = COLOR_ENEMY;
        cursorEnd.GetComponent<SpriteRenderer>().color = COLOR_ENEMY;
        CreateEnemyPickList();
        yield return MenderuTalk("それじゃあ　私は…", TalkWaitType.Time, 2f);

        if (enemyPickList.Count == 0)
        {
            // 取れないので勝利
            yield return MenderuTalk("な、なんですって！", TalkWaitType.Time);
            yield return MenderuTalk("取れる種がひとつも無いじゃない！", TalkWaitType.Time);
            yield return MenderuTalk("私の負けね…お見事だわ！", TalkWaitType.Time);
            ManagerSceneScript.GetInstance().ExitGame();
            yield break;
        }

        foreach (var pickLoc in enemyPickList)
        {
            cursorRow = pickLoc.x;
            cursorCol = pickLoc.y;
            UpdateCursorLocation();

            var pickScr = seedScripts[pickLoc.x][pickLoc.y];
            pointLNum += pickScr.GetNum();
            pointL.SetText(pointLNum.ToString());
            pickScr.Pick();

            sound.PlaySE(se_get);
            if (pickLoc == enemyPickList.Last())
            {
                yield return MenderuTalk("これね！", TalkWaitType.Time);
            }
            else
            {
                yield return MenderuTalk("これと…", TalkWaitType.Time);
            }
        }

        yield return PlayerTurnStartCoroutine();
    }

    /// <summary>
    /// プレイヤーターン開始時コルーチン
    /// </summary>
    /// <param name="init">初期化時</param>
    /// <returns></returns>
    private IEnumerator PlayerTurnStartCoroutine(bool init = false)
    {
        var input = InputManager.GetInstance();
        if (!init)
        {
            GrowUp();
            yield return new WaitForSeconds(1f);

            var enableList = GetEnableList();
            if (enableList.Count == 0)
            {
                //取れないので敗北
                yield return MenderuTalk("あら、もう取れる種が無いわ", TalkWaitType.Time);
                yield return MenderuTalk("私の勝ちね、また遊びましょう", TalkWaitType.Time);
                ManagerSceneScript.GetInstance().ExitGame();
                yield break;
            }
        }

        cursorMain.GetComponent<SpriteRenderer>().color = COLOR_PLAYER;
        cursorEnd.GetComponent<SpriteRenderer>().color = COLOR_PLAYER;
        pickCount = 0;
        yield return MenderuTalk("さあ、あなたの番よ", TalkWaitType.None);
        mouthTimer.Set(0);
        mouthTimer.MoveTo(1f, 2f, DeltaFloat.MoveType.LINE);
    }

    #region 通常メソッド
    /// <summary>
    /// 敵が取る種を判定
    /// </summary>
    private void CreateEnemyPickList()
    {
        enemyPickList.Clear();
        // 有効な種ピックアップ
        var enableSeeds = GetEnableList();

        // 3個以下なら全部とる
        if (enableSeeds.Count <= TURN_PICK_LIMIT)
        {
            enemyPickList.AddRange(enableSeeds);
            return;
        }

        // 4個以上の場合のAI

        // 瀕死リスト
        var out0List = new List<Vector2Int>();
        // あと1個で瀕死リスト
        var out1List = new Dictionary<Vector2Int, List<Vector2Int>>();
        // あと2個で瀕死リスト
        var out2List = new Dictionary<Vector2Int, List<Vector2Int>>();
        // あと3個で瀕死リスト
        var out3List = new Dictionary<Vector2Int, List<Vector2Int>>();
        // それ以外のリスト
        var elseList = new List<Vector2Int>();
        // 一番多い数字
        var maxNum = -1;
        var maxNumList = new List<Vector2Int>();
        // チェック
        foreach (var loc in enableSeeds)
        {
            var scr = GetLocationSeedScript(loc.x, loc.y);
            var yuuyo = SeedScript.SEED_MAX_NUM - scr.GetNum();
            var growNum = CalcGrowNum(loc.x, loc.y);

            if (yuuyo <= growNum)
            {
                // 瀕死である
                out0List.Add(loc);
            }
            else
            {
                var crossList = GetCrossVector(loc);
                if (crossList.Count >= yuuyo - growNum)
                {
                    // 瀕死にできる可能性がある
                    switch (yuuyo - growNum)
                    {
                        case 1: out1List.Add(loc, crossList); break;
                        case 2: out2List.Add(loc, crossList); break;
                        default: out3List.Add(loc, crossList); break;
                    }
                }
                else
                {
                    // まだまだ無理
                    elseList.Add(loc);
                }
            }

            // 最大の数字を念のためリストアップ
            if (scr.GetNum() > maxNum)
            {
                maxNum = scr.GetNum();
                maxNumList.Clear();
                maxNumList.Add(loc);
            }
            else if (scr.GetNum() == maxNum)
            {
                maxNumList.Add(loc);
            }
        }

        // 不可の場合用　あと3→2→1→0→maxListのリストから順にいくつかランダムに選ぶ
        Action Random3Pick = () =>
        {
            var count = Util.RandomInt(1, TURN_PICK_LIMIT);

            for(int i=0; i<count; ++i)
            {
                if (out3List.Count > 0)
                {
                    var idx = Util.RandomInt(0, out3List.Count - 1);
                    var v = out3List.Select(p => p.Key).ElementAt(idx);
                    enemyPickList.Add(v);
                    out3List.Remove(v);
                }else if(out2List.Count > 0)
                {
                    var idx = Util.RandomInt(0, out2List.Count - 1);
                    var v = out2List.Select(p => p.Key).ElementAt(idx);
                    enemyPickList.Add(v);
                    out2List.Remove(v);
                }
                else if(out1List.Count > 0)
                {
                    var idx = Util.RandomInt(0, out1List.Count - 1);
                    var v = out1List.Select(p => p.Key).ElementAt(idx);
                    enemyPickList.Add(v);
                    out1List.Remove(v);
                }
                else if (out0List.Count > 0)
                {
                    var idx = Util.RandomInt(0, out0List.Count - 1);
                    var v = out0List[idx];
                    enemyPickList.Add(v);
                    out0List.Remove(v);
                }
                else
                {
                    if (enemyPickList.Count > 0) break;
                    // どうしても無いなら最大のを1個
                    enemyPickList.Add(maxNumList[Util.RandomInt(0, maxNumList.Count-1)]);
                }
            }
        };

        // 不可の場合用AI
        Action NormalPick = () =>
        {
            // 余裕ある物を4の倍数残すようにする
            var elsePickCount = elseList.Count % (TURN_PICK_LIMIT + 1);
            // すでに4の倍数なら
            if (elsePickCount == 0) { Random3Pick(); }

            List<int> pickList = Util.RandomUniqueIntList(0, elseList.Count - 1, elsePickCount);
            foreach(var pindex in pickList)
            {
                enemyPickList.Add(elseList[pindex]);
            }
        };

        if (elseList.Count > TURN_PICK_LIMIT)
        {
            // 瀕死にできないものが多すぎる
            NormalPick();
            return;
        }

        var wouldPickList = new List<Vector2Int>();
        var tmp1List = out1List;
        var tmp2List = out2List;
        var tmp3List = out3List;
        // elseListは確実に取る必要あり
        foreach (var pick in elseList)
        {
            wouldPickList.Add(pick);
            PickRemoveFromList(ref tmp1List, ref tmp2List, ref tmp3List, pick);
        }

        // この時点で完成してる
        if (tmp1List.Count == 0 && tmp2List.Count == 0 && tmp3List.Count == 0)
        {
            enemyPickList.AddRange(wouldPickList);
            if (enemyPickList.Count == 0)
            {
                NormalPick();
            }
            return;
        }

        // 残ってる時点で３個使って残ってたら無理とわかる
        if (wouldPickList.Count == 3)
        {
            NormalPick();
            return;
        }

        // 重みつきリストを作成
        var kohoList = new Dictionary<Vector2Int, int>();
        Action<Vector2Int, int> KohoAdd = (v, add) =>
        {
            if (kohoList.ContainsKey(v)) kohoList[v] += add;
            else kohoList[v] = add;
        };

        // tmp3は最大1個しか無いはずだが概念のためforeachで記述する
        foreach (var tmp3 in tmp3List)
        {
            foreach (var t in tmp3.Value)
            {
                KohoAdd(t, 1);
            }
            KohoAdd(tmp3.Key, 3);
        }
        foreach (var tmp2 in tmp2List)
        {
            foreach (var t in tmp2.Value)
            {
                KohoAdd(t, 1);
            }
            KohoAdd(tmp2.Key, 2);
        }
        foreach (var tmp1 in tmp1List)
        {
            foreach (var t in tmp1.Value)
            {
                KohoAdd(t, 1);
            }
            KohoAdd(tmp1.Key, 1);
        }
        // 重みの大きい順
        var sortedKoho = kohoList.OrderBy(k => k.Value).Reverse().Select(k => k.Key).ToList();
        // 追加で選べる数
        var pickableCount = TURN_PICK_LIMIT - wouldPickList.Count;

        // 可能なやり方を探す
        var coveringList = CalcCoveringList(sortedKoho, 0, pickableCount, tmp1List, tmp2List, tmp3List);
        if (coveringList == null)
        {
            // 見つからないなら無理
            NormalPick();
            return;
        }

        // 見つかったらセットして完成
        enemyPickList.AddRange(wouldPickList);
        enemyPickList.AddRange(coveringList);
    }

    /// <summary>
    /// out1～out3を網羅できるリストを取得
    /// </summary>
    /// <param name="list">候補リスト</param>
    /// <param name="startIndex">検索開始インデックス</param>
    /// <param name="canSelectNum">候補から選択できる数</param>
    /// <param name="out1List">あと1個</param>
    /// <param name="out2List">あと2個</param>
    /// <param name="out3List">あと3個</param>
    /// <returns></returns>
    private List<Vector2Int> CalcCoveringList(List<Vector2Int> list, int startIndex, int canSelectNum,
        Dictionary<Vector2Int, List<Vector2Int>> out1List,
        Dictionary<Vector2Int, List<Vector2Int>> out2List,
        Dictionary<Vector2Int, List<Vector2Int>> out3List)
    {
        // 全部網羅できてたら成功
        if (out1List.Count == 0 && out2List.Count == 0 && out3List.Count == 0) return new List<Vector2Int>();
        // もう選べないなら失敗
        if (canSelectNum == 0) return null;

        // 1個どれか選んで再帰
        for (int i = startIndex; i < list.Count; ++i)
        {
            var v = list[i];
            var tmp1List = out1List;
            var tmp2List = out2List;
            var tmp3List = out3List;

            // 1個除いた場合の残りリスト
            PickRemoveFromList(ref tmp1List, ref tmp2List, ref tmp3List, v);

            // 再帰
            var recursive = CalcCoveringList(list, i + 1, canSelectNum - 1, tmp1List, tmp2List, tmp3List);
            if (recursive != null)
            {
                var ret = new List<Vector2Int> { v };
                ret.AddRange(recursive);

                return ret;
            }
        }

        // 見つからなかったら失敗
        return null;
    }

    /// <summary>
    /// 必要な数のリストから座標を1個取り除く
    /// </summary>
    /// <param name="list1"></param>
    /// <param name="list2"></param>
    /// <param name="list3"></param>
    /// <param name="v"></param>
    private void PickRemoveFromList(ref Dictionary<Vector2Int, List<Vector2Int>> list1,
        ref Dictionary<Vector2Int, List<Vector2Int>> list2,
        ref Dictionary<Vector2Int, List<Vector2Int>> list3,
        Vector2Int v)
    {
        list1.Remove(v);
        list2.Remove(v);
        list3.Remove(v);

        var subList = list1.Where(p => p.Value.Contains(v)).Select(p => p.Key).ToList();
        foreach (var k in subList)
        {
            list1.Remove(k);
        }

        subList = list2.Where(p => p.Value.Contains(v)).Select(p => p.Key).ToList();
        foreach (var k in subList)
        {
            var val = list2[k];
            val.Remove(v);
            list1.Add(k, val);
            list2.Remove(k);
        }

        subList = list3.Where(p => p.Value.Contains(v)).Select(p => p.Key).ToList();
        foreach (var k in subList)
        {
            var val = list3[k];
            val.Remove(v);
            list2.Add(k, val);
            list3.Remove(k);
        }
    }

    /// <summary>
    /// 有効な種リスト
    /// </summary>
    /// <returns></returns>
    private List<Vector2Int> GetEnableList()
    {
        var enableSeeds = new List<Vector2Int>();
        foreach (var seedRow in seedScripts)
        {
            foreach (var seed in seedRow.Value)
            {
                if (seed.Value.IsEnable())
                {
                    enableSeeds.Add(new Vector2Int(seedRow.Key, seed.Key));
                }
            }
        }
        return enableSeeds;
    }

    /// <summary>
    /// 種が育つ
    /// </summary>
    private void GrowUp()
    {
        foreach (var seedRow in seedScripts)
            foreach (var seed in seedRow.Value)
            {
                var scr = seed.Value;
                if (!scr.IsEnable()) continue;

                int growNum = CalcGrowNum(seedRow.Key, seed.Key);
                scr.GrowNum(growNum);
            }
    }

    /// <summary>
    /// 上下左右の4ベクトルリスト
    /// </summary>
    /// <param name="center">中心座標</param>
    /// <param name="beingOnly">種が存在する座標のみ</param>
    /// <param name="enableOnly">取得できる座標のみ</param>
    /// <returns></returns>
    private List<Vector2Int> GetCrossVector(Vector2Int center, bool beingOnly = true, bool enableOnly = true)
    {
        var koho = new List<Vector2Int>
        {
            center + new Vector2Int(-1, 0),
            center + new Vector2Int(1, 0),
            center + new Vector2Int(0, 1),
            center + new Vector2Int(0, -1),
        };

        var ret = new List<Vector2Int>();
        foreach (var v in koho)
        {
            if (beingOnly)
            {
                var scr = GetLocationSeedScript(v.x, v.y);
                if (scr == null) continue;
                if (scr.GetNum() < 0) continue;
                if (enableOnly)
                {
                    if (!scr.IsEnable()) continue;
                }
            }

            ret.Add(v);
        }

        return ret;
    }

    /// <summary>
    /// 成長する数を計算
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <returns></returns>
    private int CalcGrowNum(int row, int col)
    {
        // 上下左右カウント
        var crossList = GetCrossVector(new Vector2Int(row, col), true, false);
        return 4 - crossList.Count;
    }

    /// <summary>
    /// 種スクリプト取得
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <returns></returns>
    private SeedScript GetLocationSeedScript(int row, int col)
    {
        if (!seedScripts.ContainsKey(row)) return null;
        var seedRow = seedScripts[row];

        if (!seedRow.ContainsKey(col)) return null;

        return seedRow[col];
    }
    #endregion
}
