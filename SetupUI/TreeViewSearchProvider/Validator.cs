// Decompiled with JetBrains decompiler
// Type: TreeViewSearchProvider.Validator
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System;

#nullable disable
namespace TreeViewSearchProvider
{
  public class Validator
  {
    public static Tuple<bool, string> Range(
      int value,
      int min,
      int max,
      string module = "",
      bool throwException = false)
    {
      if (value >= min && value <= max)
        return new Tuple<bool, string>(true, "Valid.");
      string message = string.Format("{0} Value should be between {1} and {2}.", (object) module, (object) min, (object) max).Trim();
      if (throwException)
        throw new Exception(message);
      return new Tuple<bool, string>(false, message);
    }
  }
}
