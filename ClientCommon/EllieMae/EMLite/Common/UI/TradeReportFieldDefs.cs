// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.TradeReportFieldDefs
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class TradeReportFieldDefs : ReportFieldDefs
  {
    internal const string FieldPrefix = "LoanTrade";

    public TradeReportFieldDefs()
      : base(Session.DefaultInstance)
    {
    }

    protected TradeReportFieldDefs(string fileName)
      : base(Session.DefaultInstance, fileName)
    {
    }

    internal override ReportFieldDef CreateReportFieldDef(string category, XmlElement fieldElement)
    {
      return (ReportFieldDef) new TradeReportFieldDef(category, fieldElement);
    }

    internal override ReportFieldDef CreateReportFieldDef(FieldDefinition fieldDef)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public TradeReportFieldDef this[int index] => (TradeReportFieldDef) this.fieldDefs[index];

    public TradeReportFieldDef GetFieldByID(string fieldId)
    {
      return this.fieldIdLookup.ContainsKey(fieldId) ? (TradeReportFieldDef) this.fieldIdLookup[fieldId] : (TradeReportFieldDef) null;
    }

    public TradeReportFieldDef GetFieldByCriterionName(string dbname)
    {
      return this.dbnameLookup.ContainsKey(dbname) ? (TradeReportFieldDef) this.dbnameLookup[dbname] : (TradeReportFieldDef) null;
    }

    public static TradeReportFieldDefs GetFieldDefs()
    {
      TradeReportFieldDefs fieldDefs = new TradeReportFieldDefs();
      foreach (TradeReportFieldDef fieldDef in (ReportFieldDefContainer) new TradeReportFieldDefs("TradesMap.xml"))
        fieldDefs.Add((ReportFieldDef) fieldDef);
      return fieldDefs;
    }

    public static TradeReportFieldDefs GetFieldDefs(Sessions.Session session)
    {
      TradeReportFieldDefs fieldDefs = new TradeReportFieldDefs();
      foreach (TradeReportFieldDef fieldDef in (ReportFieldDefContainer) new TradeReportFieldDefs("TradesMap.xml"))
      {
        fieldDef.Category = "Loan Trade";
        fieldDef.FieldID = "LoanTrade." + fieldDef.FieldID;
        fieldDefs.Add((ReportFieldDef) fieldDef);
      }
      foreach (LoanReportFieldDef fieldDef in (ReportFieldDefContainer) LoanReportFieldDefs.GetFieldDefs(session, false, LoanReportFieldFlags.AllDatabaseFields))
        fieldDefs.Add((ReportFieldDef) fieldDef);
      return fieldDefs;
    }

    public override string GetFieldPrefix() => "LoanTrade";
  }
}
