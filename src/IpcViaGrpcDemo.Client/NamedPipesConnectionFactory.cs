using System.IO;
using System.IO.Pipes;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using IpcViaGrpcDemo.Shared;

namespace IpcViaGrpcDemo.Client
{
	internal static class NamedPipesConnectionFactory
	{
		public static async ValueTask<Stream> ConnectAsync(SocketsHttpConnectionContext context, CancellationToken cancellationToken)
		{
			var clientStream = new NamedPipeClientStream(
				serverName: ".",
				pipeName: Constants.PipeName,
				direction: PipeDirection.InOut,
				options: PipeOptions.WriteThrough | PipeOptions.Asynchronous,
				impersonationLevel: TokenImpersonationLevel.Anonymous);

			try
			{
				await clientStream.ConnectAsync(cancellationToken).ConfigureAwait(false);
				return clientStream;
			}
			catch
			{
				await clientStream.DisposeAsync();
				throw;
			}
		}
	}
}
