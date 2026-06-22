using UnityEngine;
using TMPro;

public class CreditManager : MonoBehaviour
{
    public static CreditManager Instance;

    public TextMeshProUGUI creditText;

    public int currentCredit = 100;     // 현재 보유 중인 크레딧
    private int totalUsedCredit = 0;    // ⭐ [추가] 총알 만들면서 누적으로 소모한 크레딧

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateUI();
    }

    public bool UseCredit(int amount)
    {
        if (currentCredit < amount)
            return false;

        currentCredit -= amount;
        totalUsedCredit += amount; // ⭐ 크레딧을 정상적으로 소모했을 때만 누적치에 더해줍니다.

        UpdateUI();

        return true;
    }

    public void AddCredit(int amount)
    {
        Debug.Log("추가될 크래딧 : " + amount);

        currentCredit += amount;

        Debug.Log("현재 크래딧 : " + currentCredit);

        UpdateUI();
    }

    // ⭐ 두 정보가 UI 화면에 보기 좋게 두 줄로 나오도록 수정했습니다.
    void UpdateUI()
    {
        if (creditText != null)
        {
            // \n을 사용해 한 줄은 보유량, 한 줄은 소모량으로 띄워줍니다.
            creditText.text = $"현재 크레딧 : {currentCredit}\n소모한 크레딧 : {totalUsedCredit}";
        }
    }
}