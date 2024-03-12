using System;
using System.IO;
using IpcViaGrpcDemo.Server.Services;
using IpcViaGrpcDemo.Shared;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IpcViaGrpcDemo.Server
{
	public static class Program
	{
		private static readonly ILog Log = LogManager.GetLogger("IpcViaGrpcDemo.Server");

		public static int Main(string[] args)
		{
			try
			{
				XmlConfigurator.Configure(new FileInfo("log4net.config"));

				Log.Info("IPC Server has started");

				var builder = WebApplication.CreateBuilder(args);

				builder.Logging
					.ClearProviders()
					.AddLog4Net();

				builder.WebHost.ConfigureKestrel(serverOptions =>
				{
					serverOptions.ListenNamedPipe(Constants.PipeName, listenOptions =>
					{
						listenOptions.Protocols = HttpProtocols.Http2;
					});
				});

				var services = builder.Services;
				services.AddGrpc();

				var app = builder.Build();

				app.MapGrpcService<GreeterService>();

				app.Run();

				Log.Info(".NET Core Server has finished");

				return 0;
			}
#pragma warning disable CA1031 // Do not catch general exception types
			catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
			{
				Log.Fatal(e);
				return e.HResult;
			}
		}
	}
}
