using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

/// <summary>
/// MANA対戦スピード
/// </summary>
public class MANAGameSystemA : GameSceneScriptBase
{
    #region 定数

    /// <summary>初期化時　場に出す速度</summary>
    private const float MOVE_TIME_START = 1f;
    /// <summary>プレイ時　場に出す速度</summary>
    private const float MOVE_TIME_PLAY = 0.2f;

    /// <summary>カード表示優先順位</summary>
    private const int BASE_PRIORITY = 100;

    /// <summary>CPUプレイ間隔</summary>
    private const float CPU_PLAY_TIME = 1f;

    /// <summary>Bゲーム時の間隔に掛け算</summary>
    private const float CPU_TIME_RATE_B = 0.8f;

    #endregion

    #region メンバー

    public GameObject modeA;

    public Transform posP1;
    public Transform posP2;
    public Transform posP3;
    public Transform posP4;
    public Transform posYamaP;

    public Transform posE1;
    public Transform posE2;
    public Transform posE3;
    public Transform posE4;
    public Transform posYamaE;

    public Transform posField1;
    public Transform posField2;

    public Transform cursor;

    public GameObject dummyCard;
    public Transform cardParent;

    public MANAGameMessage message;

    public AudioClip cardMoveSE;

    #endregion

    #region プライベート

    /// <summary>カード情報</summary>
    private struct CardParam
    {
        public MANAGameCard.Suit suit;
        public int num;
        public CardParam(MANAGameCard.Suit s, int n) { suit = s; num = n; }
    }
    /// <summary>プレイヤー山札情報</summary>
    private List<CardParam> playerYamaList = new List<CardParam>();
    /// <summary>敵山札情報</summary>
    private List<CardParam> enemyYamaList = new List<CardParam>();

    /// <summary>プレイヤー山札一番下のカード</summary>
    private MANAGameCard pYamaLastCard;
    /// <summary>プレイヤー手札１</summary>
    private MANAGameCard pHandCard1;
    /// <summary>プレイヤー手札２</summary>
    private MANAGameCard pHandCard2;
    /// <summary>プレイヤー手札３</summary>
    private MANAGameCard pHandCard3;
    /// <summary>プレイヤー手札４</summary>
    private MANAGameCard pHandCard4;
    /// <summary>敵山札一番下のカード</summary>
    private MANAGameCard eYamaLastCard;
    /// <summary>敵手札１</summary>
    private MANAGameCard eHandCard1;
    /// <summary>敵手札２</summary>
    private MANAGameCard eHandCard2;
    /// <summary>敵手札３</summary>
    private MANAGameCard eHandCard3;
    /// <summary>敵手札４</summary>
    private MANAGameCard eHandCard4;
    /// <summary>場の札１</summary>
    private MANAGameCard field1Card;
    /// <summary>場の札２</summary>
    private MANAGameCard field2Card;

    private MANAGameCard field1CardTmp = null;
    private MANAGameCard field2CardTmp = null;

    private enum MANAState : int
    {
        Initialize = 0,
        Start,
        StartCheck,
        Play,
        End,
    }
    /// <summary>MANAゲームの状態</summary>
    private MANAState state = MANAState.Initialize;

    /// <summary>カーソル位置</summary>
    private int playerCursor = 1;

    /// <summary>CPUの動作間隔</summary>
    private float cpuPlayWait = 0f;

    /// <summary>排他制御用オブジェクト</summary>
    private object lockObj = new object();

    #endregion

    #region 基底

