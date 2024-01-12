using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using web_api2.DOTs.Weapon;

namespace web_api2.Services.WeaponService
{
    public interface IWeaponService
    {
        Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon);
    }
}