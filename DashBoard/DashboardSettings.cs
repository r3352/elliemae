// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DashBoard.DashboardSettings
// Assembly: DashBoard, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 99BFBD49-67F8-470C-81BC-FC4FAEA6C933
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DashBoard.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;

#nullable disable
namespace EllieMae.EMLite.DashBoard
{
  public static class DashboardSettings
  {
    private static LoanReportFieldDefs fieldDefinitions;

    static DashboardSettings()
    {
      if (Session.DefaultInstance == null)
        return;
      DashboardSettings.fieldDefinitions = LoanReportFieldDefs.GetFieldDefs(Session.DefaultInstance, LoanReportFieldFlags.AllDatabaseFields);
    }

    public static LoanReportFieldDefs GetLoanReportFieldDefs(Sessions.Session session)
    {
      if (DashboardSettings.fieldDefinitions == null)
        DashboardSettings.fieldDefinitions = LoanReportFieldDefs.GetFieldDefs(session, LoanReportFieldFlags.AllDatabaseFields);
      return DashboardSettings.fieldDefinitions;
    }

    public static LoanReportFieldDefs FieldDefinitions => DashboardSettings.fieldDefinitions;
  }
}
