// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.OutputColumn
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  public class OutputColumn
  {
    public int Index { get; set; }

    public string Name { get; set; }

    public override string ToString() => this.Name;
  }
}
