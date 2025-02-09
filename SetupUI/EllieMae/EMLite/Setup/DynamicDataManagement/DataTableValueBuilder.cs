// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.DataTableValueBuilder
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public class DataTableValueBuilder
  {
    private DataTableImportRawData _rawDataTableData;

    public DataTableValueBuilder(DataTableImportRawData rawDataTableData)
    {
      this._rawDataTableData = rawDataTableData;
    }

    public List<DDMDataTableFieldValue> GetFieldValues(int rowNumber)
    {
      List<CellData> rowData = this._rawDataTableData.GetRowData(rowNumber);
      List<DDMDataTableFieldValue> fieldValues = new List<DDMDataTableFieldValue>();
      int columnId = 0;
      foreach (CellData cellData in rowData)
      {
        FieldDefinition fieldDefinition = this._rawDataTableData.GetColumnsInformation()[columnId];
        DDMCriteria tableValueCriteria = UtilsExtension.GetDataTableValueCriteria(cellData, fieldDefinition);
        string tableValuesSanitized = UtilsExtension.GetDataTableValuesSanitized(cellData);
        DDMDataTableFieldValue dataTableFieldValue = new DDMDataTableFieldValue(-1, -1, rowNumber, columnId, fieldDefinition.FieldID, tableValuesSanitized, (int) tableValueCriteria, tableValueCriteria == DDMCriteria.none);
        Dictionary<FieldDefinition, List<CsvFieldValue>> rawDataColumnized = this._rawDataTableData.GetRawDataColumnized();
        dataTableFieldValue.IsOutput = rawDataColumnized[fieldDefinition][rowNumber].IsOutput;
        fieldValues.Add(dataTableFieldValue);
        ++columnId;
      }
      return fieldValues;
    }
  }
}
