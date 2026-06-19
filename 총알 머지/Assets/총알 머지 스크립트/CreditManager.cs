using UnityEngine;
using TMPro;

public class CreditManager : MonoBehaviour
{
    public static CreditManager Instance;

    public TextMeshProUGUI creditText;

    public int currentCredit = 100;

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

        UpdateUI();

        return true;
    }

    public void AddCredit(int amount)
    {
        currentCredit += amount;

        UpdateUI();
    }

    void UpdateUI()
    {
        if (creditText != null)
        {
            creditText.text = "Å©·¡µ÷ : " + currentCredit;
        }
    }
}