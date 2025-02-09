// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.CorrespondentMasterReportFieldDefs
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
  public class CorrespondentMasterReportFieldDefs : ReportFieldDefs
  {
    internal const string FieldPrefix = "CorrespondentMaster";

    public CorrespondentMasterReportFieldDefs()
      : base(Session.DefaultInstance)
    {
    }

    private CorrespondentMasterReportFieldDefs(string fileName)
      : base(Session.DefaultInstance, fileName)
    {
    }

    internal override ReportFieldDef CreateReportFieldDef(string category, XmlElement fieldElement)
    {
      return (ReportFieldDef) new CorrespondentMasterReportFieldDef(category, fieldElement);
    }

    internal override ReportFieldDef CreateReportFieldDef(FieldDefinition fieldDef)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public CorrespondentMasterReportFieldDef this[int index]
    {
      get => (CorrespondentMasterReportFieldDef) this.fieldDefs[index];
    }

    public CorrespondentMasterReportFieldDef GetFieldByID(string fieldId)
    {
      return this.fieldIdLookup.ContainsKey(fieldId) ? (CorrespondentMasterReportFieldDef) this.fieldIdLookup[fieldId] : (CorrespondentMasterReportFieldDef) null;
    }

    public CorrespondentMasterReportFieldDef GetFieldByCriterionName(string dbname)
    {
      return this.dbnameLookup.ContainsKey(dbname) ? (CorrespondentMasterReportFieldDef) this.dbnameLookup[dbname] : (CorrespondentMasterReportFieldDef) null;
    }

    public static CorrespondentMasterReportFieldDefs GetFieldDefs()
    {
      CorrespondentMasterReportFieldDefs fieldDefs = new CorrespondentMasterReportFieldDefs();
      foreach (CorrespondentMasterReportFieldDef fieldDef in (ReportFieldDefContainer) new CorrespondentMasterReportFieldDefs("CorrespondentMasterMap.xml"))
        fieldDefs.Add((ReportFieldDef) fieldDef);
      return fieldDefs;
    }

    public static CorrespondentMasterReportFieldDefs GetFieldDefs(
      Sessions.Session session,
      bool allowPublishEvent)
    {
      CorrespondentMasterReportFieldDefs fieldDefs = new CorrespondentMasterReportFieldDefs();
      foreach (CorrespondentMasterReportFieldDef fieldDef in (ReportFieldDefContainer) new CorrespondentMasterReportFieldDefs("CorrespondentMasterMap.xml"))
      {
        fieldDef.Category = "Correspondent Master";
        fieldDef.FieldID = "CorrespondentMaster." + fieldDef.FieldID;
        fieldDefs.Add((ReportFieldDef) fieldDef);
      }
      foreach (CorrespondentTradeReportFieldDef fieldDef in (ReportFieldDefContainer) CorrespondentTradeReportFieldDefs.GetFieldDefs(Session.DefaultInstance, allowPublishEvent, false))
      {
        fieldDef.Category = "Correspondent Trade";
        fieldDef.FieldID = "CorrespondentTrade." + fieldDef.FieldID;
        fieldDefs.Add((ReportFieldDef) fieldDef);
      }
      return fieldDefs;
    }

    public override string GetFieldPrefix() => "CorrespondentMaster";
  }
}
