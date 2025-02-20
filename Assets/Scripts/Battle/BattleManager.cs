using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using System;

// 배틀의 현재 상태
public enum BattleState
{
    StartBattle,        // 배틀 시작
    PlayerSelectAction, // 플레이어 행동 선택
    PlayerSkill,        // 플레이어 기술 선택
    EnemySkill,         // 적 기술 선택
    PokemonSelection,   // 포켓몬 선택
    Inventory,          // 가방
    Run,                // 도망
    Busy,               // 행동 중..
    FinishBattle
}

// 포켓몬 전투 관리 클래스
public class BattleManager : MonoBehaviour
{
    [SerializeField] private BattleUnit playerUnit;     // 플레이어 포켓몬

    [SerializeField] private BattleUnit enemyUnit;        // 적 포켓몬

    [SerializeField] private BattleDialogBox battleDialogBox; // 대화창

    [SerializeField] private GameObject PokemonSelector;

    [SerializeField] private PartyHUD partyHUD;

    public BattleState currentState; // 현재 전투 상태

    public event Action<bool> OnBattleFinish;

    private PokemonParty playerParty;
    private Pokemon wildPokemon;

    private float timeSinceLastClick = 0; // 마지막 선택 이후 경과 시간
    [SerializeField] private float clickDelay = 0.3f; // 입력 처리 간 지연 시간

    private int selectedAction; // 현재 선택된 메뉴
    private int currentSelectedPokemon;
    private int selectedSkill; // 현재 선택된 기술

    public void HandleStartBattle(PokemonParty playerParty, Pokemon wildPokemon)
    {
        this.playerParty = playerParty;
        this.wildPokemon = wildPokemon;
        StartCoroutine(SetupBattle());    // 전투 시작 시 초기화
    }

    // 플레이어 행동 선택 매 프레임 마다 입력 처리
    public void HandleUpdate()
    {
        timeSinceLastClick += Time.deltaTime;

        // 대화창이 작동 중일 때 입력 처리 방지
        if(battleDialogBox.isWriting)
        {
            return;
        }

        if (currentState == BattleState.PlayerSelectAction)
        {
            HandlePlayerActionSelection();
        }
        else if (currentState == BattleState.PlayerSkill)
        {
            HandlePlayerSkillSelection();
        }
        else if (currentState == BattleState.PokemonSelection)
        {
            HandlePlayerPokemonSelection();
        }
    }

    // 전투 초기 설정 코루틴
    public IEnumerator SetupBattle()
    {
        currentState = BattleState.StartBattle; // 전투 상태 설정 (전투 시작)

        // 플레이어 포켓몬 설정
        playerUnit.SetupPokemon(playerParty.GetFirstAlivedPokemon());

        // 플레이어 포켓몬의 기술 정보 설정
        battleDialogBox.SetPokemonSkills(playerUnit.Pokemon.Skills);

        // 플레이어 포켓몬 HUD 설정
        playerUnit.BattleHud.SetPokemonData(playerParty.GetFirstAlivedPokemon());
        playerUnit.BattleHud.UpdatePokemonData();

        // 적 포켓몬 설정
        enemyUnit.SetupPokemon(wildPokemon);
        enemyUnit.BattleHud.SetPokemonData(enemyUnit.Pokemon);
        enemyUnit.BattleHud.UpdatePokemonData();

        partyHUD.InitPartyHUD();

        // 전투 시작 대사 출력
        yield return battleDialogBox.SetDialog($"앗! 야생의 {enemyUnit.Pokemon.Base.Name}(이)가 나타났다!");

        // 플레이어와 적의 스피드에 따라 행동 턴 정하기
        if (enemyUnit.Pokemon.Speed > playerUnit.Pokemon.Speed)
        {
            StartCoroutine(battleDialogBox.SetDialog($"{enemyUnit.Pokemon.Base.Name}의 선제공격!"));
            yield return new WaitForSeconds(1.0f);
            StartCoroutine(EnemyAction());
        }
        else
        {
            PlayerActionSelection();
        }
    }

