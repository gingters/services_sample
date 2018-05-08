using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Service
{
	public class Program
	{
		public static void Main(string[] args)
		{
			using (var host = CreateWebHostBuilder(args).Build())
			{
				var config = host.Services.GetService<IConfiguration>();

				Log.Logger = new LoggerConfiguration()
					.Enrich.FromLogContext()
					.Enrich.WithThreadId()
					.Enrich.WithProcessId()
					.Enrich.WithProcessName()
					.Enrich.WithAssemblyName()
					.Enrich.WithMachineName()
					.Enrich.WithProperty("Application", "ArticleService")
					.ReadFrom.Configuration(config)
					.WriteTo.Console()
					.WriteTo.Seq(config.GetValue<string>("seqUrl"))
					.CreateLogger();

				try
				{
					host.Run();
				}
				finally
				{
					Log.CloseAndFlush();
				}
			}
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseSerilog()
				.UseStartup<Startup>();
	}
}
