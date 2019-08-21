using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace EmitLogRouting
{
    class Program
    {
        public static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "Direct_logs",
                                        type: "direct");

                var severity = (args.Length > 0) ? args[0] : "info";
                var message = (args.Length > 1)
                              ? string.Join(" ", args.Skip(1).ToArray())
                              : "Hello World!";
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "Direct_logs",
                                     routingKey: severity,
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine(" [x] Sent '{0}':'{1}'", severity, message);
            }

            Console.WriteLine(" If u want to Exit then please press the ENTER.");
            Console.ReadLine();
        }
    }
}
