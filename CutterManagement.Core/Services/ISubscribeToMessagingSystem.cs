namespace CutterManagement.Core
{
    /// <summary>
    /// Subscriber to messages sent via <see cref="IMessenger"/>
    /// </summary>
    public interface ISubscribeToMessagingSystem
    {
        /// <summary>
        /// Receives message sent via <see cref="IMessenger"/>
        /// </summary>
        /// <param name="message">The actual message received</param>
        void ReceiveMessage(IMessage message);
    }
}