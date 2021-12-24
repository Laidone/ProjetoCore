using Dominio;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra
{
    public class ProjetoCoreAPIContext : DbContext
    {
        public ProjetoCoreAPIContext(DbContextOptions<ProjetoCoreAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movie { get; set; }
        public DbSet<Genre> Genre { get; set; }
    }
}
