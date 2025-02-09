// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.MbsPoolReportFieldDefs
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class MbsPoolReportFieldDefs : TradeReportFieldDefs
  {
    internal new const string FieldPrefix = "MBSPool";

    public MbsPoolReportFieldDefs()
    {
    }

    protected MbsPoolReportFieldDefs(string fileName)
      : base(fileName)
    {
    }

    public static MbsPoolReportFieldDefs GetFieldDefs()
    {
      MbsPoolReportFieldDefs fieldDefs = new MbsPoolReportFieldDefs();
      foreach (TradeReportFieldDef fieldDef in (ReportFieldDefContainer) new MbsPoolReportFieldDefs("MbsPoolsMap.xml"))
        fieldDefs.Add((ReportFieldDef) fieldDef);
      return fieldDefs;
    }

    public static MbsPoolReportFieldDefs GetFieldDefs(Sessions.Session session)
    {
      MbsPoolReportFieldDefs fieldDefs = new MbsPoolReportFieldDefs();
      foreach (TradeReportFieldDef fieldDef in (ReportFieldDefContainer) new MbsPoolReportFieldDefs("MbsPoolsMap.xml"))
      {
        fieldDef.Category = "MBS Pool";
        fieldDef.FieldID = "MBSPool." + fieldDef.FieldID;
        fieldDefs.Add((ReportFieldDef) fieldDef);
      }
      foreach (LoanReportFieldDef fieldDef in (ReportFieldDefContainer) LoanReportFieldDefs.GetFieldDefs(session, false, LoanReportFieldFlags.AllDatabaseFields))
        fieldDefs.Add((ReportFieldDef) fieldDef);
      foreach (TradeReportFieldDef fieldDef in (ReportFieldDefContainer) SecurityTradeReportFieldDefs.GetFieldDefs())
      {
        fieldDef.Category = "Security Trade";
        fieldDef.FieldID = "SecurityTrade." + fieldDef.FieldID;
        fieldDefs.Add((ReportFieldDef) fieldDef);
      }
      return fieldDefs;
    }

    public override string GetFieldPrefix() => "MBSPool";
  }
}
