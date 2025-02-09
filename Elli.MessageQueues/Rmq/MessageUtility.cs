// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.Rmq.MessageUtility
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using RabbitMQ.Client.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Elli.MessageQueues.Rmq
{
  internal static class MessageUtility
  {
    public static int GetDeathCount(BasicDeliverEventArgs eventArgs)
    {
      int result = 0;
      IDictionary<string, object> dictionary1 = eventArgs.BasicProperties.IsHeadersPresent() ? eventArgs.BasicProperties.Headers : (IDictionary<string, object>) new Dictionary<string, object>();
      if (!dictionary1.ContainsKey("x-death"))
        return result;
      IDictionary dictionary2 = dictionary1["x-death"] is IList list ? list[0] as IDictionary : (IDictionary) null;
      if (dictionary2 != null && dictionary2.Contains((object) "count"))
        int.TryParse(dictionary2[(object) "count"].ToString(), out result);
      return result;
    }

    public static TimeSpan GetMessageWaitInQueueTime(BasicDeliverEventArgs eventArgs)
    {
      TimeSpan messageWaitInQueueTime = new TimeSpan(0L);
      if (MessageUtility.GetDeathCount(eventArgs) > 0)
        return messageWaitInQueueTime;
      IDictionary<string, object> dictionary = eventArgs.BasicProperties.IsHeadersPresent() ? eventArgs.BasicProperties.Headers : (IDictionary<string, object>) new Dictionary<string, object>();
      if (!dictionary.ContainsKey("PublishedDate"))
        return messageWaitInQueueTime;
      try
      {
        messageWaitInQueueTime = DateTime.UtcNow - Convert.ToDateTime(Encoding.UTF8.GetString((byte[]) dictionary["PublishedDate"]));
      }
      catch
      {
      }
      return messageWaitInQueueTime;
    }
  }
}
