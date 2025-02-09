// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.ThinThick.TradingScreens.MbsPools
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common.ThinThick;
using EllieMae.EMLite.Common.ThinThick.Requests.Trading;
using EllieMae.EMLite.Common.ThinThick.Responses;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.MainUI.ThinThick.TradingScreens.Interfaces;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Reporting;
using EllieMae.EMLite.Serialization;
using EllieMae.EMLite.Trading;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI.ThinThick.TradingScreens
{
  public class MbsPools : ITradingList, ITradingScreen
  {
    private readonly MbsPoolReportFieldDefs tradeFieldDefs;
    private FieldFilterList advFilter;
    private TradeView _currentView;

    public CurrentOrArchived CurrentOrArchived { get; set; }

    public MbsPoolReportFieldDefs FieldDefs => this.tradeFieldDefs;

    public QueryCriterion Filter { get; set; }

    public SortField[] SortFields { get; set; }

    public string[] Fields { get; set; }

    public MbsPools()
    {
      this.CurrentOrArchived = CurrentOrArchived.Current;
      this.tradeFieldDefs = MbsPoolReportFieldDefs.GetFieldDefs();
    }

    public ToolStripItemCollection ToolStripItemCollection
    {
      get => MainForm.Instance.TradingMenu_MbsPools;
    }

    public string MenuName => "Trades - MbsPoolList";

    public int[] Ids { get; set; }

    public void SetIds(int[] ids) => this.Ids = ids;

    public void SetSelectedPipelineView(string xml)
    {
      this._currentView = (TradeView) new XmlSerializer().Deserialize(xml, typeof (TradeView));
      this.CurrentOrArchived = this._currentView.ViewActive ? CurrentOrArchived.Current : CurrentOrArchived.Archived;
    }

    public bool IsCurrentViewSet => this._currentView != null;

    public TableLayout Layout => this._currentView.Layout;

    public int ItemCount => this.Ids.Length;

    public ListValueCriterion FilterById
    {
      get => new ListValueCriterion("MbsPoolDetails.TradeID", (Array) this.Ids);
    }

    public QueryCriterion SubViewCriterion
    {
      get
      {
        return this.CurrentOrArchived == CurrentOrArchived.Current ? (QueryCriterion) new ListValueCriterion("MbsPoolDetails.Status", (Array) new int[5]
        {
          2,
          0,
          1,
          4,
          3
        }, true) : (QueryCriterion) new ListValueCriterion("MbsPoolDetails.Status", (Array) new int[1]
        {
          5
        }, true);
      }
    }

    public ICursor GetTradingCursor(bool exportAll)
    {
      this.Fields = this.generateFieldList();
      this.Filter = this.generateQueryCriteria();
      QueryCriterion subViewCriterion = this.SubViewCriterion;
      if (!exportAll)
        this.Filter = (QueryCriterion) this.FilterById;
      this.Filter = this.Filter != null ? this.Filter.And(subViewCriterion) : subViewCriterion;
      TableLayout.Column[] columnsByPriority = this.Layout.GetSortColumnsByPriority();
      List<SortField> sortFieldList = new List<SortField>();
      if (columnsByPriority.Length != 0)
      {
        for (int index = 0; index < columnsByPriority.Length; ++index)
          sortFieldList.Add(new SortField(columnsByPriority[index].ColumnID, columnsByPriority[index].SortOrder == SortOrder.Ascending ? FieldSortOrder.Ascending : FieldSortOrder.Descending));
      }
      this.SortFields = sortFieldList.ToArray();
      if (this.Filter == null)
        return Session.MbsPoolManager.OpenTradeCursor(new QueryCriterion[0], this.SortFields, this.Fields, true, false);
      return Session.MbsPoolManager.OpenTradeCursor(new QueryCriterion[1]
      {
        this.Filter
      }, this.SortFields, this.Fields, true, false);
    }

    private QueryCriterion generateQueryCriteria()
    {
      QueryCriterion criterion = (QueryCriterion) null;
      if (this.advFilter != null)
        criterion = this.advFilter.CreateEvaluator().ToQueryCriterion();
      QueryCriterion queryCriteria = (QueryCriterion) null;
      if (criterion != null)
        queryCriteria = queryCriteria == null ? criterion : queryCriteria.And(criterion);
      return queryCriteria;
    }

    private string[] generateFieldList()
    {
      List<string> stringList = new List<string>();
      foreach (TableLayout.Column column in this.Layout)
      {
        TradeReportFieldDef fieldByCriterionName = this.tradeFieldDefs.GetFieldByCriterionName(column.ColumnID);
        if (!stringList.Contains(column.ColumnID))
          stringList.Add(column.ColumnID);
        if (fieldByCriterionName != null)
        {
          foreach (string relatedField in fieldByCriterionName.RelatedFields)
          {
            if (!stringList.Contains(relatedField))
              stringList.Add(relatedField);
          }
        }
      }
      return stringList.ToArray();
    }

    public OpSimpleResponse ExportToExcel(bool exportAll, bool print)
    {
      OpSimpleResponse excel = new OpSimpleResponse();
      if (!MainScreen.Instance.TradingBrowser.IsCurrentViewSet)
      {
        excel.ErrorCode = ErrorCodes.ViewNotSet;
        excel.ErrorMessage = "Pipeline View is not set.";
        return excel;
      }
      TableLayout layout = MainScreen.Instance.TradingBrowser.Layout;
      if (layout.ColumnCount > ExcelHandler.GetMaximumColumnCount())
      {
        excel.ErrorCode = ErrorCodes.MaxColCountExceeded;
        excel.ErrorMessage = "You pipeline cannot be exported because the number of columns exceeds the limit supported by Excel (" + (object) ExcelHandler.GetMaximumColumnCount() + ")";
        return excel;
      }
      if (MainScreen.Instance.TradingBrowser.ItemCount > ExcelHandler.GetMaximumRowCount() - 1)
      {
        excel.ErrorCode = ErrorCodes.MaxRowCountExceeded;
        excel.ErrorMessage = "You pipeline cannot be exported because the number of rows exceeds the limit supported by Excel (" + (object) ExcelHandler.GetMaximumRowCount() + ")";
        return excel;
      }
      int itemCount = this.GetTradingCursor(exportAll).GetItemCount();
      if (itemCount == 0)
      {
        excel.ErrorCode = ErrorCodes.DataNotFound;
        excel.ErrorMessage = "There are no trades to export";
        return excel;
      }
      ExcelHandler excelHandler = new ExcelHandler();
      IEnumerator enumerator1 = (IEnumerator) layout.GetEnumerator();
      while (enumerator1.MoveNext())
      {
        TableLayout.Column current = (TableLayout.Column) enumerator1.Current;
        excelHandler.AddHeaderColumn(current.Title, (ReportFieldDef) this.FieldDefs.GetFieldByCriterionName(current.Tag));
      }
      MbsPoolViewModel[] items = (MbsPoolViewModel[]) this.GetTradingCursor(exportAll).GetItems(0, itemCount);
      List<List<string>> dataList = new List<List<string>>();
      foreach (MbsPoolViewModel mbsPoolViewModel in items)
      {
        List<string> stringList = new List<string>();
        IEnumerator enumerator2 = (IEnumerator) layout.GetEnumerator();
        while (enumerator2.MoveNext())
        {
          TableLayout.Column current = (TableLayout.Column) enumerator2.Current;
          string str = mbsPoolViewModel[current.Tag] == null ? string.Empty : mbsPoolViewModel[current.Tag].ToString();
          TradeReportFieldDef fieldByCriterionName = this.FieldDefs.GetFieldByCriterionName(current.Tag);
          stringList.Add(fieldByCriterionName.ToDisplayValue(str));
        }
        dataList.Add(stringList);
      }
      excelHandler.AddDataTable(dataList);
      if (print)
        excelHandler.Print();
      else
        excelHandler.CreateExcel();
      return excel;
    }
  }
}