    /// <summary>
    /// 初期化
    /// </summary>
    /// <returns></returns>
    override public IEnumerator Start()
    {
        state = MANAState.Initialize;
        message.SetText("");

        UpdateCursor();

        modeA.SetActive(true);
        cpuPlayWait = CPU_PLAY_TIME;
        if (GetLoseCount() > 3)
        {
            cpuPlayWait += (GetLoseCount() - 3) * 0.1f;
        }
        if (IsBossRush())
        {
            cpuPlayWait *= CPU_TIME_RATE_B;
        }

        // 山札ランダム作成
        var pRandom = Util.RandomUniqueIntList(1, 26, 26);
        foreach (var c in pRandom)
        {
            var suit = c > 13 ? MANAGameCard.Suit.Spade : MANAGameCard.Suit.Crub;
            var num = c > 13 ? c - 13 : c;
            if (IsBossRush())
                enemyYamaList.Add(new CardParam(suit, num));
            else
                playerYamaList.Add(new CardParam(suit, num));
        }

        var eRandom = Util.RandomUniqueIntList(1, 26, 26);
        foreach (var c in eRandom)
        {
            var suit = c > 13 ? MANAGameCard.Suit.Heart : MANAGameCard.Suit.Dia;
            var num = c > 13 ? c - 13 : c;
            if (IsBossRush())
                playerYamaList.Add(new CardParam(suit, num));
            else
                enemyYamaList.Add(new CardParam(suit, num));
        }

        // 山札Last表示
        var pLast = GameObject.Instantiate(dummyCard, cardParent);
        var eLast = GameObject.Instantiate(dummyCard, cardParent);
        pLast.SetActive(true);
        eLast.SetActive(true);
        pYamaLastCard = pLast.GetComponent<MANAGameCard>();
        eYamaLastCard = eLast.GetComponent<MANAGameCard>();
        pYamaLastCard.MoveTo(posYamaP.localPosition);
        eYamaLastCard.MoveTo(posYamaE.localPosition);
        pYamaLastCard.ShowCard(false);
        eYamaLastCard.ShowCard(false);
        pYamaLastCard.SetPriority(BASE_PRIORITY);
        eYamaLastCard.SetPriority(BASE_PRIORITY);

        yield return base.Start();
    }

    /// <summary>
    /// フェードイン後
    /// </summary>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn()
    {
        yield return base.AfterFadeIn();
        yield return new WaitForSeconds(0.5f);

        // 手札４枚配る
        for (var i = 4; i > 0; i--)
        {
            StartCoroutine(GetHandCoroutine(true, i, false));
            StartCoroutine(GetHandCoroutine(false, i, false));
            yield return new WaitForSeconds(MOVE_TIME_PLAY);
        }
        yield return new WaitForSeconds(1f);

        // チュートリアル表示
        var tutorial = ManagerSceneScript.GetInstance().GetMinigameTutorialWindow();
        var input = InputManager.GetInstance();
        if (IsBossRush())
        {
            tutorial.SetTitle(StringMinigameMessage.ManaB_Title);
            tutorial.SetText(StringMinigameMessage.ManaB_Tutorial);
        }
        else
        {
            tutorial.SetTitle(StringMinigameMessage.ManaA_Title);
            tutorial.SetText(StringMinigameMessage.ManaA_Tutorial);
        }
        yield return tutorial.Open();
        yield return new WaitUntil(() => input.GetKeyPress(InputManager.Keys.South));
        yield return tutorial.Close();

        StartCoroutine(InitFieldCoroutine());
        StartCoroutine(CpuPlayCoroutine());
    }

    /// <summary>
    /// フレーム処理
    /// </summary>
    override public void Update()
    {
        base.Update();

        if (ManagerSceneScript.GetInstance().SceneState != ManagerSceneScript.State.Game) { return; }
        if (state == MANAState.End) { return; }

        var input = InputManager.GetInstance();

        // 左右カーソル移動はいつでも
        if (input.GetKeyPress(InputManager.Keys.Right) || input.GetKeyPress(InputManager.Keys.Left))
        {
            var move = input.GetKeyPress(InputManager.Keys.Left) ? 1 : -1;
            playerCursor += move;
            if (playerCursor <= 0) playerCursor = 4;
            else if (playerCursor > 4) playerCursor = 1;

            UpdateCursor();
        }

        // カード出す操作はPlay中
        if (state == MANAState.Play)
        {
            if (input.GetKeyPress(InputManager.Keys.South))
            {
                StartCoroutine(CardPlayCoroutine(true, playerCursor));
            }
        }
    }

    #endregion

    #region コルーチン

