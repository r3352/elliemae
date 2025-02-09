// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.ZipCodeValidator
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public class ZipCodeValidator : IValidator
  {
    private string _usZipRegEx = "^\\d{5}(?:[-\\s]\\d{4})?$";

    public List<ImportMessage> Validate(
      CellData cellData,
      IImportErrorProvider errorProvider,
      string[] options)
    {
      bool needsUpdate = false;
      List<ImportMessage> importMessageList = new List<ImportMessage>();
      if (string.IsNullOrEmpty(cellData.Data.Trim()))
        return importMessageList;
      try
      {
        string data = cellData.Data;
        char[] chArray = new char[1]{ '|' };
        foreach (string str in data.Split(chArray))
        {
          Utils.FormatInput(str, FieldFormat.ZIPCODE, ref needsUpdate);
          if (needsUpdate)
            throw new Exception("Format mismatch");
          this.ValidateZipCode(str, ref needsUpdate);
          if (needsUpdate)
            throw new Exception("Format mismatch");
          if (ZipCodeUtils.GetMultipleZipInfoAt(str) == null)
            throw new Exception("Format mismatch");
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

    private void ValidateZipCode(string zipCode, ref bool needsUpdate)
    {
      needsUpdate = !Regex.IsMatch(zipCode, this._usZipRegEx);
    }
  }
}
