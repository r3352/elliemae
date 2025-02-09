// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.eFolder.ConcurrentUpdateModel
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.eFolder
{
  [Serializable]
  public class ConcurrentUpdateModel
  {
    public long RecordId { get; set; }

    public int LoanXRefId { get; set; }

    public string LoanGuid { get; set; }

    public ConcurrentUpdateActionType ActionType { get; set; }

    public string XmlStr { get; set; }

    public DateTime CreatedOn { get; set; }

    public long SequenceNumber { get; set; }

    public string XmlHistoryStr { get; set; }

    public Dictionary<string, object> MergeParamKeyValues { get; set; }
  }
}
