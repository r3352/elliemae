// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.Rmq.BasicPropertiesWrapper
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using RabbitMQ.Client;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Elli.MessageQueues.Rmq
{
  [Serializable]
  public class BasicPropertiesWrapper
  {
    public BasicPropertiesWrapper(IBasicProperties basicProperties)
    {
      this.ContentType = basicProperties.ContentType;
      this.ContentEncoding = basicProperties.ContentEncoding;
      this.DeliveryMode = basicProperties.DeliveryMode;
      this.Priority = basicProperties.Priority;
      this.CorrelationId = basicProperties.CorrelationId;
      this.ReplyTo = basicProperties.ReplyTo;
      this.Expiration = basicProperties.Expiration;
      this.MessageId = basicProperties.MessageId;
      this.Timestamp = basicProperties.Timestamp.UnixTime;
      this.Type = basicProperties.Type;
      this.UserId = basicProperties.UserId;
      this.AppId = basicProperties.AppId;
      this.ClusterId = basicProperties.ClusterId;
      if (!basicProperties.IsHeadersPresent())
        return;
      this.Headers = (IDictionary) new Dictionary<string, object>();
      foreach (KeyValuePair<string, object> header in (IEnumerable<KeyValuePair<string, object>>) basicProperties.Headers)
        this.Headers.Add((object) header.Key, header.Value);
    }

    public string ContentType { get; set; }

    public string ContentEncoding { get; set; }

    public IDictionary Headers { get; set; }

    public byte DeliveryMode { get; set; }

    public byte Priority { get; set; }

    public string CorrelationId { get; set; }

    public string ReplyTo { get; set; }

    public string Expiration { get; set; }

    public string MessageId { get; set; }

    public long Timestamp { get; set; }

    public string Type { get; set; }

    public string UserId { get; set; }

    public string AppId { get; set; }

    public string ClusterId { get; set; }

    public int ProtocolClassId { get; set; }

    public string ProtocolClassName { get; set; }
  }
}
