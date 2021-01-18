using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using TAAS.Domain;

namespace TAAS.Repository
{
    public class TaasContext : DbContext
    {
        public TaasContext(DbContextOptions<TaasContext> opt) : base(opt) {}

        public DbSet<MensagemDia> MensagesDias { get; set; }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(
           bool acceptAllChangesOnSuccess,
           CancellationToken cancellationToken = default(CancellationToken)
        )
        {
            OnBeforeSaving();
            return (await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken));
        }

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            var utcNow = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                if (entry.Entity is MensagemDia trackable)
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            trackable.DtModificado = utcNow;

                            // mark property as "don't touch" we don't want to update on a Modify operation
                            entry.Property("DtCadastro").IsModified = false;
                            break;

                        case EntityState.Added:
                            trackable.DtCadastro = utcNow;
                            trackable.DtModificado = utcNow;
                            break;
                    }
                }
            }
        }
    }
}
