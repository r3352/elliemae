// Decompiled with JetBrains decompiler
// Type: Elli.Metrics.Client.SFxTag
// Assembly: Elli.Metrics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D16DD1E4-F07E-4CB7-B2D0-A2DD51E6F671
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Metrics.dll

using System;

#nullable disable
namespace Elli.Metrics.Client
{
  [Serializable]
  public class SFxTag
  {
    public SFxTag(string name, string value)
    {
      this.Name = name;
      this.Value = value;
    }

    public string Name { get; protected set; }

    public string Value { get; protected set; }

    public override string ToString()
    {
      return this.Name.EndsWith("=") ? this.Name + this.Value : this.Name + "=" + this.Value;
    }
  }
}
