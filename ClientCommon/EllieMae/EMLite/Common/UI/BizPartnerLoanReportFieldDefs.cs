// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.BizPartnerLoanReportFieldDefs
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class BizPartnerLoanReportFieldDefs : ReportFieldDefs
  {
    private BizPartnerLoanReportFieldDefs(Sessions.Session session, ContactType type)
      : base(session)
    {
      foreach (ReportFieldDef fieldDef in (ReportFieldDefContainer) LoanReportFieldDefs.GetFieldDefs(session, false, LoanReportFieldFlags.AllDatabaseFields))
        this.Add(fieldDef);
      foreach (ReportFieldDef fieldDef in (ReportFieldDefContainer) BizPartnerReportFieldDefs.GetFieldDefs(session, false, type))
        this.Add(fieldDef);
    }

    private BizPartnerLoanReportFieldDefs(
      Sessions.Session session,
      BizPartnerReportFieldDefs bizPartnerReportFieldDefs)
      : base(session)
    {
      this.session = session;
      foreach (ReportFieldDef fieldDef in (ReportFieldDefContainer) LoanReportFieldDefs.GetFieldDefs(session, false, LoanReportFieldFlags.AllDatabaseFields))
        this.Add(fieldDef);
      foreach (ReportFieldDef partnerReportFieldDef in (ReportFieldDefContainer) bizPartnerReportFieldDefs)
        this.Add(partnerReportFieldDef);
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

    public static BizPartnerLoanReportFieldDefs GetFieldDefs(ContactType type)
    {
      return new BizPartnerLoanReportFieldDefs(Session.DefaultInstance, type);
    }

    public static BizPartnerLoanReportFieldDefs GetFieldDefs(
      Sessions.Session session,
      ContactType type)
    {
      return new BizPartnerLoanReportFieldDefs(session, type);
    }

    public static BizPartnerLoanReportFieldDefs GetFieldDefs(
      BizPartnerReportFieldDefs bizPartnerReportFieldDefs)
    {
      return new BizPartnerLoanReportFieldDefs(Session.DefaultInstance, bizPartnerReportFieldDefs);
    }
  }
}
