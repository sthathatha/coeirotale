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
    // ー〜
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

    #region フィールド000〜010

    public const string F003_GetKey = "小さな鍵を手に入れた！";

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

    #region フィールドXX1〜　道中ギミック系

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

    public const string F102_New1_Mati = "serif1";
    public const string F102_Lose1_Mati = "serif2";
    public const string F102_Retry1_Mati = "serif3";
    public const string F102_Win1_Mati = "serif4";

    #endregion

    #region フィールド112　まつかりすく

    public const string F112_New1_Matuka = "serif1";
    public const string F112_Lose1_Matuka = "serif2";
    public const string F112_Retry1_Matuka = "serif3";
    public const string F112_Win1_Matuka = "serif4";

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

    public const string F132_New1_Mana = "serif1";
    public const string F132_Lose1_Mana = "serif2";
    public const string F132_Retry1_Mana = "serif3";
    public const string F132_Win1_Mana = "serif4";

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
    public const string F153_You_Plant6_You = "え〜";
    public const string F153_You_Plant7_Worra = "いってあげなさいな";
    public const string F153_You_Plant8_You = "は〜い";

    public const string F153_New1_Ami = "serif1";
    public const string F153_Lose1_Ami = "serif2";
    public const string F153_Retry1_Ami = "serif3";
    public const string F153_Win1_Ami = "serif4";

    #endregion

    #region フィールド200〜　ラストダンジョン系

    public const string F008_Normal_Ami = "x";
    public const string F008_Normal_Mana = "x";
    public const string F008_Normal_Matuka = "x";
    public const string F008_Normal_Mati = "x";
    public const string F008_Normal_Pierre = "x";
    public const string F008_Normal_Menderu = "x";
    public const string F008_Normal_Drows = "y";
    public const string F008_Normal_Exa = "y";
    public const string F008_Normal_Worra = "y";
    public const string F008_Normal_Koob = "y";
    public const string F008_Normal_You = "y";

    public const string F008_Break1_Reko = "z";

    #endregion

    #region フィールド205　ラスボス部屋
    #endregion

    #region フィールド210　最終部屋
    #endregion

    #region エンディング
    #endregion
}
