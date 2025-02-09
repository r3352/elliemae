// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ReportFieldCommonExtension
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public static class ReportFieldCommonExtension
  {
    public static int GetWidth(ReportFieldDef fieldDef) => -1;

    public static TableLayout.Column ToTableLayoutColumn(
      this ReportFieldDef thisObject,
      ReportFieldCommonExtension.GetFieldDefWidth action)
    {
      return ReportFieldCommonExtension.ReportFieldClientHelper.ToTableLayoutColumn(thisObject, action);
    }

    public static TableLayout.Column ToTableLayoutColumn(
      this ContactReportFieldDef thisObject,
      ReportFieldCommonExtension.GetFieldDefWidth action)
    {
      return thisObject.DisplayType == FieldDisplayType.ContactGroup ? new TableLayout.Column(thisObject.CriterionFieldName, thisObject.Name, thisObject.Description, HorizontalAlignment.Left, action((ReportFieldDef) thisObject)) : ReportFieldCommonExtension.ReportFieldClientHelper.ToTableLayoutColumn((ReportFieldDef) thisObject, action, typeof (ReportFieldDef));
    }

    public static TableLayout.Column ToTableLayoutColumn(
      this BizPartnerReportFieldDef thisObject,
      ReportFieldCommonExtension.GetFieldDefWidth action)
    {
      return ReportFieldCommonExtension.ToTableLayoutColumn(thisObject, action);
    }

    public static TableLayout.Column ToTableLayoutColumn(
      this BorrowerReportFieldDef thisObject,
      ReportFieldCommonExtension.GetFieldDefWidth action)
    {
      return ReportFieldCommonExtension.ToTableLayoutColumn(thisObject, action);
    }

    public static TableLayout.Column ToTableLayoutColumn(
      this LoanReportFieldDef thisObject,
      ReportFieldCommonExtension.GetFieldDefWidth action)
    {
      return ReportFieldCommonExtension.ToTableLayoutColumn(thisObject, action);
    }

    public static TableLayout.Column ToTableLayoutColumn(
      this MasterContractReportFieldDef thisObject,
      ReportFieldCommonExtension.GetFieldDefWidth action)
    {
      return ReportFieldCommonExtension.ToTableLayoutColumn(thisObject, action);
    }

    public static TableLayout.Column ToTableLayoutColumn(
      this CorrespondentMasterReportFieldDef thisObject,
      ReportFieldCommonExtension.GetFieldDefWidth action)
    {
      return ReportFieldCommonExtension.ToTableLayoutColumn(thisObject, action);
    }

    public static TableLayout.Column ToTableLayoutColumn(
      this CorrespondentTradeReportFieldDef thisObject,
      ReportFieldCommonExtension.GetFieldDefWidth action)
    {
      return ReportFieldCommonExtension.ToTableLayoutColumn(thisObject, action);
    }

    public static TableLayout.Column ToTableLayoutColumn(
      this TradeReportFieldDef thisObject,
      ReportFieldCommonExtension.GetFieldDefWidth action)
    {
      return ReportFieldCommonExtension.ToTableLayoutColumn(thisObject, action);
    }

    public delegate int GetFieldDefWidth(ReportFieldDef fieldDef);

    private class ReportFieldClientHelper
    {
      public static TableLayout.Column ToTableLayoutColumn(
        ReportFieldDef thisObject,
        ReportFieldCommonExtension.GetFieldDefWidth action,
        System.Type type = null)
      {
        if ((System.Type) null == type)
          type = thisObject.GetType();
        return typeof (ContactReportFieldDef) == type || typeof (BizPartnerReportFieldDef) == type || typeof (BorrowerReportFieldDef) == type ? ReportFieldCommonExtension.ToTableLayoutColumn((ContactReportFieldDef) thisObject, action) : new TableLayout.Column(thisObject.CriterionFieldName, thisObject.Name, thisObject.Description, thisObject.FieldDefinition.Format == FieldFormat.RA_STRING || thisObject.FieldDefinition.Format == FieldFormat.RA_INTEGER || thisObject.FieldDefinition.Format == FieldFormat.RA_DECIMAL_2 || thisObject.FieldDefinition.Format == FieldFormat.RA_DECIMAL_3 ? HorizontalAlignment.Right : (thisObject.FieldType == FieldTypes.IsNumeric ? HorizontalAlignment.Right : HorizontalAlignment.Left), action(thisObject));
      }
    }
  }
}
