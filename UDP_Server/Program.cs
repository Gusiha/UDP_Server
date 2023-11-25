

using System.Net;
using System.Net.Sockets;
using System.Text;

string ipAddress = "192.168.0.105";
string remoteHostIP = "192.168.0.105";

int port = 11001;
byte[] buffer = new byte[1024];

Socket udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
IPEndPoint localIP = new IPEndPoint(IPAddress.Parse(ipAddress), port);
EndPoint remoteClient = new IPEndPoint(IPAddress.Parse(remoteHostIP), port);

udpSocket.Bind(localIP);

Console.WriteLine("UDP-сервер запущен...");


while (true)
{
    var result = await udpSocket.ReceiveFromAsync(buffer, remoteClient);
    var message = Encoding.UTF8.GetString(buffer, 0, result.ReceivedBytes);

    Console.WriteLine($"Сообщение от пользователя [{result.RemoteEndPoint}]");
    Console.WriteLine($"Получено {result.ReceivedBytes} байт");
    Console.WriteLine(message);     // выводим полученное сообщение

    var instance = message.Split(' ');

    switch (instance.Last())
    {
        case "+":
            {
                message = (Int32.Parse(instance[0]) + Int32.Parse(instance[1])).ToString();
                break;
            }

        case "-":
            {
                message = (Int32.Parse(instance[0]) - Int32.Parse(instance[1])).ToString();
                break;
            }

        case "/":
            {
                message = (Int32.Parse(instance[0]) / Int32.Parse(instance[1])).ToString();
                break;
            }

        case "*":
            {
                message = (Int32.Parse(instance[0]) * Int32.Parse(instance[1])).ToString();
                break;
            }

        default:
            message = "Error";
            break;
    }

    buffer = Encoding.UTF8.GetBytes(message);

    await udpSocket.SendToAsync(buffer, remoteClient);
    Console.WriteLine($"По адресу {remoteClient} отправлено \"{message}\"");
}

