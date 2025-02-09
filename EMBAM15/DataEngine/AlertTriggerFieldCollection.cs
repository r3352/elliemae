// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.AlertTriggerFieldCollection
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class AlertTriggerFieldCollection : List<AlertTriggerField>
  {
    public void AddIds(params string[] ids)
    {
      this.AddIds(((IEnumerable<string>) ids).AsEnumerable<string>());
    }

    public void AddIds(IEnumerable<string> ids)
    {
      foreach (string id in ids)
        this.Add(new AlertTriggerField(id));
    }

    public AlertTriggerFieldCollection Clone()
    {
      AlertTriggerFieldCollection triggerFieldCollection = new AlertTriggerFieldCollection();
      foreach (AlertTriggerField alertTriggerField in (List<AlertTriggerField>) this)
        triggerFieldCollection.Add((AlertTriggerField) alertTriggerField.Clone());
      return triggerFieldCollection;
    }
  }
}
