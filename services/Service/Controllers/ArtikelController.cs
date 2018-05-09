﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Article.Domain;
using Article.Services;
using AutoMapper;
using Domain.Abstractions;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Service.Controllers
{
	[Route("api/[controller]")]
	public class ArtikelController : ControllerBase
	{
		private readonly IArtikelRepository _artikelRepo;
		private readonly IMapper _mapper;
		private readonly ArtikelCommandHandler _handler;
		private readonly IEventStore _eventStore;
		private readonly IAggregateFactory _factory;

		public ArtikelController(IArtikelRepository artikelRepo, IMapper mapper, ArtikelCommandHandler handler, IEventStore eventStore, IAggregateFactory factory)
		{
			_artikelRepo = artikelRepo ?? throw new ArgumentNullException(nameof(artikelRepo));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_handler = handler ?? throw new ArgumentNullException(nameof(handler));
			_eventStore = eventStore;
			_factory = factory;
		}

		[HttpGet]
		public IActionResult GetAll()
		{
			var result = _mapper.Map<List<ArtikelViewModel>>(_artikelRepo.LadeAlleArtikel());
			return Ok(result);
		}

		/// <summary>
		/// Gets a specified article by its ArtikelNummer
		/// </summary>
		/// <param name="artikelNummer">The id of the article to fetch</param>
		/// <returns>An article, or Not Found</returns>
		[HttpGet("{artikelNummer}")]
		[SwaggerResponse(200, typeof(ArtikelViewModel))]
		[SwaggerResponse(404)]
		[SwaggerResponse(401)]
		[Produces("application/json", "application/xml")]
		public IActionResult GetById(int artikelNummer)
		{
			var result = _mapper.Map<ArtikelViewModel>(_artikelRepo.LadeArtikelMitKategorien(artikelNummer));

			if (result == null)
				return NotFound();

			return Ok(_mapper.Map<ArtikelViewModel>(result));
		}

		[HttpPost]
		public IActionResult AddNew([FromBody] ArtikelViewModel newArticle)
		{
			var command = new ArtikelNeuanlageCommand()
			{
				Artikelnummer = newArticle.ArtikelNummer,
				Artikelbezeichnung = newArticle.Bezeichnung,
			};

			if (newArticle.Kategorien != null)
			{
				command.Kategorien = newArticle.Kategorien.Select(k => k.Name).ToArray();
			}

			try
			{
				_handler.Handle(command);
			}
			catch (ArtikelException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception)
			{
				return new StatusCodeResult(500);
			}

			return Ok();
		}
	}
}
