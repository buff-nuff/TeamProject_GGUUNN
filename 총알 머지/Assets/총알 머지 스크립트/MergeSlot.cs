using UnityEngine;

public class MergeSlot : MonoBehaviour
{
    public Animator targetAnimator;

    public float CurrentDamage { get; private set; }

    private BulletItem equippedBullet;

    public bool IsInside(Vector3 worldPos)
    {
        Debug.Log("IsInside È£Ãâ");

        RectTransform rect =
            GetComponent<RectTransform>();

        if (rect == null)
        {
            Debug.LogError("RectTransform ¾øÀ½");
            return false;
        }

        Vector2 screenPos =
            RectTransformUtility.WorldToScreenPoint(
                null,
                worldPos);

        bool inside =
            RectTransformUtility.RectangleContainsScreenPoint(
                rect,
                screenPos);

        Debug.Log("ÀåÂøÄ­ Ã¼Å© : " + inside);

        return inside;
    }

    public bool IsEquipped(BulletItem bullet)
    {
        return equippedBullet == bullet;
    }

    public void Equip(BulletItem bullet)
    {

        Debug.Log("ÀåÂø ¼º°ø");

        CurrentDamage = bullet.damage;

        Debug.Log("ÀåÂø µ¥¹ÌÁö : " + bullet.damage);

        if (equippedBullet != null)
        {
            Unequip(equippedBullet);
        }

        bullet.RemoveFromCell();

        equippedBullet = bullet;

        bullet.transform.SetParent(transform);

        RectTransform rect =
            bullet.GetComponent<RectTransform>();

        rect.anchoredPosition = Vector2.zero;

        targetAnimator.SetBool("shot", true);
    }

    public void Unequip(BulletItem bullet)
    {

        Debug.Log("Unequip È£ÃâµÊ");

        CurrentDamage = 0;

        if (equippedBullet != bullet)
            return;

        GridCell emptyCell = null;

        foreach (GridCell cell in MergeManager.Instance.cells)
        {
            if (cell.currentItem == null)
            {
                emptyCell = cell;
                break;
            }
        }

        if (emptyCell != null)
        {
            bullet.transform.SetParent(
                emptyCell.transform.parent);

            bullet.SetCell(emptyCell);
        }

        equippedBullet = null;

        CurrentDamage = 0;

        Debug.Log("shot false ½ÇÇà");

        targetAnimator.SetBool("shot", false);
    }

    public bool HasBullet()
    {
        return equippedBullet != null;
    }
}