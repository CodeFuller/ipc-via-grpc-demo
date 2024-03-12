using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Net.Client;
using log4net;
using log4net.Config;

namespace IpcViaGrpcDemo.Client
{
	public static class Program
	{
		private static readonly ILog Log = LogManager.GetLogger("IpcViaGrpcDemo.Client");

		public static async Task<int> Main()
		{
			try
			{
				XmlConfigurator.Configure(new FileInfo("log4net.config"));

				Log.Info("IPC Client has started");

				Log.Info("Creating channel ...");
				using var channel = CreateChannel();
				var client = new Greeter.GreeterClient(channel);

				var request = new HelloRequest
				{
					Name = "CodeFuller",
				};

				Log.Info("Sending request ...");
				var response = await client.SayHelloAsync(request);
				Log.Info($"Got Response: '{response.Message}'");

				Log.Info(".NET Core Client has finished");

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

		private static GrpcChannel CreateChannel()
		{
			return GrpcChannel.ForAddress("http://localhost", new GrpcChannelOptions
			{
				HttpHandler = new SocketsHttpHandler
				{
					ConnectCallback = NamedPipesConnectionFactory.ConnectAsync,
				},
			});
		}
	}
}
