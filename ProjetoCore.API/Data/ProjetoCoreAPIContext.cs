using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjetoCore.API.Models;

namespace ProjetoCore.API.Data
{
    public class ProjetoCoreAPIContext : DbContext
    {
        public ProjetoCoreAPIContext (DbContextOptions<ProjetoCoreAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movie { get; set; }
        public DbSet<Genre> Genre { get; set; }
    }
}
