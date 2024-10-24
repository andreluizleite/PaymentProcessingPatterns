using System.Text.Json;

namespace PaymentServices;

public class PaymentService
{
    private readonly KafkaProducer _kafkaProducer;

    public PaymentService(KafkaProducer kafkaProducer)
    {
        _kafkaProducer = kafkaProducer;
    }
    public async Task ProcessPayment(PaymentRequest request)
    {
        //Validate request (omitted for simplicity)

        //Emit PaymentInitiated event
        var paymentInitiatedEvent = new PaymentInitiatedEvent
        {
            PaymentId = request.PaymentId,
            Amount = request.Amount,
            Currency = request.Currency
        };

        string message = JsonSerializer.Serialize(paymentInitiatedEvent);
        await _kafkaProducer.SendMessageAsync("Payments", request.PaymentId.ToString(), message);

    }
}
    public class PaymentRequest
    {
        public int PaymentId {get; set;}
        public decimal Amount {get;set;}
        public string Currency {get;set;}
    }
    public class PaymentInitiatedEvent
    {
        public int PaymentId {get; set;}
        public decimal Amount {get; set;}
        public string Currency {get; set;}
    }
