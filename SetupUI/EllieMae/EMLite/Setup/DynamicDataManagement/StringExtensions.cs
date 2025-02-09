// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.StringExtensions
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public static class StringExtensions
  {
    public static bool Contains(this string input, string stringToTest, bool ignoreCase)
    {
      if (string.IsNullOrEmpty(input))
        return false;
      if (string.IsNullOrEmpty(stringToTest))
        return true;
      return ignoreCase ? input.ToLower().Contains(stringToTest.ToLower()) : input.Contains(stringToTest);
    }
  }
}
