using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Experimental.GraphView;

public class PartyHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    private PartyMemberHUD[] memberHUDs;

    private List<Pokemon> pokemons;

    public void InitPartyHUD()
    {
        memberHUDs = GetComponentsInChildren<PartyMemberHUD>();
    }

    public void SetPartyData(List<Pokemon> pokemons)
    {
        this.pokemons = pokemons;
        messageText.text = "포켓몬을 골라주세요.";

        for (int i = 0; i < memberHUDs.Length; i++)
        {
            if (i < pokemons.Count)
            {
                memberHUDs[i].SetPokemonData(pokemons[i]);
                memberHUDs[i].gameObject.SetActive(true);
            }
            else
            {
                memberHUDs[i].gameObject.SetActive(false);
            }
        }
    }

    public void UpdateSelection(int selectedIndex)
    {
        for (int i = 0; i < memberHUDs.Length;i++)
        {
            memberHUDs[i].SetSelectedPokemon(i == selectedIndex);
        }
    }


    public void SetMessage(string message)
    {
        messageText.text = message;
    }

}
