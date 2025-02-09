// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.DDMDataPopulationTiming
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class DDMDataPopulationTiming
  {
    public string UserID { get; set; }

    public bool LoanSave { get; set; }

    public bool FieldChanges { get; set; }

    public List<DDMDataPopTimingField> FieldList { get; set; }

    public bool AfterLoanInitEst { get; set; }

    public bool LoanCondMet { get; set; }

    public string LoanCondMetCond { get; set; }

    public string LoanCondMetCondXml { get; set; }

    public DDMDataPopulationTiming()
    {
    }

    public DDMDataPopulationTiming(
      string userID,
      bool loanSave,
      bool fieldChanges,
      List<DDMDataPopTimingField> fieldList,
      bool afterLoanInitEst,
      bool loanCondMet,
      string loanCondMetCond,
      string loanCondMetCondXml)
    {
      this.UserID = userID;
      this.LoanSave = loanSave;
      this.FieldChanges = fieldChanges;
      this.FieldList = fieldList;
      this.AfterLoanInitEst = afterLoanInitEst;
      this.LoanCondMet = loanCondMet;
      this.LoanCondMetCond = loanCondMetCond;
      this.LoanCondMetCondXml = loanCondMetCondXml;
    }
  }
}
