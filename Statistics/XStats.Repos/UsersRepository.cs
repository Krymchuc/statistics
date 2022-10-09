using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XStats.Core;
using XStats.Repos.Dto;

namespace XStats.Repos
{
    public class UsersRepository
    {
        private readonly XStatsContext _ctx;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UsersRepository(XStatsContext ctx, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _ctx = ctx;
            _userManager = userManager;
            _roleManager= roleManager;
        }
        public async Task<IEnumerable<UserReadDto>> GetUsersAsync()
        {
            var users = new List<UserReadDto>();
            foreach(var u in _ctx.Users.ToList())
            {
                var userDto = new UserReadDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    FullName = $"{u.LastName} {u.FirstName}",
                    IsConfirmed = u.EmailConfirmed,
                    Roles = new List<IdentityRole>()
                };

                foreach(var role in await _userManager.GetRolesAsync(u))
                {
                    userDto.Roles.Add(await _ctx.Roles.FirstAsync(x => x.Name.ToLower() == role.ToLower()));
                }

                users.Add(userDto);
            }

            return users;
        }
    }
}
