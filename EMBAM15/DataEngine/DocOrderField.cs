// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.DocOrderField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.DocEngine;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class DocOrderField : VirtualField
  {
    private DocumentOrderType orderType;
    private string baseFieldId;

    public DocOrderField(DocumentOrderType orderType, FieldDefinition baseField)
      : base(DocOrderField.generateFieldID(baseField.FieldID, orderType), DocOrderField.generateFieldDescription(baseField.Description, orderType), baseField.Format)
    {
      this.orderType = orderType;
      this.baseFieldId = baseField.FieldID;
    }

    public override VirtualFieldType VirtualFieldType => VirtualFieldType.DocEngineFieldList;

    protected override string Evaluate(LoanData loan)
    {
      DocumentOrderLog lastOrderLog = this.getLastOrderLog(loan);
      return lastOrderLog == null ? "" : lastOrderLog.DocumentFields.GetField(this.baseFieldId);
    }

    private DocumentOrderLog getLastOrderLog(LoanData loan)
    {
      DocumentOrderLog lastOrderLog = (DocumentOrderLog) null;
      foreach (DocumentOrderLog documentOrderLog in loan.GetLogList().GetAllRecordsOfType(typeof (DocumentOrderLog)))
      {
        if ((documentOrderLog.OrderType & this.orderType) != DocumentOrderType.None && (lastOrderLog == null || lastOrderLog.Date < documentOrderLog.Date))
          lastOrderLog = documentOrderLog;
      }
      return lastOrderLog;
    }

    private static string generateFieldID(string baseFieldID, DocumentOrderType orderType)
    {
      return "Docs." + baseFieldID;
    }

    private static string generateFieldDescription(
      string baseDescription,
      DocumentOrderType orderType)
    {
      return orderType == DocumentOrderType.Both ? "Last Document Order - " + baseDescription.Replace("ICE Mortgage Technology Closing Document Override - ", "") : orderType.ToString() + " Docs - " + baseDescription.Replace("ICE Mortgage Technology Closing Document Override - ", "");
    }

    public DocumentOrderType OrderType => this.orderType;
  }
}
