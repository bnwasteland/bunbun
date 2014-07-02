using System;
using System.Diagnostics;
using BunBun.Core.Messaging;
using RabbitMQ.Client;

namespace BunBun.Handler {
  public class MessageLoop {
    public IModel Channel { get; private set; }
    public IDecodeTransportMessages Decoder { get; private set; }
    public IDispatchDomainMessages Dispatcher { get; private set; }
    public ILog Logger { get; private set; }
    public QueueingBasicConsumer Consumer { get; private set; }

    public MessageLoop(IModel channel, IDecodeTransportMessages decoder, IDispatchDomainMessages dispatcher, ILog logger) {
      Channel = channel;
      Decoder = decoder;
      Dispatcher = dispatcher;
      Logger = logger;
    }

    public void Run(string queue) {
      Channel.QueueDeclare(queue, true, false, false, null);

      Channel.BasicQos(0, 1, false);
      Consumer = new QueueingBasicConsumer(Channel);
      Channel.BasicConsume(queue, false, Consumer);

      Logger.Log("Waiting for messages on [{0}].".FormatWith(queue));
      
      while (true) {
        var ea = Consumer.Queue.Dequeue();
        try {
          Logger.Log("Recieved message {0} on [{1}].".FormatWith(ea.DeliveryTag, queue));
          var message = Decoder.Decode(ea.Body);

          Logger.Log("Decoded message of type '{0}'".FormatWith(message.GetType()));
          Retry(5, message);

          Logger.Log("Acknowledging message of type '{0}'".FormatWith(message.GetType()));
          Channel.BasicAck(ea.DeliveryTag, false);
        }
        catch (MessageFailedTooManyTimesException ex) {
          Logger.Log(ex.ToString());

          Logger.Log("Deadlettering message that failed too many times.");
          Channel.BasicNack(ea.DeliveryTag, false, false);
        }
      }
    }

    public void Retry(uint times, IMessage message) {
      for (int i = 1; i <= times; i++) {
        try {
          using (new MessageScope()) {
            Dispatcher.Dispatch(message);
          }
          
          Logger.Log("Processed message of type '{0}'".FormatWith(message.GetType()));
          return;
        }
        catch(Exception ex) {
          Logger.Log("Failed to process message of type '{0}'".FormatWith(message.GetType()));
          if (i == times) {
            throw new MessageFailedTooManyTimesException("Message failed " + times + " times.", ex);
          } 
          Logger.Log("Retrying.");
        }
      }
    }
  }

  public class MessageFailedTooManyTimesException : Exception {
    public MessageFailedTooManyTimesException() {}
    public MessageFailedTooManyTimesException(string message, Exception inner) : base(message, inner) {}
  }
}
