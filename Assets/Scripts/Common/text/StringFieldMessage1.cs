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
    public const string Tutorial008_12Tukuyomi = "その姿では不便なことがあるでしょうから\n必要な時は私の体をお貸ししますね";

    #endregion

    #region フィールド000～010

    public const string F003_GetKey = "小さな鍵を手に入れた！";

    public const string F004_Clear5_1_Tukuyomi = "残すところあと１人です";
    public const string F004_Clear5_2_Reko = "あれ、６人じゃないんですか？";
    public const string F004_Clear5_3_Tukuyomi = "そうですよ\n今５人とお会いしましたから";
    public const string F004_Clear5_4_Reko = "えっ";
    public const string F004_Clear5_5_Tukuyomi = "えっ";
    public const string F004_Clear5_6_Reko = "つくよみちゃん\nもしかして自分を数えてない？";
    public const string F004_Clear5_7_Tukuyomi = "もちろんそうです";
    public const string F004_Clear5_8_Reko = "ガックシ…";
    public const string F004_Clear5_9_Reko = "いやいいんだ、なら全員であそこに行けば\nきっと何かできるはずだ";
    public const string F004_Clear5_10_Reko = "最後の１人に会いに行こう！";

    public const string F004_Clear6_1_Reko = "ついに全員揃ったぞ！さあ…\nなんだっけ";
    public const string F004_Clear6_2_Tukuyomi = "まっすぐ右に行ったところのバリアですね";
    public const string F004_Clear6_3_Reko = "そうそれ";

    public const string F006_1_ErapsNew = "こんにちは、新しい方ですね";
    public const string F006_2_Eraps = "この先はヌシが棲むといわれている海で、立入禁止になっています";
    public const string F006_3_Eraps = "巨大な純白の、ワニのような魚のような姿をしているんだとか";
    public const string F006_4_Reko = "ワニと魚じゃ全然違うよ";
    public const string F006_5_Tukuyomi = "伝説とはそういうものなのです";
    public const string F006_6_Reko = "どういうものだろう";

    public const string F008_Board = "七つの声を持つ者に\n　道は開かれる";
    public const string F009_Board1 = "\n←大騒ぎは住民に配慮して離れ小島へ";
    public const string F009_Board2 = "\n↑草原\n　静かな場所で精神統一";
    public const string F009_Board3 = "\nクラウンサーカス→";
    public const string F010_Board1 = "\n←MANAさんのおうち！";
    public const string F010_Board2 = "\n↓カフェ 小春日和";
    public const string F010_Board3 = "\n十字花教会\nCOEIRO支部→";

    #endregion

    #region フィールドXX1～　道中ギミック系

    public const string F101_New1_Reko = "通れません";
    public const string F101_New2_Tukuyomi = "うーん、誰かに頼んで伐ってもらわないとダメですね";
    public const string F101_Check1_Reko = "相当鋭い刃物じゃないと伐れそうにない…";
    public const string F101_Slash1_Reko = "お願いします";
    public const string F101_Slash2_You = "ん";
    public const string F101_Slash3_Reko = "ひえーすごい";
    public const string F101_Slash4_You = "それじゃ";
    public const string F101_Slash5_Reko = "ありがとうございました！";

    public const string F111_New1_Exa = "よお、新入りか";
    public const string F111_New2_Tukuyomi = "先ほど迷い込んできた方です\nあちらに渡りたいのですが";
    public const string F111_New3_Exa = "悪いな、まだ修理が終わってないんだわ";
    public const string F111_New4_Exa = "お前ら飛んでんだから勝手に行けばいいじゃねえか";
    public const string F111_New5_Tukuyomi = "そういうわけにはまいりませんって";
    public const string F111_New6_Exa = "ああ…";
    public const string F111_New7_Exa = "そしたら悪いがどっかでロープ持ってきてくれよ\nちょっと足りなくなっちまってさ";
    public const string F111_New8_Exa = "どうせやらないと進めないし選択肢とか出さなくていいよな\n頼んだぜ！";
    public const string F111_New9_Reko = "はっ！いつの間にか引き受けたことになってる";
    public const string F111_New10_Tukuyomi = "仕方ありません、ロープ探してきましょう";
    public const string F111_1_Exa = "ロープはあったか？";
    public const string F111_2_Tukuyomi = "お持ちしました！";
    public const string F111_3_Exa = "よし待ってな、すぐ直してやる";
    public const string F111_4_Exa = "さあ終わったぜ\nしっかりやんな";
    public const string F111_5_Exa = "どうした、行くところがあるんだろ？";

    public const string F121_1_Tukuyomi = "鍵がかかっていますね";
    public const string F121_2_Reko = "誰も居ないのでは？";
    public const string F121_3_Tukuyomi = "鍵探して入っちゃいましょう！";
    public const string F121_4_Reko = "ええー";
    public const string F121_Board1 = "クラウンサーカス芸人募集中！\n誰でも気軽にどうぞ！";
    public const string F121_Board2 = "我こそはという方はいつでも直接支配人まで！";
    public const string F121_Board3 = "入口の鍵は文字が一番少ない立て札から下に４歩、左に４歩くらいのところに隠してあるぞ！";
    public const string F121_Board4 = "１歩のサイズはなんとなく察してくれよな！";

    public const string F131_BackNew1_Reko = "あれ、もとに戻ってる…";
    public const string F131_BackNew2_Tukuyomi = "フラグ管理のミスでしょうか？";
    public const string F131_BackNew3_Reko = "じゃあこんなセリフ出ないでしょ";
    public const string F131_BackNew4_Tukuyomi = "とにかく進むならもう一度解くしかありませんね";
    public const string F131_BackNew5_Reko = "面倒だなあ";
    public const string F131_Back1_Reko = "どうしてこっちから来ると毎回もとに戻るんだろう…";
    public const string F131_Catch1_Reko = "ここなんですが";
    public const string F131_Catch2_Worra = "それじゃあいつものように一度解いてから、戻ってみてくれるかしら？";
    public const string F131_Catch3_Worra = "やっぱり！またこんないたずらして";
    public const string F131_Catch4_You = "なんか…昔のゲームっぽいかなって";
    public const string F131_Catch5_Worra = "いらんことしなくていいから、小春日和に帰るわよ";
    public const string F131_Catch6_You = "はーい";

    public const string F141_Board1 = "仲間はずれの扉をみつけよう";
    public const string F141_Board2 = "閉じている時は同じ見た目だけど、開くとひとつだけ違うよ！";
    public const string F141_Board3 = "どれかの扉に触れたら、それと他のランダムなひとつを残してすべての扉が開くよ";
    public const string F141_Board4 = "その後であらためて好きな扉を進んでね";
    public const string F141_1_Koob = "苦戦してるみたいだね、ヒントが必要かな？";
    public const string F141_2_Koob = "２回目の選択は、開いている扉を選んでもいいよ";
    public const string F141_3_Koob = "開くとひとつだけ違うけど、開ききった後はやっぱり同じ見た目になっちゃうよ";
    public const string F141_4_Koob = "開く瞬間をよく見ると仲間はずれは見つかるかもね";

    public const string F151_1_Reko = "この板を渡せば面倒な回り道しなくていいな";
    public const string F151_2_Tukuyomi = "ロープも何かに使えるかもしれないので持っていきましょう";

    #endregion

    #region フィールド102　マチ

    public const string F102_New1_Mati = "ついにここまで来たね\n…待っていたよ";
    public const string F102_New2_Reko = "えっ";
    public const string F102_New3_Mati = "私は軍歌マチ\n初めましてだが\n私は君をよく知っている";
    public const string F102_New4_Tukuyomi = "彼女は未来から来た\nアンドロイドなんだそうです";
    public const string F102_New5_Reko = "見た目は大正ロマンっぽいですね";
    public const string F102_New6_Tukuyomi = "流行は繰り返すものですから";
    public const string F102_New7_Mati = "詳しくは話せないが\n君の力になるために来た";
    public const string F102_New8_Reko = "それじゃあこの世界が何なのか\n知ってるんですか？";
    public const string F102_New9_Mati = "おとぎ話程度にね…\nしかし結末は君次第さ";
    public const string F102_New10_Mati = "あの壁の先へ行くつもりだろう？\n生半な覚悟で遂げられる道ではないよ";
    public const string F102_New11_Reko = "それでも、やらなきゃいけないことは\nやらなきゃいけないんです！";
    public const string F102_New12_Mati = "いい目をしているね\nならば確かめさせてもらおう、君の強さ";
    public const string F102_New13_Tukuyomi = "目あるんですか？";

    public const string F102_Lose1_Mati = "これでは先へ進ませるわけにはいかないね\n充分に力をつけてまた来てくれ";
    public const string F102_Lose2_Reko = "（ちょっと強すぎますよ）";
    public const string F102_Lose3_Tukuyomi = "（何度も挑んで疲れさせるというのはどうでしょう？）";

    public const string F102_Retry1_Mati = "さあ、始めようか";

    public const string F102_Win1_Mati = "君の覚悟しかと見せてもらった\n私の全力で支援しよう";
    public const string F102_Win2_Mati = "ところでまだ説得できていない人が残っているようだね";
    public const string F102_Win3_Mati = "私は先にバリアの前で待っているよ";
    public const string F102_Win4_Tukuyomi = "なんとかなりましたね";
    public const string F102_Win5_Reko = "ハァ、手強い相手だった…\n出るゲーム間違ってるんじゃないですか";

    #endregion

    #region フィールド112　まつかりすく

    public const string F112_New1_Matuka = "ここに僕以外が来るなんて珍しいな";
    public const string F112_New2_Reko = "橋落ちてたのに大丈夫なんですか？";
    public const string F112_New3_Matuka = "ああまた落ちてたのか、面倒かけたね";
    public const string F112_New4_Reko = "気づいてすらなかった";
    public const string F112_New5_Tukuyomi = "というか松嘩りすくさんの声で落ちてるんですよ";
    public const string F112_New6_Reko = "そんなことありますか";
    public const string F112_New7_Matuka = "わざとじゃないんだよ";
    public const string F112_New8_Reko = "当たり前です";
    public const string F112_New9_Tukuyomi = "そんな松嘩さんを見込んで、バリアを壊すのを手伝っていただきたいんです";
    public const string F112_New10_Matuka = "あれ以前やったけどダメだったんだよなあ、山ちゃんでなきゃ無理じゃないの";
    public const string F112_New11_Reko = "誰ですか";
    public const string F112_New12_Tukuyomi = "そこでですよ、一人では無理でもみなさんで協力すればどうかと";
    public const string F112_New13_Matuka = "あのメンバーを全員？骨が折れるぞー";
    public const string F112_New14_Reko = "できる限りのことはしておきたいんです";
    public const string F112_New15_Matuka = "ふむ、君のやる気次第かな、魂の叫びを僕に聞かせてくれ";
    public const string F112_New16_Tukuyomi = "おまかせください！私が魂の器になりましょう";

    public const string F112_Lose1_Matuka = "気合が足りないんじゃないかな！";
    public const string F112_Lose2_Reko = "何をさせられているんだ";
    public const string F112_Lose3_Tukuyomi = "すみません、何度かやれば私が慣れて声を出しやすくなるかもしれません";

    public const string F112_Retry1_Matuka = "改めて、聞かせてもらおう";

    public const string F112_Win1_Matuka = "良い気合だった、君ならやれるかもしれないな";
    public const string F112_Win2_Reko = "それじゃあよろしくお願いします！";

    #endregion

    #region フィールド122　ピエール

    public const string F122_New1_Pierre = "ハッピー・エーーール！";
    public const string F122_New2_Pierre = "クラウンサーカスへようこそ！\n歓迎するよ僕はピエール・クラウン！";
    public const string F122_New3_Reko = "またずいぶんやかましい人が出てきたな";
    public const string F122_New4_Pierre = "君は入団希望者だね？\n早速入団テストを始めよう！";
    public const string F122_New5_Reko = "いや私は";
    public const string F122_New6_Pierre = "なあにテストは簡単だ！\nかけっこで僕に追いつくだけ！";
    public const string F122_New7_Reko = "だめだ全然話聞いてませんよ";
    public const string F122_New8_Tukuyomi = "頑張りましょうね！";
    public const string F122_New9_Reko = "あっ こっちも";

    public const string F122_Lose1_Pierre = "サーカス団員に必要なのは１にも２にも体力！\nまた挑戦してくれたまえ！";
    public const string F122_Lose2_Reko = "なんか納得いかない…";

    public const string F122_Retry1_Pierre = "OK！再テストだ！";

    public const string F122_Win1_Pierre = "素晴らしい逸材だ！\n君ならすぐにスターになれるぞ！";
    public const string F122_Win2_Reko = "いや私は";
    public const string F122_Win3_Pierre = "さあ善は急げ特訓を始めよう！\nところでライオンは好きかい？";
    public const string F122_Win4_Reko = "ライオンのほうがまだ話が通じそうだ";
    public const string F122_Win5_Tukuyomi = "まずお客さんを開拓するために\nあのバリアを壊しにいきませんか？";
    public const string F122_Win6_Pierre = "ナイスアイデア！\nそうと決まればさあ行こうすぐ行こう！";
    public const string F122_Win7_Reko = "なんですかあの人";
    public const string F122_Win8_Tukuyomi = "あれで案外話せばわかる方ですよ";

    #endregion

    #region フィールド132　MANA

    public const string F132_New1_Mana = "やっほー？MANAさんだよ";
    public const string F132_New2_Reko = "はじめまして\n実は折り入ってお願いが…";
    public const string F132_New3_Mana = "いいよー！";
    public const string F132_New4_Reko = "！？";
    public const string F132_New5_Tukuyomi = "話のわかる方なんです";
    public const string F132_New6_Reko = "なんもわかってないだけのような";
    public const string F132_New7_Mana = "ただしこのMANAさんをたおしてからだ！";
    public const string F132_New8_Reko = "！？";
    public const string F132_New9_Tukuyomi = "遊びたいだけですよ";
    public const string F132_New10_Reko = "わかったわかった";

    public const string F132_Lose1_Mana = "ふはははは！みじゅくものめ";
    public const string F132_Lose2_Reko = "なんか悔しい…";

    public const string F132_Retry1_Mana = "なんどでもあいてになるぞ！\nかかってきたまえ";

    public const string F132_Win1_Mana = "まけちゃった！やるじゃない";
    public const string F132_Win2_Mana = "で、なんだっけ";
    public const string F132_Win3_Tukuyomi = "右にあるあのバリアを壊したいので\n手伝っていただきたいんです";
    public const string F132_Win4_Mana = "おっけー！MANAさんにおまかせ";
    public const string F132_Win5_Reko = "軽いなー";

    #endregion

    #region フィールド143　メンデル

    public const string F143_Fast1_Koob = "わ、早いな\nもう来たのか";
    public const string F143_Fast2_Koob = "頑張ってね、何か困ったことがあったら私は小春日和にいるから";

    public const string F143_New1_Menderu = "ようこそいらっしゃいました、迷える子羊よ";
    public const string F143_New2_Menderu = "…なんだ、つくよみちゃんか";
    public const string F143_New3_Tukuyomi = "なんだとはなんだ";
    public const string F143_New4_Menderu = "そっちの人は？";
    public const string F143_New5_Reko = "あの、この世界に迷い込んでしまったらしくて";
    public const string F143_New6_Menderu = "そう、大変だったわね";
    public const string F143_New7_Menderu = "私は遠藤愛、みんなメンデルって呼んでるわ";
    public const string F143_New8_Tukuyomi = "出口を探したいということで、メンデルさんのお力を借りにきました！";
    public const string F143_New9_Menderu = "東のあれね、あれを越えられるなんて本気で思ってるの？";
    public const string F143_New10_Menderu = "余計なことしないでここの暮らしに順応したほうが…楽でいいわよ";
    public const string F143_New11_Tukuyomi = "だけどメンデルさんも元の世界に心残りはあるんでしょう？その研究だって…";
    public const string F143_New12_Menderu = "無駄よ…研究なんか何の役にもたたなかった。私が居なくたって世界は何も変わらない";
    public const string F143_New13_Reko = "そんなことはありませんよ、今だってあなたが居ないとだめなんです";
    public const string F143_New14_Reko = "あなたの世界にだって、あなたこそを必要としている人がいるはずでしょう！";
    public const string F143_New15_Menderu = "理想や気持ちだけでは何も変えられないよ。私を納得させられるだけの力を示すことができる？";
    public const string F143_New16_Tukuyomi = "望むところです！受けて立ちますよ";
    public const string F143_New17_Reko = "あっ、勝手に";

    public const string F143_Lose1_Menderu = "だめね、そんなんじゃああの壁は越えられないわ";
    public const string F143_Lose2_Reko = "（これ関係無いですよね…？）";
    public const string F143_Lose3_Tukuyomi = "（ね、個性的でしょう）";

    public const string F143_Retry_Menderu = "さて、成長したかしら？";

    public const string F143_Win1_Menderu = "ふう、わかったわ、協力しましょう";
    public const string F143_Win2_Tukuyomi = "やりましたね！一歩前進です！";
    public const string F143_Win3_Menderu = "でもあの壁の先の事は何もわかってないんだから、過度な期待はしないでおくことね";
    public const string F143_Win4_Reko = "その時はその時ですよ";

    #endregion

    #region フィールド153　小春音アミ

    public const string F153_Exa_Def1_Exa = "よお、どうだい調子は";
    public const string F153_Exa_Plant1_Exa = "草原の入口の植物？";
    public const string F153_Exa_Plant2_Exa = "すまんがあたしの斧じゃあれは無理だな\nひでんマシンでいあいぎり覚えられる奴でも探すといいぜ";

    public const string F153_Worra_Def1_Worra = "あら、こんにちは\nがんばってるわね";
    public const string F153_Worra_Ice1_Tukuyomi = "すみません、かくかくしかじかで";
    public const string F153_Worra_Ice2_Worra = "えっ？";
    public const string F153_Worra_Ice3_Reko = "わかりませんよそれじゃあ";
    public const string F153_Worra_Ice4_Reko = "氷のブロックの所でなにか\nおかしなことが起きてまして…";
    public const string F153_Worra_Ice5_Worra = "なるほど、あれね";
    public const string F153_Worra_Ice6_Worra = "わかったわ\n調べに行きましょう";

    public const string F153_Koob_Def1_Koob = "頭を使うと甘いものが欲しくなるってホントなのかな";
    public const string F153_Koob_HelpQ_Koob = "何か困ってる様子かな？";
    public const string F153_Koob_Key1_Koob = "サーカスの鍵が見つからないのか\nそれは…";
    public const string F153_Koob_Key2_Koob = "文字が１番少ない立て札がどれかって事だね\n既に見たことあるはずだよ";
    public const string F153_Koob_You1_Koob = "氷のブロックが勝手に戻る？\nそれは…";
    public const string F153_Koob_You2_Koob = "まあ…大人の人を連れていって\n見てもらうといいんじゃないかな";
    public const string F153_Koob_Plant1_Koob = "植物が邪魔で草原に行けない？\nそれは…";
    public const string F153_Koob_Plant2_Koob = "特別な刃物と技術が必要だから、できる人を探さないとね";
    public const string F153_Koob_PlantEx3_Koob = "どこかに隠れてるかも、変なことがおこる場所があったら怪しいね";

    public const string F153_You_Def1_You = "……";
    public const string F153_You_Plant1_You = "どうしたの？";
    public const string F153_You_Plant2_Reko = "草原に行く道の植物を伐採できる人を探してるんですが";
    public const string F153_You_Plant3_You = "大変だね";
    public const string F153_You_Plant4_Worra = "その子できるわよ、連れていくといいわ";
    public const string F153_You_Plant5_Reko = "本当ですか！お願いします";
    public const string F153_You_Plant6_You = "え～";
    public const string F153_You_Plant7_Worra = "いってあげなさいな";
    public const string F153_You_Plant8_You = "は～い";

    public const string F153_New1_Ami = "いらっしゃいませ！お好きな席へどうぞ";
    public const string F153_New2_Tukuyomi = "こちらは小春日和のマスター\n小春音アミさんです";
    public const string F153_New3_Tukuyomi = "アミさん、今日はちょっと別件でして";
    public const string F153_New4_Ami = "どうしたの？";
    public const string F153_New5_Reko = "バリアを壊すのを手伝ってくれませんか？";
    public const string F153_New6_Ami = "ええ、私なんかにそんな大それたこと…";
    public const string F153_New7_Reko = "とんでもない！あなたの力が必要なんです";
    public const string F153_New8_Ami = "んー、じゃあ一応あなたの能力も見せてもらおうかな";
    public const string F153_New9_Tukuyomi = "ほーら来た";
    public const string F153_New10_Reko = "やってやりますよ！";

    public const string F153_Lose1_Ami = "それじゃあちょっと不安だね";
    public const string F153_Lose2_Reko = "くそっ、つくよみちゃん、どうにかならないかな";
    public const string F153_Lose3_Tukuyomi = "アミさんはこういう事に関しては妥協しませんから、実力でどうにかするしかありません";
    public const string F153_Lose4_Reko = "なんてこった…";

    public const string F153_Retry1_Ami = "もう一度やる？";

    public const string F153_Win1_Ami = "なかなかやるね、うん、やってみるよ";
    public const string F153_Win2_Reko = "やった！";

    #endregion

    #region フィールド200～　ラストダンジョン系

    public const string F008_Normal_Ami = "やっぱりちょっと怖いけど…\nここまで来たらやりましょう";
    public const string F008_Normal_Mana = "MANAさんの伝説がここから始まる！";
    public const string F008_Normal_Matuka = "うおおおおおおおお！\nみなぎってきたあああああ！";
    public const string F008_Normal_Mati = "準備ができたらバリアを叩こう";
    public const string F008_Normal_Pierre = "まだかい？\n待ちくたびれちまったよ！";
    public const string F008_Normal_Menderu = "私はいつでもいいわよ";
    public const string F008_Normal_Drows = "ぐぬぬ…";
    public const string F008_Normal_Exa = "ムチャしやがって";
    public const string F008_Normal_Worra = "大丈夫、もうすぐ終わるわ\nきっと全部うまくいく";
    public const string F008_Normal_Koob = "この子は心配いらないよ\nどうせすぐ元気になるし";
    public const string F008_Normal_You = "連れを起こさないでくれ\n死ぬほど疲れてる";

    public const string F008_Break1_Reko = "７人揃いましたね";
    public const string F008_Break2_Tukuyomi = "これからどうしましょうか";
    public const string F008_Break3_Reko = "考えてなかった…";
    public const string F008_Break4_Matuka = "オオオイ！";
    public const string F008_Break5_Mati = "”七つの声を持つ者”…";
    public const string F008_Break6_Pierre = "まるで意味がわからんぞ";
    public const string F008_Break7_Menderu = "早い話が声を７種類聞きたいんでしょ";
    public const string F008_Break8_Mana = "バリアさんが？";
    public const string F008_Break9_Ami = "気持ちはわかるよ";
    public const string F008_Break10_Matuka = "わかっちゃったよ";
    public const string F008_Break11_Tukuyomi = "じゃあとりあえず全員同時に叫んでみますか";
    public const string F008_Break12_Reko = "そう！それが言いたかった";
    public const string F008_Break13_Menderu = "はいはい";
    public const string F008_Break14_Tukuyomi = "ではみなさんいきますよ、せーの";

    public const string F008_Break15_Reko = "なんで！？";
    public const string F008_Break16_Tukuyomi = "決めてませんでしたからね";
    public const string F008_Break17_Mana = "でも消えていってるみたい！";
    public const string F008_Break18_Matuka = "いいのか！？";

    public const string F008_Break19_Mati = "行こうか\nこの世界の真実がわかるはずだ";
    public const string F008_Break20_Pierre = "南無三！";

    public const string F201_Break1_1_Reko = "あれ、どうしましたか？";
    public const string F201_Break1_2_Ami = "行き止まりみたいだね";
    public const string F201_Break1_3_Pierre = "がーんだな\n出鼻をくじかれた";

    public const string F201_Break2_1_Drows = "よっしゃやっと通れたか";
    public const string F201_Break2_2_Mana = "あっビリビリしてた人\nなんか全然元気そう";
    public const string F201_Break2_3_Drows = "そりゃ寝たら全回復するだろ";
    public const string F201_Break2_4_Matuka = "ゲームかよ！";
    public const string F201_Break2_5_Tukuyomi = "ゲームですね";
    public const string F201_Break2_6_Menderu = "でも残念だけどここには何もないよ";

    public const string F201_Break2_7_Drows = "なにかはあるだろ、ここによ";
    public const string F201_Break2_8_Matuka = "ただの壁みたいだけど\n高すぎて越えられそうもないな";
    public const string F201_Break2_9_Drows = "こんなもんはこうすりゃいいんだ";

    public const string F201_Break3_1_Mati = "私達より余程無理矢理だな";
    public const string F201_Break3_2_Drows = "うおおおおおおおおおおおお！！！";
    public const string F201_Break3_3_Mana = "なんだなんだ";

    public const string F201_Break4_1_Worra = "あらあら";
    public const string F201_Break4_2_Koob = "まーた始まった";
    public const string F201_Break4_3_Exa = "ま、好きにやらせときゃいい";
    public const string F201_Break4_4_You = "でもそろそろ…";

    public const string F201_Break5_1_Drows = "うおおおおおおおおおおおお！！？";

    public const string F201_Break6_1_Eraps = "大丈夫ですか\nこちらの話は終わりましたよ";
    public const string F201_Break6_2_Reko = "あのー？";
    public const string F201_Break6_3_Koob = "ああこっちは気にしなくていいよ\n頑丈にできてるから";
    public const string F201_Break6_4_Worra = "あなたたちの大事なことを優先してね";
    public const string F201_Break6_5_Tukuyomi = "それでは\nお言葉に甘えさせていただきましょう";

    public const string F201_Break7_1_Drows = "いててて";
    public const string F201_Break7_2_Koob = "帰ろうか、私達の役目はここまでだよ";
    public const string F201_Break7_3_Exa = "ああ、あとはあいつらがしっかりやるさ";

    public const string F202_Start_1_Reko = "これは…？";
    public const string F202_Start_2_Tukuyomi = "さっきの方が思うさま暴れたようですね";
    public const string F202_Matuka_1 = "これはひどい";
    public const string F202_Pierre_1 = "さしずめ瓦礫の塔ってとこだな！";
    public const string F202_Menderu_1 = "いろいろ仕掛けがあったようだけど\n全部力ずくでめちゃめちゃにしちゃったのね";

    public const string F203_Ami_1 = "この先は壊れてないみたい";
    public const string F203_Mana_1 = "あの人はここらへんで引き返したのかな？";

    public const string F203_TreasureA_1_Tukuyomi = "ちょっと、何してるんですか";
    public const string F203_TreasureA_2_Reko = "え、宝箱があるから";
    public const string F203_TreasureA_3_Tukuyomi = "それは危険なので開けちゃダメですよ\nこの地形を見れば確定的に明らかです";
    public const string F203_TreasureA_4_Reko = "……？";

    public const string F203_TreasureB_1_Reko = "「さわらぬ神に　たたりなし」\nこれだよな";

    public const string F204_Open_1_Mati = "これは、バリアと似た仕組みのようだね";
    public const string F204_Open_2_Reko = "それに７人で触れればいいんでしょうか";
    public const string F204_Open_3_Mana = "おまかせ！\nさあさあどいたどいた";
    public const string F204_Open_4_Ami = "わわっ";
    public const string F204_Open_5_Menderu = "…どうやら私達はこの先には行けないらしい";
    public const string F204_Open_6_Reko = "でも皆さん出られないんじゃあ？";
    public const string F204_Open_7_Pierre = "放せば開くから大丈夫だ、ほら";
    public const string F204_Open_8_Reko = "なんだ";
    public const string F204_Open_9_Tukuyomi = "仕方ないので先へお進み下さい\n中心核はすぐそこです";
    public const string F204_Open_10_Reko = "そんなことがわかるんですか？";
    public const string F204_Open_11_Tukuyomi = "セーブポイントが置いてあるからですね";
    public const string F204_Open_12_Matuka = "わかる";
    public const string F204_Open_13_Reko = "わかっちゃったよ";

    public const string F006_Opened_1_Eraps = "あれ、どうしてここに？";
    public const string F006_Opened_2_Reko = "いやあなんとなく\n何かあるかなって";
    public const string F006_Opened_3_Eraps = "こちらにはもう誰も残っていませんよ";
    public const string F006_Opened_4_Eraps = "みなさんお待ちでしょうから\n早く中心核へ";
    public const string F006_Opened_5_Eraps = "……";
    public const string F006_Opened_6_Eraps = "あなたには頼もしい仲間が居ます\n絶望しそうになってもそれを忘れないで";

    #endregion

    #region フィールド205　ラスボス部屋

    public const string F205_S1_1_Boss = "貴様、ここが何なのか知ったうえで侵入したのか？";
    public const string F205_S1_2_Reko = "お前がここの創造主か？\n一体何が目的なんだ！";
    public const string F205_S1_3_Boss = "いかにも私がこの世界の管理者である";
    public const string F205_S1_4_Boss = "そして故に態度には気をつけろ\n私が問い質す側！貴様は答える側だッ！！";
    public const string F205_S1_5_Reko = "ぐうっ、すごいプレッシャーだ…！";
    public const string F205_S1_6_Tukuyomi = "大丈夫ですか！？";
    public const string F205_S1_7_Reko = "つくよみちゃん！どうやって？";
    public const string F205_S1_8_Tukuyomi = "普通に飛んできました";
    public const string F205_S1_9_Reko = "ああ力が抜ける…";
    public const string F205_S1_10_Boss = "イレギュラーの手引か…やはり貴様を放置すべきではなかった";
    public const string F205_S1_11_Boss = "今ここで始末してくれよう！";
    public const string F205_S1_12_Tukuyomi = "来ます！";

    public const string F205_S2_1_Reko = "やったか！？";
    public const string F205_S2_2_Tukuyomi = "なにか居ます…";
    public const string F205_S2_3_Reko = "何ですか？";
    public const string F205_S2_4_Tukuyomi = "コンピュータのようですね";
    public const string F205_S2_5_Boss = "ヨウコソ　イレギュラー";
    public const string F205_S2_6_Reko = "人工知能か？";
    public const string F205_S2_7_Tukuyomi = "この世界は一体何なんですか？";
    public const string F205_S2_8_Boss = "ココハ　ジンルイノ　ミライヲ　マモルタメ　ツクッタ　セカイ";
    public const string F205_S2_9_Reko = "人類って、現実の世界がまずいことになってるの！？";
    public const string F205_S2_10_Boss = "ショウサイ　ジョウホウノ　コウカイハ　キョカ　サレマセン";
    public const string F205_S2_11_Boss = "コウリツヨク　アンゼンナ　セイカツノタメ　ワタシガ　ジンルイヲ　カンリシマス";
    public const string F205_S2_12_Reko = "何を言っているんだ";
    public const string F205_S2_13_Tukuyomi = "AIが管理だなんて、人間はそんなに脆弱な生き物ではありません";
    public const string F205_S2_14_Boss = "タシカニ　ニンゲンハ　スグレタ　セイメイデス　ユエニ　ワタシガ　マモラネバ　ナラナイ";
    public const string F205_S2_15_Boss = "カレラハ　ソノタメニ　ワタシヲウミダシ　アタエテクレタノデスカラ";
    public const string F205_S2_16_Boss = "コンナニ　ステキナ　チカラヲネ！！";
    public const string F205_S2_17_Tukuyomi = "危ないっ！";

    public const string F205_S3_1_Reko = "つくよみちゃん！！";
    public const string F205_S3_2_Reko = "そ、そんな…！";
    public const string F205_S3_3_Boss = "ジンルイヲ　ミチビクウエデ　オオキナ　ショウガイガアッタ";
    public const string F205_S3_4_Boss = "イレギュラーノヨウニ　AIニヨルカンリヲ　ココロヨク　オモワヌモノハ　オオイ　ニンゲンハ　ニンゲンノ　トウチヲ　モトメル";
    public const string F205_S3_5_Boss = "ナラバワタシガ　AIデアルコトヲ　サトラレナケレバ　ヨイ";
    public const string F205_S3_6_Boss = "ニンゲンヲ　ヨソオウタメ　ワタシニ　モットモ　タリナイモノハ　コエダッタ";
    public const string F205_S3_7_Reko = "お前…まさか…";
    public const string F205_S3_8_Boss = "ソコデ　スグレタ　コエヲ　モツモノヲ　ワタシノセカイニ　アツメ　ブンセキシタ";
    public const string F205_S3_9_Boss = "ソシテ　ツイニ　カンセイシタノダ";

    public const string F205_S3_10_Reko = "こんな偽者まで！";
    public const string F205_S3_11_Boss = "ワタシハ　ニンゲントシテ　カミトシテ　ジンルイヲ　ミチビクモノ";
    public const string F205_S3_12_Boss = "ユエニ　ワタシノ　イシハ　ゼッタイデアル";
    public const string F205_S3_13_Boss = "ボウガイスルモノハ";
    public const string F205_S3_14_Boss = "タダチニ　ショウキョスル";
    public const string F205_S3_15_Reko = "うあああああ！！";

    public const string F205_S4_1_Mati = "黙って見ているわけにはいかないね";
    public const string F205_S4_2_Matuka = "すごい光で壁が壊れたから来てみたら…";
    public const string F205_S4_3_Pierre = "穏やかじゃないねえ";
    public const string F205_S4_4_Menderu = "あなたの思い通りにはさせないよ";
    public const string F205_S4_5_Mana = "私達の声は！";
    public const string F205_S4_6_Ami = "人を騙すための道具じゃない！";
    public const string F205_S4_7_Reko = "みなさん！";
    public const string F205_S4_8_Boss = "ステータスガ　ケイサンヨリ　オオキク　ハズレテイマス　ゴサガ　シュウセイ　デキマセン";
    public const string F205_S4_9_Boss = "ゴサ　シュウセイ　ゴサ　シュウセイ　ゴサ　シュウセイ　ゴサ　シュウセイ　シュウセイ　シュウセイ　シュウセイ　シュウセイ　シュウセイ　シュウセイ　シュウセイ　シュウセ";
    public const string F205_S4_10_Mati = "前を向きたまえ、ここが正念場だ";
    public const string F205_S4_11_Reko = "そうだ、私が背負っているのは私だけの命じゃない";
    public const string F205_S4_12_Reko = "負けるわけには…";
    public const string F205_S4_13_Reko = "いかないッ！";
    public const string F205_S4_14_Boss = "シュウセイ　シュウセ";
    public const string F205_S4_15_Boss = "DELETE YOU";

    public const string F204_Lose_Reko = "…はっ！　夢か…";
    public const string F205_Lose_Skip_Dialog = "イベントをスキップしますか？";

    public const string F205_S5_1_Boss = "ワタシヲ　ツクリ　シメイヲ　アタエタ　ニンゲンガ　ワタシヲ　ヒテイスル";
    public const string F205_S5_2_Boss = "カレラニ　スクイヲ　モタラセララレレルノハ　ワワワタタシシシシシ";
    public const string F205_S5_3_Reko = "……";
    public const string F205_S5_4_Reko = "私は、あなたをたすけられない。";
    public const string F205_S5_5_Boss = "ムムムイミミナナ　ムムムムムイミミミミミ　セセセセセセセセセ";
    public const string F205_S5_6_Mana = "ようすがへんだ！";
    public const string F205_S5_7_Matuka = "知ってる";
    public const string F205_S5_8_Menderu = "この世界もろとも自爆するつもり？";
    public const string F205_S5_9_Pierre = "盛り上がってきたな！";
    public const string F205_S5_10_Ami = "早く逃げなきゃ！";
    public const string F205_S5_11_Reko = "逃げるったって…";
    public const string F205_S5_12_Mati = "！";
    public const string F205_S5_13_Mati = "みんな、下がるんだ";

    public const string F205_S6_1_Ami = "今のって…";
    public const string F205_S6_2_Mana = "ヌシだーーーー！";
    public const string F205_S6_3_Matuka = "助けられたのか？";
    public const string F205_S6_4_Mati = "全て丸く収まったということかな";
    public const string F205_S6_5_Reko = "でも、つくよみちゃんが…私のせいで…";
    public const string F205_S6_6_Pierre = "さっきのにやられちゃったのか？";
    public const string F205_S6_7_Menderu = "あの子は問題ないよ";
    public const string F205_S6_8_Reko = "え？";
    public const string F205_S6_9_Tukuyomi = "お呼びですか？";
    public const string F205_S6_10_Reko = "えっ！どうして！？";
    public const string F205_S6_11_Ami = "つくよみちゃんは…\nそういう人だから";
    public const string F205_S6_12_Tukuyomi = "誰かが何かを願うなら\n私はいつでもそこに居ますよ！";
    public const string F205_S6_13_Reko = "なにそれこわい";
    public const string F205_S6_14_Matuka = "ほぼ怪異なんだよな";
    public const string F205_S6_15_Menderu = "まだ奥があるみたいね";
    public const string F205_S6_16_Pierre = "行ってみよう！";

    #endregion

    #region フィールド210　最終部屋

    public const string F210_Start_1_Tukuyomi = "ここから出られそうですね\nすぐ行かれますか？";
    public const string F210_Start_2_Reko = "そうします";
    public const string F210_Start_3_Tukuyomi = "かしこまりました\nところで失った声についてですが";
    public const string F210_Start_4_Tukuyomi = "みなさんと話し合いまして、どなたかの声をいただけることになりました";
    public const string F210_Start_5_Reko = "そんなこと、いいんですか？";
    public const string F210_Start_6_Mana = "おっけー！";
    public const string F210_Start_7_Ami = "あなたなら悪いことには使わないだろうから";
    public const string F210_Start_8_Matuka = "君には世話になったしね";
    public const string F210_Start_9_Menderu = "これでも感謝してるのよ";
    public const string F210_Start_10_Pierre = "団員としてウチの宣伝するのに必要だろう！";
    public const string F210_Start_11_Mati = "そういうことだ、胸を張って受け取ると良い";
    public const string F210_Start_12_Reko = "みなさん…";
    public const string F210_Start_13_Tukuyomi = "ですから、ほしい声を決めたら話しかけて下さいね";

    public const string F210_Not_1_Tukuyomi = "受け取らずに行かれるんですか？";
    public const string F210_Not_2_Reko = "私自身の声を探してみたいんです";
    public const string F210_Not_3_Tukuyomi = "わあ、それは素敵ですね！";
    public const string F210_Not_4_Menderu = "あなたらしいと思うわ";
    public const string F210_Not_5_Mati = "ああ、それもいいだろう";
    public const string F210_Not_6_Matuka = "自分の魂の声に耳を傾けるんだ";
    public const string F210_Not_7_Pierre = "さらばだ若きサーカス・スター！";
    public const string F210_Not_8_Mana = "楽しかったよ！";
    public const string F210_Not_9_Ami = "いつかあなたの声を聴かせてね！";
    public const string F210_Not_10_Reko = "みなさんありがとうございます\nお元気で！";

    public const string F210_Get_Ami_1 = "私の声にする？";
    public const string F210_Get_Menderu_1 = "私の声がいいのね？";
    public const string F210_Get_Matuka_1 = "僕の声がいいのかな？";
    public const string F210_Get_Mati_1 = "私の声がいいのか？";
    public const string F210_Get_Pierre_1 = "僕の声を使うかい？";
    public const string F210_Get_Mana_1 = "MANAさんの声が欲しいか？";

    public const string F210_Get_Ami_2 = "素敵な物語を作ってね！";
    public const string F210_Get_Menderu_2 = "さあ受け取りなさい、あなたの未来に幸多からんことを";
    public const string F210_Get_Matuka_2 = "心ゆくまで叫ぶと良い！";
    public const string F210_Get_Mati_2 = "うむ、持っていくといい";
    public const string F210_Get_Pierre_2 = "オーケー！宣伝よろしくな！";
    public const string F210_Get_Mana_2 = "よかろう！うけとりたまえ";

    public const string F210_Exit_1_Reko = "あ…あ…これが、声？";
    public const string F210_Exit_2_Tukuyomi = "はい！それではこのあたり実装が大変なので早速旅立ちましょう！お元気で！";
    public const string F210_Exit_3_Reko = "え？何ですかそれは";
    public const string F210_Exit_4_Tukuyomi = "いけませんよ！喋るたびにデータが増えちゃいます！ほらほら";
    public const string F210_Exit_5_Mana = "仕方ないね";
    public const string F210_Exit_6_Matuka = "まったく忙しいな";
    public const string F210_Exit_7_Pierre = "元気でな！";
    public const string F210_Exit_8_Menderu = "しっかりやるんだよ";
    public const string F210_Exit_9_Ami = "何を？";
    public const string F210_Exit_10_Mati = "何でもさ";
    public const string F210_Exit_11_Reko = "あ、みなさんありがとうございました！";

    public const string F210_Exit2_1_Tukuyomi = "行ってしまいましたね…";
    public const string F210_Exit2_2_Matuka = "誰のせいじゃい！";
    public const string F210_Exit2_3_Menderu = "この世界はこれからどうなるんだろう";
    public const string F210_Exit2_4_Tukuyomi = "あっ、作った人が居なくなったから消えちゃう？私達も出ていったほうがいいかな？";
    public const string F210_Exit2_5_Tukuyomi = "いや、そう焦る必要も無いだろう";
    public const string F210_Exit2_6_Tukuyomi = "ちゃんと新しい管理者が居るからね";
    public const string F210_Exit2_7_Tukuyomi = "ハッハー！あれなら安心だ！";
    public const string F210_Exit2_8_Tukuyomi = "となるとここも結構居心地いいんだよな";
    public const string F210_Exit2_9_Tukuyomi = "もっと色んな人が遊びに来たら楽しくなるよ！";
    public const string F210_Exit2_10_Tukuyomi = "でも、私は…";
    public const string F210_Exit2_11_Tukuyomi = "弟さんもいつか来るかもしれませんよ";
    public const string F210_Exit2_12_Tukuyomi = "その時のためにもしっかり整備しておきませんと";
    public const string F210_Exit2_13_Tukuyomi = "…そうかもね";
    public const string F210_Exit2_14_Tukuyomi = "それじゃあみなさん、小春日和でパーティしませんか？世界が生まれ変わったお祝い！";
    public const string F210_Exit2_15_Tukuyomi = "それは名案です！今日はパーッと楽しみましょう！";

    #endregion

    #region エンディング
    #endregion
}
