// See https://aka.ms/new-console-template for more information


// Get the parameter defining the server to query
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
        TelnetClient tc = new TelnetClient("192.168.123.11", 5555);
        Console.WriteLine(tc.Receive());
        tc.StartSend("status");
        Console.WriteLine(tc.Receive());
        Console.WriteLine("Einde!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed: {ex}");
    }
}