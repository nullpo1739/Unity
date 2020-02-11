using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropPlace : MonoBehaviour, IDropHandler
{
    // フィールドか手札か
    public enum TYPE
    {
        HAND,
        FIELD,
    }
    public TYPE type;

    public void OnDrop(PointerEventData eventData)
    {
        if(type == TYPE.HAND)
        {
            return;
        }
        CardController card = eventData.pointerDrag.GetComponent<CardController>();

        if(card != null)
        {
            if(!card.cardMovement.isDrag)
            {
                return;
            }
            card.cardMovement.defaultParent = this.transform;
            // cardモデルがフィールドのカードだった場合は減らす必要がないので処理終了
            if (card.cardModel.isFieldCard)
            {
                return;
            }

            card.OnField(true);
        }
    }
}
