using System.Text.Json;

namespace PaymentServices;

public class TransactionService
{
    private readonly KafkaConsumer _kafkaConsumer;

    public TransactionService(KafkaConsumer kafkaConsumer)
    {
        _kafkaConsumer = kafkaConsumer;
    }
    public void Start()
    {
        //Start consuming events from kafka
        _kafkaConsumer.StartConsuming("Payments");
    }
    public void HandleMessage(string message)
    {
        var paymentEvent = JsonSerializer.Deserialize<PaymentInitiatedEvent>(message);
        bool paymentSucess = ProcessPayment(paymentEvent);

        //Emit PaymentProcessing or PaymentFailed based on the result

        IPaymentEvent eventToSend = paymentSucess ? 
        new PaymentProcessedEvent { PaymentId = paymentEvent.PaymentId }:
        new PaymentFailedEvent { PaymentId = paymentEvent.PaymentId };
    }

    private bool ProcessPayment(PaymentInitiatedEvent? paymentEvent)
    {
         // Simulated payment processing logic
        Console.WriteLine($"Processing payment for ID: {paymentEvent.PaymentId}, Amount: {paymentEvent.Amount}");
        return true; // Simulating a successful payment
    }
}

public interface IPaymentEvent{}
public class PaymentProcessedEvent: IPaymentEvent
{
    public int PaymentId {get; set;}
}
public class PaymentFailedEvent: IPaymentEvent
{
    public int PaymentId {get; set;}
}