    void BattleFinish(bool playerIsWin)
    {
        currentState = BattleState.FinishBattle;
        OnBattleFinish(playerIsWin);
    }

    // 플레이어 행동 선택
    private void PlayerActionSelection()
    {
        currentState = BattleState.PlayerSelectAction; // 전투 상태 설정 (플레이어 행동 선택)

        StartCoroutine(battleDialogBox.SetDialog($"{playerUnit.Pokemon.Base.Name}는(은) 무엇을 할까?"));
        battleDialogBox.ToggleDialogText(true);
        battleDialogBox.ToggleActionSelector(true);
        battleDialogBox.ToggleSkillSelector(false);

        selectedAction = 0; // 초기 상태의 선택된 메뉴는 0(싸운다)
        battleDialogBox.SelectAction(selectedAction);
    }


    // 플레이어 기술 선택
    private void PlayerSkillSelection()
    {
        currentState = BattleState.PlayerSkill; // 전투 상태 설정 (플레이어 기술 선택)

        battleDialogBox.ToggleDialogText(false);
        battleDialogBox.ToggleActionSelector(false);
        battleDialogBox.ToggleSkillSelector(true);

        selectedSkill = 0; // 초기 상태의 선택된 메뉴는 0(기술 1)
        battleDialogBox.SelectSkill(selectedSkill, playerUnit.Pokemon.Skills[selectedSkill]);
    }


    // 플레이어 포켓몬 창 선택
    private void PlayerPokemonSelection()
    {
        partyHUD.SetPartyData(playerParty.Pokemons);
        partyHUD.gameObject.SetActive(true);
        currentState = BattleState.PokemonSelection;

        currentSelectedPokemon = playerParty.Pokemons.IndexOf(playerUnit.Pokemon);
        partyHUD.UpdateSelection(currentSelectedPokemon);
    }


    // 플레이어 가방 선택
    private void PlayerInventorySelection()
    {
        
    }



    // 플레이어 메뉴 선택
    private void HandlePlayerActionSelection()
    {
        if (timeSinceLastClick >= clickDelay)
        {
            int prevAction = selectedAction;

            float vInput = Input.GetAxisRaw("Vertical");
            if (vInput != 0)
            {
                if (selectedAction % 2 == 0) // 왼쪽 열
                {
                    selectedAction = selectedAction == 0 ? 2 : 0;
                }
                else // 오른쪽 열
                {
                    selectedAction = selectedAction == 1 ? 3 : 1;
                }
                timeSinceLastClick = 0;
            }

            float hInput = Input.GetAxisRaw("Horizontal");
            if (hInput != 0)
            {
                if (selectedAction < 2) // 윗줄
                {
                    selectedAction = selectedAction == 0 ? 1 : 0;
                }
                else // 아랫줄
                {
                    selectedAction = selectedAction == 2 ? 3 : 2;
                }
                timeSinceLastClick = 0;
            }

            // 선택된 메뉴가 변경되었을 때 UI 업데이트
            if (prevAction != selectedAction)
            {
                battleDialogBox.SelectAction(selectedAction);
            }
        }

        if (Input.GetAxisRaw("Submit") != 0)
        {
            timeSinceLastClick = 0;

            if (selectedAction == 0)
            {
                Debug.Log("싸우기 선택");
                PlayerSkillSelection();
            }
            else if (selectedAction == 1)
            {
                Debug.Log("가방 선택");
                PlayerInventorySelection();
            }
            else if (selectedAction == 2)
            {
                Debug.Log("포켓몬 선택");
                PlayerPokemonSelection();
            }
            else if (selectedAction == 3)
            {
                Debug.Log("도망가기 선택");
                // PlayerEscapeSelection();
            }
        }
    }

