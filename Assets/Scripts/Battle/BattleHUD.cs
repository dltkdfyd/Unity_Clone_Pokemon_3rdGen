using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

// 배틀 화면에서 포켓몬의 상태를 표시하는 HUD 클래스
public class BattleHUD : MonoBehaviour
{
    public TextMeshProUGUI pokemonName;      // 포켓몬 이름 텍스트
    public TextMeshProUGUI pokemonLevel;    // 포켓몬 레벨 텍스트
    public HPBar hpBar;                    // HP바
    public TextMeshProUGUI hpText;         // HP 수치 텍스트

    private Pokemon _pokemon;

    // 포켓몬 정보를 HUD에 표시
    // pokemon: 표시할 포켓몬 데이터
    public void SetPokemonData(Pokemon pokemon)
    {
        _pokemon = pokemon;

        // 이름과 레벨 설정
        pokemonName.text = pokemon.Base.Name;
        pokemonLevel.text = $"Lv. {pokemon.Level}";

        hpBar.SetHP((float)_pokemon.HP / _pokemon.MaxHP);
        StartCoroutine(UpdatePokemonData());
        UpdateUI();
    }

    protected void UpdateUI()
    {
        if (hpText != null)
        {
            hpText.text = $"{_pokemon.HP} / {_pokemon.MaxHP}";
        }
    }

    // 포켓몬 정보 업데이트
    public IEnumerator UpdatePokemonData()
    {
        yield return hpBar.SetSmoothHP((float)_pokemon.HP / _pokemon.MaxHP);
        UpdateUI();
    }


}

