using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web_api2.DOTs.Character
{
    public class UpdateCharacterDto
    {
        internal int strength;
        
        public int Id { get; set; }
        public string? Name { get; set; } = "Tle";
        public int Hitpoints { get; set; } = 100;
        public int Strength { get; set; } = 10;
        public int Defense { get; set; } = 10;
        public float Intelligence { get; set; } = 10;
        public RpgClass Class { get; set; } = RpgClass.Assassin;
    }
}