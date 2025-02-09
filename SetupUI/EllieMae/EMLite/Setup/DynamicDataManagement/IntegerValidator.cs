// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.IntegerValidator
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public class IntegerValidator : IValidator
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
        if (cellData.Criteria == DDMCriteria.Range)
        {
          string[] strArray = cellData.Data.Split('-');
          if (strArray.Length != 2)
          {
            int messageCode = errorProvider.GetMessageCode(ValidationErrorType.InvalidRange);
            ImportMessage importMessage = new ImportMessage(cellData, messageCode, ImportMessageType.Error);
            importMessageList.Add(importMessage);
            return importMessageList;
          }
          Utils.FormatInput(strArray[0], FieldFormat.INTEGER, ref needsUpdate);
          if (needsUpdate)
            throw new Exception("Format mismatch");
          Utils.FormatInput(strArray[1], FieldFormat.INTEGER, ref needsUpdate);
          if (Convert.ToInt32(strArray[0]) >= Convert.ToInt32(strArray[1]))
          {
            int messageCode = errorProvider.GetMessageCode(ValidationErrorType.InvalidRange);
            ImportMessage importMessage = new ImportMessage(cellData, messageCode, ImportMessageType.Error);
            importMessageList.Add(importMessage);
            return importMessageList;
          }
          if (options != null && options.Length != 0 && (!((IEnumerable<string>) options).Contains<string>(strArray[0]) || !((IEnumerable<string>) options).Contains<string>(strArray[1])))
            throw new Exception("Invalid range found for the field");
        }
        else if (cellData.Criteria == DDMCriteria.ListOfValues)
        {
          string[] first = cellData.Data.Split('|');
          if (options != null && options.Length != 0)
          {
            if (((IEnumerable<string>) first).Except<string>((IEnumerable<string>) options).Count<string>() > 0)
              throw new Exception("Invalid values found for the field");
          }
          else
          {
            foreach (string orgval in first)
            {
              Utils.FormatInput(orgval, FieldFormat.INTEGER, ref needsUpdate);
              if (needsUpdate)
                throw new Exception("Format mismatch");
            }
          }
        }
        else
        {
          if (options != null && options.Length != 0 && !((IEnumerable<string>) options).Contains<string>(cellData.Data))
            throw new Exception("Invalid value found for the field");
          Utils.FormatInput(cellData.Data, FieldFormat.INTEGER, ref needsUpdate);
        }
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
