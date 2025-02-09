// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.LogRecordField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class LogRecordField
  {
    private LogRecordFieldList parentList;
    private string fieldID = "";
    private string fieldValue = "";

    internal LogRecordField(LogRecordFieldList parentList, string fieldID, string fieldValue)
    {
      this.parentList = parentList;
      this.fieldID = fieldID;
      this.fieldValue = fieldValue;
    }

    public string FieldID => this.fieldID;

    public string FieldValue
    {
      get => this.fieldValue;
      set
      {
        this.fieldValue = value;
        this.parentList.MarkAsDirty();
      }
    }
  }
}
