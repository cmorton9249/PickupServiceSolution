using Grpc.Core;
using Grpc.Net.Client;
using PickupService;
using System;
using System.Threading.Tasks;

namespace TestClient
{
	class Program
	{
		static async Task Main(string[] args)
		{
			Console.WriteLine("Calling a gRPC Service");
			Console.WriteLine("Hit Enter to call the service!");
			Console.ReadLine();

			using var channel = GrpcChannel.ForAddress("https://localhost:5001");

			var client = new Greeter.GreeterClient(channel);
			var reply = await client.SayHelloAsync(new HelloRequest { Name = "Chris" });

			Console.WriteLine($"Got a response - {reply.Message}");
			Console.ReadLine();

			//Console.WriteLine("Hit enter to get directions...");
			//Console.ReadLine();

			//var clientRouting = new TurnByTurn.TurnByTurnClient(channel);
			//var destination = new Destination { DestinationName = "Taco Bell" };
			//var replyRoute = clientRouting.StartGuidance(destination);

			//await foreach(var step in replyRoute.ResponseStream.ReadAllAsync())
			//{
			//	Console.WriteLine(step);
			//}

			Console.ReadLine();

			var estimatorClient = new PickupEstimator.PickupEstimatorClient(channel);

			var pickupRequest = new PickupRequest
			{
				For = "Chris",
			};
			pickupRequest.Items.AddRange(new int[] { 1, 2, 3, 4 });

			var estimatorResponse = await estimatorClient.GetPickupTimeAsync(pickupRequest);
			Console.WriteLine($"Order will be ready on {estimatorResponse.PickupTime.ToDateTime().ToShortDateString()}");
		}
	}
}
