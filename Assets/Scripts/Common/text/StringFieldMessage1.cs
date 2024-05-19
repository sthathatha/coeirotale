using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// フィールドで使用するメッセージ
/// </summary>
public partial class StringFieldMessage
{
    #region デバッグ

    // 固定文言
    // あいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめもやゆよらりるれろわをん
    // アイウエオカキクケコサシスセソタチツテトナニヌネノハヒフヘホマミムメモヤユヨラリルレロワヲン
    // がぎぐげござじずぜぞだぢづでどばびぶべぼぱぴぷぺぽガギグゲゴザジズゼゾダヂヅデドバビブベボパピプペポ
    // ー～
    // ぁぃぅぇぉゃゅょっゎァィゥェォャュョッヮヵヶｧｨｩｪｫｬｭｮｯ
    // abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ
    // ａｂｃｄｅｆｇｈｉｊｋｌｍｎｏｐｑｒｓｔｕｖｗｘｙｚＡＢＣＤＥＦＧＨＩＪＫＬＭＮＯＰＱＲＳＴＵＶＷＸＹＺ
    // 0123456789０１２３４５６７８９
    // 「」"'”’

    public const string DebugMap_Menderu = "私と遊ぶ？";
    public const string DebugMap_Pierre = "僕と遊ぶかい？";
    public const string DebugMap_Mati = "私と遊ぶのか？";
    public const string DebugMap_Matuka = "あ”あ”あ”あ”あ”あ”あ”！！";
    public const string DebugMap_Mana = "おー？\nMANAさんと勝負するかー？";
    public const string DebugMap_Ami = "いらっしゃいませ！遊んでいきますか？";
    public const string DebugMap_Ami2 = "もっと難しい曲がいいですか？";

    #endregion

    #region チュートリアル

    public const string Tutorial000_01Reko = "あれ…ここは…";
    public const string Tutorial000_02Tukuyomi = "お目覚めのようですね";
    public const string Tutorial000_03Reko = "誰！？";
    public const string Tutorial000_04Tukuyomi = "ああ　落ち着いてください\n焦ることはありません";
    public const string Tutorial000_05Tukuyomi = "あなたにお話があります　いいですか？\nどうか　落ち着いて";
    public const string Tutorial000_06Tukuyomi = "あなたは何らかのふしぎな力にひっぱられてここに迷い込みました";
    public const string Tutorial000_07Tukuyomi = "ええ　ええ　わかってます\n声ですね？";
    public const string Tutorial000_08Tukuyomi = "あなたは、声を失ってしまったようですね";
    public const string Tutorial000_09Reko = "意味がわからない！\n私は今…";
    public const string Tutorial000_10Tukuyomi = "大丈夫　落ち着いてください";
    public const string Tutorial000_11Tukuyomi = "この世界にくるとなぜかそうなるんです\n失わずに済んでいるのはたった６人だけ…";
    public const string Tutorial000_12Reko = "どうしてこんなことに？";
    public const string Tutorial000_13Tukuyomi = "わかりません…いったい誰が\n何のためにこんな世界を作ったのか";
    public const string Tutorial000_14Tukuyomi = "ひとまず私がこの世界をご案内しますよ";
    public const string Tutorial000_15Reko = "あっ！あなたは何者なんですか？";
    public const string Tutorial000_16Tukuyomi = "申し遅れました、私はフリー素材キャラクターのつくよみちゃん\n今はここで、迷い込んだ方のフォローをしています";
    public const string Tutorial000_17Tukuyomi = "困ったことがあったらなんでも言ってくださいね";
    public const string Tutorial000_18Reko = "フリーソザイキャラクターってなんだ…？";

    public const string Tutorial001_01Tukuyomi = "これは壁です";
    public const string Tutorial001_02Reko = "見ればわかります";
    public const string Tutorial001_03Tukuyomi = "壁は通れません";
    public const string Tutorial001_04Reko = "だから…";
    public const string Tutorial001_05Tukuyomi = "操作はキーボードかゲームパッドですか？\nどれかのボタンを押せば怪しい場所を調べることができますよ";
    public const string Tutorial001_06Reko = "それは教えてくれないんだ";

    public const string Tutorial002_01Tukuyomi = "こちらはを押すたびに切り替えられるタイプですね\nただしひとつのスイッチでいくつかの壁が動きます";
    public const string Tutorial002_02Reko = "なんでそんな面倒なもの作ったんですか";
    public const string Tutorial002_03Tukuyomi = "ほら、階段の明かりって上下どちらでもオンオフできるじゃないですか";
    public const string Tutorial002_04Reko = "絶対関係無いと思う";

    public const string Tutorial003_01Tukuyomi = "おめでとうございます！\nあなたはもうこの世界の歩き方を充分理解できました！";
    public const string Tutorial003_02Reko = "壁動かしただけですけど";
    public const string Tutorial003_03Tukuyomi = "やっぱりだめでしょうか";
    public const string Tutorial003_04Reko = "行かなければいけない場所があった気がするんです\nどうすればいいんですか？";
    public const string Tutorial003_05Tukuyomi = "それが…わからないんです\nここはとても小さい世界ですが、出口はどこにもありません";
    public const string Tutorial003_06Reko = "そんな！出られないんですか？";
    public const string Tutorial003_07Tukuyomi = "１箇所だけ探していない場所…というか入れない場所があります\n一応見に行ってみますか？";
    public const string Tutorial003_08Reko = "もちろん！";
    public const string Tutorial003_09Tukuyomi = "かしこまりました\nどうぞこちらへ";

    #endregion

    #region フィールド000～0

    public const string F003_GetKey = "小さな鍵を手に入れた！";

    #endregion
}
