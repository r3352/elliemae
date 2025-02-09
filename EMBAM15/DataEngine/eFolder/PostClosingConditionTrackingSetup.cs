// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.eFolder.PostClosingConditionTrackingSetup
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine.eFolder
{
  [Serializable]
  public class PostClosingConditionTrackingSetup : ConditionTrackingSetup
  {
    public PostClosingConditionTrackingSetup()
    {
    }

    public PostClosingConditionTrackingSetup(XmlSerializationInfo info)
    {
      foreach (string name in info)
        this.Add((ConditionTemplate) info.GetValue(name, typeof (PostClosingConditionTemplate)));
    }

    public override ConditionType ConditionType => ConditionType.PostClosing;

    public PostClosingConditionTemplate GetByIndex(int index)
    {
      return (PostClosingConditionTemplate) base.GetByIndex(index);
    }

    public PostClosingConditionTemplate GetByName(string name)
    {
      return (PostClosingConditionTemplate) base.GetByName(name);
    }

    public PostClosingConditionTemplate GetByID(string guid)
    {
      return (PostClosingConditionTemplate) base.GetByID(guid);
    }
  }
}
