// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.ReportFieldDefinitions.TradeAssignedLoanFieldDefs
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Common.UI.ReportFieldDefinitions
{
  public class TradeAssignedLoanFieldDefs : ReportFieldDefs
  {
    private TradeAssignedLoanFieldDefs(Sessions.Session session)
      : base(session)
    {
      foreach (ReportFieldDef fieldDef in (ReportFieldDefContainer) LoanReportFieldDefs.GetFieldDefs(session, false, LoanReportFieldFlags.DatabaseFieldsNoAudit))
        this.Add(fieldDef);
    }

    internal override ReportFieldDef CreateReportFieldDef(string category, XmlElement fieldElement)
    {
      return (ReportFieldDef) new LoanReportFieldDef(category, fieldElement);
    }

    internal override ReportFieldDef CreateReportFieldDef(FieldDefinition fieldDef)
    {
      return (ReportFieldDef) new LoanReportFieldDef(fieldDef);
    }

    public override string GetFieldPrefix() => "";

    public new ReportFieldDef this[int index] => this.fieldDefs[index];

    public new ReportFieldDef GetFieldByID(string fieldId)
    {
      return this.fieldIdLookup.ContainsKey(fieldId) ? this.fieldIdLookup[fieldId] : (ReportFieldDef) null;
    }

    public new ReportFieldDef GetFieldByCriterionName(string dbname)
    {
      return this.dbnameLookup.ContainsKey(dbname) ? this.dbnameLookup[dbname] : (ReportFieldDef) null;
    }

    public static TradeAssignedLoanFieldDefs GetFieldDefs()
    {
      return new TradeAssignedLoanFieldDefs(Session.DefaultInstance);
    }
  }
}
