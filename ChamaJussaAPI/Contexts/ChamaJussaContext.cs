using System;
using System.Collections.Generic;
using ChamaJussaAPI.Domains;
using Microsoft.EntityFrameworkCore;

namespace ChamaJussaAPI.Contexts;

public partial class ChamaJussaContext : DbContext
{
    public ChamaJussaContext()
    {
    }

    public ChamaJussaContext(DbContextOptions<ChamaJussaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Fila> Fila { get; set; }

    public virtual DbSet<Localizacao> Localizacao { get; set; }

    public virtual DbSet<OrdemDeServico> OrdemDeServico { get; set; }

    public virtual DbSet<StatusOS> StatusOS { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=ChamaJussa;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Fila>(entity =>
        {
            entity.HasKey(e => e.FilaID).HasName("PK__Fila__6E0F8A5900A0F597");

            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Localizacao>(entity =>
        {
            entity.HasKey(e => e.LocalizacaoID).HasName("PK__Localiza__83ABDECA5EB382E9");

            entity.Property(e => e.Andar)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<OrdemDeServico>(entity =>
        {
            entity.HasKey(e => e.OSID).HasName("PK__OrdemDeS__AEE4ACDD9FD63AD2");

            entity.Property(e => e.DTCriacao).HasColumnType("datetime");
            entity.Property(e => e.Descricao).HasMaxLength(255);
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Fila).WithMany(p => p.OrdemDeServico)
                .HasForeignKey(d => d.FilaID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Fila");

            entity.HasOne(d => d.Localizacao).WithMany(p => p.OrdemDeServico)
                .HasForeignKey(d => d.LocalizacaoID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Localizacao");

            entity.HasOne(d => d.Status).WithMany(p => p.OrdemDeServico)
                .HasForeignKey(d => d.StatusID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Status");

            entity.HasOne(d => d.Usuario).WithMany(p => p.OrdemDeServico)
                .HasForeignKey(d => d.UsuarioID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Solicitante");
        });

        modelBuilder.Entity<StatusOS>(entity =>
        {
            entity.HasKey(e => e.StatusID).HasName("PK__StatusOS__C8EE20438488CBD9");

            entity.Property(e => e.Nome)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioID).HasName("PK__Usuario__2B3DE79833B81785");

            entity.HasIndex(e => e.Email, "UQ__Usuario__A9D105343089037C").IsUnique();

            entity.HasIndex(e => e.NIF, "UQ__Usuario__C7DEC330B841448E").IsUnique();

            entity.Property(e => e.UsuarioID).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.Nome).HasMaxLength(100);
            entity.Property(e => e.Senha).HasMaxLength(32);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
