// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DocEngine.DocEngineStackingOrderInfo
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System;

#nullable disable
namespace EllieMae.EMLite.DocEngine
{
  [Serializable]
  public class DocEngineStackingOrderInfo
  {
    private string guid;
    private string name;
    private DocumentOrderType orderType;
    private bool isDefault;
    private int elementCount;

    public DocEngineStackingOrderInfo(
      string guid,
      string name,
      DocumentOrderType orderType,
      bool isDefault,
      int elementCount)
    {
      this.guid = guid;
      this.name = name;
      this.orderType = orderType;
      this.isDefault = isDefault;
      this.elementCount = elementCount;
    }

    public string Guid => this.guid;

    public string Name => this.name;

    public DocumentOrderType Type => this.orderType;

    public bool IsDefault
    {
      get => this.isDefault;
      set => this.isDefault = value;
    }

    public int ElementCount => this.elementCount;

    public bool AppliesToOrderType(DocumentOrderType type) => (this.orderType & type) != 0;
  }
}
