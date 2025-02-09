// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.MasterContractReportFieldDefs
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
  public class MasterContractReportFieldDefs : ReportFieldDefs
  {
    internal const string FieldPrefix = "MasterContract";

    public MasterContractReportFieldDefs()
      : base(Session.DefaultInstance)
    {
    }

    private MasterContractReportFieldDefs(string fileName)
      : base(Session.DefaultInstance, fileName)
    {
    }

    internal override ReportFieldDef CreateReportFieldDef(string category, XmlElement fieldElement)
    {
      return (ReportFieldDef) new MasterContractReportFieldDef(category, fieldElement);
    }

    internal override ReportFieldDef CreateReportFieldDef(FieldDefinition fieldDef)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public MasterContractReportFieldDef this[int index]
    {
      get => (MasterContractReportFieldDef) this.fieldDefs[index];
    }

    public MasterContractReportFieldDef GetFieldByID(string fieldId)
    {
      return this.fieldIdLookup.ContainsKey(fieldId) ? (MasterContractReportFieldDef) this.fieldIdLookup[fieldId] : (MasterContractReportFieldDef) null;
    }

    public MasterContractReportFieldDef GetFieldByCriterionName(string dbname)
    {
      return this.dbnameLookup.ContainsKey(dbname) ? (MasterContractReportFieldDef) this.dbnameLookup[dbname] : (MasterContractReportFieldDef) null;
    }

    public static MasterContractReportFieldDefs GetFieldDefs()
    {
      MasterContractReportFieldDefs fieldDefs = new MasterContractReportFieldDefs();
      foreach (MasterContractReportFieldDef fieldDef in (ReportFieldDefContainer) new MasterContractReportFieldDefs("MasterContractsMap.xml"))
        fieldDefs.Add((ReportFieldDef) fieldDef);
      return fieldDefs;
    }

    public static MasterContractReportFieldDefs GetFieldDefs(Sessions.Session session)
    {
      MasterContractReportFieldDefs fieldDefs = new MasterContractReportFieldDefs();
      foreach (MasterContractReportFieldDef fieldDef in (ReportFieldDefContainer) new MasterContractReportFieldDefs("MasterContractsMap.xml"))
      {
        fieldDef.Category = "Master Contract";
        fieldDef.FieldID = "MasterContract." + fieldDef.FieldID;
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

    public override string GetFieldPrefix() => "MasterContract";
  }
}
