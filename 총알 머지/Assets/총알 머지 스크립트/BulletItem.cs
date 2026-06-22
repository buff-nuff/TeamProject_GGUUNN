using UnityEngine;
using UnityEngine.EventSystems;

public class BulletItem : MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    public int level;

    public float damage = 100f;

    private GridCell currentCell;
    private Vector3 startPosition;

    public void SetCell(GridCell cell)
    {
        currentCell = cell;
        cell.currentItem = this;

        transform.position = cell.transform.position;
    }

    public void RemoveFromCell()
    {
        if (currentCell != null)
        {
            currentCell.currentItem = null;
            currentCell = null;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransform rect = GetComponent<RectTransform>();

        rect.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        DeleteSlot deleteSlot =
    FindFirstObjectByType<DeleteSlot>();

        if (deleteSlot != null)
        {
            if (deleteSlot.IsInside(transform.position))
            {
                deleteSlot.DeleteBullet(this);
                return;
            }
        }

        MergeSlot slot = FindFirstObjectByType<MergeSlot>();

        if (slot != null)
        {
            if (slot.IsInside(transform.position))
            {
                slot.Equip(this);
                return;
            }

            Debug.Log("ÀåÂøÁß ¿©ºÎ : " + slot.IsEquipped(this));

            if (slot.IsEquipped(this))
            {
                slot.Unequip(this);
            }
        }

        GridCell targetCell =
            MergeManager.Instance.GetClosestCell(transform.position);

        if (targetCell == null)
        {
            transform.position = startPosition;
            return;
        }

        if (targetCell.currentItem == null)
        {
            if (currentCell != null)
                currentCell.currentItem = null;

            currentCell = targetCell;
            targetCell.currentItem = this;

            transform.position = targetCell.transform.position;
            return;
        }

        BulletItem other = targetCell.currentItem;

        if (other == this)
        {
            transform.position = targetCell.transform.position;
            return;
        }

        if (other.level != level)
        {
            transform.position = startPosition;
            return;
        }

        if (slot != null)
        {
            if (slot.IsEquipped(this))
            {
                transform.position = startPosition;
                return;
            }

            if (other != null && slot.IsEquipped(other))
            {
                transform.position = startPosition;
                return;
            }
        }

        if (other.level == level)
        {
            if (level >= 4)
            {
                transform.position = targetCell.transform.position;
                return;
            }

            int nextLevel = level + 1;

            currentCell.currentItem = null;
            targetCell.currentItem = null;

            Destroy(other.gameObject);
            Destroy(gameObject);

            MergeManager.Instance.SpawnBullet(nextLevel, targetCell);
        }
    }
}