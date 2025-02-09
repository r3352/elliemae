// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.FieldChangeTracker
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class FieldChangeTracker
  {
    public Dictionary<string, FieldChangeInfo> FieldChanges { get; set; } = new Dictionary<string, FieldChangeInfo>();

    public bool IgnoreFieldChangeTrackingForXDB { get; set; } = true;

    public bool IncludesVirtualFields { get; set; }

    public bool UseFieldChangesValues { get; set; }

    public void Clear()
    {
      this.FieldChanges.Clear();
      this.IgnoreFieldChangeTrackingForXDB = true;
    }

    public FieldChangeTracker Clone()
    {
      return new FieldChangeTracker()
      {
        FieldChanges = new Dictionary<string, FieldChangeInfo>((IDictionary<string, FieldChangeInfo>) this.FieldChanges),
        IgnoreFieldChangeTrackingForXDB = this.IgnoreFieldChangeTrackingForXDB,
        IncludesVirtualFields = this.IncludesVirtualFields,
        UseFieldChangesValues = this.UseFieldChangesValues
      };
    }
  }
}
