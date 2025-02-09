// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DocEngine.FieldConflict
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

#nullable disable
namespace EllieMae.EMLite.DocEngine
{
  public class FieldConflict
  {
    private string fieldId;
    private object docEngineValue;
    private object encompassValue;

    public FieldConflict(string fieldId, object docEngineValue, object encompassValue)
    {
      this.fieldId = fieldId;
      this.docEngineValue = docEngineValue;
      this.encompassValue = encompassValue;
    }

    public string FieldID => this.fieldId;

    public object DocEngineValue => this.docEngineValue;

    public object EncompassValue => this.encompassValue;
  }
}
