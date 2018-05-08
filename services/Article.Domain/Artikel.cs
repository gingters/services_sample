using System;
using System.Collections.Generic;
using System.Text;

namespace Article.Domain
{
	public class Artikel
	{		
		public int ArtikelNummer { get; private set; }
		public string Bezeichnung { get; private set; }

		public HashSet<ArtikelKategorie> Kategorien { get; private set; } = new HashSet<ArtikelKategorie>();
	}
}
