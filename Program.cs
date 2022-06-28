// See https://aka.ms/new-console-template for more information


// Get the parameter defining the server to query
using System.Diagnostics;

var serverIp = args.Length > 0 ? args[0] : null;
if(string.IsNullOrWhiteSpace(serverIp)) { EchoOptions(); return; }
QueryServer(serverIp);

void EchoOptions() 
{
    System.Console.WriteLine("Provide the server IP in the call arguments");
}

void QueryServer(string serverIp) 
{ 
    try
    {
        // Connect and retrieve
        TelnetClient tc = new TelnetClient(serverIp, 5555);
        var welcomeMessage = tc.Receive();
        Debug.WriteLine($"Connected: {welcomeMessage}");
        tc.StartSend("status");
        var receivedData = tc.Receive();
        Debug.WriteLine($"Received: {receivedData}");
        
        // Parse the information
        var connections = new OVPNStreamParser().GetConnectionsFromStream(receivedData);
        System.Console.WriteLine($"Found {connections.Count()} connections");
        foreach(var c in connections) 
        {
            System.Console.WriteLine($"*****");
            System.Console.WriteLine($"Common Name:     {c.CommonName}");
            System.Console.WriteLine($"Origin:          {c.RealAdress}");
            System.Console.WriteLine($"Sent/Received:   {c.BytesSent}/{c.BytesReceived}");
            System.Console.WriteLine($"Connected since: {c.ConnectedSince}");
            System.Console.WriteLine($"*****");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed: {ex}");
    }
}