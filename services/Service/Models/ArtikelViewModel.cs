using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Article.Domain;

namespace Service.Models
{
	public class ArtikelViewModel
	{
		public int ArtikelNummer { get; set; }
		public string Bezeichnung { get; set; }
		public List<ArtikelKategorieViewModel> Kategorien { get; set; }
	}
}
