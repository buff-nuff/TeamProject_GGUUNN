using UnityEngine;

public class Item : MonoBehaviour
{
    // 아이템 레벨
    public int level;

    // 현재 들어있는 칸
    [HideInInspector]
    public Cell currentCell;

    // 드래그 위치 보정값
    private Vector3 offset;

    // 보드 매니저
    private BoardManager board;

    void Start()
    {
        // 보드 매니저 찾기
        board = FindObjectOfType<BoardManager>();
    }

    void OnMouseDown()
    {
        // 마우스 위치 가져오기
        Vector3 mouse =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mouse.z = 0;

        // 드래그 위치 보정
        offset = transform.position - mouse;
    }

    void OnMouseDrag()
    {
        // 마우스 위치 가져오기
        Vector3 mouse =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mouse.z = 0;

        // 아이템 이동
        transform.position = mouse + offset;
    }

    void OnMouseUp()
    {
        // 아이템 배치 시도
        board.TryPlace(this);
    }
}