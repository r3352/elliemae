// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.FieldPairInfo
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class FieldPairInfo
  {
    private string fieldID;
    private int pairIndex;

    public FieldPairInfo(string fieldId, int pairIndex)
    {
      if (pairIndex < 0)
        pairIndex = 0;
      this.fieldID = fieldId;
      this.pairIndex = pairIndex;
    }

    public string FieldID => this.fieldID;

    public int PairIndex => this.pairIndex;

    public static string GetPairDescription(int index)
    {
      if (index == 1)
        return "";
      if (index % 10 == 1)
        return index.ToString() + "st";
      if (index % 10 == 2)
        return index.ToString() + "nd";
      return index % 10 == 3 ? index.ToString() + "rd" : index.ToString() + "th";
    }
  }
}
