using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using async_shell.dependencies.data_holders;
using async_shell.dependencies.networking.network_manager;
using async_shell.dependencies.networking.network_resource;
using async_shell.dependencies.serializer;
using async_shell.dependencies.networking.protocol;


namespace async_shell
{
    class Program
    {
        static Semaphore _semaphore = new Semaphore(1,1);

        static int buffer_size = 100;

        static async Task<Socket> get_client_socket() 
        {   
            int port = 12345;
            IPAddress ip_address = IPAddress.Parse("127.0.0.1");
            IPEndPoint local_address = new IPEndPoint(ip_address, port);

            Socket server = new Socket(ip_address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);  
            // Start listening for client requests.
            
            server.Bind(local_address);
            server.Listen(1);
            _semaphore.Release();
            Socket client_socket = await server.AcceptAsync();
            return client_socket;
        }

        static async void start_server()
        {
            System.Console.WriteLine("Server started");
            Socket sock = await get_client_socket();
            System.Console.WriteLine("client socket connected");

            // byte[] holding_buffer = new byte[buffer_size];

            TcpResource r = new TcpResource(buffer_size, sock);
            IPausableDataSender reciver = new PausableDataSender(r);
            
            byte[] a = Encoding.UTF8.GetBytes("a");
            
            while (true)
            {
                byte[] data = new byte[buffer_size];
                r.Receive(data);
                string data_in_string = Encoding.UTF8.GetString(data);
                System.Console.WriteLine(data_in_string);
                byte[] buffer = BitConverter.GetBytes(a.Length);
                byte[] res = buffer.Concat(a).ToArray();
                r.Send(res, 0, res.Length);
            } 
        }

        static int send_data(INetworkingManager<DataWithAttributes> m, string content, int priority)
        {
            TextData d = new TextData();
            d.Content = content;
            d.DataType = "TextData";
            return m.AddDataToSendPool(d, priority);
        }

        static void client()
        {
            _semaphore.WaitOne();
            IPAddress ip_address = IPAddress.Parse("127.0.0.1");
            int port = 12345;
            TcpResource r = new TcpResource(buffer_size, ip_address, port);
            ISerializer<DataWithAttributes> s = new JsonSerializer<DataWithAttributes>();
            INetworkingManager<DataWithAttributes> m = new SingeResourceNetworkingManager<DataWithAttributes>(r, s);
            string d = String.Concat(Enumerable.Repeat("a", 101));
            int a = send_data(m, d, 2);
            // int b = send_data(m, "hello moses", 3);

            m.StartByTaskID(a);
            // m.StartByTaskID(b);
            
        }

        static async Task Main()
        {
            _semaphore.WaitOne();
            Action s = start_server;
            var moses = Task.Run(start_server);

            Action a = new Action(client);
            var res = Task.Run(a);

            await moses;
            await res;
        }
    }
}
