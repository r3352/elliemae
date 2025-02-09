// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.DataTableImportRawData
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public class DataTableImportRawData : 
    IRawImportData,
    IEnumerator<CellData>,
    IDisposable,
    IEnumerator
  {
    private Dictionary<FieldDefinition, List<CsvFieldValue>> _rawDataTableData;
    private FieldDefinition[] _columns;
    private int _rowCount;
    private int _columnCount;
    private int _currentColumnIndex;
    private int _currentRowIndex;
    private bool _iteratorInitialized;
    private FieldSettings _fieldSettings;
    private StandardFields _standardFields;
    private CellData _currentCell;

    public DataTableImportRawData(
      string[] columns,
      int rowCount,
      Dictionary<string, List<CsvFieldValue>> rawDataTableData)
    {
      this._rawDataTableData = new Dictionary<FieldDefinition, List<CsvFieldValue>>();
      this._columns = new FieldDefinition[columns.Length];
      this._rowCount = rowCount;
      this._columnCount = columns.Length;
      int index = 0;
      foreach (string column in columns)
      {
        FieldDefinition field = EncompassFields.GetField(column);
        List<CsvFieldValue> csvFieldValueList = rawDataTableData[column];
        this._columns[index] = field;
        this._rawDataTableData.Add(field, csvFieldValueList);
        ++index;
      }
    }

    public DataTableImportRawData(
      DDMDataTableInfo dataTableInfo,
      int rowCount,
      Dictionary<string, List<string>> rawDataTableData,
      Dictionary<string, List<string>> rawOutputColDataTableData,
      StandardFields standardFields,
      FieldSettings fieldSettings)
    {
      this._rawDataTableData = new Dictionary<FieldDefinition, List<CsvFieldValue>>();
      this._fieldSettings = fieldSettings;
      this._standardFields = standardFields;
      this._columns = new FieldDefinition[dataTableInfo.Fields.Length];
      this._rowCount = rowCount;
      this._columnCount = this._columns.Length;
      int index = 0;
      foreach (DDMDataTableFieldInfo field in dataTableInfo.Fields)
      {
        if (!field.IsOutput)
        {
          List<CsvFieldValue> dtFieldVal = this.mapFieldValToDTFieldVal(rawDataTableData[field.FieldId.ToUpperInvariant()]);
          FieldDefinition key = EncompassFields.GetField(field.FieldId, this._fieldSettings);
          if (key == null && this._standardFields.VirtualFields.Contains(field.FieldId))
            key = this._standardFields.VirtualFields[field.FieldId];
          if (key != null && key.Category != FieldCategory.Common)
            key = (FieldDefinition) new CustomField(new CustomFieldInfo(field.FieldId)
            {
              Description = key.Description,
              Format = key.Format,
              MaxLength = key.MaxLength
            });
          this._columns[index] = key;
          this._rawDataTableData.Add(key, dtFieldVal);
          ++index;
        }
        else
        {
          List<CsvFieldValue> csvFieldValueList;
          if (!rawOutputColDataTableData.ContainsKey(field.FieldId.ToUpperInvariant()))
          {
            csvFieldValueList = new List<CsvFieldValue>();
            csvFieldValueList.Add(new CsvFieldValue()
            {
              Data = string.Empty,
              IsOutput = true
            });
          }
          else
          {
            csvFieldValueList = this.mapFieldValToDTFieldVal(rawOutputColDataTableData[field.FieldId.ToUpperInvariant()]);
            foreach (CsvFieldValue csvFieldValue in csvFieldValueList)
            {
              csvFieldValue.IsOutput = true;
              csvFieldValue.Data = csvFieldValue.Data.Replace("\"\"", "\"");
            }
          }
          FieldDefinition key = (FieldDefinition) new CustomField(new CustomFieldInfo(field.FieldId)
          {
            Description = "",
            Format = FieldFormat.NONE,
            MaxLength = 120
          });
          this._columns[index] = key;
          this._rawDataTableData.Add(key, csvFieldValueList);
          ++index;
        }
      }
    }

    public bool HasData() => this._rowCount != 0;

    private CellData SetCurrentCellData()
    {
      if (this._currentColumnIndex == -1 && this._currentRowIndex == -1)
      {
        this._currentCell = new CellData(-1, -1, (string) null);
        return this._currentCell;
      }
      this._currentCell = new CellData(this._currentRowIndex, this._currentColumnIndex, this._rawDataTableData[this._columns[this._currentColumnIndex]][this._currentRowIndex].Data);
      return this._currentCell;
    }

    public FieldDefinition[] GetColumnsInformation() => this._columns;

    public Dictionary<FieldDefinition, List<CsvFieldValue>> GetRawDataColumnized()
    {
      return this._rawDataTableData;
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

    public List<CsvFieldValue> GetRowDataUnformatted(int rowNumber)
    {
      List<CsvFieldValue> rowDataUnformatted = new List<CsvFieldValue>();
      foreach (FieldDefinition column in this._columns)
        rowDataUnformatted.Add(this._rawDataTableData[column][rowNumber]);
      return rowDataUnformatted;
    }

    public List<CellData> GetRowData(int rowNumber)
    {
      List<CellData> rowData = new List<CellData>();
      for (int column1 = 0; column1 < this._columnCount; ++column1)
      {
        FieldDefinition column2 = this._columns[column1];
        CellData cellData = new CellData(rowNumber, column1, this._rawDataTableData[column2][rowNumber].Data);
        rowData.Add(cellData);
      }
      return rowData;
    }

    public void Reset()
    {
      this._iteratorInitialized = false;
      this._currentColumnIndex = this._currentRowIndex = -1;
      this.SetCurrentCellData();
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

    private List<CsvFieldValue> mapFieldValToDTFieldVal(List<string> fieldVal)
    {
      List<CsvFieldValue> dtFieldVal = new List<CsvFieldValue>();
      foreach (string str in fieldVal)
        dtFieldVal.Add(new CsvFieldValue()
        {
          Data = str,
          IsOutput = false
        });
      return dtFieldVal;
    }
  }
}
