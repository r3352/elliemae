// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.ImportMessage
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public class ImportMessage
  {
    private static readonly string DataTableImportNoError = string.Empty;
    private static readonly string NoData = "Import cannot proceed as there is no data";
    private static readonly string DataTableImportErrorInvalidValue = "{0} does not have a valid value(s) {1}";
    private static readonly string DataTableImportErrorInvalidRange = "{0} does not have a valid range. Please check the values provided ({1})";
    private static readonly string DataTableImportErrorAdvancedCodeBlank = "{0} does not have advanced code. Please check the advanced code ({1})";
    private static readonly string DataTableImportErrorAdvancedCodeInvalidCharacters = "{0} has invalid characters. Please check the advanced code ({1})";
    private static readonly string DataTableImportErrorAdvancedCodeNotValidFormula = "{0} does not have valid formula. Please check the advanced code ({1})";
    private static readonly string DataTableImportErrorAdvancedCodeUnknownError = "{0} has some unknown error. Please check the advanced code ({1})";
    private static readonly Dictionary<int, string> ImportMessageFormat = new Dictionary<int, string>();

    static ImportMessage()
    {
      ImportMessage.ImportMessageFormat.Add(0, ImportMessage.DataTableImportNoError);
      ImportMessage.ImportMessageFormat.Add(1, ImportMessage.NoData);
      ImportMessage.ImportMessageFormat.Add(2, ImportMessage.DataTableImportErrorInvalidValue);
      ImportMessage.ImportMessageFormat.Add(3, ImportMessage.DataTableImportErrorInvalidRange);
      ImportMessage.ImportMessageFormat.Add(4, ImportMessage.DataTableImportErrorAdvancedCodeBlank);
      ImportMessage.ImportMessageFormat.Add(5, ImportMessage.DataTableImportErrorAdvancedCodeInvalidCharacters);
      ImportMessage.ImportMessageFormat.Add(6, ImportMessage.DataTableImportErrorAdvancedCodeNotValidFormula);
      ImportMessage.ImportMessageFormat.Add(7, ImportMessage.DataTableImportErrorAdvancedCodeUnknownError);
    }

    public int MessageCode { get; private set; }

    public ImportMessageType ImportMessageType { get; private set; }

    public CellData CellData { get; private set; }

    public ImportMessage(CellData cellData, int messageCode, ImportMessageType importMessageType)
    {
      this.CellData = cellData;
      this.MessageCode = messageCode;
      if (MessageCodes.NonErrors.Contains(messageCode))
        this.ImportMessageType = ImportMessageType.Info;
      else
        this.ImportMessageType = importMessageType;
    }

    public string GetMessageFormat() => ImportMessage.ImportMessageFormat[this.MessageCode];

    public string Message(params object[] arguments)
    {
      return string.Format(this.GetMessageFormat(), arguments);
    }
  }
}
