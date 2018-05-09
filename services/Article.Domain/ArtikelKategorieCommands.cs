using System;
using System.Collections.Generic;
using System.Text;

namespace Article.Domain
{
	public class ArtikelKategorieHinzufuegenCommand
	{
		public int Artikelnummer { get; set; }
		public string Kategoriename { get; set; }

	}
	
	public class ArtikelKategorieEntfernenCommand
	{
		public int Artikelnummer { get; set; }
		public string Kategoriename { get; set; }

	}
}
