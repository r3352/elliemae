// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.ARMType
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class ARMType
  {
    private string typeId;
    private string description;

    public ARMType(string typeId, string description)
    {
      this.typeId = typeId;
      this.description = description;
    }

    public string TypeID => this.typeId;

    public string Description => this.description;
  }
}
