using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Article.Domain;
using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Article.Services
{
	public class ArtikelRepository : IArtikelRepository
	{
		private readonly ILogger<ArtikelRepository> _logger;

		private readonly IAggregateFactory _factory;
		private readonly IEventStore _store;

		public ArtikelRepository(ILogger<ArtikelRepository> logger, IAggregateFactory factory, IEventStore store)
		{
			_logger = logger;
			_factory = factory;
			_store = store;
		}

		public Artikel LadeArtikelMitKategorien(int artikelNummer)
		{
			// Todo:
			// Try to load a previous version from another store (i.e. db)
			// if we have one, only get new events starting from the version of the entity from the other store
			// var article = _dbContext.Articles.Find(artikelNummer)...

			var events = _store.Get(artikelNummer); // , article?.Version ?? 0
			if (!events.Any())
				return null;

			// if we already got the entity from the other store, do not create via factory...
			// var article = article ?? _factory.CreateEntity<Artikel>(artikelNummer);
			var article = _factory.CreateEntity<Artikel>(artikelNummer);
			foreach (var evt in events)
			{
				article.ApplyEvent(evt);
			}

			_logger?.LogInformation("Artikel {ArtikelNummer} wurde geladen: {ArtikelBezeichnung}", artikelNummer, article.Bezeichnung);

			article.EventRaised += (s, e) => { _store.Store(e.Event); };
			
			return article.ArtikelNummer > 0 ? article : null;
		}

		public IEnumerable<Artikel> LadeAlleArtikel()
		{
			// return _context.Artikel.Include(a => a.Kategorien).ToArray();

			var liste = new List<Artikel>();

			// read all Article-Events from store
			foreach (var evt in _store.GetAllEvents())
			{
				var artikel = liste.FirstOrDefault(a => a.ArtikelNummer == evt.AggregateId);
				if (artikel == null)
				{
					artikel = _factory.CreateEntity<Artikel>(evt.AggregateId);
					liste.Add(artikel);
				}

				artikel.ApplyEvent(evt);
			}

			// until here was replay, from now on the entity lives
			foreach (var artikel in liste)
			{
				artikel.EventRaised += (s, e) => { _store.Store(e.Event); };
			}

			return liste.Where(a => a.ArtikelNummer > 0);
		}

		public IEnumerable<Artikel> LadeArtikelViaKatgeorie(string kategorieName)
		{
			var artikel = LadeAlleArtikel()
				.Where(a => a.Kategorien.Any(k => k.Name == kategorieName))
				.ToArray();

			_logger?.LogInformation("Es wurden {ArtikelAnzahl} Artikel mit der Kategorie {KategorieName} gefunden.", artikel.Length, kategorieName);

			return artikel;
		}

		public Artikel CreateNew(int artikelNummer)
		{
			// Move to factory
			var artikel = _factory.CreateEntity<Artikel>(artikelNummer);

			// This is the 'Store change to repo'
			artikel.EventRaised += (s, e) => { _store.Store(e.Event); };
			return artikel;
		}

		//public void SpeichereNeuenArtikel(Artikel artikel)
		//{
		//	_context.Artikel.Add(artikel);
		//	_context.SaveChanges();
		//}
	}
}
