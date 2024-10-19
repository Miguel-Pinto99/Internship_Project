namespace Project1.Infrastructure
{
    public interface IMqttService
    {
        Task PublishOnTopicAsync(string payLoad, string topic, CancellationToken cancellationToken);
    }
}