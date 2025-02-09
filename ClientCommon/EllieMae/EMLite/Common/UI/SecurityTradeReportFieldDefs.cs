// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.SecurityTradeReportFieldDefs
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class SecurityTradeReportFieldDefs : TradeReportFieldDefs
  {
    internal new const string FieldPrefix = "SecurityTrade";

    public SecurityTradeReportFieldDefs()
    {
    }

    protected SecurityTradeReportFieldDefs(string fileName)
      : base(fileName)
    {
    }

    public static SecurityTradeReportFieldDefs GetFieldDefs()
    {
      SecurityTradeReportFieldDefs fieldDefs = new SecurityTradeReportFieldDefs();
      foreach (TradeReportFieldDef fieldDef in (ReportFieldDefContainer) new SecurityTradeReportFieldDefs("SecurityTradesMap.xml"))
        fieldDefs.Add((ReportFieldDef) fieldDef);
      return fieldDefs;
    }

    public static SecurityTradeReportFieldDefs GetFieldDefs(Sessions.Session session)
    {
      SecurityTradeReportFieldDefs fieldDefs = new SecurityTradeReportFieldDefs();
      foreach (TradeReportFieldDef fieldDef in (ReportFieldDefContainer) new SecurityTradeReportFieldDefs("SecurityTradesMap.xml"))
      {
        fieldDef.Category = "Security Trade";
        fieldDef.FieldID = "SecurityTrade." + fieldDef.FieldID;
        fieldDefs.Add((ReportFieldDef) fieldDef);
      }
      foreach (TradeReportFieldDef fieldDef in (ReportFieldDefContainer) TradeReportFieldDefs.GetFieldDefs())
      {
        fieldDef.Category = "Loan Trade";
        fieldDef.FieldID = "LoanTrade." + fieldDef.FieldID;
        fieldDefs.Add((ReportFieldDef) fieldDef);
      }
      foreach (TradeReportFieldDef fieldDef in (ReportFieldDefContainer) MbsPoolReportFieldDefs.GetFieldDefs())
      {
        fieldDef.Category = "MBS Pool";
        fieldDef.FieldID = "MBSPool." + fieldDef.FieldID;
        fieldDefs.Add((ReportFieldDef) fieldDef);
      }
      return fieldDefs;
    }

    public override string GetFieldPrefix() => "SecurityTrade";
  }
}
