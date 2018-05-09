using System;
using System.Collections.Generic;
using System.Text;
using Article.Domain;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Art = Article.Domain.Artikel;

namespace Artikel.UnitTests.ArtikelCommandHandler
{
	[TestClass]
	public class Handle
	{
		//[TestMethod]
		//public void Should_Throw_On_Existing_Article()
		//{
		//	// Arrange
		//	var cmd = new ArtikelNeuanlageCommand() { Artikelnummer = 1 };

		//	var repoMock = new Mock<IArtikelRepository>(MockBehavior.Strict);
		//	repoMock.Setup(r => r.LadeArtikelMitKategorien(1))
		//		.Returns(new Art(null, 1));

		//	var subject = new Article.Services.ArtikelCommandHandler(repoMock.Object);

		//	// Act
		//	Action action = () => subject.Handle(cmd);

		//	// Assert
		//	action.Should().Throw<InvalidOperationException>();
		//	repoMock.Verify(r => r.LadeArtikelMitKategorien(1), Times.Once);
		//}

		//[TestMethod]
		//public void Should_Store_New_Article()
		//{
		//	// Arrange
		//	var cmd = new ArtikelNeuanlageCommand() { Artikelnummer = 1, Artikelbezeichnung = "Dummy"};

		//	var repoMock = new Mock<IArtikelRepository>(MockBehavior.Strict);
		//	repoMock.Setup(r => r.LadeArtikelMitKategorien(1))
		//		.Returns(default(Art));
		//	repoMock.Setup(r => r.SpeichereNeuenArtikel(It.IsAny<Art>()));

		//	var subject = new Article.Services.ArtikelCommandHandler(repoMock.Object);

		//	// Act
		//	subject.Handle(cmd);

		//	var artikel = new Art(null, 1);

		//	artikel.ArtikelNummer.Should().Be(0);
		//	artikel.Bezeichnung.Should().BeNull();

		//	// Assert
		//	repoMock.Verify(r => r.LadeArtikelMitKategorien(1), Times.Once);
		//	repoMock.Verify(r => r.SpeichereNeuenArtikel(It.IsAny<Art>()), Times.AtLeastOnce);

		//}
	}
}
