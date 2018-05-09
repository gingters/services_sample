using System;
using System.Collections.Generic;
using System.Text;
using Article.Domain;
using Domain.Abstractions;
using Domain.Services;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Art = Article.Domain.Artikel;

namespace Artikel.UnitTests.ArtikelCommandHandler
{
	[TestClass]
	public class Handle
	{
		[TestMethod]
		public void Should_Throw_On_Existing_Article()
		{
			// Arrange
			var cmd = new ArtikelNeuanlageCommand() { Artikelnummer = 1 };

			var repoMock = new Mock<IArtikelRepository>(MockBehavior.Strict);
			repoMock.Setup(r => r.LadeArtikelMitKategorien(1))
				.Returns(new Art(null, 1));

			var subject = new Article.Services.ArtikelCommandHandler(repoMock.Object);

			// Act
			Action action = () => subject.Handle(cmd);

			// Assert
			action.Should().Throw<InvalidOperationException>();
			repoMock.Verify(r => r.LadeArtikelMitKategorien(1), Times.Once);
		}

		[TestMethod]
		public void Should_Store_New_Article()
		{
			// Arrange
			IEvent evt = null;
			var newArtikel = new Art(new TypeBasedEventDispatcher<Art>(null), 1);
			newArtikel.EventRaised += (s, e) => evt = e.Event; 

			var cmd = new ArtikelNeuanlageCommand() { Artikelnummer = 1, Artikelbezeichnung = "Dummy" };

			var repoMock = new Mock<IArtikelRepository>(MockBehavior.Strict);
			repoMock.Setup(r => r.LadeArtikelMitKategorien(1))
				.Returns(default(Art));
			repoMock.Setup(r => r.CreateNew(1)).Returns(newArtikel);


			var subject = new Article.Services.ArtikelCommandHandler(repoMock.Object);

			// Act
			subject.Handle(cmd);

			// Assert
			repoMock.Verify(r => r.LadeArtikelMitKategorien(1), Times.Once);
			repoMock.Verify(r => r.CreateNew(1), Times.Once);

			newArtikel.Bezeichnung.Should().Be("Dummy");
			evt.Should().NotBeNull();
			evt.AggregateId.Should().Be(1);
			evt.Should().BeAssignableTo<ArtikelAngelegtEvent>();
			(evt as ArtikelAngelegtEvent).Artikelbezeichnung.Should().Be("Dummy");
		}
	}
}
