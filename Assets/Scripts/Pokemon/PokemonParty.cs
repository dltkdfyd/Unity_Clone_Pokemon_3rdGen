using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PokemonParty : MonoBehaviour
{
    [SerializeField] List<Pokemon> pokemons;
    [SerializeField] Pokemon hp;

    public List<Pokemon> Pokemons
    {
        get => pokemons;
    }

    private void Start()
    {
        foreach (var pokemon in pokemons)
        {
            pokemon.InitPokemon();
        }
    }

    public Pokemon GetFirstAlivedPokemon()
    {
        return pokemons.Where(p => p.HP > 0).FirstOrDefault();
    }

}   

