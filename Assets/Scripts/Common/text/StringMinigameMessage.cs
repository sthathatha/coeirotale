using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ミニゲームで使用するメッセージ
/// </summary>
public class StringMinigameMessage
{
    #region マチ

    public const string MatiA_Title = "風凛花斬";
    public const string MatiA_Tutorial = "「！」マークがあらわれたら、すかさずボタンをおそう！";

    public const string MatiB_Title = "落歌狼藉";
    public const string MatiB_Tutorial = "表示される方向キーと決定ボタンを左から順におそう！\n７回成功で勝ち、３回失敗で負け！";

    #endregion

    #region メンデル

    public const string MenderuA_Title = "メンデルの種";
    public const string MenderuA_Tutorial = "25個の種を交互に1〜3個取り合います。\n" +
            "種はターンが移る時に上下左右にある空欄の数だけ成長し、10まで成長すると取れなくなります。\n" +
            "自分のターンに1個も取れなかった場合、負けとなります。";
    public const string MenderuA_Serif_Start = "「メンデルの種」で勝負よ！";
    public const string MenderuA_Serif_PTurn0 = "さあ、あなたの番よ";
    public const string MenderuA_Serif_ETurn0 = "それじゃあ　私は…";
    public const string MenderuA_Serif_ETurn0_1 = "これと…";
    public const string MenderuA_Serif_ETurn0_2 = "これね！";
    public const string MenderuA_Serif_ETurn1_0 = "な、なんですって！";
    public const string MenderuA_Serif_ETurn1_1 = "取れる種がひとつも無いわ！";
    public const string MenderuA_Serif_ETurn1_2 = "私の負けね…お見事！";
    public const string MenderuA_Serif_ETurn2_0 = "あら、もう取れる種が無いわ";
    public const string MenderuA_Serif_ETurn2_1 = "私の勝ちね、また遊びましょう";

    public const string MenderuB_Title = "メンデルの種";
    public const string MenderuB_Tutorial = "取った種の数字が得点となり、最後により多くの点を獲得した側が勝利となります。";

    #endregion

    #region ピエール

    public const string PierreA_Title = "平地競走";
    public const string PierreA_Tutorial = "ボールを踏んで加速して、ピエールを追いかけよう！\n" +
            "３回タッチしたら勝ち、３回ころんだら負けだぞ！";
    public const string PierreA_Serif0 = "ヘイヘイヘーイ！！";
    public const string PierreA_Random0 = "ハーッハッハッハ！";
    public const string PierreA_Random1 = "ィヤッホォォォウ！";
    public const string PierreA_Random2 = "ヘイヘイヘーイ！";
    public const string PierreA_Random3 = "ほらほらどうした！";
    public const string PierreA_Random4 = "まだまだいけるよねえ！";
    public const string PierreA_Random5 = "これならどうかな！";
    public const string PierreA_Random6 = "ここまでおいで！";
    public const string PierreA_Win = "まいった！君の勝ちだ！";
    public const string PierreA_Lose = "アッハッハ残念！\nまたいつでもおいで！";

    public const string PierreB_Title = "Say Ho!Yo!Yo!More!";
    public const string PierreB_Tutorial = "決定ボタンでボールを発射できるぞ！\nやられるまえにやれ！";

    public const string PierreB_Music = "♪Conjurer";
    public const string PierreB_Spell1 = "上演「アペイロンの火の輪くぐり」";
    public const string PierreB_Spell2 = "上演「ランブリングパレード」";

    #endregion

    #region まつかりすく

    public const string MatukaA_Title = "説明";
    public const string MatukaA_Tutorial = "とにかく連打しろ！";
    public const string MatukaA_Win = "合格\n祝ってやる";
    public const string MatukaA_Lose = "不合格";
    public const string MatukaA_Serif1 = "手本を見せよう";
    public const string MatukaA_Serif2 = "さあ　やってみたまえ";
    public const string MatukaA_Naration1 = "よーい";
    public const string MatukaA_Naration2 = "はじめ！";
    public const string MatukaA_Naration3 = "そこまで！";

    public const string MatukaB_Win = "勝ち";
    public const string MatukaB_Lose = "負け";

    #endregion

    #region MANA

    public const string ManaA_Title = "スピード";
    public const string ManaA_Tutorial = "場にあるカードより１多いか１少ないカードを出してね\n" +
            "先に全部のカードを出したほうが勝ち！";
    public const string ManaA_Ready = "よーい";
    public const string ManaA_Go = "すたーと！";
    public const string ManaA_Win = "だいしょうり！";
    public const string ManaA_Lose = "まけ…";

    public const string ManaB_Title = "ちょうスピード";
    public const string ManaB_Tutorial = "ちょっとてごわいぞ！";

    #endregion

    #region アミ

    public const string AmiA_Title = "ダンスバトル！";
    public const string AmiA_Tutorial = "タイミングよく上下左右を押して踊ろう！\n十字ボタン、ABXYボタン、\nキーボード矢印、DFJKキーで可";
    public const string AmiA_Great = "Great";
    public const string AmiA_Good = "Good";
    public const string AmiA_Bad = "Bad";

    public const string AmiB_Title = "ボスラッシュ";
    public const string AmiB_Tutorial = "にせものをたくさん倒すと最後のたたかいが有利になるよ";

