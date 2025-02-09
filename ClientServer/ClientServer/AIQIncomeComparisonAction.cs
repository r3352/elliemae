// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.AIQIncomeComparisonAction
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public class AIQIncomeComparisonAction : EPassMessageAction
  {
    public AIQIncomeComparisonAction(string description, DateTime alertDateTime, string messageID)
      : base(EPassMessageActionType.AIQIncomeComparison, description)
    {
      this.AlertDateTime = alertDateTime;
      this.MessageID = messageID;
    }

    public DateTime AlertDateTime { get; }

    public string MessageID { get; }
  }
}
