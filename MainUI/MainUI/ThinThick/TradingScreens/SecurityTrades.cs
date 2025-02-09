// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.ThinThick.TradingScreens.SecurityTrades
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
  public class SecurityTrades : ITradingList, ITradingScreen
  {
    private readonly TradeReportFieldDefs tradeFieldDefs;
    private FieldFilterList advFilter;
    private SecurityTradeView _currentView;

    public CurrentOrArchived CurrentOrArchived { get; set; }

    public TradeReportFieldDefs FieldDefs => this.tradeFieldDefs;

    public QueryCriterion Filter { get; set; }

    public SortField[] SortFields { get; set; }

    public string[] Fields { get; set; }

    public SecurityTrades()
    {
      this.CurrentOrArchived = CurrentOrArchived.Current;
      this.tradeFieldDefs = (TradeReportFieldDefs) SecurityTradeReportFieldDefs.GetFieldDefs();
    }

    public ToolStripItemCollection ToolStripItemCollection
    {
      get => MainForm.Instance.TradingMenu_SecurityTrades;
    }

    public string MenuName => "Trades - SecurityTradeList";

    public int[] Ids { get; set; }

    public void SetIds(int[] ids) => this.Ids = ids;

    public void SetSelectedPipelineView(string xml)
    {
      this._currentView = (SecurityTradeView) new XmlSerializer().Deserialize(xml, typeof (SecurityTradeView));
    }

    public bool IsCurrentViewSet => this._currentView != null;

    public TableLayout Layout => this._currentView.Layout;

    public int ItemCount => this.Ids.Length;

    public ListValueCriterion FilterById
    {
      get => new ListValueCriterion("SecurityTradeDetails.TradeID", (Array) this.Ids);
    }

    public QueryCriterion SubViewCriterion
    {
      get
      {
        FieldFilterList fieldFilterList = new FieldFilterList();
        if (this.CurrentOrArchived == CurrentOrArchived.Current)
          fieldFilterList.Add(new FieldFilter(FieldTypes.IsNumeric, "Status", "Status", OperatorTypes.NotEqual, 5.ToString())
          {
            JointToken = JointTokens.And
          });
        else
          fieldFilterList.Add(new FieldFilter(FieldTypes.IsNumeric, "Status", "Status", OperatorTypes.Equals, 5.ToString())
          {
            JointToken = JointTokens.And
          });
        return fieldFilterList.CreateEvaluator().ToQueryCriterion();
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
        return Session.SecurityTradeManager.OpenTradeCursor(new QueryCriterion[0], this.SortFields, this.Fields, true, false);
      return Session.SecurityTradeManager.OpenTradeCursor(new QueryCriterion[1]
      {
        this.Filter
      }, this.SortFields, this.Fields, true, false);
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
      SecurityTradeViewModel[] items = (SecurityTradeViewModel[]) this.GetTradingCursor(exportAll).GetItems(0, itemCount);
      List<List<string>> dataList = new List<List<string>>();
      foreach (SecurityTradeViewModel securityTradeViewModel in items)
      {
        List<string> stringList = new List<string>();
        IEnumerator enumerator2 = (IEnumerator) layout.GetEnumerator();
        while (enumerator2.MoveNext())
        {
          TableLayout.Column current = (TableLayout.Column) enumerator2.Current;
          string str = securityTradeViewModel[current.Tag] == null ? string.Empty : securityTradeViewModel[current.Tag].ToString();
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
  }
}
