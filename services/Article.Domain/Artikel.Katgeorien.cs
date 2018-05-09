using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Abstractions;
using Domain.Services;

namespace Article.Domain
{
	public class ArtikelKategorieAngelegtEvent : Event
	{
		// Todo: Events should be immutable. Set this via ctor
		public string Kategorie { get; set; }
	}
	
	public class ArtikelKategorieEntferntEvent : Event
	{
		// Todo: Events should be immutable. Set this via ctor
		public string Kategorie { get; set; }
	}

	public partial class Artikel
	{
		public void FuegeKategorieHinzu(string kategorie)
		{
			if (String.IsNullOrWhiteSpace(kategorie))
				throw new ArtikelException("Kategorie darf nicht leer sein.");

			if (Kategorien.Any(k => k.Name.Equals(kategorie, StringComparison.InvariantCultureIgnoreCase)))
				throw new ArtikelException("Kategorie existiert schon");

			var evt = new ArtikelKategorieAngelegtEvent()
			{
				TimeStamp = DateTime.UtcNow,
				Version = 1,
				Type = nameof(ArtikelKategorieAngelegtEvent),

				AggregateId = ArtikelNummer,
				Kategorie = kategorie,
			};

			ApplyEvent(evt);
			OnEventRaised(new EventRaisedEventArgs(evt));
		}

		private void Apply(ArtikelKategorieAngelegtEvent evt)
		{
			Kategorien.Add(new ArtikelKategorie() {Artikel = this, ArtikelNummer = ArtikelNummer, Name = evt.Kategorie});
		}

		public void EntferneKategorie(string kategorie)
		{
			if (String.IsNullOrWhiteSpace(kategorie))
				throw new ArtikelException("Kategorie darf nicht leer sein.");

			if (!Kategorien.Any(k => k.Name.Equals(kategorie, StringComparison.InvariantCultureIgnoreCase)))
				throw new ArtikelException("Kategorie existiert nicht");

			var evt = new ArtikelKategorieEntferntEvent()
			{
				TimeStamp = DateTime.UtcNow,
				Version = 1,
				Type = nameof(ArtikelKategorieEntferntEvent),

				AggregateId = ArtikelNummer,
				Kategorie = kategorie,
			};

			ApplyEvent(evt);
			OnEventRaised(new EventRaisedEventArgs(evt));
		}

		private void Apply(ArtikelKategorieEntferntEvent evt)
		{
			var kat = Kategorien.FirstOrDefault(k => k.Name.Equals(evt.Kategorie, StringComparison.InvariantCultureIgnoreCase));
			
			if (kat != null)
				Kategorien.Remove(kat);	
		}
	}
}
