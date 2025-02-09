// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.DropDownListValidator
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public class DropDownListValidator : IValidator
  {
    public List<ImportMessage> Validate(
      CellData cellData,
      IImportErrorProvider errorProvider,
      string[] options)
    {
      bool needsUpdate = false;
      List<ImportMessage> importMessageList = new List<ImportMessage>();
      try
      {
        Utils.FormatInput(cellData.Data, FieldFormat.DROPDOWNLIST, ref needsUpdate);
      }
      catch (Exception ex)
      {
        int messageCode = errorProvider.GetMessageCode(ValidationErrorType.FormatMismatch);
        ImportMessage importMessage = new ImportMessage(cellData, messageCode, ImportMessageType.Error);
        importMessageList.Add(importMessage);
      }
      return importMessageList;
    }
  }
}
