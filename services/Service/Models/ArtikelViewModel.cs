using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Article.Domain;

namespace Service.Models
{
	/// <summary>
	/// Represents an article that can be ordered
	/// </summary>
	public class ArtikelViewModel
	{
		/// <summary>
		/// The Id of the article
		/// </summary>
		public int ArtikelNummer { get; set; }
		/// <summary>
		/// A description of the article
		/// </summary>
		public string Bezeichnung { get; set; }
		/// <summary>
		/// A further specification of the article
		/// </summary>
		public List<ArtikelKategorieViewModel> Kategorien { get; set; }
	}
}
