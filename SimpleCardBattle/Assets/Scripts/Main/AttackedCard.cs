using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 攻撃される側
public class AttackedCard : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        // 攻撃側カートの選択
        CardController attacker = eventData.pointerDrag.GetComponent<CardController>();
        // 守備カードを選択
        CardController defender = GetComponent<CardController>();

        if (attacker == null || defender == null)
        {
            return; 
        }
        if (attacker.cardModel.isPlayerCard == defender.cardModel.isPlayerCard)
        {
            return;
        }
        if (attacker.cardModel.canAttack)
        {
            // 戦いの実施
            GameManager.instance.CardsBattle(attacker, defender);
        }
    }
}
