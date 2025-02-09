// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.GridViewStatusFilterManager
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.DataDocs;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public class GridViewStatusFilterManager(Sessions.Session session, GridView gridView) : 
    GridViewFilterManager(session, gridView)
  {
    public Control CreateColumnFilter(
      int columnIndex,
      FieldFormat format,
      DeliveryStatus[] statuses,
      bool isStatusField = false)
    {
      if (!isStatusField)
        return this.CreateColumnFilter(columnIndex, format);
      ComboBox columnFilter = (ComboBox) this.CreateColumnFilter(columnIndex, GridViewFilterControlType.Dropdown);
      columnFilter.Items.Clear();
      columnFilter.Items.Add((object) "");
      foreach (DeliveryStatus statuse in statuses)
        columnFilter.Items.Add((object) DataDocsConstants.DeliveryStatusToString(statuse));
      return (Control) columnFilter;
    }

    public Control CreateColumnFilter(
      int columnIndex,
      FieldFormat format,
      SubmissionType[] statuses,
      bool isStatusField = false)
    {
      if (!isStatusField)
        return this.CreateColumnFilter(columnIndex, format);
      ComboBox columnFilter = (ComboBox) this.CreateColumnFilter(columnIndex, GridViewFilterControlType.Dropdown);
      columnFilter.Items.Clear();
      columnFilter.Items.Add((object) "");
      foreach (SubmissionType statuse in statuses)
        columnFilter.Items.Add((object) DataDocsConstants.SubmissionTypeToString(statuse));
      return (Control) columnFilter;
    }
  }
}
