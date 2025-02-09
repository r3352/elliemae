// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.LogRecordLink
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class LogRecordLink : Hyperlink
  {
    private LogRecordBase logEntry;

    public LogRecordLink(string text, LogRecordBase logEntry)
      : base(text)
    {
      this.logEntry = logEntry;
      this.Click += new EventHandler(this.LogRecordLink_Click);
    }

    private void LogRecordLink_Click(object sender, EventArgs e)
    {
      Session.Application.GetService<ILoanEditor>().OpenLogRecord(this.logEntry);
    }
  }
}
