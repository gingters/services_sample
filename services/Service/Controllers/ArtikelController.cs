using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Article.Domain;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Models;

namespace Service.Controllers
{
	[Route("api/[controller]")]
	public class ArtikelController : ControllerBase
	{
		private readonly IArtikelRepository _artikelRepo;
		private readonly IMapper _mapper;

		public ArtikelController(IArtikelRepository artikelRepo, IMapper mapper)
		{
			_artikelRepo = artikelRepo ?? throw new ArgumentNullException(nameof(artikelRepo));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		[HttpGet]
		public IActionResult GetAll()
		{
			var result = _mapper.Map<List<ArtikelViewModel>>(_artikelRepo.LadeAlleArtikel());
			return Ok(result);
		}

		[HttpGet("{artikelNummer}")]

		public IActionResult GetById(int artikelNummer)
		{
			var result = _mapper.Map<ArtikelViewModel>(_artikelRepo.LadeArtikelMitKategorien(artikelNummer));
			return Ok(result);
		}
	}
}
