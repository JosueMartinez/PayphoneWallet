using Microsoft.EntityFrameworkCore;
using PayphoneWallet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayphoneWallet.Infraestructure.Data
{
    public class WalletDbContext : DbContext
    {
        public WalletDbContext(DbContextOptions<WalletDbContext> options) : base(options)
        {
        }

        #region Entites
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        #endregion

        #region FLuent API

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de Wallet
            modelBuilder.Entity<Wallet>(entity =>
            {
                entity.HasKey(w => w.Id);
                entity.Property(w => w.Name).IsRequired();
                entity.Property(w => w.Balance).HasColumnType("decimal(18,2)");
                entity.Property(w => w.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(w => w.UpdatedAt).HasDefaultValueSql("GETDATE()");
            });

            // Configuración de Transaction
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.SourceWallet)
                .WithMany()
                .HasForeignKey(t => t.SourceWalletId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.DestinationWallet)
                .WithMany()
                .HasForeignKey(t => t.DestinationWalletId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        #endregion
    }
}
