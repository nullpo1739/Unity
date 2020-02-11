using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    // 見かけに関する操作
    public CardView cardView;
    // データに関する操作
    public CardModel cardModel;
    // カードの動きに関する動作
    public CardMovement cardMovement;

    private void Awake()
    {
        cardView = GetComponent<CardView>();
        cardMovement = GetComponent<CardMovement>();
    }

    public void OnField(bool isPlayer)
    {
        GameManager.instance.ReduceManaCost(cardModel.cost, isPlayer);
        cardModel.isFieldCard = true;
        if(cardModel.avirity == Avirity.INET_ATTACK)
        {
            // 出したターンに攻撃ができるように設定する
            SetCanAttack(true);
        }
    }

    public void Init(int cardId, bool isPlayer)
    {
        cardModel = new CardModel(cardId, isPlayer);
        cardView.Show(cardModel);
    }

    public void Attack(CardController enemyCard)
    {
        cardModel.Attack(enemyCard);
        SetCanAttack(false);
    }

    public void SetCanAttack(bool canAttack)
    {
        cardModel.canAttack = canAttack;
        cardView.SetActiveSelectablePanel(canAttack);
    }

    public void CheckAlive()
    {
        if(cardModel.isAlive)
        {
            cardView.Refresh(cardModel);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
