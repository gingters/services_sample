using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Article.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Article.Services
{
	public class ArtikelRepository : IArtikelRepository
	{
		private readonly ILogger<ArtikelRepository> _logger;
		private readonly ArtikelContext _context;

		public ArtikelRepository(ILogger<ArtikelRepository> logger, ArtikelContext context)
		{
			_logger = logger;
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public Artikel LadeArtikelMitKategorien(int artikelNummer)
		{
			var artikel = _context.Artikel
				.Include(a => a.Kategorien)
				.FirstOrDefault(a => a.ArtikelNummer == artikelNummer);

			if (artikel == null)
				return null;

			_logger?.LogInformation("Artikel {ArtikelNummer} wurde geladen: {ArtikelBezeichnung}", artikelNummer, artikel.Bezeichnung);
			return artikel;
		}

		public IEnumerable<Artikel> LadeAlleArtikel()
		{
			return _context.Artikel.Include(a => a.Kategorien).ToArray();
		}

		public IEnumerable<Artikel> LadeArtikelViaKatgeorie(string kategorieName)
		{
			var artikel = _context.Artikel
				.Include(a => a.Kategorien)
				.Where(a => a.Kategorien.Any(k => k.Name == kategorieName))
				.ToArray();

			_logger?.LogInformation("Es wurden {ArtikelAnzahl} Artikel mit der Kategorie {KategorieName} gefunden.", artikel.Length, kategorieName);

			return artikel;
		}
	}
}
