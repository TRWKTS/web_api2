using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using web_api2.DOTs.Skill;
using web_api2.DOTs.Weapon;

namespace web_api2.DOTs.Character
{
    public class GetCharacterDto
    {
         public int Id { get; set; }
        public string? Name { get; set; }
        public int Hitpoints { get; set; }
        public int Strength { get; set; }
        public int Defense { get; set; }
        public float Intelligence { get; set; }
        public RpgClass Class { get; set; }
        public GetWeaponDto? Weapon { get; set; }
        public List<GetSkillDto>? Skills { get; set; }
        public int Fights { get; set; }
        public int Victories { get; set; }
        public int Defeats { get; set; }
    }
}