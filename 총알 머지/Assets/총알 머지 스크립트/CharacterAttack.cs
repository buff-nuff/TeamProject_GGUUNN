using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    public BattleUIManager battleUIManager;
    public MergeSlot mergeSlot;

    public void DealDamage()
    {
        if (battleUIManager == null)
        {
            Debug.LogError("battleUIManager 없음");
            return;
        }

        if (mergeSlot == null)
        {
            Debug.LogError("mergeSlot 없음");
            return;
        }

        Debug.Log("현재 데미지 : " + mergeSlot.CurrentDamage);

        battleUIManager.ApplyDamage(mergeSlot.CurrentDamage);
    }
}