    private void HandlePlayerSkillSelection()
    {
        if (timeSinceLastClick >= clickDelay)
        {
            int prevSkill = selectedSkill;
            int skillCount = playerUnit.Pokemon.Skills.Count;

            float vInput = Input.GetAxisRaw("Vertical");
            if (vInput != 0)
            {
                if (selectedSkill % 2 == 0) // 왼쪽 열
                {
                    int newSkill = selectedSkill == 0 ? 2 : 0;
                    if (newSkill < skillCount)
                    {
                        selectedSkill = newSkill;
                    }
                }
                else // 오른쪽 열
                {
                    int newSkill = selectedSkill == 1 ? 3 : 1;
                    if (newSkill < skillCount)
                    {
                        selectedSkill = newSkill;
                    }
                }
                timeSinceLastClick = 0;
            }

            float hInput = Input.GetAxisRaw("Horizontal");
            if (hInput != 0)
            {
                if (selectedSkill < 2) // 윗줄
                {
                    int newSkill = selectedSkill == 0 ? 1 : 0;
                    if (newSkill < skillCount)
                    {
                        selectedSkill = newSkill;
                    }
                }
                else // 아랫줄
                {
                    int newSkill = selectedSkill == 2 ? 3 : 2;
                    if (newSkill < skillCount)
                    {
                        selectedSkill = newSkill;
                    }
                }
                timeSinceLastClick = 0;
            }

            // 선택된 메뉴가 변경되었을 때 UI 업데이트
            if (prevSkill != selectedSkill)
            {
                battleDialogBox.SelectSkill(selectedSkill, playerUnit.Pokemon.Skills[selectedSkill]);
            }

            if (Input.GetAxisRaw("Submit") != 0)
            {
                timeSinceLastClick = 0;

                battleDialogBox.ToggleSkillSelector(false);
                battleDialogBox.ToggleDialogText(true);
                StartCoroutine(UsingSkill());
            }

            if (Input.GetAxisRaw("Cancel") != 0)
            {
                PlayerActionSelection();
            }
        }
    }

    // 플레이어 포켓몬 기술 사용
    IEnumerator UsingSkill()
    {
        currentState = BattleState.PlayerSkill;

        Skill skill = playerUnit.Pokemon.Skills[selectedSkill];

        if(skill.Pp <= 0)
        {
            yield break;
        }

        yield return PerformSkill(playerUnit, enemyUnit, skill);

        if (currentState == BattleState.PlayerSkill)
        {
            StartCoroutine(EnemyAction());
        }
    }


    // 적 포켓몬 기술 사용
    private IEnumerator EnemyAction()
    {
        currentState = BattleState.EnemySkill;

        Skill skill = enemyUnit.Pokemon.RandomSkill();
        if (skill.Pp <= 0)
        {
            yield break;
        }

        yield return PerformSkill(enemyUnit, playerUnit, skill);

        if (currentState == BattleState.EnemySkill)
        {
            PlayerActionSelection();
        }
    }

    // 기술 사용
    IEnumerator PerformSkill(BattleUnit attackUnit, BattleUnit targetUnit, Skill skill)
    {
        skill.Pp--;
        yield return battleDialogBox.SetDialog($"{attackUnit.Pokemon.Base.Name}의\n{skill.Base.Name}!");

        attackUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1.0f);
        targetUnit.PlayHitAnimation();

        var damageDesc = targetUnit.Pokemon.RecieveDamage(skill, attackUnit.Pokemon);
        yield return targetUnit.BattleHud.UpdatePokemonData();
        yield return ShowDamageDescription(damageDesc);

