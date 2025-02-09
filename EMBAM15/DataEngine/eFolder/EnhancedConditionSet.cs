// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.eFolder.EnhancedConditionSet
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine.eFolder
{
  [Serializable]
  public class EnhancedConditionSet
  {
    public Guid Id { get; set; }

    public string SetName { get; set; }

    public string Description { get; set; }

    public bool Deleted { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public string LastModifiedBy { get; set; }

    public DateTime LastModifiedDate { get; set; }

    public List<EnhancedConditionTemplate> ConditionTemplates { get; set; }

    public override string ToString() => this.SetName;
  }
}
