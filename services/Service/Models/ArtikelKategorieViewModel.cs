namespace Service.Models
{
	/// <summary>
	/// A catagory that specifies an article
	/// </summary>
	public class ArtikelKategorieViewModel
	{
		/// <summary>
		/// Determines to which article this category belongs
		/// </summary>
		public int ArtikelNummer { get; set; }
		/// <summary>
		/// An dexcription of this article category
		/// </summary>
		public string Name { get; set; }
		
	}
}