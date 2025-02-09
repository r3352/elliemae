// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.IPipeline
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using System;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public interface IPipeline
  {
    void RefreshPipeline();

    void RefreshPipeline(bool preserveSelection);

    void InvalidatePipeline();

    void DisplayPipeline(
      bool usersLoansOnly,
      FieldFilterList filters,
      SortField[] sortFields,
      int sqlRead);

    void DisplayPipeline(bool usersLoansOnly, FieldFilterList filters, SortField[] sortFields);

    void DisplayPipeline(string loanFolder, string viewName, int sqlRead, bool fromHomePage);

    void DisplayPipeline(
      string loanFolder,
      bool usersLoansOnly,
      FieldFilterList filters,
      SortField[] sortFields);

    void DisplayPipeline(
      string loanFolder,
      bool usersLoansOnly,
      FieldFilterList filters,
      SortField[] sortFields,
      int sqlRead);

    void DisplayPipeline(string loanFolder, string viewName);

    void DisplayPipeline(string loanFolder, string viewName, int sqlRead);

    void RefreshViewableColumns();

    void RefreshFolders();

    void EnableRefreshTimer();

    void DisableRefreshTimer();

    void toolStripMenuItem1_Click(object sender, EventArgs e);

    void OpenLoan(string loanGUID);

    void DisplayAlertMessage();
  }
}
