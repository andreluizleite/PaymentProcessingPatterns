using Confluent.Kafka;

namespace PaymentServices;

//Kafka Producer configuration for Payment Service
public class KafkaProducer
{
    private readonly IProducer<string, string> _producer;
    public KafkaProducer(string bootstrapServers)
    {
        var config = new ProducerConfig  { BootstrapServers  = bootstrapServers };
        _producer = new ProducerBuilder<string, string>(config).Build();
    }
    public async Task SendMessageAsync(string topic, string key, string message)
    {
        await _producer.ProduceAsync(topic, new Message<string, string> { Key=key, Value= message});
    }
}

//Kafka Consumer Configuration for Transaction Service
public class KafkaConsumer
{
    private readonly IConsumer<string, string> _consumer;
    public KafkaConsumer(string bootstrapServers, string groupId)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = bootstrapServers,
            GroupId = groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        _consumer = new ConsumerBuilder<string, string>(config).Build();
    }
    public void StartConsuming(string topic)
    {
        _consumer.Subscribe(topic);
        while(true)
        {
            var cr = _consumer.Consume(CancellationToken.None);
            //Handle the Message
            HandleMessage(cr.Message.Value);

        }
    }
    public void HandleMessage(string message)
    {
        //Process the received message (e.g., JSON deserialization)
        Console.WriteLine($"Received Message: {message}");
    }
}