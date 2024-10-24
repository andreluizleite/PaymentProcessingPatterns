using PaymentServices;

class Program
{
    static async Task Main(string[] args)
    {
        var kafkaProducer = new KafkaProducer("localhost:9092");
        var paymentService = new PaymentService(kafkaProducer);

        // Simulate payment processing
        var paymentRequest = new PaymentRequest
        {
            PaymentId = 1,
            Amount = 100.0m,
            Currency = "USD"
        };

        try
        {
            await paymentService.ProcessPayment(paymentRequest);
            Console.WriteLine($"Payment processed: {paymentRequest.PaymentId}, Amount: {paymentRequest.Amount}, Currency: {paymentRequest.Currency}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing payment: {ex.Message}");
        }

        // Initialize Transaction Service and start consuming
        Console.WriteLine("Initializing Transaction Service and starting to consume...");
        var kafkaConsumer = new KafkaConsumer("localhost:9092", "transaction-group");
        var transactionService = new TransactionService(kafkaConsumer);

        // Start consuming messages asynchronously
        _ = Task.Run(() => transactionService.Start());

        // Wait for user input to terminate the program gracefully
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
