// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Workflow.TemplateFreeRole
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Workflow
{
  [Serializable]
  public class TemplateFreeRole : IComparable<TemplateFreeRole>
  {
    private int roleId;

    public TemplateFreeRole(int roleId) => this.roleId = roleId;

    public TemplateFreeRole(XmlElement e)
    {
      this.roleId = new AttributeReader(e).GetInteger(nameof (RoleID));
    }

    public int RoleID
    {
      get => this.roleId;
      set => this.roleId = value;
    }

    public override bool Equals(object obj)
    {
      return obj is TemplateFreeRole templateFreeRole && templateFreeRole.RoleID == this.RoleID;
    }

    public override int GetHashCode() => this.roleId.GetHashCode();

    int IComparable<TemplateFreeRole>.CompareTo(TemplateFreeRole other)
    {
      throw new NotImplementedException();
    }
  }
}
