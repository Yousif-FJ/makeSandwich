using Microsoft.AspNetCore.SignalR;

namespace server_a.RealTime;

// This class is used to send real-time updates to the clients.
// It is empty because we don't need to send any updates from the client to the server.
// We only need to send updates from the server to the client using hub context in OrderStatusUpdater.
public class OrderStatusHub : Hub
{
}