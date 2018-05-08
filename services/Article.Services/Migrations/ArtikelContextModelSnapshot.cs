﻿// <auto-generated />
using Article.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Article.Services.Migrations
{
    [DbContext(typeof(ArtikelContext))]
    partial class ArtikelContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.0-rc1-32029")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Article.Domain.Artikel", b =>
                {
                    b.Property<int>("ArtikelNummer")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Bezeichnung")
                        .IsRequired();

                    b.HasKey("ArtikelNummer");

                    b.ToTable("Artikel");

                    b.HasData(
                        new { ArtikelNummer = 1701, Bezeichnung = "Plüscheinhorn" }
                    );
                });

            modelBuilder.Entity("Article.Domain.ArtikelKategorie", b =>
                {
                    b.Property<int>("ArtikelNummer");

                    b.Property<string>("Name");

                    b.HasKey("ArtikelNummer", "Name");

                    b.ToTable("ArtikelKategorien");

                    b.HasData(
                        new { ArtikelNummer = 1701, Name = "groß" },
                        new { ArtikelNummer = 1701, Name = "mittel" },
                        new { ArtikelNummer = 1701, Name = "klein" }
                    );
                });

            modelBuilder.Entity("Article.Domain.ArtikelKategorie", b =>
                {
                    b.HasOne("Article.Domain.Artikel", "Artikel")
                        .WithMany("Kategorien")
                        .HasForeignKey("ArtikelNummer")
                        .HasConstraintName("FK_Kategorie_Artikel")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
