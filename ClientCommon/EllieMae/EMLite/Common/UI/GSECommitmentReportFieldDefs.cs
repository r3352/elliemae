// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.GSECommitmentReportFieldDefs
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
  public class GSECommitmentReportFieldDefs : ReportFieldDefs
  {
    internal const string FieldPrefix = "GSECommitment";

    public GSECommitmentReportFieldDefs()
      : base(Session.DefaultInstance)
    {
    }

    private GSECommitmentReportFieldDefs(string fileName)
      : base(Session.DefaultInstance, fileName)
    {
    }

    internal override ReportFieldDef CreateReportFieldDef(string category, XmlElement fieldElement)
    {
      return (ReportFieldDef) new GSECommitmentReportFieldDef(category, fieldElement);
    }

    internal override ReportFieldDef CreateReportFieldDef(FieldDefinition fieldDef)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public GSECommitmentReportFieldDef this[int index]
    {
      get => (GSECommitmentReportFieldDef) this.fieldDefs[index];
    }

    public GSECommitmentReportFieldDef GetFieldByID(string fieldId)
    {
      return this.fieldIdLookup.ContainsKey(fieldId) ? (GSECommitmentReportFieldDef) this.fieldIdLookup[fieldId] : (GSECommitmentReportFieldDef) null;
    }

    public GSECommitmentReportFieldDef GetFieldByCriterionName(string dbname)
    {
      return this.dbnameLookup.ContainsKey(dbname) ? (GSECommitmentReportFieldDef) this.dbnameLookup[dbname] : (GSECommitmentReportFieldDef) null;
    }

    public static GSECommitmentReportFieldDefs GetFieldDefs()
    {
      GSECommitmentReportFieldDefs fieldDefs = new GSECommitmentReportFieldDefs();
      foreach (GSECommitmentReportFieldDef fieldDef in (ReportFieldDefContainer) new GSECommitmentReportFieldDefs("GSECommitmentMap.xml"))
        fieldDefs.Add((ReportFieldDef) fieldDef);
      return fieldDefs;
    }

    public static GSECommitmentReportFieldDefs GetFieldDefs(Sessions.Session session)
    {
      GSECommitmentReportFieldDefs fieldDefs = new GSECommitmentReportFieldDefs();
      foreach (GSECommitmentReportFieldDef fieldDef in (ReportFieldDefContainer) new GSECommitmentReportFieldDefs("CorrespondentTradesMap.xml"))
      {
        fieldDef.Category = "GSE Commitment";
        fieldDef.FieldID = "GSECommitment." + fieldDef.FieldID;
        fieldDefs.Add((ReportFieldDef) fieldDef);
      }
      foreach (LoanReportFieldDef fieldDef in (ReportFieldDefContainer) LoanReportFieldDefs.GetFieldDefs(session, false, LoanReportFieldFlags.AllDatabaseFields))
        fieldDefs.Add((ReportFieldDef) fieldDef);
      return fieldDefs;
    }

    public override string GetFieldPrefix() => "GSECommitment";
  }
}
