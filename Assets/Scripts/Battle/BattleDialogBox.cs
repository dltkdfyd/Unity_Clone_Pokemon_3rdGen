using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;


public class BattleDialogBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogText;              // 대화 텍스트

    [SerializeField] private GameObject actionSelector;              // 행동 선택 창
    [SerializeField] private GameObject skillSelector;              // 기술 선택 창
    [SerializeField] private GameObject skillDescription;           // 기술 설명 창

    [SerializeField] private List<TextMeshProUGUI> actionTexts = new List<TextMeshProUGUI>();   // 행동 선택 텍스트
    [SerializeField] private List<TextMeshProUGUI> skillTexts = new List<TextMeshProUGUI>();     // 기술 선택 텍스트

    [SerializeField] private TextMeshProUGUI ppText;               // 기술 PP 텍스트
    [SerializeField] private TextMeshProUGUI typeText;             // 기술 타입 텍스트

    [SerializeField] private Color selectedColor = Color.blue;       // 선택된 텍스트 색상
    [SerializeField] private Color unselectedColor = Color.black;  // 선택되지 않은 텍스트 색상

    public float timeToWaitAfterText = 1.0f;
    public float charactersPerSecond = 30.0f;       // 문자 출력 속도 (초당 30자)   

    public bool isWriting = false; // 대화창 작동 중인지 여부

    // 대화 텍스트 출력 코루틴
    // message : 출력할 메시지
    public IEnumerator SetDialog(string message)
    {
        isWriting = true;

        dialogText.text = "";
        foreach (var character in message)
        {
            dialogText.text += character;
            yield return new WaitForSeconds(1f / charactersPerSecond); // 문자 출력 속도 설정
        }

        // 대화(문장)마다 1초씩 딜레이
        yield return new WaitForSeconds(timeToWaitAfterText);

        isWriting = false;
    }


    // 대화창 텍스트의 표시/숨김 설정
    // active: true면 표시, false면 숨김
    public void ToggleDialogText(bool active)
    {
        dialogText.enabled = active;

    }

    // 행동 선택 메뉴의 표시/숨김 설정
    // active: true면 표시, false면 숨김
    public void ToggleActionSelector(bool active)
    {
        actionSelector.SetActive(active);
    }

    // 기술 선택 메뉴의 표시/숨김 설정
    // active: true면 표시, false면 숨김
    public void ToggleSkillSelector(bool active)
    {
        skillSelector.SetActive(active);
        skillDescription.SetActive(active);
    }

    // 행동 선택 메뉴의 선택 상태 업데이트
    // selectedAction: 선택된 행동의 인덱스
    public void SelectAction(int selectedAction)
    {
        for (int i = 0; i < actionTexts.Count; i++)
        {
            actionTexts[i].color = (i == selectedAction) ? selectedColor : unselectedColor; // 선택된 행동은 파란색, 나머지는 검정색
        }
    }

    // 포켓몬의 기술 정보 설정
    // skills: 포켓몬의 기술 정보
    public void SetPokemonSkills(List<Skill> skills)
    {
        for (int i = 0; i < skillTexts.Count; i++)
        {
            if (i < skills.Count)
            {
                skillTexts[i].text = skills[i].Base.Name;
            }
            else
            {
                skillTexts[i].text = "---";
            }
        }
    }

    // 기술 선택 메뉴의 선택 상태 업데이트
    // selectedSkill: 선택된 기술의 인덱스
    public void SelectSkill(int selectedSkill, Skill skill)
    {
        for (int i = 0; i < skillTexts.Count; i++)
        {
            skillTexts[i].color = (i == selectedSkill) ? selectedColor : unselectedColor; // 선택된 기술은 파란색, 나머지는 검정색
        }

        ppText.text = $"{skill.Pp} / {skill.Base.PP}";
        typeText.text = $"타입 / {PokemonTypeExtensionsCls.GetTypeNameInKorean(skill.Base.Type)}";

        ppText.color = (skill.Pp <= 0 ? Color.red : Color.black);
    }


}

