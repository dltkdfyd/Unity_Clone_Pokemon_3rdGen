using UnityEngine;

// 게임 상태
public enum GameState
{
    OverWorld, // 오버월드
    Battle // 배틀
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private BattleManager battleManager;
    [SerializeField] private Camera overWorldCamera;

    [SerializeField] private GameState _gameState;

    private void Awake()
    {
        _gameState = GameState.OverWorld;
    }

    private void Start()
    {
        playerController.OnPokemonEncountered += StartPokemonBattle;
        battleManager.OnBattleFinish += FinishPokemonBattle;
        battleManager.gameObject.SetActive(false);
    }

    private void StartPokemonBattle()
    {
        _gameState = GameState.Battle;
        battleManager.gameObject.SetActive(true);
        overWorldCamera.gameObject.SetActive(false);

        var playerParty = playerController.GetComponent<PokemonParty>();
        var wildPokemon = FindFirstObjectByType<PokemonMapArea>().GetComponent<PokemonMapArea>().GetRandomWildPokemon();

        battleManager.HandleStartBattle(playerParty, wildPokemon);
    }

    private void FinishPokemonBattle(bool playerWin)
    {
        _gameState = GameState.OverWorld;
        battleManager.gameObject.SetActive(false);
        overWorldCamera.gameObject.SetActive(true);

        if(!playerWin)
        {
            
        }
    }

    private void Update()
    {
        if (_gameState == GameState.OverWorld)
        {
            playerController.HandleUpdate();
        }
        else if (_gameState == GameState.Battle)
        {
            battleManager.HandleUpdate();
        }
    }
}
