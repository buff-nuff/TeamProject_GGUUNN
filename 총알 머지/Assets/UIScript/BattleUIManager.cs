using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class BattleUIManager : MonoBehaviour
{
    [Header("--- Boss UI Elements ---")]
    public GameObject bossHPBar;
    public GameObject bossTimerBar;

    [Header("--- General Dummy UI Elements ---")]
    public GameObject generalHPBar;
    public TextMeshProUGUI dummyHPText;

    [Header("--- Reference Scripts ---")]
    public StageManager stageManager;

    [Header("--- Fade & Target UI ---")]
    public Animator fadeAnimator;
    public Image dummyImage;
    public TextMeshProUGUI dummyNameText;

    [Header("--- Background UI ---")]
    public Image backgroundImage;

    [Header("--- Dummy Sprites ---")]
    public Sprite generalDummySprite;
    public Sprite bossDummySprite;

    [Header("--- Background Sprites (아침/점심/저녁) ---")]
    public Sprite bg_Morning;
    public Sprite bg_Afternoon;
    public Sprite bg_Evening;

    [Header("--- Stage Selection UI Lock ---")]
    // 인스펙터창에 연결된 1-1 ~ 1-9 입장 버튼 리스트
    public List<Button> stageEnterButtons = new List<Button>();

    [Header("--- Boss Timer Settings ---")]
    private float bossLimitTime = 15f;
    private float bossCurrentTime = 15f;

    private int currentStage = 1;
    private int maxUnlockedStage = 1;
    private float maxHP = 100f;
    private float currentHP = 100f;
    private bool isBossActive = false;
    private bool isTransitioning = false;
    private bool isGameCleared = false;

    private void Start()
    {
        DisableSliderInteraction(generalHPBar);
        DisableSliderInteraction(bossHPBar);
        DisableSliderInteraction(bossTimerBar);

        InitGeneralStage();

        // 게임 시작할 때 잠긴 버튼들 바로 숨기기
        UpdateStageButtonsInteractable();
    }

    private void Update()
    {
        if (isGameCleared) return;

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

    private void InitGeneralStage()
    {
        isBossActive = false;

        if (generalHPBar != null) generalHPBar.SetActive(true);
        if (bossHPBar != null) bossHPBar.SetActive(false);
        if (bossTimerBar != null) bossTimerBar.SetActive(false);

        if (dummyImage != null && generalDummySprite != null) dummyImage.sprite = generalDummySprite;
        if (dummyNameText != null) dummyNameText.text = $"1-{currentStage} 일반 허수아비";

        string stageKey = $"1-{currentStage}";
        SetDummyMaxHPByStageKey(stageKey);

        UpdateBackground();
    }

    private void UpdateBackground()
    {
        if (backgroundImage == null) return;

        if (currentStage >= 1 && currentStage <= 3)
        {
            if (bg_Morning != null) backgroundImage.sprite = bg_Morning;
            backgroundImage.color = Color.white;
        }
        else if (currentStage >= 4 && currentStage <= 6)
        {
            if (bg_Afternoon != null) backgroundImage.sprite = bg_Afternoon;
            backgroundImage.color = Color.white;
        }
        else if (currentStage >= 7 && currentStage <= 9)
        {
            if (bg_Evening != null) backgroundImage.sprite = bg_Evening;
            backgroundImage.color = Color.white;
        }
    }

    private void SetDummyMaxHPByStageKey(string stageKey)
    {
        if (DummyData.DummyHealths.ContainsKey(stageKey))
        {
            maxHP = DummyData.DummyHealths[stageKey];
        }
        else
        {
            maxHP = 9999f;
        }

        currentHP = maxHP;
        UpdateHPUI();
    }

    public void ClickToMoveStage(int targetStage)
    {
        if (targetStage > maxUnlockedStage || isTransitioning || isBossActive) return;

        currentStage = targetStage;
        string targetStageName = $"1-{currentStage}";
        if (stageManager != null)
        {
            stageManager.ChangeStage(targetStageName);
        }
        InitGeneralStage();
    }

    public void ApplyDamage(float damage)
    {
        Debug.Log("들어온 데미지 : " + damage);

        int creditReward = Mathf.FloorToInt(damage * 0.5f);

        Debug.Log("획득 크래딧 : " + creditReward);

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

    private IEnumerator TransitionRoutine()
    {
        isTransitioning = true;

        if (fadeAnimator != null)
        {
            fadeAnimator.gameObject.SetActive(true);
            fadeAnimator.Play("FadeInOut", -1, 0f);
        }

        yield return new WaitForSeconds(0.4f);

        if (currentStage == 9)
        {
            isGameCleared = true;
            isBossActive = false;

            if (bossHPBar != null) bossHPBar.SetActive(false);
            if (bossTimerBar != null) bossTimerBar.SetActive(false);
            if (generalHPBar != null) generalHPBar.SetActive(false);

            if (dummyNameText != null) dummyNameText.text = "🎉 GAME ALL CLEAR! 🎉";
            if (dummyHPText != null) dummyHPText.text = "1챕터 완결을 축하합니다!";
            if (dummyImage != null) dummyImage.color = new Color(1, 1, 1, 0.3f);

            yield return new WaitForSeconds(0.6f);
            if (fadeAnimator != null)
            {
                fadeAnimator.gameObject.SetActive(false);
            }

            isTransitioning = false;
            yield break;
        }

        currentStage++;

        if (currentStage > maxUnlockedStage)
        {
            maxUnlockedStage = currentStage;
            // ⭐ 다음 스테이지가 해금되었으니 숨겨진 버튼을 다시 켜주러 갑니다.
            UpdateStageButtonsInteractable();
        }

        string nextStageName = $"1-{currentStage}";
        if (stageManager != null)
        {
            stageManager.ChangeStage(nextStageName);
        }

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

    // ⭐ [수정] 잠겨있는 버튼은 SetActive(false)로 눈앞에서 아예 지워버리는 로직
    private void UpdateStageButtonsInteractable()
    {
        for (int i = 0; i < stageEnterButtons.Count; i++)
        {
            if (stageEnterButtons[i] == null) continue;

            int buttonStageNum = i + 1;

            if (buttonStageNum <= maxUnlockedStage)
            {
                // 이미 깬/진입한 스테이지: [입장] 버튼 보이게 하기
                stageEnterButtons[i].gameObject.SetActive(true);
                stageEnterButtons[i].interactable = true;
            }
            else
            {
                // 아직 못 깬 미지의 스테이지: [입장] 버튼을 통째로 숨겨버리기 🚫
                stageEnterButtons[i].gameObject.SetActive(false);
            }
        }
    }
}