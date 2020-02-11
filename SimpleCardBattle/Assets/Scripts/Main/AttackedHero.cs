using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 攻撃される側
public class AttackedHero : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        // 攻撃側カートの選択
        CardController attacker = eventData.pointerDrag.GetComponent<CardController>();

        if (attacker == null)
        {
            return;
        }
        if (attacker.cardModel.canAttack)
        {
            // アタッカーがヒーローを攻撃する
            GameManager.instance.AttackToHero(attacker, true);
            // ヒーロのHPのチェック
            GameManager.instance.CheckHeroHp();
        }
    }
}
