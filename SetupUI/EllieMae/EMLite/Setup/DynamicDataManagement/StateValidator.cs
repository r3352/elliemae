// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.StateValidator
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
  public class StateValidator : IValidator
  {
    public List<ImportMessage> Validate(
      CellData cellData,
      IImportErrorProvider errorProvider,
      string[] options)
    {
      string[] states = Utils.GetStates();
      bool needsUpdate = false;
      List<ImportMessage> importMessageList = new List<ImportMessage>();
      if (string.IsNullOrEmpty(cellData.Data.Trim()))
        return importMessageList;
      try
      {
        string[] first = cellData.Data.Split('|');
        if (((IEnumerable<string>) first).Except<string>((IEnumerable<string>) states).Count<string>() > 0)
          throw new Exception("Format mismatch");
        foreach (string orgval in first)
        {
          Utils.FormatInput(orgval, FieldFormat.STATE, ref needsUpdate);
          if (needsUpdate)
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
  }
}
