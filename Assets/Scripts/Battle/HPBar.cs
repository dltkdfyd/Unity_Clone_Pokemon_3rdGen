using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


// 포켓몬의 HP바를 관리하는 클래스
public class HPBar : MonoBehaviour
{
    [SerializeField] private Image hpBarForeground;  // 체력바가 채워지는 부분 (기본값: 초록색)
    [SerializeField] private Sprite greenHPBar;
    [SerializeField] private Sprite yellowHPBar;
    [SerializeField] private Sprite redHPBar;

    // 체력바 채우기
    // normalizedHP : 현재 체력의 비율 (0 ~ 100%)
    public void SetHP(float normalizedHP)   
    {
        hpBarForeground.fillAmount = normalizedHP;
        UpdateHPBarSprite(normalizedHP);
    }

    private void UpdateHPBarSprite(float normalizedHP)
    {
        if (normalizedHP > 0.5f)
            hpBarForeground.sprite = greenHPBar;
        else if (normalizedHP > 0.15f)
            hpBarForeground.sprite = yellowHPBar;
        else
            hpBarForeground.sprite = redHPBar;
    }

    // 체력바 변화를 부드럽게 처리
    // targetValue : 체력바 채우기 목표 비율 (0 ~ 100%)
    public IEnumerator SetSmoothHP(float targetValue)
    {
        float currentFill = hpBarForeground.transform.localScale.x;
        float changeAmount = currentFill - targetValue;

        while(currentFill - targetValue > Mathf.Epsilon)
        {
            currentFill -= changeAmount * Time.deltaTime;
            hpBarForeground.transform.localScale = new Vector3(currentFill, 1);
            UpdateHPBarSprite(currentFill);
            yield return null;
        }
        
        hpBarForeground.transform.localScale = new Vector3(targetValue, 1);
        UpdateHPBarSprite(targetValue);
    }

}
