// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.ImportProcessResult
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public class ImportProcessResult
  {
    public ImportResult Result
    {
      get
      {
        return this.Messages.Count<ImportMessage>((Func<ImportMessage, bool>) (message => message.ImportMessageType == ImportMessageType.Error)) > 0 ? ImportResult.Error : ImportResult.Success;
      }
    }

    public List<ImportMessage> ProcessLevelMessage
    {
      get
      {
        return this.Messages.Where<ImportMessage>((Func<ImportMessage, bool>) (message => message.CellData == CellData.InvalidCell)).ToList<ImportMessage>();
      }
    }

    public List<ImportMessage> Messages { get; set; }

    public ImportProcessResult() => this.Messages = new List<ImportMessage>();

    public List<ImportMessage> GetCellErrors(CellData cellData)
    {
      return this.Messages.Where<ImportMessage>((Func<ImportMessage, bool>) (message => message.CellData == cellData)).ToList<ImportMessage>();
    }

    public List<ImportMessage> GetRowErrors(int rowNumber)
    {
      return this.Messages.Where<ImportMessage>((Func<ImportMessage, bool>) (message => message.CellData.Row == rowNumber)).ToList<ImportMessage>();
    }
  }
}