    /// <summary>
    /// CPUプレイ
    /// </summary>
    /// <returns></returns>
    private IEnumerator CpuPlayCoroutine()
    {
        var timer = new DeltaFloat();
        timer.Set(0);
        while (true)
        {
            // プレイ中以外はプレイまで待つ
            if (state != MANAState.Play)
            {
                yield return new WaitUntil(() => state == MANAState.Play);
                timer.MoveTo(0, cpuPlayWait, DeltaFloat.MoveType.LINE);
            }

            if (timer.IsActive())
            {
                timer.Update(Time.deltaTime);
                yield return null;
                continue;
            }

            // 出すカード選択
            var playIndex = -1;
            if (CanPlayCard(eHandCard1))
            {
                playIndex = 1;
            }
            else if (CanPlayCard(eHandCard2))
            {
                playIndex = 2;
            }
            else if (CanPlayCard(eHandCard3))
            {
                playIndex = 3;
            }
            else if (CanPlayCard(eHandCard4))
            {
                playIndex = 4;
            }

            if (playIndex > 0)
            {
                // 出す
                yield return CardPlayCoroutine(false, playIndex);
                timer.MoveTo(0, cpuPlayWait, DeltaFloat.MoveType.LINE);
            }
            else
            {
                // 出せるのが無いからちょっと待つ
                timer.MoveTo(0, cpuPlayWait / 2f, DeltaFloat.MoveType.LINE);
            }
        }
    }

    /// <summary>
    /// 場に２枚出す
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    private IEnumerator InitFieldCoroutine(float delay = -1f)
    {
        lock (lockObj)
        {
            if (state == MANAState.Start) { yield break; }
            state = MANAState.Start;
        }

        if (delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }

        // 山札または手札を選択
        MANAGameCard pCard = null;
        if (playerYamaList.Count > 0)
        {
            pCard = PickUpYamaCard(true);
        }
        else if (pHandCard1 != null)
        {
            pCard = pHandCard1;
            pHandCard1 = null;
        }
        else if (pHandCard2 != null)
        {
            pCard = pHandCard2;
            pHandCard2 = null;
        }
        else if (pHandCard3 != null)
        {
            pCard = pHandCard3;
            pHandCard3 = null;
        }
        else if (pHandCard4 != null)
        {
            pCard = pHandCard4;
            pHandCard4 = null;
        }
        MANAGameCard eCard = null;
        if (enemyYamaList.Count > 0)
        {
            eCard = PickUpYamaCard(false);
        }
        else if (eHandCard1 != null)
        {
            eCard = eHandCard1;
            eHandCard1 = null;
        }
        else if (eHandCard2 != null)
        {
            eCard = eHandCard2;
            eHandCard2 = null;
        }
        else if (eHandCard3 != null)
        {
            eCard = eHandCard3;
            eHandCard3 = null;
        }
        else if (eHandCard4 != null)
        {
            eCard = eHandCard4;
            eHandCard4 = null;
        }

        // フィールド１と２に移動
        pCard.MoveTo(posField1.localPosition, MOVE_TIME_START);
        eCard.MoveTo(posField2.localPosition, MOVE_TIME_START);
        pCard.SetPriority(BASE_PRIORITY + 10);
        eCard.SetPriority(BASE_PRIORITY + 20);
        ManagerSceneScript.GetInstance().soundMan.PlaySE(cardMoveSE);
        yield return new WaitWhile(() => pCard.IsMoving() || eCard.IsMoving());
        pCard.SetPriority(BASE_PRIORITY);
        eCard.SetPriority(BASE_PRIORITY);

        // フィールドに前のカードがあったら消す
        if (field1Card != null)
        {
            Destroy(field1Card.gameObject);
        }
        if (field2Card != null)
        {
            Destroy(field2Card.gameObject);
        }
        field1Card = pCard;
        field2Card = eCard;

        // メッセージ表示
        message.SetText(StringMinigameMessage.ManaA_Ready);
        message.SetAlpha(1);
        yield return new WaitForSeconds(1f);
        message.SetAlpha(0, 1f);
        message.SetText(StringMinigameMessage.ManaA_Go);

        field1Card.ShowCard(true);
        field2Card.ShowCard(true);

        state = MANAState.StartCheck;
        CheckGame();
    }

