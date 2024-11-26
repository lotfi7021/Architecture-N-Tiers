using Microsoft.EntityFrameworkCore;
using Projet.Entities;
using System.Reflection.Metadata;

namespace Projet.Context
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public virtual DbSet<User> User { get; set; }

        public virtual DbSet<Client> Client { get; set; }

    }
}