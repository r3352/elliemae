// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.Schema.LogFieldName`1
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using System;

#nullable disable
namespace Encompass.Diagnostics.Logging.Schema
{
  [Serializable]
  public class LogFieldName<T> : ILogField
  {
    private readonly int hashCode;

    public LogFieldName(string name)
    {
      this.Name = name;
      this.hashCode = StringComparer.OrdinalIgnoreCase.GetHashCode(this.Name);
    }

    public Type Type => typeof (T);

    public string Name { get; }

    public override string ToString() => this.Name;

    public override bool Equals(object obj)
    {
      return obj is LogFieldName<T> logFieldName && string.Equals(this.Name, logFieldName.Name, StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode() => this.hashCode;
  }
}
