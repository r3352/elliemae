// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.BankerEditionVirtualField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public abstract class BankerEditionVirtualField : VirtualField
  {
    public BankerEditionVirtualField(string fieldId, string description, FieldFormat format)
      : base(fieldId, description, format)
    {
    }

    public override bool AppliesToEdition(EncompassEdition edition)
    {
      return edition == EncompassEdition.Banker;
    }
  }
}
