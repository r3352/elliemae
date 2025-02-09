// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.IRawImportData
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.DataEngine;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public interface IRawImportData : IEnumerator<CellData>, IDisposable, IEnumerator
  {
    bool HasData();

    FieldDefinition[] GetColumnsInformation();

    Dictionary<FieldDefinition, List<CsvFieldValue>> GetRawDataColumnized();

    List<CsvFieldValue> GetRowDataUnformatted(int rowNumber);

    List<CellData> GetRowData(int rowNumber);

    int RowCount();

    int ColumnCount();

    object[] GetCellMetadata(CellData cellData);
  }
}
