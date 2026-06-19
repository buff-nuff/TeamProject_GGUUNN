using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class BattleUIManager : MonoBehaviour
{
    [Header("--- Boss UI Elements ---")]
    public GameObject bossHPBar;        // 타겟 머리 위 보스 체력바 (Slider)
    public GameObject bossTimerBar;     // 화면 상단 보스 타이머 바 (Slider)

    [Header("--- General Dummy UI Elements ---")]
    public GameObject generalHPBar;     // 일반 더미 전용 체력바 (Slider)
    public TextMeshProUGUI dummyHPText;  // 체력 수치 텍스트 (예: 100 / 100)

    [Header("--- Reference Scripts ---")]
    public StageManager stageManager;    // 현재 스테이지 정보를 가져올 스크립트

    [Header("--- Fade & Target UI ---")]
    public Animator fadeAnimator;       // Fade_Panel의 애니메이터
    public Image dummyImage;            // 화면 우측 허수아비의 Image 컴포넌트
    public TextMeshProUGUI dummyNameText;// 허수아비 이름 텍스트

    [Header("--- Dummy Sprites ---")]
    public Sprite generalDummySprite;   // 일반 허수아비 이미지
    public Sprite bossDummySprite;      // 보스 몬스터 이미지

    [Header("--- Boss Timer Settings ---")]
    private float bossLimitTime = 15f;
    private float bossCurrentTime = 15f;

    // 인게임 데이터 내부 변수
    private int currentChapter = 1;     // 앞 숫자 (1-1의 '1')
    private int currentStage = 1;       // 뒤 숫자 (1-1의 '1')
    private float maxHP = 100f;
    private float currentHP = 100f;
    private bool isBossActive = false;
    private bool isTransitioning = false;

    private void Start()
    {
        DisableSliderInteraction(generalHPBar);
        DisableSliderInteraction(bossHPBar);
        DisableSliderInteraction(bossTimerBar);

        InitGeneralStage();
    }

    private void Update()
    {
        if (isBossActive && !isTransitioning)
        {
            bossCurrentTime -= Time.deltaTime;

            if (bossCurrentTime < 0) bossCurrentTime = 0;

            if (bossTimerBar != null)
            {
                bossTimerBar.GetComponent<Slider>().value = bossCurrentTime / bossLimitTime;
            }

            if (bossCurrentTime <= 0)
            {
                Debug.LogWarning("보스 제한 시간 초과! 패배 처리됩니다.");
                StartCoroutine(TransitionToGeneralRoutine());
            }
        }
    }

    private void DisableSliderInteraction(GameObject sliderObj)
    {
        if (sliderObj != null)
        {
            Slider slider = sliderObj.GetComponent<Slider>();
            if (slider != null)
            {
                slider.interactable = false;
                slider.transition = Selectable.Transition.None;
            }
        }
    }

    // 일반 스테이지 화면 세팅 함수
    private void InitGeneralStage()
    {
        isBossActive = false;

        if (generalHPBar != null) generalHPBar.SetActive(true);
        if (bossHPBar != null) bossHPBar.SetActive(false);
        if (bossTimerBar != null) bossTimerBar.SetActive(false);

        if (dummyImage != null && generalDummySprite != null) dummyImage.sprite = generalDummySprite;
        if (dummyNameText != null) dummyNameText.text = $"{currentChapter}-{currentStage} 일반 허수아비";

        // ⭐ 변경: 이제 "1-1" 이라는 완성된 문자열을 전달하여 체력을 세팅합니다.
        string stageKey = $"{currentChapter}-{currentStage}";
        SetDummyMaxHPByStageKey(stageKey);
    }

    // ⭐ [이름 기반 체력 세팅 함수] 
    private void SetDummyMaxHPByStageKey(string stageKey)
    {
        // DummyData 창고에 "2-1" 같은 문자가 등록되어 있는지 검사
        if (DummyData.DummyHealths.ContainsKey(stageKey))
        {
            // 기획자가 테이블에 정성스레 적어놓은 해당 스테이지 고유 체력 대입!
            maxHP = DummyData.DummyHealths[stageKey];
        }
        else
        {
            // 만약 테이블에 없는 먼 미래의 스테이지라면 방어용 예외 처리 공식 적용
            maxHP = 9999f;
        }

        currentHP = maxHP;
        UpdateHPUI();
    }

    // [보스 도전] 버튼 클릭 시
    public void EnterBossBattle()
    {
        if (stageManager == null || isTransitioning) return;

        int currentStageNum = stageManager.GetCurrentStageNumber();

        // 뒤쪽 스테이지 숫자가 딱 '5'일 때 보스전 활성화
        if (currentStageNum == 5)
        {
            isBossActive = true;
            bossCurrentTime = bossLimitTime;

            if (generalHPBar != null) generalHPBar.SetActive(false);
            if (bossHPBar != null) bossHPBar.SetActive(true);
            if (bossTimerBar != null) bossTimerBar.SetActive(true);

            if (dummyImage != null && bossDummySprite != null) dummyImage.sprite = bossDummySprite;
            if (dummyNameText != null) dummyNameText.text = $"CHAPTER {currentChapter} BOSS 등장!";

            // ⭐ 보스 체력도 마찬가지로 조합된 키(예: "1-5", "2-5")를 넘겨서 뺍니다.
            string stageKey = $"{currentChapter}-{currentStage}";
            SetDummyMaxHPByStageKey(stageKey);
        }
    }

    // 공격 버튼 터치 시
    public void OnAttackDummy()
    {
        if (isTransitioning)
            return;
    }

    //대미지 들어갈 때
    public void ApplyDamage(float damage)
    {

        int creditReward = Mathf.FloorToInt(damage * 0.5f);

        CreditManager.Instance.AddCredit(creditReward);

        if (isTransitioning)
            return;

        currentHP -= damage;

        if (currentHP < 0)
            currentHP = 0;

        UpdateHPUI();

        if (currentHP <= 0)
        {
            StartCoroutine(TransitionRoutine());
        }
    }

    private void UpdateHPUI()
    {
        if (isBossActive)
        {
            if (bossHPBar != null) bossHPBar.GetComponent<Slider>().value = currentHP / maxHP;
        }
        else
        {
            if (generalHPBar != null) generalHPBar.GetComponent<Slider>().value = currentHP / maxHP;
        }

        if (dummyHPText != null) dummyHPText.text = $"{currentHP} / {maxHP}";
    }

    // [승리 시] 자동으로 다음 스테이지 이동 계산 루틴
    private IEnumerator TransitionRoutine()
    {
        isTransitioning = true;

        if (fadeAnimator != null)
        {
            fadeAnimator.gameObject.SetActive(true);
            fadeAnimator.Play("FadeInOut", -1, 0f);
        }

        yield return new WaitForSeconds(0.4f);

        currentStage++;

        // 5스테이지 보스를 깨고 넘어가면 챕터가 올라감
        if (currentStage > 5)
        {
            currentChapter++;
            currentStage = 1;
        }

        string nextStageName = $"{currentChapter}-{currentStage}";
        if (stageManager != null)
        {
            stageManager.ChangeStage(nextStageName);
        }

        // 새 스테이지 세팅 호출 (이 내부에서 "2-1" 문자열을 만들어 새 피통을 가져옴)
        InitGeneralStage();

        yield return new WaitForSeconds(0.6f);

        if (fadeAnimator != null)
        {
            fadeAnimator.gameObject.SetActive(false);
        }
        isTransitioning = false;
    }

    private IEnumerator TransitionToGeneralRoutine()
    {
        isTransitioning = true;

        if (fadeAnimator != null)
        {
            fadeAnimator.gameObject.SetActive(true);
            fadeAnimator.Play("FadeInOut", -1, 0f);
        }

        yield return new WaitForSeconds(0.4f);

        InitGeneralStage();

        yield return new WaitForSeconds(0.6f);

        if (fadeAnimator != null)
        {
            fadeAnimator.gameObject.SetActive(false);
        }
        isTransitioning = false;
    }
}