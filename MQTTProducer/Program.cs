using System;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client.Options;
using Nito.AsyncEx;

namespace MQTTProducer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Need 3 args: hostname or IP of MQTT broker, then client ID, then topic.");
            }
            else
            {
                string broker = args[0];
                string clientId = args[1];
                string topic = args[2];

                Console.WriteLine("Sending to: " + broker);

                while (true)
                {
                    AsyncContext.Run(async () => await AsyncMain(broker, clientId, topic));
                }
            } 
        }

        private static async Task AsyncMain(string broker, string clientId, string topic)
        {
            // Create a new MQTT client.
            var factory = new MqttFactory();
            var mqttClient = factory.CreateMqttClient();

            Random rnd = new Random();
            int reading = rnd.Next(1000);
            string payload = "{\"reading\":" + reading.ToString() + "}";

            var options = new MqttClientOptionsBuilder()
                .WithClientId(clientId)
                .WithTcpServer(broker)
                .Build();
            await mqttClient.ConnectAsync(options, CancellationToken.None);

            Console.WriteLine("Sending message...");
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .Build();

            await mqttClient.PublishAsync(message, CancellationToken.None);

            await Task.Delay(1000);

        }
    }
}
