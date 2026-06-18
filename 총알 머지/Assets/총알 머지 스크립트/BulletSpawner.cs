using UnityEngine;
using System.Collections.Generic;

public class BulletSpawner : MonoBehaviour
{
    public void SpawnLv1Bullet()
    {
        if (MergeManager.Instance == null)
        {
            Debug.LogError("MergeManager가 씬에 없습니다.");
            return;
        }

        if (MergeManager.Instance.cells == null ||
            MergeManager.Instance.cells.Length == 0)
        {
            Debug.LogError("MergeManager의 Cells가 비어있습니다.");
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
        {
            Debug.Log("빈 칸이 없습니다.");
            return;
        }

        GridCell randomCell =
            emptyCells[Random.Range(0, emptyCells.Count)];

        MergeManager.Instance.SpawnBullet(0, randomCell);
    }
}