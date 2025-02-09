// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.StringValidator
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public class StringValidator : IValidator
  {
    public List<ImportMessage> Validate(
      CellData cellData,
      IImportErrorProvider errorProvider,
      string[] options)
    {
      List<ImportMessage> importMessageList = new List<ImportMessage>();
      try
      {
        if (options != null)
        {
          if (options.Length != 0)
          {
            if (cellData.Criteria == DDMCriteria.ListOfValues)
            {
              if (((IEnumerable<string>) cellData.Data.Split('|')).Except<string>((IEnumerable<string>) options).Count<string>() > 0)
                throw new Exception("Invalid values found for the field");
            }
            else if (!((IEnumerable<string>) options).Contains<string>(cellData.Data))
              throw new Exception("Invalid value found for the field");
          }
        }
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
