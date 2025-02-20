using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using Random = UnityEngine.Random;
using System.Linq;

[Serializable]
public class Pokemon
{
    [SerializeField] private PokemonBase _base; // 포켓몬의 기본 정보 (종족값)
    public PokemonBase Base
    {
        get => _base;

    }

    [SerializeField] private int _level; // 포켓몬의 레벨
    public int Level
    {
        get => _level;
        set => _level = value;
    }

    private int _hp; // 포켓몬의 체력
    public int HP
    {
        get => _hp;
        set => _hp = value;
    }

    private List<Skill> _skills; // 포켓몬이 보유한 기술 목록  
    public List<Skill> Skills
    {
        get => _skills;
        set => _skills = value;
    }


    public void InitPokemon()
    {
        _hp = MaxHP;
        _skills = new List<Skill>();

        foreach (var skillList in _base.LearnableSkills)
        {
            // 현재 레벨보다 낮거나 같은 기술만 습득
            if (skillList.Level <= _level)
            {
                _skills.Add(new Skill(skillList.Skill));
            }

            // 기술 목록이 4개를 초과하지 않도록 제한
            if (_skills.Count >= 4)
            {
                break;
            }
        }
    }

    // 스탯 계산법
    public int MaxHP => Mathf.FloorToInt((_base.MaxHP * 2 * _level) / 100.0f + _level + 10);
    public int Attack => Mathf.FloorToInt((_base.Attack * _level) / 100.0f + 5);
    public int Defense => Mathf.FloorToInt((_base.Defense * _level) / 100.0f + 5);
    public int SpAttack => Mathf.FloorToInt((_base.SpAttack * _level) / 100.0f + 5);
    public int SpDefense => Mathf.FloorToInt((_base.SpDefense * _level) / 100.0f + 5);
    public int Speed => Mathf.FloorToInt((_base.Speed * _level) / 100.0f + 5);


    public DamageDescription RecieveDamage(Skill skill, Pokemon attacker)
    {
        float critical = 1f;
        if(Random.Range(1.0f, 100.0f) < 8f)
        {
            critical = 2f;
        }

        float type1 = TypeMatrix.EffectiveDamage(skill.Base.Type, this.Base.Type1);
        float type2 = TypeMatrix.EffectiveDamage(skill.Base.Type, this.Base.Type2);

        var damageDesc = new DamageDescription()
        {
            Critical = critical,
            Type = type1 * type2,
            Killed = false
        };

        float attack = (skill.Base.isSpecialSkill ? attacker.SpAttack : attacker.Attack);
        float defense = (skill.Base.isSpecialSkill ? this.SpDefense : this.Defense);

        // 데미지 보정치
        float modifier = Random.Range(0.85f, 1.0f) * type1 * type2 * critical; 

        float baseDamage = (2 * attacker.Level / 5.0f + 2) * skill.Base.Power * ((float) attack / defense) / 50.0f;
        int totalDamage = Mathf.FloorToInt(baseDamage * modifier);

        HP -= totalDamage;
        if(HP <= 0)
        {
            HP = 0;
            damageDesc.Killed = true; // 포켓몬 기절 반환
        }

        return damageDesc;
    }

    public Skill RandomSkill()
    {
        var skillWithPP = Skills.Where(s => s.Pp > 0).ToList();
        if(skillWithPP.Count > 0)
        {
            int randomIndex = Random.Range(0, skillWithPP.Count);
            return skillWithPP[randomIndex];
        }

        // TODO : 모든 기술의 pp가 없으면 본인의 체력 닳기 구현 
        return null;
    }
}

public class DamageDescription
{
    public float Critical { get; set; }
    public float Type { get; set; }
    public bool Killed { get; set; }
}
