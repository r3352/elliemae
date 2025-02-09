// Decompiled with JetBrains decompiler
// Type: DotfuscatorAttribute
// Assembly: Jed, Version=1.0.1234.56789, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D965D698-A97D-45D6-911B-975853D5C21D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Jed.dll

using System;

#nullable disable
[AttributeUsage(AttributeTargets.Assembly)]
public sealed class DotfuscatorAttribute : Attribute
{
  private string a;

  public DotfuscatorAttribute(string a)
  {
    DotfuscatorAttribute dotfuscatorAttribute = this;
    // ISSUE: explicit constructor call
    dotfuscatorAttribute.\u002Ector();
    dotfuscatorAttribute.a = a;
  }

  public string A => this.a;

  public string a() => this.a;
}
