// See https://aka.ms/new-console-template for more information

using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using RabbitMQ.Client;

DotEnv.DotEnv.LoadDotEnvPathFromTheSolutionDirectory();
var factory = new ConnectionFactory
{
    Uri = new Uri(Environment.GetEnvironmentVariable("MQ_URL"))
};

string queueName = Environment.GetEnvironmentVariable("Q_NAME");

using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(queue: queueName,
    durable: true,
    exclusive: false,
    autoDelete: false,
    arguments: null
);

var message = new
{
    Name = "First",
    Message= "My Message"
};
var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
await channel.BasicPublishAsync(exchange: "", routingKey: queueName, body: body);