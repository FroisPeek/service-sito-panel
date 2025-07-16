using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ServiceSitoPanel.Helpers;
using ServiceSitoPanel.src.model;

namespace leapcert_back.src.context
{
    public class ApplicationDbContext : DbContext
    {
        private readonly JwtService _jwtService;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, JwtService jwtService) : base(options)
        {
            _jwtService = jwtService;
        }

        public DbSet<Users> users { get; set; }
        public DbSet<Tenant> tenant { get; set; }
        public DbSet<Profile> profiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var tenant = _jwtService.GetTenantFromToken();

            // modelBuilder.Entity<Users>().HasQueryFilter(c => // ! tabela de user nao pode ter filter tenant
            // c.tenant_id == int.Parse(tenant));
        }
    }
}