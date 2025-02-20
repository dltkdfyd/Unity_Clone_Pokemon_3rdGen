using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class PokemonMapArea : MonoBehaviour
{
    [SerializeField] private List<Pokemon> wildPokemons;

    public Pokemon GetRandomWildPokemon()
    {
        var pokemon =  wildPokemons[Random.Range(0, wildPokemons.Count)];
        pokemon.InitPokemon();
        return pokemon;
    }
}
