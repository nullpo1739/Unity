using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CardMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    // カードの親
    public Transform defaultParent;

    // ドラッグ&ドロップが可能かどうか
    public bool isDrag;
    public void OnBeginDrag(PointerEventData eventData)
    {
        // カードのコストとプレイヤーのコストを比較
        CardController card = GetComponent<CardController>();
        if(!card.cardModel.isFieldCard && card.cardModel.cost <= GameManager.instance.playerManaCost)
        {
            isDrag = true;
        }
        else if(card.cardModel.isFieldCard && card.cardModel.canAttack)
        {
            isDrag = true;
        }
        else
        {
            isDrag = false;
        }

        if (!isDrag)
        {
            return;
        }
        defaultParent = transform.parent;
        transform.SetParent(defaultParent.parent, false);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDrag)
        {
            return;
        }
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDrag)
        {
            return;
        }
        transform.SetParent(defaultParent, false);
        GetComponent<CanvasGroup>().blocksRaycasts  = true;
    }

    public IEnumerator MoveToField(Transform field)
    {
        // 一度親をCanvasに設定する
        transform.SetParent(defaultParent.parent);
        // DOTweenで動きを与える
        transform.DOMove(field.position, 0.25f);
        yield return new WaitForSeconds(0.25f);
        defaultParent = field;
        transform.SetParent(defaultParent);
    }

    public IEnumerator MoveToTarget(Transform target)
    {
        // 攻撃後戻ってくるために一度自分の場所と並びを保存しておく
        Vector3 curentPosition = transform.position;
        int siblingIndex = transform.GetSiblingIndex();
        // 一度親をCanvasに設定する
        transform.SetParent(defaultParent.parent);
        // DOTweenで動きを与える(Target)
        transform.DOMove(target.position, 0.25f);
        yield return new WaitForSeconds(0.25f);
        // 元の位置に戻す
        transform.DOMove(curentPosition, 0.25f);
        yield return new WaitForSeconds(0.25f);
        transform.SetParent(defaultParent);
        transform.SetSiblingIndex(siblingIndex);
    }

    void Start()
    {
        defaultParent = transform.parent;
    }
}
