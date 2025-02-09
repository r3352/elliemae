// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.DateValidator
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public class DateValidator : IValidator
  {
    private DateTime _minDate = new DateTime(1900, 1, 1);
    private DateTime _maxDate = new DateTime(2199, 12, 31);

    public List<ImportMessage> Validate(
      CellData cellData,
      IImportErrorProvider errorProvider,
      string[] options)
    {
      bool needsUpdate = false;
      List<ImportMessage> importMessageList = new List<ImportMessage>();
      try
      {
        if (string.IsNullOrEmpty(cellData.Data.Trim()))
          return importMessageList;
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
          Utils.FormatInput(Convert.ToDateTime(strArray[0]).ToString("MM/dd/yyyy"), FieldFormat.DATE, ref needsUpdate);
          if (needsUpdate)
            throw new Exception("Format mismatch");
          if (!this.CheckBoundaryCondition(Convert.ToDateTime(strArray[0])))
            throw new Exception("Value provided is outside the range 01/01/1900 - 12/31/2199");
          Utils.FormatInput(Convert.ToDateTime(strArray[1]).ToString("MM/dd/yyyy"), FieldFormat.DATE, ref needsUpdate);
          if (!this.CheckBoundaryCondition(Convert.ToDateTime(strArray[1])))
            throw new Exception("Value provided is outside the range 01/01/1900 - 12/31/2199");
          if (Convert.ToDateTime(strArray[0]) >= Convert.ToDateTime(strArray[1]))
          {
            int messageCode = errorProvider.GetMessageCode(ValidationErrorType.InvalidRange);
            ImportMessage importMessage = new ImportMessage(cellData, messageCode, ImportMessageType.Error);
            importMessageList.Add(importMessage);
            return importMessageList;
          }
        }
        else if (cellData.Criteria == DDMCriteria.ListOfValues)
        {
          string data = cellData.Data;
          char[] chArray = new char[1]{ '|' };
          foreach (string str in data.Split(chArray))
          {
            string loanInternalValue = Utils.ConvertToLoanInternalValue(str, FieldFormat.DATE);
            Utils.FormatInput(Convert.ToDateTime(loanInternalValue).ToString("MM/dd/yyyy"), FieldFormat.DATE, ref needsUpdate);
            if (needsUpdate)
              throw new Exception("Format mismatch");
            if (!this.CheckBoundaryCondition(Convert.ToDateTime(loanInternalValue)))
              throw new Exception("Value provided is outside the range 01/01/1900 - 12/31/2199");
          }
        }
        else
        {
          Utils.FormatInput(Convert.ToDateTime(cellData.Data).ToString("MM/dd/yyyy"), FieldFormat.DATE, ref needsUpdate);
          if (!this.CheckBoundaryCondition(Convert.ToDateTime(cellData.Data)))
            throw new Exception("Value provided is outside the range 01/01/1900 - 12/31/2199");
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

    private bool CheckBoundaryCondition(DateTime date)
    {
      return !(date < this._minDate) && !(date > this._maxDate);
    }
  }
}