    #endregion

    #region ラスボス本戦

    public const string BossB_Win = "勝利ッ！！";
    public const string BossB_Lose = "敗北…";

    public const string BossB_SkillA1_Name = "サイクロトロン";
    public const string BossB_SkillA1_Detail = "Ｃ：\n行動速度アップ";
    public const string BossB_SkillA2_Name = "オクトストライク";
    public const string BossB_SkillA2_Detail = "Ｏ：\n連続の打撃で大ダメージを与える";
    public const string BossB_SkillA3_Name = "イーグルダイブ";
    public const string BossB_SkillA3_Detail = "Ｅ：\n離れた場所に強力な物理攻撃";
    public const string BossB_SkillA4_Name = "インビンシブル";
    public const string BossB_SkillA4_Detail = "Ｉ：\n次の自分のターンまで無敵";
    public const string BossB_SkillA5_Name = "リインカーネーション";
    public const string BossB_SkillA5_Detail = "Ｒ：\n体力を全回復";
    public const string BossB_SkillA6_Name = "オリジン";
    public const string BossB_SkillA6_Detail = "Ｏ：\n敵味方全員のステータス変化と、全ての地形をクリア";
    public const string BossB_SkillA7_Name = "イグニッション";
    public const string BossB_SkillA7_Detail = "Ｉ：\n物理攻撃力アップ";
    public const string BossB_SkillA8_Name = "ナイトメア";
    public const string BossB_SkillA8_Detail = "Ｎ：\n範囲に小ダメージを与え、まれに行動を遅らせる";
    public const string BossB_SkillA9_Name = "キーニングシルフ";
    public const string BossB_SkillA9_Detail = "Ｋ：\n周囲の広範囲に風のダメージ";

    public const string BossB_SkillBAmi1_Name = "はるのとなり";
    public const string BossB_SkillBAmi1_Detail = "体力を大回復\nあまつさえ素早さと物理攻撃力アップ";
    public const string BossB_SkillBMana1_Name = "ショウダウン";
    public const string BossB_SkillBMana1_Detail = "味方全体になにかがおこる！";
    public const string BossB_SkillBMatuka1_Name = "喝";
    public const string BossB_SkillBMatuka1_Detail = "全体を威圧して行動を遅らせる";
    public const string BossB_SkillBMati1_Name = "刹那の見斬り";
    public const string BossB_SkillBMati1_Detail = "目にも留まらぬ居合抜き";
    public const string BossB_SkillBMenderu1_Name = "マントラップヴァイン";
    public const string BossB_SkillBMenderu1_Detail = "ツタ地形を生成し、足止めする";
    public const string BossB_SkillBPierre1_Name = "ジャグリングヒット";
    public const string BossB_SkillBPierre1_Detail = "何かを大量に投げつけて攻撃";
    public const string BossB_SkillBBoss1_Name = "カーネイジ";
    public const string BossB_SkillBBoss2_Name = "プラズマフィールド";
    public const string BossB_SkillBBoss3_Name = "ネオアームストロングサイクロンジェットアームストロング砲";
    public const string BossB_SkillBBoss4_Name = "トランキーライザー";

    #endregion

    #region つくよみちゃん

    public const string Tuku_Start_First_0 = "今日はステキな日です";
    public const string Tuku_Start_First_1 = "あなたのおかげで世界は解放され、みなさんの未来は希望にあふれています";
    public const string Tuku_Start_First_2 = "そんな日に　この世界を旅立つあなたに…";
    public const string Tuku_Start_First_3 = "最後の試練を与えましょう";
    public const string Tuku_Start_Second = "もうおわかりですよね？";

    public const string Tuku_Start2_First_0 = "ふふ、驚きましたか？";
    public const string Tuku_Start2_First_1 = "試練は既に始まっているんですよ";
    public const string Tuku_Start2_Second = "さあ、楽しみましょう";

    public const string Tuku_Game_1_1 = "私は人間が大好きです";
    public const string Tuku_Game_1_2 = "思いきり一緒に遊んで、思い切り抱きしめてあげたい";
    public const string Tuku_Game_2_1 = "だけど少し力を入れると簡単に壊れてしまうぐらい、人間はとても脆くて儚くて";
    public const string Tuku_Game_2_2 = "そして、だからこそ愛おしい";
    public const string Tuku_Game_3_1 = "だから…ここであなたと出会えたことが嬉しいんです";
    public const string Tuku_Game_3_2 = "きっとあなたは、誰よりも強い人";
    public const string Tuku_Game_4_1 = "だから";
    public const string Tuku_Game_4_2 = "私の初めてのわがまま、聞いてくれますか？";
    public const string Tuku_Game_5_1 = "私を…";
    public const string Tuku_Game_5_2 = "私の本気を、受け止めてください！";

    public const string Tuku_System_1 = "サイコーに　ごきげんな　ようすだ。";
    public const string Tuku_System_2 = "つくよみちゃんとの　おもいでの　かずかずが　せすじを　つたう。";
    public const string Tuku_System_3 = "もっと　あそび　つづけたがっている。";
    public const string Tuku_System_4 = "つくよみちゃんは　なにかを　じゅんび　している。";
    public const string Tuku_System_5 = "もう　このメッセージは　ひつよう　ない。";

    #endregion
}
