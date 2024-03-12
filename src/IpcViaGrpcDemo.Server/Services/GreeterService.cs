using System.Threading.Tasks;
using Grpc.Core;

namespace IpcViaGrpcDemo.Server.Services
{
	public class GreeterService : Greeter.GreeterBase
	{
		public override Task<HelloResponse> SayHello(HelloRequest request, ServerCallContext context)
		{
			var response = new HelloResponse
			{
				Message = $"Hello, {request.Name}!",
			};

			return Task.FromResult(response);
		}
	}
}
