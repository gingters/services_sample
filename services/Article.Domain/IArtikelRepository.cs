using System;
using System.Collections.Generic;
using System.Text;

namespace Article.Domain
{
	public interface IArtikelRepository
	{
		Artikel LadeArtikelMitKategorien(int artikelNummer);
		IEnumerable<Artikel> LadeAlleArtikel();
		IEnumerable<Artikel> LadeArtikelViaKatgeorie(string kategorieName);
		// void SpeichereNeuenArtikel(Artikel artikel);
		Artikel CreateNew(int artikelNummer);
	}
}
