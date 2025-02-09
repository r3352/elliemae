// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.VerifSummaryChangeInfo
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class VerifSummaryChangeInfo
  {
    public string ItemName = "";
    public object ItemValue;
    public string Indx = "-1";

    public VerifSummaryChangeInfo(string itemName, object itemValue, string indx = "-1")
    {
      this.ItemName = itemName;
      this.ItemValue = itemValue;
      this.Indx = indx;
    }
  }
}
