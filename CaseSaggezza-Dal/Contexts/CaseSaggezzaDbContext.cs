using CaseSaggezza_Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseSaggezza_Dal.Contexts
{
    public partial class CaseSaggezzaDbContext : DbContext
    {
        public CaseSaggezzaDbContext(DbContextOptions<CaseSaggezzaDbContext> options) : base(options)
        {

        }

        public virtual DbSet<Fornecedor> Fornecedores { get; set; }
        public virtual DbSet<Produto> Produtos { get; set; }
        public virtual DbSet<Entrega> Entregas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("CaseSaggezza");

            modelBuilder.Entity<Fornecedor>(entity => {
                entity.HasKey(x => x.Id);
            });

            modelBuilder.Entity<Produto>(entity => {
                entity.HasKey(x => x.Id);
            });

            modelBuilder.Entity<Entrega>(entity => {
                entity.HasKey(x => x.Id);

                entity.HasMany(x => x.Produtos)
                      .WithMany(y => y.Entregas)
                      .UsingEntity(z => z.ToTable("EntregaProduto"));

                entity.HasMany(x => x.Fornecedores)
                      .WithMany(y => y.Entregas)
                      .UsingEntity(z => z.ToTable("EntregaFornecedor"));
            });


            //modelBuilder.Entity<Pedido>(entity => {
            //    entity.HasKey(x => x.Id);

            //    entity.HasOne(x => x.Cliente)
            //            .WithMany()
            //            .HasForeignKey(x => x.ClienteId)
            //            .IsRequired();
            //});

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
