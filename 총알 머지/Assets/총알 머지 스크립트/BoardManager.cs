using UnityEngine;

public class BoardManager : MonoBehaviour
{
    // 가로 칸 수
    public int width = 5;

    // 세로 칸 수
    public int height = 5;

    // 칸 간격
    public float spacing = 1.2f;

    // Cell 프리팹
    public GameObject cellPrefab;

    // 아이템 프리팹
    public GameObject[] itemPrefabs;

    // 생성된 Cell 저장
    private Cell[,] cells;

    void Start()
    {
        // 배열 생성
        cells = new Cell[width, height];

        // 바둑판 생성
        GenerateBoard();

        // 테스트 아이템 생성
        SpawnTestItem(0, 0, 0);
        SpawnTestItem(1, 0, 0);
    }

    // 바둑판 생성
    void GenerateBoard()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3 pos =
                    new Vector3(
                        x * spacing,
                        y * spacing,
                        0
                    );

                GameObject obj =
                    Instantiate(
                        cellPrefab,
                        pos,
                        Quaternion.identity
                    );

                Cell cell = obj.GetComponent<Cell>();

                cell.gridPos = new Vector2Int(x, y);

                cells[x, y] = cell;
            }
        }
    }

    // 테스트 아이템 생성
    void SpawnTestItem(int x, int y, int level)
    {
        Cell cell = cells[x, y];

        GameObject obj =
            Instantiate(
                itemPrefabs[level],
                cell.transform.position,
                Quaternion.identity
            );

        Item item = obj.GetComponent<Item>();

        item.level = level;

        item.currentCell = cell;

        cell.currentItem = item;
    }

    // 아이템 배치
    public void TryPlace(Item item)
    {
        Cell nearestCell =
            GetNearestCell(item.transform.position);

        if (nearestCell == null)
        {
            ReturnItem(item);
            return;
        }

        if (nearestCell.currentItem == null)
        {
            MoveToCell(item, nearestCell);
        }
        else
        {
            Item target = nearestCell.currentItem;

            if (target.level == item.level &&
                target != item)
            {
                Merge(item, target, nearestCell);
            }
            else
            {
                ReturnItem(item);
            }
        }
    }

    // 가장 가까운 칸 찾기
    Cell GetNearestCell(Vector3 pos)
    {
        Cell nearest = null;

        float minDistance = 999f;

        foreach (Cell cell in cells)
        {
            float distance =
                Vector2.Distance(
                    pos,
                    cell.transform.position
                );

            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = cell;
            }
        }

        if (minDistance > 1f)
            return null;

        return nearest;
    }

    // 칸 이동
    void MoveToCell(Item item, Cell cell)
    {
        if (item.currentCell != null)
        {
            item.currentCell.currentItem = null;
        }

        item.currentCell = cell;

        cell.currentItem = item;

        item.transform.position =
            cell.transform.position;
    }

    // 원래 위치 복귀
    void ReturnItem(Item item)
    {
        item.transform.position =
            item.currentCell.transform.position;
    }

    // 합체 처리
    void Merge(Item a, Item b, Cell cell)
    {
        if (a.currentCell != null)
            a.currentCell.currentItem = null;

        Destroy(a.gameObject);
        Destroy(b.gameObject);

        int nextLevel =
            Mathf.Min(
                a.level + 1,
                itemPrefabs.Length - 1
            );

        GameObject obj =
            Instantiate(
                itemPrefabs[nextLevel],
                cell.transform.position,
                Quaternion.identity
            );

        Item newItem = obj.GetComponent<Item>();

        newItem.level = nextLevel;

        newItem.currentCell = cell;

        cell.currentItem = newItem;
    }
}