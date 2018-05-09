using System;

namespace Article.Domain
{
	public class ArtikelException : Exception
	{
		public ArtikelException(string message)
			: base(message)
		{

		}
	}
}
