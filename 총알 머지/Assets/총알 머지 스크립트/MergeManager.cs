using UnityEngine;
using System.Collections.Generic;

public class MergeManager : MonoBehaviour
{
    public static MergeManager Instance;

    public GridCell[] cells;
    public GameObject[] bulletPrefabs;

    public int startMinCount = 5;
    public int startMaxCount = 10;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SpawnStartBullets();
    }

    void SpawnStartBullets()
    {
        List<GridCell> emptyCells = new List<GridCell>(cells);

        int count = Random.Range(startMinCount, startMaxCount + 1);

        for (int i = 0; i < count; i++)
        {
            if (emptyCells.Count == 0)
                return;

            int randomIndex = Random.Range(0, emptyCells.Count);

            GridCell cell = emptyCells[randomIndex];

            emptyCells.RemoveAt(randomIndex);

            SpawnBullet(0, cell);
        }
    }

    public void SpawnBullet(int level, GridCell cell)
    {
        GameObject obj = Instantiate(
            bulletPrefabs[level],
            cell.transform.position,
            Quaternion.identity,
            cell.transform.parent);

        BulletItem bullet = obj.GetComponent<BulletItem>();

        bullet.level = level;
        bullet.SetCell(cell);
    }

    public GridCell GetClosestCell(Vector3 position)
    {
        GridCell closest = null;
        float minDistance = float.MaxValue;

        foreach (GridCell cell in cells)
        {
            float distance =
                Vector3.Distance(position, cell.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                closest = cell;
            }
        }

        return closest;
    }
}