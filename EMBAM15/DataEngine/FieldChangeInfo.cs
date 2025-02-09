// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.FieldChangeInfo
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class FieldChangeInfo
  {
    private string fieldId;
    private BorrowerPair pair;
    private string priorValue;
    private string newValue;

    public FieldChangeInfo()
    {
    }

    public FieldChangeInfo(string fieldId, BorrowerPair pair, string priorValue, string newValue)
    {
      this.fieldId = fieldId;
      this.pair = pair;
      this.priorValue = priorValue;
      this.newValue = newValue;
    }

    public string FieldID
    {
      get => this.fieldId;
      set => this.fieldId = value;
    }

    public BorrowerPair BorrowerPair
    {
      get => this.pair;
      set => this.pair = value;
    }

    public string PriorValue
    {
      get => this.priorValue;
      set => this.priorValue = value;
    }

    public string NewValue
    {
      get => this.newValue;
      set => this.newValue = value;
    }
  }
}
