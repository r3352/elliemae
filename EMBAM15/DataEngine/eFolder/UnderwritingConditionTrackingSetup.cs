// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.eFolder.UnderwritingConditionTrackingSetup
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine.eFolder
{
  [Serializable]
  public class UnderwritingConditionTrackingSetup : ConditionTrackingSetup
  {
    public UnderwritingConditionTrackingSetup()
    {
    }

    public UnderwritingConditionTrackingSetup(XmlSerializationInfo info)
    {
      foreach (string name in info)
        this.Add((ConditionTemplate) info.GetValue(name, typeof (UnderwritingConditionTemplate)));
    }

    public override ConditionType ConditionType => ConditionType.Underwriting;

    public UnderwritingConditionTemplate GetByIndex(int index)
    {
      return (UnderwritingConditionTemplate) base.GetByIndex(index);
    }

    public UnderwritingConditionTemplate GetByName(string name)
    {
      return (UnderwritingConditionTemplate) base.GetByName(name);
    }

    public UnderwritingConditionTemplate GetByID(string guid)
    {
      return (UnderwritingConditionTemplate) base.GetByID(guid);
    }
  }
}
