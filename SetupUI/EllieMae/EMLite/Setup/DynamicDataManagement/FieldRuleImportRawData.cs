// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.FieldRuleImportRawData
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public class FieldRuleImportRawData : 
    IRawImportData,
    IEnumerator<CellData>,
    IDisposable,
    IEnumerator
  {
    private FieldSettings _fieldSettings;
    private StandardFields _standardFields;
    private Dictionary<FieldDefinition, List<CsvFieldValue>> _rawFieldRuleData;
    private int _columnCount;
    private int _rowCount;
    private FieldDefinition[] _columns;
    private int _currentColumnIndex;
    private int _currentRowIndex;
    private bool _iteratorInitialized;
    private CellData _currentCell;

    public FieldRuleImportRawData(
      Dictionary<string, List<string>> rawFieldRuleData,
      StandardFields standardFields,
      FieldSettings fieldSettings)
    {
      this._fieldSettings = fieldSettings;
      this._standardFields = standardFields;
      this._rawFieldRuleData = this.ConvertToRawData(rawFieldRuleData);
      this._columns = this._rawFieldRuleData.Keys.ToArray<FieldDefinition>();
      this._columnCount = this._rawFieldRuleData.Keys.Count;
      this._rowCount = this._rawFieldRuleData[this._rawFieldRuleData.Keys.First<FieldDefinition>()].Count;
    }

    private Dictionary<FieldDefinition, List<CsvFieldValue>> ConvertToRawData(
      Dictionary<string, List<string>> rawFieldRuleData)
    {
      Dictionary<FieldDefinition, List<CsvFieldValue>> rawData = new Dictionary<FieldDefinition, List<CsvFieldValue>>();
      foreach (KeyValuePair<string, List<string>> keyValuePair in rawFieldRuleData)
      {
        FieldDefinition key = !keyValuePair.Key.ToString().StartsWith("CX.") ? EncompassFields.GetField(keyValuePair.Key) : EncompassFields.GetField(keyValuePair.Key, this._fieldSettings);
        List<CsvFieldValue> csvFieldValueList = new List<CsvFieldValue>();
        foreach (string str in keyValuePair.Value)
          csvFieldValueList.Add(new CsvFieldValue()
          {
            Data = str,
            IsOutput = false
          });
        rawData.Add(key, csvFieldValueList);
      }
      return rawData;
    }

    public bool HasData() => this._rawFieldRuleData.Count != 0;

    public FieldDefinition[] GetColumnsInformation() => this._columns;

    public Dictionary<FieldDefinition, List<CsvFieldValue>> GetRawDataColumnized()
    {
      return this._rawFieldRuleData;
    }

    public List<CsvFieldValue> GetRowDataUnformatted(int rowNumber)
    {
      if (rowNumber < 0 || rowNumber > this._rowCount)
        return new List<CsvFieldValue>();
      List<CsvFieldValue> rowDataUnformatted = new List<CsvFieldValue>();
      foreach (KeyValuePair<FieldDefinition, List<CsvFieldValue>> keyValuePair in this._rawFieldRuleData)
        rowDataUnformatted.Add(this._rawFieldRuleData[keyValuePair.Key][rowNumber]);
      return rowDataUnformatted;
    }

    public List<CellData> GetRowData(int rowNumber)
    {
      if (rowNumber < 0 || rowNumber > this._rowCount)
        return new List<CellData>();
      List<CellData> rowData = new List<CellData>();
      int column = 0;
      foreach (KeyValuePair<FieldDefinition, List<CsvFieldValue>> keyValuePair in this._rawFieldRuleData)
      {
        CellData cellData = new CellData(rowNumber, column, this._rawFieldRuleData[keyValuePair.Key][rowNumber].Data);
        ++column;
      }
      return rowData;
    }

    public int RowCount() => this._rowCount;

    public int ColumnCount() => this._columnCount;

    public object[] GetCellMetadata(CellData cellData)
    {
      List<string> stringList = new List<string>();
      FieldDefinition column = this._columns[cellData.Column];
      stringList.Add(string.Format("[{0}] - {1}", (object) column.FieldID, (object) column.Description));
      if (cellData.Criteria == DDMCriteria.Range || cellData.Criteria == DDMCriteria.ListOfValues)
      {
        stringList.Add(cellData.Data);
      }
      else
      {
        string cellDataType = this.GetCellDataType(column.Format);
        stringList.Add(cellDataType);
      }
      return (object[]) stringList.ToArray();
    }

    private string GetCellDataType(FieldFormat fieldFormat)
    {
      switch (fieldFormat)
      {
        case FieldFormat.NONE:
        case FieldFormat.STRING:
        case FieldFormat.RA_STRING:
        case FieldFormat.RA_INTEGER:
        case FieldFormat.RA_DECIMAL_2:
        case FieldFormat.RA_DECIMAL_3:
          return string.Empty;
        case FieldFormat.YN:
          return "options. Available options: Yes|No";
        case FieldFormat.X:
          return "values. Available options: 1|0";
        case FieldFormat.ZIPCODE:
          return "zipcode";
        case FieldFormat.STATE:
          return "state";
        case FieldFormat.PHONE:
          return "phone number format";
        case FieldFormat.SSN:
          return "SSN format";
        case FieldFormat.INTEGER:
        case FieldFormat.DECIMAL_1:
        case FieldFormat.DECIMAL_2:
        case FieldFormat.DECIMAL_3:
        case FieldFormat.DECIMAL_4:
        case FieldFormat.DECIMAL_6:
        case FieldFormat.DECIMAL_5:
        case FieldFormat.DECIMAL_7:
        case FieldFormat.DECIMAL:
        case FieldFormat.DECIMAL_10:
          return "number";
        case FieldFormat.DATE:
          return "date";
        case FieldFormat.MONTHDAY:
          return "monthday";
        case FieldFormat.DATETIME:
          return "date & time";
        case FieldFormat.DROPDOWNLIST:
          return "dropdownlist";
        case FieldFormat.DROPDOWN:
          return "dropdown";
        case FieldFormat.AUDIT:
          return "audit";
        default:
          return string.Empty;
      }
    }

    public CellData Current
    {
      get
      {
        if (!this._iteratorInitialized)
          throw new InvalidOperationException("Iterator is not initialized. Call MoveNext to begin iterator.");
        return this._currentCell;
      }
    }

    private CellData SetCurrentCellData()
    {
      if (this._currentColumnIndex == -1 && this._currentRowIndex == -1)
      {
        this._currentCell = new CellData(-1, -1, (string) null);
        return this._currentCell;
      }
      this._currentCell = new CellData(this._currentRowIndex, this._currentColumnIndex, this._rawFieldRuleData[this._columns[this._currentColumnIndex]][this._currentRowIndex].Data);
      return this._currentCell;
    }

    public void Dispose()
    {
    }

    object IEnumerator.Current => (object) this.Current;

    public bool MoveNext()
    {
      if (!this._iteratorInitialized)
      {
        this._iteratorInitialized = true;
        this._currentColumnIndex = this._currentRowIndex = 0;
        this.SetCurrentCellData();
        return true;
      }
      if (this._currentCell.CompareTo(CellData.InvalidCell) == 0)
        return false;
      if (this._currentColumnIndex == this._columns.Length - 1 && this._currentRowIndex == this._rowCount - 1)
      {
        this._currentRowIndex = this._currentColumnIndex = -1;
        this.SetCurrentCellData();
        return false;
      }
      if (this._currentColumnIndex == this._columns.Length - 1 && this._currentRowIndex < this._rowCount - 1)
      {
        this._currentColumnIndex = 0;
        ++this._currentRowIndex;
        this.SetCurrentCellData();
        return true;
      }
      ++this._currentColumnIndex;
      this.SetCurrentCellData();
      return true;
    }

    public void Reset()
    {
      this._iteratorInitialized = false;
      this._currentColumnIndex = this._currentRowIndex = -1;
      this.SetCurrentCellData();
    }
  }
}
