using UnityEngine;

[CreateAssetMenu(fileName = "SkillBase", menuName = "기술/기술베이스")]
public class SkillBase : ScriptableObject
{
    [SerializeField] private int ID; // 스킬 아이디

    [SerializeField] private string skillName; // 스킬 이름
    public string Name => skillName;

    [TextArea][SerializeField] private string skillDescription; // 스킬 설명
    public string Description => skillDescription;

    [SerializeField] private PokemonType type; // 스킬 타입
    public PokemonType Type => type;

    [SerializeField] private int power; // 스킬 위력
    public int Power => power;

    [SerializeField] private int accuracy; // 스킬 정확도
    public int Accuracy => accuracy;

    [SerializeField] private int pp; // 스킬 포인트
    public int PP => pp;


    public bool isSpecialSkill
    {
        get
        {
            if (type == PokemonType.Fire        || type == PokemonType.Water    ||
                type == PokemonType.Grass       || type == PokemonType.Ice      ||
                type == PokemonType.Electric    || type == PokemonType.Dragon   ||
                type == PokemonType.Dark        || type == PokemonType.Psychic   )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}
