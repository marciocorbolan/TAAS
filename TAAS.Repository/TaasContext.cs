using Microsoft.EntityFrameworkCore;
using System;
using TAAS.Domain;

namespace TAAS.Repository
{
    public class TaasContext : DbContext
    {
        public TaasContext(DbContextOptions<TaasContext> opt) : base(opt) {}

        public DbSet<MensagemDia> MensagesDias { get; set; }
    }
}
