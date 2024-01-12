using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using web_api2.DOTs.Fight;
using web_api2.DOTs.Skill;
using web_api2.DOTs.Weapon;

namespace web_api2
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character,GetCharacterDto>();
            CreateMap<AddCharacterDto,Character>();
            CreateMap<Weapon,GetWeaponDto>();
            CreateMap<Skill, GetSkillDto>();
            CreateMap<Character, HighscoreDto>();
        }
    }
}