// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.eFolder.PurchaseConditionStatus
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine.eFolder
{
  [Serializable]
  public class PurchaseConditionStatus
  {
    private string statusName;
    private string statusInterpretation;
    private int statusOrder;
    private bool canBeDeleted;

    public int StatusId { get; set; }

    public string StatusName
    {
      get => this.statusName;
      set => this.statusName = value;
    }

    public string StatusInterpretation
    {
      get => this.statusInterpretation;
      set => this.statusInterpretation = value;
    }

    public int StatusOrder
    {
      get => this.statusOrder;
      set => this.statusOrder = value;
    }

    public bool CanBeDeleted
    {
      get => this.canBeDeleted;
      set => this.canBeDeleted = value;
    }
  }
}
