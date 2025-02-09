// Decompiled with JetBrains decompiler
// Type: Encompass.Export.RequiredList
// Assembly: Export.Validate, Version=1.0.7933.30763, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 617E5049-06C8-448B-B2D8-44769B16A732
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EMN\Export.Validate.dll

#nullable disable
namespace Encompass.Export
{
  internal class RequiredList
  {
    private string list;

    internal string List
    {
      get => this.list;
      set => this.list = value;
    }

    internal RequiredList() => this.list = string.Empty;

    internal void Add(string id, string desc)
    {
      if (this.list == string.Empty)
      {
        this.list = id + "@" + desc;
      }
      else
      {
        RequiredList requiredList = this;
        requiredList.list = requiredList.list + "|" + id + "@" + desc;
      }
    }
  }
}
