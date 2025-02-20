using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class PartyMemberHUD : MonoBehaviour
{
    public TextMeshProUGUI nameText, LevelText, hpText;
    public HPBar hpBar;
    public Image pokemonImage;

    private Pokemon _pokemon;

    [SerializeField] private Image PokemonHUD;
    [SerializeField] private Sprite DefaultPokemonHUD;
    [SerializeField] private Sprite SelectedPokemonHUD;

    public void SetPokemonData(Pokemon pokemon)
    {
        _pokemon = pokemon;

        nameText.text = pokemon.Base.Name;
        LevelText.text = $"Lv. {pokemon.Level}";
        hpText.text = $"{pokemon.HP} / {pokemon.MaxHP}";
        hpBar.SetHP((float)pokemon.HP / pokemon.MaxHP);
        pokemonImage.sprite = pokemon.Base.FrontSprite;
        PokemonHUD.sprite = DefaultPokemonHUD;
    }

    public void SetSelectedPokemon(bool selected)
    {
        PokemonHUD.sprite = selected ? SelectedPokemonHUD : DefaultPokemonHUD;
    }
}
