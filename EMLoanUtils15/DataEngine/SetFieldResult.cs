// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.SetFieldResult
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class SetFieldResult
  {
    public string FieldId { get; set; }

    public string OriginalValue { get; set; }

    public string NewValue { get; set; }

    public bool LockIconActivated { get; set; }

    public bool Result { get; set; }

    public void Init(string fd, string origVal, string newVal, bool result = true)
    {
      this.FieldId = fd;
      this.OriginalValue = origVal;
      this.NewValue = newVal;
      this.Result = result;
    }
  }
}
