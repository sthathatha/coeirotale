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

    public const string Tutorial004_01Tukuyomi = "この上と下で、みなさん思い思いにすごされています";
    public const string Tutorial004_02Tukuyomi = "まっすぐ下に行くとカフェがあるので、基本的にはそちらが生活のベースになりますね";
    public const string Tutorial004_03Reko = "この光は？";
    public const string Tutorial004_04Tukuyomi = "それを触るとセーブできますが、今は使えません";
    public const string Tutorial004_05Tukuyomi = "チュートリアルの途中でセーブされるといろいろ面倒ですから";
    public const string Tutorial004_06Reko = "よくわからない";
    public const string Tutorial004_07Tukuyomi = "例の場所は右の奥です";

    public const string Tutorial005_01Tukuyomi = "この上は海になっていて、危険なので進めません";
    public const string Tutorial005_02Tukuyomi = "今は入れないように柵を作ってますから、そこまでなら行ってもかまいませんけどね";
    public const string Tutorial005_03Reko = "電子の海の渚…\nうっ　頭が";

    public const string Tutorial008_01Tukuyomi = "ごらんの通り、不思議な力で通れずこれ以上奥に入れないのです";
    public const string Tutorial008_02Reko = "あれは？";
    public const string Tutorial008_03Tukuyomi = "あの方はここに来てからずっと無理矢理通ろうと頑張っているんですが、だめみたいですね";
    public const string Tutorial008_04Reko = "そ、そうですか";
    public const string Tutorial008_05Tukuyomi = "噂では、７つの声を持つ者だけがこの先に進めるらしいんです";
    public const string Tutorial008_06Reko = "どうしてそんな噂が？";
    public const string Tutorial008_07Tukuyomi = "そこの立て札に書いてあります";
    public const string Tutorial008_08Reko = "ああ、そう…";
    public const string Tutorial008_09Reko = "７つの声…６人は声が残ってるって言ってましたよね？あと１人居ればもしかしたら…";
    public const string Tutorial008_10Tukuyomi = "会いに行きますか？みなさん個性的な方ですぐには手を貸してくれないかもしれませんが、協力しますよ！";
    public const string Tutorial008_11Reko = "そうしましょう、よろしくお願いします";
    public const string Tutorial008_12Tukuyomi = "必要とあれば私の体をお使いください！";

    #endregion

    #region フィールド000～010

    public const string F003_GetKey = "小さな鍵を手に入れた！";
    public const string F008_Board = "七つの声を持つ者に\n　道は開かれる";
    public const string F009_Board1 = "\n←大騒ぎは住民に配慮して離れ小島へ";
    public const string F009_Board2 = "\n↑草原\n　静かな場所で精神統一";
    public const string F009_Board3 = "\n→クラウンサーカス";
    public const string F010_Board1 = "\n←MANAさんのおうち！";
    public const string F010_Board2 = "\n↓カフェ 小春日和";
    public const string F010_Board3 = "\n→十字花教会　COEIRO支部";

    #endregion
}