    /// <summary>
    /// 山札から手に１枚移動
    /// </summary>
    /// <param name="isPlayer"></param>
    /// <param name="index"></param>
    /// <param name="check"></param>
    /// <returns></returns>
    private IEnumerator GetHandCoroutine(bool isPlayer, int index = -1, bool check = true)
    {
        var card = PickUpYamaCard(isPlayer);
        // 山無ければなにもしない
        if (card == null)
        {
            yield break;
        }

        // 場所選択
        Vector3 newPos;
        if (isPlayer)
        {
            if (index < 0)
            {
                if (pHandCard1 == null) index = 1;
                else if (pHandCard2 == null) index = 2;
                else if (pHandCard3 == null) index = 3;
                else if (pHandCard4 == null) index = 4;
            }

            newPos = index switch
            {
                1 => posP1.localPosition,
                2 => posP2.localPosition,
                3 => posP3.localPosition,
                _ => posP4.localPosition
            };
        }
        else
        {
            if (index < 0)
            {
                if (eHandCard1 == null) index = 1;
                else if (eHandCard2 == null) index = 2;
                else if (eHandCard3 == null) index = 3;
                else if (eHandCard4 == null) index = 4;
            }

            newPos = index switch
            {
                1 => posE1.localPosition,
                2 => posE2.localPosition,
                3 => posE3.localPosition,
                _ => posE4.localPosition
            };
        }

        // 相手のCheckGameでnull扱いになってしまうため、取り始めの瞬間から持ってることにする
        switch (isPlayer, index)
        {
            case (true, 1): pHandCard1 = card; break;
            case (true, 2): pHandCard2 = card; break;
            case (true, 3): pHandCard3 = card; break;
            case (true, 4): pHandCard4 = card; break;
            case (false, 1): eHandCard1 = card; break;
            case (false, 2): eHandCard2 = card; break;
            case (false, 3): eHandCard3 = card; break;
            case (false, 4): eHandCard4 = card; break;
        }

        // 移動
        card.MoveTo(newPos, MOVE_TIME_PLAY);
        ManagerSceneScript.GetInstance().soundMan.PlaySE(cardMoveSE);
        yield return new WaitWhile(() => card.IsMoving());

        // 表にして手札に設定
        card.ShowCard(true);
        card.SetPriority(BASE_PRIORITY);

        if (check)
        {
            CheckGame();
        }
    }

