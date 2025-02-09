// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.BorrowerLoanReportFieldDefs
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class BorrowerLoanReportFieldDefs : ReportFieldDefs
  {
    public BorrowerLoanReportFieldDefs()
      : this(Session.DefaultInstance)
    {
    }

    public BorrowerLoanReportFieldDefs(Sessions.Session session)
      : base(session)
    {
      foreach (ReportFieldDef fieldDef in (ReportFieldDefContainer) LoanReportFieldDefs.GetFieldDefs(session, false, LoanReportFieldFlags.AllDatabaseFields))
        this.Add(fieldDef);
      foreach (ReportFieldDef fieldDef in (ReportFieldDefContainer) BorrowerReportFieldDefs.GetFieldDefs(session, false))
        this.Add(fieldDef);
    }

    public BorrowerLoanReportFieldDefs(BorrowerReportFieldDefs borReportFieldDefs)
      : base(Session.DefaultInstance)
    {
      foreach (ReportFieldDef fieldDef in (ReportFieldDefContainer) LoanReportFieldDefs.GetFieldDefs(Session.DefaultInstance, false, LoanReportFieldFlags.AllDatabaseFields))
        this.Add(fieldDef);
      foreach (ReportFieldDef borReportFieldDef in (ReportFieldDefContainer) borReportFieldDefs)
        this.Add(borReportFieldDef);
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

    public new ReportFieldDef GetFieldByID(string fieldId)
    {
      return this.fieldIdLookup.ContainsKey(fieldId) ? this.fieldIdLookup[fieldId] : (ReportFieldDef) null;
    }

    public new ReportFieldDef GetFieldByCriterionName(string dbname)
    {
      return this.dbnameLookup.ContainsKey(dbname) ? this.dbnameLookup[dbname] : (ReportFieldDef) null;
    }

    public static BorrowerLoanReportFieldDefs GetFieldDefs() => new BorrowerLoanReportFieldDefs();

    public static BorrowerLoanReportFieldDefs GetFieldDefs(Sessions.Session session)
    {
      return new BorrowerLoanReportFieldDefs(session);
    }

    public static BorrowerLoanReportFieldDefs GetFieldDefs(
      BorrowerReportFieldDefs borReportFieldDefs)
    {
      return new BorrowerLoanReportFieldDefs(borReportFieldDefs);
    }
  }
}
