namespace Mango.Services.ShoppingCartAPI.RabbmitMQSender
{
    public interface IRabbmitMQAuthMessageSender
    {
        void SendMessage(Object message, string queueName);
    }
}
