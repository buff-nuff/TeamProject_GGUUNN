using UnityEngine;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour
{
    [Header("--- Boss UI Elements ---")]
    public GameObject bossHPBar;      // 타겟 머리 위 보스 체력바
    public GameObject bossTimerBar;   // 화면 상단 보스 타이머 바

    [Header("--- Reference Scripts ---")]
    public StageManager stageManager;  // 현재 스테이지 텍스트 정보를 가져올 스크립트

    // [보스 도전] 버튼을 눌렀을 때 실행되는 함수
    public void EnterBossBattle()
    {
        // 1. StageManager가 잘 연결되었는지 확인
        if (stageManager == null)
        {
            Debug.LogError("StageManager가 BattleUIManager에 연결되지 않았습니다!");
            return;
        }

        // 2. StageManager에서 현재 스테이지 번호(숫자)를 가져옵니다.
        int currentStage = stageManager.GetCurrentStageNumber();

        // 3. 현재 스테이지가 5의 배수인지 확인 (5로 나눈 나머지가 0인지 확인)
        if (currentStage % 5 == 0)
        {
            // 5의 배수 스테이지라면 보스 UI 활성화!
            if (bossHPBar != null) bossHPBar.SetActive(true);
            if (bossTimerBar != null) bossTimerBar.SetActive(true);

            Debug.Log($"[성공] {currentStage}스테이지 보스전 진입! UI를 켭니다.");
        }
        else
        {
            // 5의 배수가 아니라면 보스전 진입 불가 알림 (UI 안 켜짐)
            Debug.LogWarning($"[불가] 보스는 5스테이지마다 등장합니다! (현재 {currentStage}스테이지)");
        }
    }
}