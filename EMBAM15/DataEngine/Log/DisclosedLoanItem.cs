// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.DisclosedLoanItem
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class DisclosedLoanItem
  {
    private string fieldID = "";
    private string fieldValue = "";

    public DisclosedLoanItem(string fieldID, string fieldValue)
    {
      this.fieldID = fieldID;
      this.fieldValue = fieldValue;
    }

    public string FieldID => this.fieldID;

    public string FieldValue
    {
      get => this.fieldValue;
      set => this.fieldValue = value;
    }

    public override bool Equals(object obj)
    {
      return obj is DisclosedLoanItem && this.fieldID == ((DisclosedLoanItem) obj).fieldID;
    }

    public override int GetHashCode() => base.GetHashCode();

    public enum FieldType
    {
      GFEItemization,
      HUDGFE,
      Both,
    }
  }
}
