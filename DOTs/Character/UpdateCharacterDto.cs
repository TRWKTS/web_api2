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
        public string? Name { get; set; }
        public int Hitpoints { get; set; }
        public int Strength { get; set; }
        public int Defense { get; set; }
        public int Intelligence { get; set; }
        public RpgClass Class { get; set; }
    }
}