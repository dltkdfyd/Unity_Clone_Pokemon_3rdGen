using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

// 배틀에 참여하는 포켓몬 유닛을 관리하는 클래스
public class BattleUnit : MonoBehaviour
{
    // 포켓몬 기본 정보
    public PokemonBase _base;   // 포켓몬 기본 정보
    public int _level;         // 포켓몬 레벨

    [SerializeField] private bool isPlayer;      // 플레이어 포켓몬인지 여부

    [SerializeField] private BattleHUD _battleHud;

    private Image pokemonImage; // 포켓몬 이미지
    private Vector3 originalPosition; // 원래 위치

    private Color originalColor;

    // 애니메이션 시간  
    [SerializeField] float startAnimTime = 1.0f;
    [SerializeField] float attackAnimTime = 0.1f;
    [SerializeField] float hitAnimTime = 0.15f;
    [SerializeField] float deathAnimTime = 1.0f;

    public Pokemon Pokemon
    {
        get;
        set;
    }

    public bool IsPlayer => isPlayer;

    public BattleHUD BattleHud => _battleHud;

    private void Awake()
    {
        pokemonImage = GetComponent<Image>();
        originalPosition = pokemonImage.transform.localPosition;
        originalColor = pokemonImage.color;
    }
    
    // 포켓몬 정보 초기화 및스프라이트 설정
    public void SetupPokemon(Pokemon pokemon)
    {
        // 포켓몬 인스턴스 설정
        Pokemon = pokemon;

        if(isPlayer)
        {
            transform.SetSiblingIndex(4);
        }

        // 포켓몬 스프라이트 설정
        // 플레이어 포켓몬은 뒷모습, 적 포켓몬은 앞모습
        pokemonImage.sprite = (isPlayer ? Pokemon.Base.BackSprite : Pokemon.Base.FrontSprite);
        pokemonImage.color = originalColor;
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);

        _battleHud.SetPokemonData(pokemon);

        PlayStartAnimation();
    }

    // 포켓몬 시작 애니메이션
    public void PlayStartAnimation()
    {
        pokemonImage.transform.localPosition = new Vector3(originalPosition.x + (isPlayer ? -1 : 1) * 400f, originalPosition.y);

        pokemonImage.transform.DOLocalMoveX(originalPosition.x, startAnimTime);
    }

    // 포켓몬 공격 애니메이션
    public void PlayAttackAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(pokemonImage.transform.DOLocalMoveX(originalPosition.x + (isPlayer ? 1 : -1) * 50f, attackAnimTime));
        sequence.Append(pokemonImage.transform.DOLocalMoveX(originalPosition.x, attackAnimTime));
    }


    public Ease EaseType = Ease.InElastic;
    public void PlayHitAnimation()
    {
        var colortween = pokemonImage.DOColor(Color.gray, hitAnimTime).SetLoops(4, LoopType.Yoyo).OnComplete(() => {
            // 색상 바꿨을때 원상복구용
        }
        );

        pokemonImage.transform.DOLocalMoveX(originalPosition.x + (isPlayer ? -1 : 1) * 25, hitAnimTime)
            .SetLoops(4, LoopType.Yoyo)
            .OnComplete(() => { })
            .SetEase(EaseType);

        var sequence = DOTween.Sequence();
        sequence.Append(colortween);
    
        //sequence.Append(pokemonImage.DOColor(Color.gray, hitAnimTime));
        //sequence.Append(pokemonImage.transform.DOShakePosition());
        //sequence.Append(pokemonImage.DOColor(originalColor, hitAnimTime));
    }

    // 포켓몬 사망 애니메이션
    public void PlayDeathAnimation()
    {
        var sequence = DOTween.Sequence();

        if (isPlayer)
        {
            transform.SetSiblingIndex(4);
        }

        sequence.Append(pokemonImage.DOFade(0.5f, deathAnimTime * 0.1f).SetLoops(3, LoopType.Yoyo));
        sequence.Append(pokemonImage.transform.DOLocalMoveY(originalPosition.y - 600f, deathAnimTime * 0.7f));
        sequence.Join(pokemonImage.DOFade(0f, deathAnimTime * 0.7f));
    }

    [ContextMenu("[배틀 시작 확인용]")]
    void _TestEditor_PlayerStartAni()
    {
        PlayStartAnimation();
    }

    [ContextMenu("[공격 확인용]")]
    void _TestEditor_PlayerAttackAni()
    {
        PlayAttackAnimation();
    }

    [ContextMenu("[히트 확인용]")]
    void _TestEditor_PlayerHitAni()
    {
        PlayHitAnimation();
    }

    [ContextMenu("[사망 확인용]")]
    void _TestEditor_PlayerDeathAni()
    {
        PlayDeathAnimation();
    }

}

