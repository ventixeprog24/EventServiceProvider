using Grpc.Net.Client;
using TestClient;

var channel = GrpcChannel.ForAddress("https://localhost:7026");

var client = new EventContract.EventContractClient(channel);

var eventsReply = await client.GetEventsAsync(new GetEventsRequest());
foreach (var ev in eventsReply.Events)
{
    Console.WriteLine($"EventId: {ev.EventId}, EventTitle: {ev.EventTitle}, Description: {ev.Description}");
}

Console.ReadKey();