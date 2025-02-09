// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.CorrespondentTradeReportFieldDefs
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class CorrespondentTradeReportFieldDefs : ReportFieldDefs
  {
    internal const string FieldPrefix = "CorrespondentTrade";

    public CorrespondentTradeReportFieldDefs()
      : base(Session.DefaultInstance)
    {
    }

    private CorrespondentTradeReportFieldDefs(string fileName)
      : base(Session.DefaultInstance, fileName)
    {
    }

    internal override ReportFieldDef CreateReportFieldDef(string category, XmlElement fieldElement)
    {
      return (ReportFieldDef) new CorrespondentTradeReportFieldDef(category, fieldElement);
    }

    internal override ReportFieldDef CreateReportFieldDef(FieldDefinition fieldDef)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public CorrespondentTradeReportFieldDef this[int index]
    {
      get => (CorrespondentTradeReportFieldDef) this.fieldDefs[index];
    }

    public CorrespondentTradeReportFieldDef GetFieldByID(string fieldId)
    {
      return this.fieldIdLookup.ContainsKey(fieldId) ? (CorrespondentTradeReportFieldDef) this.fieldIdLookup[fieldId] : (CorrespondentTradeReportFieldDef) null;
    }

    public CorrespondentTradeReportFieldDef GetFieldByCriterionName(string dbname)
    {
      return this.dbnameLookup.ContainsKey(dbname) ? (CorrespondentTradeReportFieldDef) this.dbnameLookup[dbname] : (CorrespondentTradeReportFieldDef) null;
    }

    public static CorrespondentTradeReportFieldDefs GetFieldDefs(
      Sessions.Session session,
      bool allowPublishEvent,
      bool includeLoanFields = true)
    {
      CorrespondentTradeReportFieldDefs source1 = new CorrespondentTradeReportFieldDefs();
      foreach (CorrespondentTradeReportFieldDef fieldDef in (ReportFieldDefContainer) new CorrespondentTradeReportFieldDefs("CorrespondentTradesMap.xml"))
      {
        fieldDef.Category = "Correspondent Trade";
        fieldDef.FieldID = "CorrespondentTrade." + fieldDef.FieldID;
        source1.Add((ReportFieldDef) fieldDef);
      }
      if (includeLoanFields)
      {
        foreach (LoanReportFieldDef fieldDef in (ReportFieldDefContainer) LoanReportFieldDefs.GetFieldDefs(session, false, LoanReportFieldFlags.AllDatabaseFields))
          source1.Add((ReportFieldDef) fieldDef);
      }
      if (session != null && !allowPublishEvent)
      {
        IEnumerable<ReportFieldDef> source2 = source1.Where<ReportFieldDef>((Func<ReportFieldDef, bool>) (field => field.FieldID.Equals("CorrespondentTrade.LastPublishedDateTime"))).Select<ReportFieldDef, ReportFieldDef>((Func<ReportFieldDef, ReportFieldDef>) (field => field));
        if (source2.Count<ReportFieldDef>() > 0)
          source1.Remove(source2.First<ReportFieldDef>());
        IEnumerable<ReportFieldDef> source3 = source1.Where<ReportFieldDef>((Func<ReportFieldDef, bool>) (field => field.FieldID.Equals("CorrespondentTrade.Status"))).Select<ReportFieldDef, ReportFieldDef>((Func<ReportFieldDef, ReportFieldDef>) (field => field));
        if (source3.Count<ReportFieldDef>() > 0)
        {
          FieldOptionCollection optionCollection = new FieldOptionCollection();
          FieldOption optionByValue = source3.First<ReportFieldDef>().FieldDefinition.Options.GetOptionByValue(8.ToString());
          if (optionByValue != null)
            source3.First<ReportFieldDef>().FieldDefinition.Options.RemoveOption(optionByValue);
        }
      }
      return source1;
    }

    public override string GetFieldPrefix() => "CorrespondentTrade";
  }
}
