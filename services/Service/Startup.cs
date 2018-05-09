using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Article.Domain;
using Article.Services;
using AutoMapper;
using Domain.Abstractions;
using Domain.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Service.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace Service
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			var mapperCOnfig = new MapperConfiguration(c =>
			{	// Todo: Move this to a Mapping profile
				c.CreateMap<Artikel, ArtikelViewModel>();
				c.CreateMap<ArtikelKategorie, ArtikelKategorieViewModel>();
			});
			services.AddScoped<ArtikelCommandHandler>();
			services.AddSingleton<IMapper>(ctx => mapperCOnfig.CreateMapper());
			services.AddScoped<IAggregateFactory, AggregateFactory>();
			services.AddSingleton<IEventStore, EventStore>();
			services.AddScoped<IEventDispatcher<Artikel>, TypeBasedEventDispatcher<Artikel>>();
			//services.AddDbContext<ArtikelContext>(options => options
			//	.UseSqlServer(Configuration.GetConnectionString("sqlserver")));
			services.AddScoped<IArtikelRepository, ArtikelRepository>();

			services.AddMvc(options => options.RespectBrowserAcceptHeader = true)
				.AddXmlSerializerFormatters()
				.SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.Authority = Configuration.GetSection("IdentityServer").GetValue<string>("Url");
					options.Audience = Configuration.GetSection("IdentityServer").GetValue<string>("Audience");
					options.RequireHttpsMetadata = false; // Do not use in Production!!!
				});

			services.AddResponseCompression();
			services.AddSwaggerGen(c =>
				{
					c.SwaggerDoc("v1", new Info()
					{
						Contact = new Contact() {  Name = "Ich", Email = "ich@email.de"},
						Title = "Artikel API",
						Version = "v1",
					});
					c.IncludeXmlComments(Path.Combine(System.AppContext.BaseDirectory, "Service.xml"));
					c.AddSecurityDefinition("oauth2", new OAuth2Scheme()
					{
						Type = "oauth2",
						Flow = "password",
						TokenUrl = $"{Configuration.GetSection("IdentityServer").GetValue<string>("Url")}/connect/token",
						Scopes = new Dictionary<string, string>()
						{
							{ "service", "Access to the Article API" },
						},
					});
					c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>()
					{
						{ "oauth", new [] { "service" } }
					});
				});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				//using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
				//{ 
				//	// Probably not good for PROD, should use a separate tool for that ;)
				//	scope.ServiceProvider.GetRequiredService<ArtikelContext>()
				//		.Database.Migrate();
				//}

				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseHsts();
				app.UseHttpsRedirection();
			}


			app.UseResponseCompression();
			app.UseCors(builder => builder
				.AllowAnyHeader()
				.AllowAnyMethod()
				.AllowAnyOrigin()
				.AllowCredentials());

			app.UseAuthentication();
			app.UseMvc();
			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "ArtikelAPI v1");
				c.OAuthClientId("guiclient");
				c.OAuthClientSecret("guisecret");
				c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
			});

		}
	}
}
