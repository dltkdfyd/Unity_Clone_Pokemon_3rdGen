using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.CompilerServices;

[CreateAssetMenu(fileName = "PokemonBase", menuName = "포켓몬/포켓몬베이스")]
public class PokemonBase : ScriptableObject
{
    [SerializeField] private int ID; // 포켓몬 아이디

    [SerializeField] private string pokemonName; // 포켓몬 이름
    public string Name => pokemonName;

    [TextArea][SerializeField] private string pokemonDescription; // 포켓몬 설명
    public string Description => pokemonDescription;

    [SerializeField] private Sprite frontSprite; // 포켓몬 앞면 이미지
    public Sprite FrontSprite => frontSprite;
    [SerializeField] private Sprite backSprite; // 포켓몬 뒷면 이미지
    public Sprite BackSprite => backSprite;
    [SerializeField] private Sprite icon; // 포켓몬 아이콘
    public Sprite Icon => icon;

    [SerializeField] private PokemonType type1; // 포켓몬 타입1
    public PokemonType Type1 => type1;
    [SerializeField] private PokemonType type2; // 포켓몬 타입2
    public PokemonType Type2 => type2;

    [SerializeField] private int catchRate; // 포켓몬 포획률

    // 포켓몬 능력치
    [SerializeField] private int maxHP; // 최대 체력
    public int MaxHP => maxHP;

    [SerializeField] private int attack; // 공격력
    public int Attack => attack;

    [SerializeField] private int defense; // 방어력
    public int Defense => defense;

    [SerializeField] private int spAttack; // 특수 공격력
    public int SpAttack => spAttack;

    [SerializeField] private int spDefense; // 특수 방어력
    public int SpDefense => spDefense;

    [SerializeField] private int speed; // 스피드
    public int Speed => speed;

    [SerializeField] private List<LearnableSkill> learnableSkills; // 배울 수 있는 기술
    public List<LearnableSkill> LearnableSkills => learnableSkills;

}

// 포켓몬 타입
public enum PokemonType
{
    None,
    Normal,
    Fire,
    Water,
    Electric,
    Grass,
    Ice,
    Fighting,
    Poison,
    Ground,
    Flying,
    Psychic,
    Bug,
    Rock,
    Ghost,
    Dragon,
    Dark,
    Steel
}

public class TypeMatrix
{

    private static float[][] matrix =
    {
        //ATK:   ///   DEF:   Nor   Fir   Wat   Elc   Grs   Ice   Fig   Poi   Gro   Fly   Psy   Bug   Roc   Gho   Drg   Drk   Stl
        /*Nor*/ new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.5f, 0.0f, 1.0f, 1.0f, 0.5f},
        /*Fir*/ new float[] { 1.0f, 0.5f, 0.5f, 1.0f, 2.0f, 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 0.5f, 1.0f, 0.5f, 1.0f, 2.0f},
        /*Wat*/ new float[] { 1.0f, 2.0f, 0.5f, 1.0f, 0.5f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 0.5f, 1.0f, 1.0f},
        /*Elc*/ new float[] { 1.0f, 1.0f, 2.0f, 0.5f, 0.5f, 1.0f, 1.0f, 1.0f, 0.0f, 2.0f, 1.0f, 1.0f, 0.5f, 1.0f, 0.5f, 1.0f, 1.0f},
        /*Grs*/ new float[] { 1.0f, 0.5f, 2.0f, 1.0f, 0.5f, 1.0f, 1.0f, 0.5f, 2.0f, 0.5f, 1.0f, 0.5f, 2.0f, 1.0f, 0.5f, 1.0f, 0.5f},
        /*Ice*/ new float[] { 1.0f, 0.5f, 0.5f, 1.0f, 2.0f, 0.5f, 1.0f, 1.0f, 2.0f, 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 0.5f},
        /*Fig*/ new float[] { 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 0.5f, 1.0f, 0.5f, 0.5f, 0.5f, 2.0f, 0.0f, 1.0f, 2.0f, 2.0f},
        /*Poi*/ new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 1.0f, 0.5f, 0.5f, 1.0f, 1.0f, 1.0f, 0.5f, 0.5f, 1.0f, 1.0f, 0.0f},
        /*Gro*/ new float[] { 1.0f, 2.0f, 1.0f, 2.0f, 0.5f, 1.0f, 1.0f, 2.0f, 1.0f, 0.0f, 1.0f, 0.5f, 2.0f, 1.0f, 1.0f, 1.0f, 2.0f},
        /*Fly*/ new float[] { 1.0f, 1.0f, 1.0f, 0.5f, 2.0f, 1.0f, 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 0.5f, 1.0f, 1.0f, 1.0f, 0.5f},
        /*Psy*/ new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 2.0f, 1.0f, 1.0f, 0.5f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.5f},
        /*Bug*/ new float[] { 1.0f, 0.5f, 1.0f, 1.0f, 2.0f, 1.0f, 0.5f, 0.5f, 1.0f, 0.5f, 2.0f, 1.0f, 1.0f, 0.5f, 1.0f, 2.0f, 0.5f},
        /*Roc*/ new float[] { 1.0f, 2.0f, 1.0f, 1.0f, 1.0f, 2.0f, 0.5f, 1.0f, 0.5f, 2.0f, 1.0f, 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.5f},
        /*Gho*/ new float[] { 0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 1.0f, 2.0f, 1.0f, 0.5f, 0.5f},
        /*Drg*/ new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 0.5f},
        /*Drk*/ new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.5f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 1.0f, 2.0f, 1.0f, 0.5f, 0.5f},
        /*Stl*/ new float[] { 1.0f, 0.5f, 0.5f, 0.5f, 1.0f, 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 1.0f, 1.0f, 0.5f},
    };

    public static float EffectiveDamage(PokemonType attackType, PokemonType defenderType)
    {
        // 타입이 없으면 기본배수 1.0 반환
        if (attackType == PokemonType.None || defenderType == PokemonType.None)
        {
            return 1.0f;
        }

        int row = (int)attackType;
        int col = (int)defenderType;

        // 상성표에 None이 없으므로 상성표의 인덱스는 -1씩 하여 반환
        return matrix[row - 1][col - 1];
    }
}


public class PokemonTypeExtensionsCls
{
    protected static Dictionary<PokemonType, string> typeNames = new Dictionary<PokemonType, string>()
    {
        { PokemonType.None, "없음" },       
        { PokemonType.Normal, "노말" },
        { PokemonType.Fire, "불꽃" },
        { PokemonType.Water, "물" },
        { PokemonType.Grass, "풀" },
        { PokemonType.Electric, "전기" },
        { PokemonType.Ice, "얼음" },
        { PokemonType.Fighting, "격투" },
        { PokemonType.Poison, "독" },
        { PokemonType.Ground, "땅" }, 
        { PokemonType.Flying, "비행" },
        { PokemonType.Psychic, "정신" },
        { PokemonType.Bug, "벌레" },
        { PokemonType.Rock, "바위" },
        { PokemonType.Ghost, "고스트" },
        { PokemonType.Dragon, "드래곤" },
        { PokemonType.Dark, "악" },
        { PokemonType.Steel, "강철" },
         
    };

    public static string GetTypeNameInKorean(PokemonType type)
    {
        return typeNames[type];
    }
} 

// 배울 수 있는 기술
[System.Serializable]
public class LearnableSkill
{
    [SerializeField] private SkillBase skill; // 기술
    public SkillBase Skill => skill;

    [SerializeField] private int level; // 배우는 레벨
    public int Level => level;
}
