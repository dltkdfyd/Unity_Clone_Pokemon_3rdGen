using UnityEngine;

public class Skill
{
    private SkillBase _base;
    private int _pp; // 현재 기술의 남은 PP

    // 기술의 기본 정보
    public SkillBase Base
    {
        get => _base;   
        set => _base = value;
    }

    // 현재 기술의 남은 PP
    public int Pp
    {
        get => _pp;
        set => _pp = value;
    }

    public Skill(SkillBase sBase)
    {
        Base = sBase;
        Pp = sBase.PP;      // 기술의 초기 PP 설정
    }
}
