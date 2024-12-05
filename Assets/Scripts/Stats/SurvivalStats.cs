using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalStats : MonoBehaviour
{
    [Header("Hunger Settings")]
    public float maxHunger = 100;               //최대 허기량
    public float currentHunger;                 //현재 허기량
    public float hungerDecreaseRate = 1;        //초당 허기 감소량

    [Header("Space Suit Settings")]
    public float maxSuitDurability = 100;       //최대 우주복 내구도
    public float currentSuitDurability;         //현재 우주복 내구도
    public float havestingDamage = 5.0f;        //수집시 우주복 데미지
    public float craftingDamage = 3.0f;           //제작시 우주복 데미지

    private bool isGameOver = false;            //게임 오버 상태
    private bool isPaused = false;              //일시 정시 상태
    private float hungerTimer = 0;              //허기 감소 타이머 


    // Start is called before the first frame update
    void Start()
    {
        //게임 시작시 스텟들은 최대 인 상태로 시작 
        currentHunger = maxHunger;
        currentSuitDurability = maxSuitDurability;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver || isPaused) return;

        hungerTimer += Time.deltaTime;

        if(hungerTimer >= 1.0f)
        {
            currentHunger = Mathf.Max(0, currentHunger - hungerDecreaseRate);
            hungerTimer = 0;

            CheckDeath();
        }

    }

    //아이템 수집시 우주복 데미지
    public void DamageOnHarvesting()
    {
        if (isGameOver || isPaused) return;

        currentSuitDurability = Mathf.Max(0, currentSuitDurability - havestingDamage);  //0값 이하로 안 내려가게 막기 위해서 
        CheckDeath();
    }

    //아이템 제작시 우주복 데미지 
    public void DamageOnCrafting()
    {
        if (isGameOver || isPaused) return;

        currentSuitDurability = Mathf.Max(0, currentSuitDurability - havestingDamage);  //0값 이하로 안 내려가게 막기 위해서 
        CheckDeath();
    }

    //음식 섭취로 허기 회복
    public void EatFood(float amount)
    {
        if (isGameOver || isPaused) return;                                         //게임 종료나 중단 상태에서는 동작하지 않게

        currentHunger = Mathf.Min(maxHunger, currentHunger + amount);               //maxHunger 값을 넘기지 않기 위해 

        if (FloatingTextManager.Instance != null)
        {
            FloatingTextManager.Instance.Show($"허기 회복 수리 + {amount}", transform.position + Vector3.up);
        }

    }

    //우주복 수리 (크리스탈로 제작한 수리 키트 사용)
    public void RepairSuit(float amount)
    {
        if (isGameOver || isPaused) return;                                                     //게임 종료나 중단 상태에서는 동작하지 않게

        currentSuitDurability = Mathf.Min(maxSuitDurability, currentSuitDurability + amount);  //maxSuitDurability 값을 넘기지 않기 위해 

        if (FloatingTextManager.Instance != null)
        {
            FloatingTextManager.Instance.Show($"우주복 수리 + {amount}", transform.position + Vector3.up);
        }
    }

    private void CheckDeath()                               //플레이어 사망 처리 체크 함수 
    {
        if(currentHunger <= 0 || currentSuitDurability <= 0)
        {
            PlayerDeath();
        }
    }

    private void PlayerDeath()                              //플레이어 사망 함수 
    {
        isGameOver = true;
        Debug.Log("플레이어 사망!");
        //TODO : 사망 처리 추가 (게임오버 UI, 리스폰 등등)
    }

    public float GetHungerPercentage()                  //허기짐 % 리턴 함수 
    {
        return (currentHunger / maxHunger) * 100;
    }

    public float GetSuitDurabilityPercentage()                  //슈트 % 리턴 함수 
    {
        return (currentSuitDurability / maxSuitDurability) * 100;
    }

    public bool IsGameOver()                                        //게임 종료 확인 함수 
    {
        return isGameOver;
    }

    public void ResetStats()            //리셋 함수 작성 (변수들 초기화 용도)
    {
        isGameOver = false;
        isPaused = false;
        currentHunger = maxHunger;
        currentSuitDurability = maxSuitDurability;
        hungerTimer = 0;
    }
}
