// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Tasks.LockComparisonFieldsNotificationStatus
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Server.Tasks
{
  public class LockComparisonFieldsNotificationStatus
  {
    public string Topic { get; set; }

    public string ConsumerGroupId { get; set; }

    public string LastReceivedMessage { get; set; }

    public override string ToString()
    {
      return "Topic: " + this.Topic + " " + Environment.NewLine + " ConsumerGroupId: " + this.ConsumerGroupId + " " + Environment.NewLine + " LastReceivedMessage: " + this.LastReceivedMessage;
    }
  }
}
