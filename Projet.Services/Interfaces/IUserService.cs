using Microsoft.Extensions.Configuration;
using Projet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> RegisterAsync(User user);
        Task<User?> LoginAsync(string username, string password);

        public string GenerateJwtToken(User user, IConfiguration configuration);
    }
}
