// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.PhoneNumberValidator
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public class PhoneNumberValidator : IValidator
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
        if (cellData.Criteria == DDMCriteria.ListOfValues)
        {
          string data = cellData.Data;
          char[] chArray = new char[1]{ '|' };
          foreach (string orgval in data.Split(chArray))
          {
            Utils.FormatInput(orgval, FieldFormat.PHONE, ref needsUpdate);
            if (needsUpdate)
              throw new Exception("Format mismatch");
          }
        }
        else if (cellData.Criteria == DDMCriteria.strNotContains || cellData.Criteria == DDMCriteria.strContains || cellData.Criteria == DDMCriteria.strBegins || cellData.Criteria == DDMCriteria.strEnds)
          Utils.FormatInput(cellData.Data, FieldFormat.PHONE, ref needsUpdate);
        else
          Utils.FormatInput(cellData.Data, FieldFormat.PHONE, ref needsUpdate);
        if (needsUpdate)
          throw new Exception("Format mismatch");
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
