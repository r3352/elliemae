// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.ImportErrorDataProvider
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.DataEngine;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public class ImportErrorDataProvider
  {
    private IRawImportData _rawImportData;
    private ImportProcessResult _importResult;

    public ImportErrorDataProvider(IRawImportData rawImportData, ImportProcessResult importResult)
    {
      this._rawImportData = rawImportData;
      this._importResult = importResult;
    }

    public List<List<string>> GetErrorDataForReport()
    {
      List<List<string>> errorDataForReport = new List<List<string>>();
      List<string> stringList1 = new List<string>();
      stringList1.Add("Error");
      IEnumerable<string> columnData = this.GetColumnData();
      stringList1.AddRange(columnData);
      errorDataForReport.Add(stringList1);
      for (int rowNumber = 0; rowNumber < this._rawImportData.RowCount(); ++rowNumber)
      {
        List<string> stringList2 = new List<string>();
        List<string> list = this.GetRowErrors(this._importResult.GetRowErrors(rowNumber)).ToList<string>();
        stringList2.Add(string.Join("|", (IEnumerable<string>) list));
        List<string> fieldVal = this.mapDTFieldValToFieldVal(this._rawImportData.GetRowDataUnformatted(rowNumber));
        stringList2.AddRange((IEnumerable<string>) fieldVal);
        errorDataForReport.Add(stringList2);
      }
      return errorDataForReport;
    }

    private IEnumerable<string> GetColumnData()
    {
      FieldDefinition[] fieldDefinitionArray = this._rawImportData.GetColumnsInformation();
      for (int index = 0; index < fieldDefinitionArray.Length; ++index)
        yield return fieldDefinitionArray[index].FieldID;
      fieldDefinitionArray = (FieldDefinition[]) null;
    }

    private IEnumerable<string> GetRowErrors(List<ImportMessage> errorMessages)
    {
      foreach (ImportMessage errorMessage in errorMessages)
      {
        if (errorMessage.ImportMessageType == ImportMessageType.Error)
          yield return errorMessage.Message(this._rawImportData.GetCellMetadata(errorMessage.CellData));
      }
    }

    private List<string> mapDTFieldValToFieldVal(List<CsvFieldValue> dtFieldVals)
    {
      List<string> fieldVal = new List<string>();
      foreach (CsvFieldValue dtFieldVal in dtFieldVals)
        fieldVal.Add(dtFieldVal.Data);
      return fieldVal;
    }
  }
}