    /// <summary>
    /// カードを出す操作
    /// </summary>
    /// <param name="isPlayer"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    private IEnumerator CardPlayCoroutine(bool isPlayer, int index)
    {
        var sound = ManagerSceneScript.GetInstance().soundMan;

        // 出すカードのクラス
        MANAGameCard playCard;
        playCard = (isPlayer, index) switch
        {
            (true, 1) => pHandCard1,
            (true, 2) => pHandCard2,
            (true, 3) => pHandCard3,
            (true, 4) => pHandCard4,
            (false, 1) => eHandCard1,
            (false, 2) => eHandCard2,
            (false, 3) => eHandCard3,
            _ => eHandCard4,
        };

        // 出すカードが無い時・処理中の時はエラー
        if (playCard == null || playCard.IsMoving())
        {
            if (isPlayer)
            {
                sound.PlaySE(sound.commonSeError);
            }
            yield break;
        }

        // 出す場を選択
        var target = 0;
        var f1TmpNum = field1CardTmp?.GetNum() ?? 0;
        var f2TmpNum = field2CardTmp?.GetNum() ?? 0;
        var f1Num = field1Card.GetNum();
        var f2Num = field2Card.GetNum();
        var playNum = playCard.GetNum();
        if (f1TmpNum == 0 && CalcCardDiff(f1Num, playNum) == 1)
        {
            // １がフリーで出せる
            target = 1;
        }
        else if (f2TmpNum == 0 && CalcCardDiff(f2Num, playNum) == 1)
        {
            // ２がフリーで出せる
            target = 2;
        }
        else if (f1TmpNum != 0 && CalcCardDiff(f1TmpNum, playNum) == 1)
        {
            // １に出てきてる所に出せる
            target = 1;
        }
        else if (f2TmpNum != 0 && CalcCardDiff(f2TmpNum, playNum) == 1)
        {
            // ２に出てきてる所に出せる
            target = 2;
        }

        // 出せない場合
        if (target == 0)
        {
            if (f1TmpNum != 0 && CalcCardDiff(f1Num, playNum) == 1)
            {
                // １に先に出されそう（お手つきする）
                target = 1;
            }
            else if (f2TmpNum != 0 && CalcCardDiff(f2Num, playNum) == 1)
            {
                // ２に先に出されそう
                target = 2;
            }
        }

        // それ以外は近い場所に
        if (target == 0)
        {
            target = index > 2 ? 2 : 1;
        }

        // カードを場に動かす
        var tmp = target == 1 ? field1CardTmp : field2CardTmp;
        playCard.SetPriority(tmp == null ? BASE_PRIORITY + 10 : tmp.GetPriority() + 2);
        playCard.MoveTo(target == 1 ? posField1.localPosition : posField2.localPosition, MOVE_TIME_PLAY);
        if (target == 1)
        {
            field1CardTmp = playCard;
        }
        else
        {
            field2CardTmp = playCard;
        }
        sound.PlaySE(cardMoveSE);
        yield return new WaitWhile(() => playCard.IsMoving());

        // tmpをクリア
        if (target == 1 && field1CardTmp == playCard)
        {
            field1CardTmp = null;
        }
        else if (target == 2 && field2CardTmp == playCard)
        {
            field2CardTmp = null;
        }

        // 
        var targetCard = target == 1 ? field1Card : field2Card;
        var nowDiff = CalcCardDiff(targetCard.GetNum(), playCard.GetNum());
        if (nowDiff == 1)
        {
            // 下のカードを消す
            Destroy(targetCard.gameObject);
            if (target == 1)
            {
                field1Card = playCard;
            }
            else
            {
                field2Card = playCard;
            }
            playCard.SetPriority(BASE_PRIORITY);

            // 出した手札を空にして山から引く
            switch (isPlayer, index)
            {
                case (true, 1): pHandCard1 = null; break;
                case (true, 2): pHandCard2 = null; break;
                case (true, 3): pHandCard3 = null; break;
                case (true, 4): pHandCard4 = null; break;
                case (false, 1): eHandCard1 = null; break;
                case (false, 2): eHandCard2 = null; break;
                case (false, 3): eHandCard3 = null; break;
                case (false, 4): eHandCard4 = null; break;
            }

            if (isPlayer && playerYamaList.Count == 0 ||
                isPlayer == false && enemyYamaList.Count == 0)
            {
                CheckGame();
            }
            else
            {
                StartCoroutine(GetHandCoroutine(isPlayer, index));
            }
        }
        else
        {
            // おてつきで元の位置に戻す
            var backPos = (isPlayer, index) switch
            {
                (true, 1) => posP1,
                (true, 2) => posP2,
                (true, 3) => posP3,
                (true, 4) => posP4,
                (false, 1) => posE1,
                (false, 2) => posE2,
                (false, 3) => posE3,
                _ => posE4,
            };
            playCard.SetPriority(BASE_PRIORITY + 2 * index);
            playCard.MoveTo(backPos.localPosition, MOVE_TIME_PLAY * 5);
            yield return new WaitWhile(() => playCard.IsMoving());
            playCard.SetPriority(BASE_PRIORITY);

            CheckGame();
        }
    }

    /// <summary>
    /// 勝利・敗北で終了処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator GameEndCoroutine()
    {
        var end = false;
        if (pHandCard1 == null &&
            pHandCard2 == null &&
            pHandCard3 == null &&
            pHandCard4 == null)
        {
            // 勝ち
            SetGameResult(true);
            if (IsBossRush())
            {
                Global.GetTemporaryData().bossRushManaWon = true;
            }
            yield return new WaitForSeconds(1f);

            message.SetText(StringMinigameMessage.ManaA_Win);
            message.SetAlpha(1f);
            end = true;
        }
        else if (eHandCard1 == null &&
            eHandCard2 == null &&
            eHandCard3 == null &&
            eHandCard4 == null)
        {
            // 負け
            SetGameResult(false);
            if (IsBossRush())
            {
                Global.GetTemporaryData().bossRushManaWon = false;
            }
            yield return new WaitForSeconds(1f);

            message.SetText(StringMinigameMessage.ManaA_Lose);
            message.SetAlpha(1f);
            end = true;
        }

        if (end)
        {
            yield return new WaitForSeconds(2f);

            if (IsBossRush())
            {
                ManagerSceneScript.GetInstance().NextGame("GameSceneMenderuB");
            }
            else
            {
                ManagerSceneScript.GetInstance().ExitGame();
            }
        }
    }

    #endregion

