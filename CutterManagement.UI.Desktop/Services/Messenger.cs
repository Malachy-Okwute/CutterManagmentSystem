using CutterManagement.Core;
using System.Diagnostics;

namespace CutterManagement.UI.Desktop
{    
    /// <summary>
    /// Messenger that serves as center of communication between subscribed members
    /// </summary>
    public class Messenger : IMessenger
    {
        /// <summary>
        /// A static instance of messenger
        /// </summary>
        private static IMessenger _instance;

        /// <summary>
        /// Collection of subscribers
        /// </summary>
        private ICollection<ISubscribeToMessagingSystem> _subscribers = new List<ISubscribeToMessagingSystem>();

        /// <summary>
        /// Message sender
        /// </summary>
        public static IMessenger MessageSender => _instance = _instance ?? new Messenger();

        /// <summary>
        /// Registers members that wants to send or receive messages
        /// </summary>
        /// <param name="subscriber">The subscriber</param>
        /// <exception cref="ArgumentException">Throws an exception if subscriber already subscribes to messenger</exception>
        public void RegisterMessenger(ISubscribeToMessagingSystem subscriber)
        {
            if (_subscribers.Contains(subscriber))
            {
                throw new ArgumentException($"{subscriber} has already been registered");
            }

            _subscribers.Add(subscriber);
        }

        /// <summary>
        /// Sends message to subscriber
        /// </summary>
        /// <param name="message">Message to send to subscriber</param>
        public void SendMessage(IMessage message)
        {
            StackFrame[] frame = new StackTrace().GetFrames();
            Type? sender = null;

            if (frame.Length > 1)
            {
                sender = frame[1].GetMethod()?.DeclaringType;
            }

            foreach (ISubscribeToMessagingSystem subscriber in _subscribers)
            {
                // Do not send current message to the current message sender
                if(subscriber.GetType().FullName == sender?.DeclaringType?.FullName)
                {
                    continue;
                }

                // Send message
                subscriber.ReceiveMessage(message);
            }
        }
    }
}
