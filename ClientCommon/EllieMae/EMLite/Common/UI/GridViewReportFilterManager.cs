// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.GridViewReportFilterManager
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class GridViewReportFilterManager(Sessions.Session session, GridView gridView) : 
    GridViewFilterManager(session, gridView)
  {
    public bool IsGlobalSearchOn { get; set; }

    public void CreateColumnFilters(ReportFieldDefs fieldDefs)
    {
      List<Control> controlList = new List<Control>((IEnumerable<Control>) this.FilterControls);
      this.FilterControls.Clear();
      int columnIndex = 0;
      foreach (GVColumn column in this.GridView.Columns)
      {
        if (column.FilterControl == null)
        {
          if (column.Tag != null)
          {
            TableLayout.Column tag = (TableLayout.Column) column.Tag;
            ReportFieldDef fieldByCriterionName = fieldDefs.GetFieldByCriterionName(tag.ColumnID);
            if (fieldByCriterionName != null)
              this.CreateColumnFilter(columnIndex, fieldByCriterionName);
          }
        }
        else
          this.FilterControls.Add(column.FilterControl);
        ++columnIndex;
      }
      foreach (Control c in controlList)
      {
        if (!this.FilterControls.Contains(c))
          this.ReleaseControl(c);
      }
    }

    public Control CreateColumnFilter(int columnIndex, ReportFieldDef fieldDef)
    {
      FieldFormat format = FieldTypeUtilities.FieldTypeToFieldFormat(fieldDef.FieldType);
      if (fieldDef.FieldDefinition != null)
      {
        format = fieldDef.FieldDefinition.Format;
        if (fieldDef.FieldDefinition.Options.RequireValueFromList)
          format = FieldFormat.DROPDOWNLIST;
        else if (fieldDef.FieldDefinition.Options.Count > 0)
          format = FieldFormat.DROPDOWN;
      }
      Control columnFilter = this.CreateColumnFilter(columnIndex, format, fieldDef.DisplayType);
      if (fieldDef.FieldDefinition != null && columnFilter.GetType() == typeof (ComboBox))
      {
        ComboBox comboBox = (ComboBox) columnFilter;
        comboBox.Items.Clear();
        comboBox.Items.Add((object) "");
        comboBox.Items.AddRange((object[]) fieldDef.FieldDefinition.Options.ToArray());
      }
      return columnFilter;
    }

    public Control CreateColumnFilter(
      int columnIndex,
      FieldFormat format,
      FieldDisplayType displayType)
    {
      switch (displayType)
      {
        case FieldDisplayType.Milestone:
          return this.CreateColumnFilter(columnIndex, GridViewFilterControlType.Milestone);
        case FieldDisplayType.CoreMilestone:
          return this.CreateColumnFilter(columnIndex, GridViewFilterControlType.CoreMilestone);
        case FieldDisplayType.RateLock:
          return this.CreateColumnFilter(columnIndex, GridViewFilterControlType.RateLock);
        case FieldDisplayType.ContactGroup:
          return this.CreateColumnFilter(columnIndex, GridViewFilterControlType.ContactGroup);
        case FieldDisplayType.BizContactGroup:
          return this.CreateColumnFilter(columnIndex, GridViewFilterControlType.BizContactGroup);
        case FieldDisplayType.PublicContactGroup:
          return this.CreateColumnFilter(columnIndex, GridViewFilterControlType.PublicContactGroup);
        case FieldDisplayType.RateLockAndRequest:
          return this.CreateColumnFilter(columnIndex, GridViewFilterControlType.RateLockAndRequest);
        case FieldDisplayType.NextMilestone:
          return this.CreateColumnFilter(columnIndex, GridViewFilterControlType.NextMilestone);
        case FieldDisplayType.LockValidationStatus:
          return this.CreateColumnFilter(columnIndex, GridViewFilterControlType.LockValidationStatus);
        default:
          return this.CreateColumnFilter(columnIndex, format);
      }
    }

    public void DisableFilterControl(List<int> idxs)
    {
      int num = 0;
      foreach (Control filterControl in this.FilterControls)
      {
        filterControl.Text = string.Empty;
        if (!idxs.Contains(num++))
          filterControl.Enabled = false;
      }
    }

    public void EnableFilterControl()
    {
      foreach (Control filterControl in this.FilterControls)
        filterControl.Enabled = true;
    }
  }
}
