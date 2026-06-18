using UnityEngine;

public class MergeSlot : MonoBehaviour
{
    public Animator targetAnimator;

    public float CurrentDamage { get; private set; }

    private BulletItem equippedBullet;

    public bool IsInside(Vector3 worldPos)
    {
        RectTransform rect =
            GetComponent<RectTransform>();

        Vector2 screenPos =
            RectTransformUtility.WorldToScreenPoint(
                null,
                worldPos);

        return RectTransformUtility.RectangleContainsScreenPoint(
            rect,
            screenPos);
    }

    public bool IsEquipped(BulletItem bullet)
    {
        return equippedBullet == bullet;
    }

    public void Equip(BulletItem bullet)
    {

        CurrentDamage = bullet.damage;

        Debug.Log("¿Â¬¯ µ•πÃ¡ˆ : " + bullet.damage);

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

        targetAnimator.SetBool("Equipped", true);
    }

    public void Unequip(BulletItem bullet)
    {

        Debug.Log("Unequip »£√‚µ ");

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

        targetAnimator.SetBool("Equipped", false);
    }
}