namespace CutterManagement.Core
{
    /// <summary>
    /// Messenger that serves as center of communication between subscribed members
    /// </summary>
    public interface IMessenger 
    {
        /// <summary>
        /// Registers members that wants to send or receive messages
        /// </summary>
        /// <param name="subscriber">The subscriber</param>
        void RegisterMessenger(ISubscribeToMessagingSystem subscriber);

        /// <summary>
        /// Sends message to subscriber
        /// </summary>
        /// <param name="message">Message to send to subscriber</param>
        void SendMessage(IMessage message);
    }
}