        if (damageDesc.Killed)
        {
            yield return new WaitForSeconds(0.5f);
            yield return battleDialogBox.SetDialog($"{targetUnit.Pokemon.Base.Name}는(은) 쓰러졌다!");
            yield return new WaitForSeconds(0.5f);
            targetUnit.PlayDeathAnimation();
            yield return new WaitForSeconds(1.5f);

            CheckForBattleFinish(targetUnit);
        }
    }

    // 배틀 종료 확인
    private void CheckForBattleFinish(BattleUnit killedUnit)
    {
        if (killedUnit.IsPlayer)
        {
            var nextPokemon = playerParty.GetFirstAlivedPokemon();
            if (nextPokemon != null)
            {
                PlayerPokemonSelection();
            }
            else
            {
                BattleFinish(false);
            }
        }
        else
        {
            BattleFinish(true);
        }
    }

    // 데미지 설명
    IEnumerator ShowDamageDescription(DamageDescription desc)
    {
        if(desc.Critical > 1f)
        {
            yield return battleDialogBox.SetDialog("급소에 맞았다!");
        }
        
        if(desc.Type > 1f)
        {
            yield return battleDialogBox.SetDialog("효과가 굉장했다!");
        }
        else if (desc.Type < 1f)
        {
            yield return battleDialogBox.SetDialog("효과가 별로인듯 하다..");
        }
        else if (desc.Type == 0)
        {
            yield return battleDialogBox.SetDialog("효과가 없다..");
        }
    }


    // 포켓몬 선택 메뉴 행동 관리
    private void HandlePlayerPokemonSelection()
    {
        if (timeSinceLastClick >= clickDelay)
        {
            int prevPokemon = currentSelectedPokemon;
            int pokemonCount = playerParty.Pokemons.Count;

            float vInput = Input.GetAxisRaw("Vertical");
            if (vInput != 0)
            {
                if (vInput > 0) // 위 방향키
                {
                    int newPokemon = currentSelectedPokemon - 1;
                    if (newPokemon < 0)
                    {
                        newPokemon = pokemonCount - 1;
                    }
                    currentSelectedPokemon = newPokemon;
                }
                else // 아래 방향키
                {
                    int newPokemon = (currentSelectedPokemon + 1) % pokemonCount;
                    currentSelectedPokemon = newPokemon;
                }
                timeSinceLastClick = 0;
            }

            // 선택된 포켓몬이 변경되었을 때 UI 업데이트
            if (prevPokemon != currentSelectedPokemon)
            {
                partyHUD.UpdateSelection(currentSelectedPokemon);
            }

            if (Input.GetAxisRaw("Submit") != 0)
            {
                timeSinceLastClick = 0;
                var selectedPokemon = playerParty.Pokemons[currentSelectedPokemon];
                if (selectedPokemon.HP <= 0)
                {
                    partyHUD.SetMessage("기절한 포켓몬은 내보낼 수 없습니다.");
                    return;
                }
                else if (selectedPokemon == playerUnit.Pokemon)
                {
                    partyHUD.SetMessage($"{selectedPokemon.Base.Name}는(은) 이미 전투중입니다.");
                    return;
                }

                partyHUD.gameObject.SetActive(false);
                currentState = BattleState.Busy;
                StartCoroutine(SwitchPokemon(selectedPokemon));            
            }

            if (Input.GetAxisRaw("Cancel") != 0)
            {
                timeSinceLastClick = 0;
                partyHUD.gameObject.SetActive(false);  // 포켓몬 선택창 비활성화
                currentState = BattleState.PlayerSelectAction;  // 상태를 행동 선택으로 변경
                PlayerActionSelection();  // 행동 선택 메뉴로 돌아가기
            }
        }
    }
    
    IEnumerator SwitchPokemon(Pokemon newPokemon)
    {
        if(playerUnit.Pokemon.HP > 0)
        {
            yield return battleDialogBox.SetDialog($"돌아와! {playerUnit.Pokemon.Base.Name}!");
            playerUnit.PlayDeathAnimation();
            yield return new WaitForSeconds(1.5f);
        }

        playerUnit.SetupPokemon(newPokemon);
        yield return playerUnit.BattleHud.UpdatePokemonData();
        battleDialogBox.SetPokemonSkills(newPokemon.Skills);

        yield return battleDialogBox.SetDialog($"가라! {newPokemon.Base.Name}!");
        StartCoroutine(EnemyAction());
    }

    
}



