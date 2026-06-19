using UnityEngine;
using System.Collections.Generic;

public class BulletSpawner : MonoBehaviour
{
    public void SpawnLv1Bullet()
    {
        if (!CreditManager.Instance.UseCredit(10))
        {
            Debug.Log("Å©·¡µ÷ ºÎÁ·");
            return;
        }

        List<GridCell> emptyCells = new List<GridCell>();

        foreach (GridCell cell in MergeManager.Instance.cells)
        {
            if (cell != null && cell.currentItem == null)
            {
                emptyCells.Add(cell);
            }
        }

        if (emptyCells.Count == 0)
            return;

        GridCell randomCell =
            emptyCells[Random.Range(0, emptyCells.Count)];

        MergeManager.Instance.SpawnBullet(0, randomCell);
    }
}