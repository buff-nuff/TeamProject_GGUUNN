using UnityEngine;
using UnityEngine.EventSystems;

public class BulletItem : MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    public int level;

    private GridCell currentCell;
    private Vector3 startPosition;

    public void SetCell(GridCell cell)
    {
        currentCell = cell;
        cell.currentItem = this;

        transform.position = cell.transform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position += (Vector3)eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GridCell targetCell =
            MergeManager.Instance.GetClosestCell(transform.position);

        if (targetCell == null)
        {
            transform.position = startPosition;
            return;
        }

        if (targetCell.currentItem == null)
        {
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

        if (other.level == level)
        {
            int nextLevel = level + 1;

            currentCell.currentItem = null;
            targetCell.currentItem = null;

            Destroy(other.gameObject);
            Destroy(gameObject);

            MergeManager.Instance
                .SpawnBullet(nextLevel, targetCell);
        }
        else
        {
            transform.position = startPosition;
        }
    }
}