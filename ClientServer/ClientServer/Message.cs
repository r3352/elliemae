// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Message
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class Message
  {
    private string text;
    private MessagePriority priority;
    private SessionInfo source;
    private DateTime timestamp;
    private bool displayMessageFrom = true;

    public Message(string text, MessagePriority priority)
    {
      this.text = text;
      this.source = (SessionInfo) null;
      this.priority = priority;
      this.timestamp = DateTime.Now;
    }

    public Message(string text)
      : this(text, MessagePriority.Normal)
    {
    }

    public Message(string text, bool displayMessageFrom)
      : this(text)
    {
      this.displayMessageFrom = displayMessageFrom;
    }

    public Message(string text, MessagePriority priority, bool displayMessageFrom)
      : this(text, priority)
    {
      this.displayMessageFrom = displayMessageFrom;
    }

    protected Message(Message source, SessionInfo info)
    {
      this.text = source.text;
      this.priority = source.priority;
      this.source = info;
      this.timestamp = DateTime.Now;
      this.displayMessageFrom = source.DisplayMessageFrom;
    }

    protected Message(string text, SessionInfo info)
    {
      this.text = text;
      this.source = info;
    }

    public virtual Message Clone(SessionInfo info) => new Message(this, info);

    public string Text => this.text;

    public SessionInfo Source => this.source;

    public MessagePriority Priority => this.priority;

    public DateTime Timestamp => this.timestamp;

    public bool DisplayMessageFrom => this.displayMessageFrom;
  }
}
