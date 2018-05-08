using System;
using System.Collections.Generic;
using System.Text;

namespace Article.Domain
{
	public class ArtikelKategorie
	{
		public int ArtikelNummer { get; internal set; }
		public string Name { get; internal set; }

		public Artikel Artikel { get; internal set; }
	}
}
