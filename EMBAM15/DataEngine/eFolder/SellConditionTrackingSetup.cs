// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.eFolder.SellConditionTrackingSetup
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine.eFolder
{
  [Serializable]
  public class SellConditionTrackingSetup : ConditionTrackingSetup
  {
    public SellConditionTrackingSetup()
    {
    }

    public SellConditionTrackingSetup(XmlSerializationInfo info)
    {
      foreach (string name in info)
        this.Add((ConditionTemplate) info.GetValue(name, typeof (SellConditionTemplate)));
    }

    public override ConditionType ConditionType => ConditionType.Sell;

    public SellConditionTemplate GetByIndex(int index)
    {
      return (SellConditionTemplate) base.GetByIndex(index);
    }

    public SellConditionTemplate GetByName(string name)
    {
      return (SellConditionTemplate) base.GetByName(name);
    }

    public SellConditionTemplate GetByID(string guid) => (SellConditionTemplate) base.GetByID(guid);
  }
}
