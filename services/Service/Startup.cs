using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Article.Domain;
using Article.Services;
using AutoMapper;
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

			services.AddSingleton<IMapper>(ctx => mapperCOnfig.CreateMapper());

			services.AddDbContext<ArtikelContext>(options => options
				.UseSqlServer(Configuration.GetConnectionString("sqlserver")));
			services.AddScoped<IArtikelRepository, ArtikelRepository>();

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.Authority = Configuration.GetSection("IdentityServer").GetValue<string>("Url");
					options.Audience = Configuration.GetSection("IdentityServer").GetValue<string>("Audience");
					options.RequireHttpsMetadata = false; // Do not use in Production!!!
				});

			services.AddResponseCompression();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
				{ 
					// Probably not good for PROD, should use a separate tool for that ;)
					scope.ServiceProvider.GetRequiredService<ArtikelContext>()
						.Database.Migrate();
				}

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
		}
	}
}
