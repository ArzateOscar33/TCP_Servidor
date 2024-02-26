using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TCPIP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            server();
            Console.ReadKey();
        }

        public static void server()
        {
            //localhost =>127.0.0.1
            //Configuraciones del servidor
            IPHostEntry host = Dns.GetHostEntry("localhost");
            //solo se necesita la primer ip de la lista de IP
            IPAddress ipAddress = host.AddressList[0];
            //le damos como parametro la direccion ip y un puerto
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress,11200);

            try
            {
                //creacion del socktet que esta escuchando
                Socket listener = new Socket(ipAddress.AddressFamily,SocketType.Stream, ProtocolType.Tcp);
                //Unir el endPoint al socket
                listener.Bind(localEndPoint);
                //este listener puede escchar hasta 10  conexiones antes de decir que esta ocupado

                listener.Listen(10);
                Console.WriteLine("Esperando conexion");
                //se recibe una conexion y se le entrega a un socket para qu ela maneje
                Socket handler= listener.Accept();
                string data = null;
                byte[] bytes = null;

                while (true)
                {
                    //recibir los datos desd el cliente
                    bytes = new byte[1024];
                    //convertir los datos desde bytes a string
                    int byteRec = handler.Receive(bytes);
                    data += Encoding.ASCII.GetString(bytes,0,byteRec);
                    //verificar cuadno el cleinte dejo de enviar datos
                    if (data.IndexOf("<EOF>") > -1)
                        break;
                    
                }//while
                //Mostrart el mensaje del cliente por pantalla
                Console.WriteLine("Texto del cliente : " + data);


            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadKey();
            }


        }
    }
}
