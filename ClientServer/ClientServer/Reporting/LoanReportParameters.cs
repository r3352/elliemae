// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Reporting.LoanReportParameters
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Reporting
{
  [Serializable]
  public class LoanReportParameters : ReportParameters
  {
    private bool useDBField;
    private bool useDBFilter;
    private string[] folders;

    public bool UseDBField
    {
      get => this.useDBField;
      set => this.useDBField = value;
    }

    public bool UseDBFilter
    {
      get => this.useDBFilter;
      set => this.useDBFilter = value;
    }

    public bool UseExternalOrganization { get; set; }

    public bool ExcludeArchiveLoans { get; set; }

    public bool RequiresDirectLoanAccess
    {
      get
      {
        if (!this.useDBField)
          return true;
        return !this.useDBFilter && this.FieldFilters.Count > 0;
      }
    }

    public string[] Folders
    {
      get => this.folders;
      set => this.folders = value;
    }
  }
}