    #region メソッド

    /// <summary>
    /// 山札の一番上のカードを取る
    /// </summary>
    /// <param name="isPlayer"></param>
    /// <returns></returns>
    private MANAGameCard PickUpYamaCard(bool isPlayer)
    {
        MANAGameCard ret;
        if (isPlayer)
        {
            if (playerYamaList.Count == 0) { return null; }

            else if (playerYamaList.Count == 1)
            {
                ret = pYamaLastCard;
            }
            else
            {
                var newCard = GameObject.Instantiate(dummyCard, cardParent);
                newCard.SetActive(true);
                ret = newCard.GetComponent<MANAGameCard>();
            }
            ret.SetCard(playerYamaList.First().suit, playerYamaList.First().num);
            ret.ShowCard(false);
            ret.MoveTo(posYamaP.localPosition);

            playerYamaList.RemoveAt(0);
        }
        else
        {
            if (enemyYamaList.Count == 1)
            {
                ret = eYamaLastCard;
            }
            else
            {
                var newCard = GameObject.Instantiate(dummyCard, cardParent);
                newCard.SetActive(true);
                ret = newCard.GetComponent<MANAGameCard>();
            }
            ret.SetCard(enemyYamaList.First().suit, enemyYamaList.First().num);
            ret.ShowCard(false);
            ret.MoveTo(posYamaE.localPosition);

            enemyYamaList.RemoveAt(0);
        }

        ret.SetPriority(BASE_PRIORITY + 20);
        return ret;
    }

    /// <summary>
    /// カーソル位置更新
    /// </summary>
    private void UpdateCursor()
    {
        var x = playerCursor switch
        {
            1 => posP1.localPosition.x,
            2 => posP2.localPosition.x,
            3 => posP3.localPosition.x,
            _ => posP4.localPosition.x,
        };

        var pos = cursor.localPosition;
        pos.x = x;
        cursor.localPosition = pos;
    }

    /// <summary>
    /// カードの差を計算
    /// </summary>
    /// <param name="num1"></param>
    /// <param name="num2"></param>
    /// <returns></returns>
    private int CalcCardDiff(int num1, int num2)
    {
        var diff = Mathf.Abs(num1 - num2);
        if (diff >= 7) diff = 13 - diff;

        return diff;
    }

    /// <summary>
    /// カードを出せるかどうか判定
    /// 動いてるものは認識しない
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    private bool CanPlayCard(MANAGameCard c)
    {
        if (c == null || c.IsMoving()) return false;

        var f1Num = field1Card?.GetNum();
        var f2Num = field2Card?.GetNum();

        if (f1Num.HasValue)
        {
            var diff = CalcCardDiff(f1Num.Value, c.GetNum());
            if (diff == 1) return true;
        }
        if (f2Num.HasValue)
        {
            var diff = CalcCardDiff(f2Num.Value, c.GetNum());
            if (diff == 1) return true;
        }

        return false;
    }

    /// <summary>
    /// ゲーム状態判定
    /// </summary>
    /// <returns></returns>
    private bool CheckGame()
    {
        // 終了中ならもう無視
        if (state == MANAState.End) return false;

        // 手札が空なら終了
        if (pHandCard1 == null &&
            pHandCard2 == null &&
            pHandCard3 == null &&
            pHandCard4 == null ||
            eHandCard1 == null &&
            eHandCard2 == null &&
            eHandCard3 == null &&
            eHandCard4 == null)
        {
            state = MANAState.End;
            StartCoroutine(GameEndCoroutine());
            return false;
        }

        // 残ってる場合両方の手札をチェック
        MANAGameCard[] cards =
        {
            pHandCard1, pHandCard2, pHandCard3, pHandCard4,
            eHandCard1, eHandCard2, eHandCard3, eHandCard4,
        };

        foreach (var card in cards)
        {
            // 動いてたら何もしない
            if (card?.IsMoving() == true)
            {
                state = MANAState.Play;
                return true;
            }
        }

        // 出せるか判定
        foreach (var card in cards)
        {
            if (CanPlayCard(card))
            {
                state = MANAState.Play;
                return true;
            }
        }

        // 出せるのが無い時は山札から出す
        StartCoroutine(InitFieldCoroutine(1f));
        return false;
    }
    #endregion
}
