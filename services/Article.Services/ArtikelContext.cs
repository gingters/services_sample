using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Article.Domain;
using Microsoft.EntityFrameworkCore;

namespace Article.Services
{
	public class ArtikelContext : DbContext
	{
		public DbSet<Artikel> Artikel { get; set; }
		public DbSet<ArtikelKategorie> ArtikelKategorien { get; set; }

		public ArtikelContext(DbContextOptions<ArtikelContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Artikel>()
				.HasKey(a => a.ArtikelNummer);
			modelBuilder.Entity<Artikel>()
				.Property(a => a.Bezeichnung)
				.IsRequired();

			modelBuilder.Entity<ArtikelKategorie>()
				.HasKey(k => new { k.ArtikelNummer, k.Name });
			modelBuilder.Entity<ArtikelKategorie>()
				.Property(k => k.Name)
				.IsRequired();

			modelBuilder.Entity<ArtikelKategorie>()
				.HasOne(k => k.Artikel)
				.WithMany(a => a.Kategorien)
				.HasForeignKey(k => k.ArtikelNummer)
				.HasConstraintName("FK_Kategorie_Artikel");

			modelBuilder.Entity<Artikel>()
				.HasData(new {ArtikelNummer = 1701, Bezeichnung = "Plüscheinhorn"});
			modelBuilder.Entity<ArtikelKategorie>()
				.HasData(
					new {ArtikelNummer = 1701, Name = "groß"},
					new {ArtikelNummer = 1701, Name = "mittel"},
					new {ArtikelNummer = 1701, Name = "klein"}
				);


			base.OnModelCreating(modelBuilder);
		}
	}
}
