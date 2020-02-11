using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CardEntity", menuName = "CriateCard")]
// カードデータそのもの
public class CardEntity : ScriptableObject
{
    // カードのデータ
    public new string name;
    // ライフ
    public int hp;
    // 攻撃力
    public int at;
    // カードのコスト
    public int cost;
    // カード画像
    public Sprite icon;
    // アビリティ
    public Avirity avirity;
}

public enum Avirity
{
    NONE,
    INET_ATTACK,
    SHIELD,
}
