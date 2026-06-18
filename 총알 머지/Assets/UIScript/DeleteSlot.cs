using UnityEngine;

public class DeleteSlot : MonoBehaviour
{
    public bool IsInside(Vector3 worldPos)
    {
        RectTransform rect = GetComponent<RectTransform>();

        Vector2 screenPos =
            RectTransformUtility.WorldToScreenPoint(
                null,
                worldPos);

        return RectTransformUtility.RectangleContainsScreenPoint(
            rect,
            screenPos);
    }

    public void DeleteBullet(BulletItem bullet)
    {
        bullet.RemoveFromCell();

        Destroy(bullet.gameObject);
    }
}