using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardModel : MonoBehaviour
{
    // カードのデータ
    public string name;
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
    // 生きているか死んでいるか判定
    public bool isAlive;
    // 攻撃可能かどうか
    public bool canAttack;
    // カードがフィールドのカードかどうか
    public bool isFieldCard;
    // 自分カードかどうか
    public bool isPlayerCard;

    public CardModel(int cardId, bool isPlayer)
    {
        CardEntity cardEntity = Resources.Load<CardEntity>("CardDataList/Card" + cardId);
        name = cardEntity.name;
        hp = cardEntity.hp;
        at = cardEntity.at;
        cost = cardEntity.cost;
        icon = cardEntity.icon;
        avirity = cardEntity.avirity;
        isAlive = true;
        isPlayerCard = isPlayer;
    }

    public void Damage (int dmg)
    {
        hp -= dmg;
        if(hp <= 0)
        {
            hp = 0;
            isAlive = false;
        }
    }

    public void Attack(CardController card)
    {
        card.cardModel.Damage(at);
    }

}
