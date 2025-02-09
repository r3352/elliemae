// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DashBoard.DashboardTemplateValidator
// Assembly: DashBoard, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 99BFBD49-67F8-470C-81BC-FC4FAEA6C933
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DashBoard.dll

using EllieMae.EMLite.ClientServer.Dashboard;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common.UI;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.DashBoard
{
  public class DashboardTemplateValidator
  {
    private LoanReportFieldDefs fieldDefinitions;

    public DashboardTemplateValidator(LoanReportFieldDefs fieldDefinitions)
    {
      this.fieldDefinitions = fieldDefinitions;
    }

    public bool Validate(DashboardTemplate dashboardTemplate, out string errorMessage)
    {
      bool flag = false;
      errorMessage = string.Empty;
      if (this.fieldDefinitions == null || dashboardTemplate == null)
        return flag;
      List<string> errorList = new List<string>();
      switch (dashboardTemplate.ChartType)
      {
        case DashboardChartType.BarChart:
          this.validateBarChartTemplate(dashboardTemplate, errorList);
          break;
        case DashboardChartType.TrendChart:
          this.validateTrendChartTemplate(dashboardTemplate, errorList);
          break;
        case DashboardChartType.LoanTable:
          this.validateLoanTableTemplate(dashboardTemplate, errorList);
          break;
        case DashboardChartType.UserTable:
          this.validateUserTableTemplate(dashboardTemplate, errorList);
          break;
        default:
          errorList.Add("The Chart Type selected on the Snapshot tab is invalid.");
          break;
      }
      if (dashboardTemplate.Folders.Count == 0)
        errorList.Add("There are no Folders selected on the Folders tab");
      if (0 < dashboardTemplate.Filters.Count)
      {
        foreach (FieldFilter filter in (List<FieldFilter>) dashboardTemplate.Filters)
        {
          if (this.fieldDefinitions.GetFieldByCriterionName(filter.CriterionName) == null)
          {
            errorList.Add("The filter references one or more fields which are no longer available or accessible.");
            break;
          }
        }
      }
      if (0 >= errorList.Count)
        return true;
      Encoding encoding = Encoding.Default;
      byte[] bytes = new byte[1]{ (byte) 149 };
      string str1 = Encoding.GetEncoding(1252).GetString(bytes);
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("The selected snapshot has some invalid or missing selections.");
      stringBuilder.AppendLine("Use the Manage Snapshots screen to correct these problems:\n");
      foreach (string str2 in errorList)
        stringBuilder.AppendLine(str1 + " " + str2 + ".");
      errorMessage = stringBuilder.ToString();
      return false;
    }

    private void validateBarChartTemplate(
      DashboardTemplate dashboardTemplate,
      List<string> errorList)
    {
      if (this.fieldDefinitions.GetFieldByCriterionName(dashboardTemplate.XAxisField) == null)
        errorList.Add("The field selected for the X axis is no longer available or is inaccessible.");
      if ((dashboardTemplate.YAxisSummaryType == ColumnSummaryType.Average || dashboardTemplate.YAxisSummaryType == ColumnSummaryType.Total) && this.fieldDefinitions.GetFieldByCriterionName(dashboardTemplate.YAxisField) == null)
        errorList.Add("The field selected for the Y axis is no longer available or is inaccessible.");
      if (!(dashboardTemplate.TimeFrameField != "") || this.fieldDefinitions.GetFieldByCriterionName(dashboardTemplate.TimeFrameField) != null)
        return;
      errorList.Add("The field selected for the date range is no longer available or is inaccessible.");
    }

    private void validateTrendChartTemplate(
      DashboardTemplate dashboardTemplate,
      List<string> errorList)
    {
      if (dashboardTemplate.GroupByField != "" && dashboardTemplate.GroupByField != "Dashboard.NoGroupBy" && this.fieldDefinitions.GetFieldByCriterionName(dashboardTemplate.GroupByField) == null)
        errorList.Add("The field selected for data grouping is no longer available or is inaccessible.");
      if (dashboardTemplate.XAxisField != "" && this.fieldDefinitions.GetFieldByCriterionName(dashboardTemplate.XAxisField) == null)
        errorList.Add("The field selected for the X axis is no longer available or is inaccessible.");
      if (dashboardTemplate.YAxisSummaryType != ColumnSummaryType.Average && dashboardTemplate.YAxisSummaryType != ColumnSummaryType.Total || this.fieldDefinitions.GetFieldByCriterionName(dashboardTemplate.YAxisField) != null)
        return;
      errorList.Add("The field selected for the Y axis is no longer available or is inaccessible.");
    }

    private void validateLoanTableTemplate(
      DashboardTemplate dashboardTemplate,
      List<string> errorList)
    {
      foreach (ColumnInfo field in dashboardTemplate.Fields)
      {
        if (this.fieldDefinitions.GetFieldByCriterionName(field.CriterionName) == null)
          errorList.Add("The field '" + field.Description + "' is no longer available or is inaccessible.");
      }
      if (dashboardTemplate.Fields.Count != 0)
        return;
      errorList.Add("There are no Columns selected on the Snapshot tab");
    }

    private void validateUserTableTemplate(
      DashboardTemplate dashboardTemplate,
      List<string> errorList)
    {
      if (dashboardTemplate.SummaryField1 != "Dashboard.NoSummary" && dashboardTemplate.SummaryField1 != "Dashboard.LoanCount" && this.fieldDefinitions.GetFieldByCriterionName(dashboardTemplate.SummaryField1) == null)
        errorList.Add("This field selected for Column 1 is no longer available or is inaccessible.");
      if (dashboardTemplate.SummaryField2 != "Dashboard.NoSummary" && dashboardTemplate.SummaryField2 != "Dashboard.LoanCount" && this.fieldDefinitions.GetFieldByCriterionName(dashboardTemplate.SummaryField2) == null)
        errorList.Add("This field selected for Column 2 is no longer available or is inaccessible.");
      if (dashboardTemplate.SummaryField2 != "Dashboard.NoSummary" && dashboardTemplate.SummaryField2 != "Dashboard.LoanCount" && this.fieldDefinitions.GetFieldByCriterionName(dashboardTemplate.SummaryField2) == null)
        errorList.Add("This field selected for Column 3 is no longer available or is inaccessible.");
      if (!(dashboardTemplate.GroupByField != "") || !(dashboardTemplate.GroupByField != "Dashboard.NoGroupBy") || this.fieldDefinitions.GetFieldByCriterionName(dashboardTemplate.GroupByField) != null)
        return;
      errorList.Add("The field selected for data grouping is no longer available or is inaccessible.");
    }
  }
}
