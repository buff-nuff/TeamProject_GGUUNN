using UnityEngine;
using TMPro; // TextMeshPro를 사용할 경우 필수

public class StageManager : MonoBehaviour
{
    [Header("--- UI Elements ---")]
    public TextMeshProUGUI stageText; // 메인 화면의 스테이지 글자
    public GameObject mapPopup;       // 맵 선택 팝업창

    // ⭐ 추가: 메인 화면에 있는 [보스 도전 버튼] 오브젝트를 연결할 곳
    public GameObject bossChallengeButton;

    private int currentStageNum = 1; // 현재 스테이지 숫자를 기억할 변수
    public GameObject skinPopup; // 스킨 팝업창 오브젝트 연결용

    private void Start()
    {
        // 게임 시작 시 첫 스테이지(1-1)이므로 보스 버튼을 꺼둡니다.
        CheckBossButtonVisibility();
    }

    // 스테이지 선택 버튼을 눌렀을 때 실행되는 함수
    public void ChangeStage(string stageName)
    {
        // 1. 메인 화면의 텍스트 변경
        stageText.text = "스테이지 " + stageName;

        // 2. "1-5" 형태에서 뒤의 숫자만 추출하는 로직
        string[] parts = stageName.Split('-');
        if (parts.Length > 1)
        {
            int.TryParse(parts[1].Trim(), out currentStageNum);
        }
        else
        {
            int.TryParse(stageName.Trim(), out currentStageNum);
        }

        // 3. ⭐ 추가: 바뀐 스테이지 번호에 따라 보스 버튼을 켤지 끌지 결정
        CheckBossButtonVisibility();

        // 4. 맵 선택 팝업창 닫기
        if (mapPopup != null)
        {
            mapPopup.SetActive(false);
        }
    }

    // ⭐ 추가: 5의 배수 스테이지 체크하여 버튼을 켜고 끄는 함수
    private void CheckBossButtonVisibility()
    {
        if (bossChallengeButton != null)
        {
            // 5의 배수 스테이지라면 보스 버튼을 활성화(true), 아니면 비활성화(false)
            bool isBossStage = (currentStageNum % 5 == 0);
            bossChallengeButton.SetActive(isBossStage);
        }
    }

    // BattleUIManager가 현재 스테이지 숫자를 물어볼 때 답해주는 함수
    public int GetCurrentStageNumber()
    {
        return currentStageNum;
    }
    public void OpenSkinPopup()
    {
        if (skinPopup != null)
        {
            skinPopup.SetActive(true);
            skinPopup.transform.SetAsLastSibling(); // 켜질 때 제일 앞으로 오게 강제 고정
        }
    }

    public void CloseSkinPopup()
    {
        if (skinPopup != null)
        {
            skinPopup.SetActive(false);
        }
    }
}