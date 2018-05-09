using System;
using System.Collections.Generic;
using System.Text;

namespace Article.Domain
{
	public class ArtikelNeuanlageCommand
	{
		public string Artikelbezeichnung { get; set; }
		public int Artikelnummer { get; set; }
		public IEnumerable<string> Kategorien { get; set; } = new string[0];

	}
}
