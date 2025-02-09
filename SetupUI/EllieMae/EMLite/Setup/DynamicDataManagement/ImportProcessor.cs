// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.ImportProcessor
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.DataEngine;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public class ImportProcessor
  {
    private IImportErrorProvider _errorProvider;

    public ImportProcessor(IImportErrorProvider errorProvider)
    {
      this._errorProvider = errorProvider;
    }

    public ImportProcessResult Process(IRawImportData importData)
    {
      ImportProcessResult importProcessResult = new ImportProcessResult();
      if (!importData.HasData())
      {
        importProcessResult.Messages.Add(new ImportMessage(CellData.InvalidCell, 1, ImportMessageType.Warning));
        return importProcessResult;
      }
      FieldDefinition[] columnsInformation = importData.GetColumnsInformation();
      while (importData.MoveNext())
      {
        CellData current = importData.Current;
        FieldDefinition fieldDefinition = columnsInformation[current.Column];
        string[] options = fieldDefinition.Options.GetValues();
        if (fieldDefinition.Options.Count > 0)
          options = new List<string>((IEnumerable<string>) fieldDefinition.Options.GetValues())
          {
            string.Empty
          }.ToArray();
        List<ImportMessage> collection = ValidatorFactory.GetValidator(columnsInformation[current.Column].Format).Validate(current, this._errorProvider, options);
        importProcessResult.Messages.AddRange((IEnumerable<ImportMessage>) collection);
      }
      return importProcessResult;
    }
  }
}
