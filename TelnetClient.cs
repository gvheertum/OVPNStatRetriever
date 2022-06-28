using System.Net;
using System.Net.Sockets;
using System.Text;

class TelnetClient
{
    private int _bufferSize = 255;

    private Socket _socket;
    public TelnetClient(string ip, int port)
    {
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        _socket.Connect(endPoint);
    }

    public void StartSend(string message) 
    {
        var buffer = Encoding.Default.GetBytes($"{message}\n");
        var x = _socket.Send(buffer, 0, buffer.Length, 0);
    }

    public string Receive()
    {
        StringBuilder sb = new StringBuilder();
        var buffer = new byte[_bufferSize];
        int rec;
        do
        {
            rec = _socket.Receive(buffer, 0, buffer.Length, 0);

            sb.Append(Encoding.Default.GetString(buffer));
        } while (rec >= _bufferSize);
        return sb.ToString();
    }
}