// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.Utils.BasicUtils
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.Encompass.AsmResolver.Utils
{
  public class BasicUtils
  {
    internal static readonly string Localization = "l10n";
    internal static readonly string Internationalization = "i18n";
    private static char[] wildcards = new char[2]
    {
      '*',
      '?'
    };

    public static bool IsNullOrEmpty(string s) => s == null || s.Trim() == string.Empty;

    public static string[] RemoveNullAndEmpty(string[] input)
    {
      if (input == null)
        return (string[]) null;
      List<string> stringList = new List<string>();
      for (int index = 0; index < input.Length; ++index)
      {
        if (input[index] != null)
        {
          string str = input[index].Trim();
          if (str != string.Empty)
            stringList.Add(str);
        }
      }
      return stringList.ToArray();
    }

    public static bool IsHttpOrHttps(string url)
    {
      return !BasicUtils.IsNullOrEmpty(url) && (url.ToLower().StartsWith(ResolverConsts.HttpPrefix) || url.ToLower().StartsWith(ResolverConsts.HttpsPrefix));
    }

    public static bool PatternMatch(string input, string pattern, bool caseSensitive)
    {
      if (BasicUtils.IsNullOrEmpty(pattern) || BasicUtils.IsNullOrEmpty(input))
        throw new Exception("Matching pattern and/or string cannot be null or empty");
      if (!caseSensitive)
      {
        pattern = pattern.ToLower();
        input = input.ToLower();
      }
      if (pattern.IndexOfAny(BasicUtils.wildcards) < 0)
        return input == pattern;
      if (pattern.IndexOfAny(BasicUtils.wildcards) < 0)
        return input == pattern;
      int index1 = 0;
      int index2;
      for (index2 = 0; index1 < input.Length && index2 < pattern.Length && pattern[index2] != '*'; ++index2)
      {
        if ((int) pattern[index2] != (int) input[index1] && pattern[index2] != '?')
          return false;
        ++index1;
      }
      if (index2 == pattern.Length)
        return input.Length == pattern.Length;
      int num1 = 0;
      int num2 = 0;
      while (index1 < input.Length)
      {
        if (index2 < pattern.Length && pattern[index2] == '*')
        {
          ++index2;
          if (index2 >= pattern.Length)
            return true;
          num2 = index2;
          num1 = index1 + 1;
        }
        else if (index2 < pattern.Length && ((int) pattern[index2] == (int) input[index1] || pattern[index2] == '?'))
        {
          ++index2;
          ++index1;
        }
        else
        {
          index2 = num2;
          index1 = num1++;
        }
      }
      while (index2 < pattern.Length && pattern[index2] == '*')
        ++index2;
      return index2 >= pattern.Length;
    }

    public static bool PathMatch(string input, string pattern)
    {
      return BasicUtils.PathMatch(input, pattern, false);
    }

    public static bool PathMatch(string input, string pattern, bool caseSensitive)
    {
      if (BasicUtils.IsNullOrEmpty(pattern) || BasicUtils.IsNullOrEmpty(input))
        throw new Exception("Matching pattern and/or string cannot be null or empty");
      string[] strArray1 = !Path.IsPathRooted(input) && !Path.IsPathRooted(pattern) ? input.Split(new string[1]
      {
        "\\"
      }, StringSplitOptions.RemoveEmptyEntries) : throw new Exception("Matching pattern and/or string cannot be rooted");
      string[] strArray2 = pattern.Split(new string[1]
      {
        "\\"
      }, StringSplitOptions.RemoveEmptyEntries);
      if (strArray1.Length != strArray2.Length)
        return false;
      for (int index = 0; index < strArray1.Length; ++index)
      {
        if (!BasicUtils.PatternMatch(strArray1[index], strArray2[index], caseSensitive))
          return false;
      }
      return true;
    }

    public static bool PathMatch(string input, string[] patterns)
    {
      return BasicUtils.PathMatch(input, patterns, false);
    }

    public static bool PathMatch(string input, string[] patterns, bool caseSensitive)
    {
      if (BasicUtils.IsNullOrEmpty(input) || patterns == null)
        throw new Exception("Matching patterns and/or string cannot be null or empty");
      foreach (string pattern in patterns)
      {
        if (BasicUtils.PathMatch(input, pattern, caseSensitive))
          return true;
      }
      return false;
    }

    public static int RegistryDebugLevel => BasicUtils.GetRegistryDebugLevel((string) null);

    public static int GetRegistryDebugLevel(string appCompanyName)
    {
      object registryValue = BasicUtils.GetRegistryValue("DebugLevel", appCompanyName);
      return registryValue == null ? 0 : Convert.ToInt32((string) registryValue);
    }

    internal static string[] DebugCategories
    {
      get
      {
        if (!(BasicUtils.GetRegistryValue("Debug", (string) null) is string registryValue))
          return (string[]) null;
        char[] separator = new char[2]{ ',', ';' };
        string[] debugCategories = registryValue.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        for (int index = 0; index < debugCategories.Length; ++index)
          debugCategories[index] = debugCategories[index].Trim();
        return debugCategories;
      }
    }

    internal static void DisplayDebuggingInfo(string category, string msg)
    {
      string[] debugCategories = BasicUtils.DebugCategories;
      if (debugCategories == null)
        return;
      foreach (string strA in debugCategories)
      {
        if (string.Compare(strA, category, true) == 0 || strA == "*")
        {
          int num = (int) MessageBox.Show(msg, "Encompass SmartClient");
          break;
        }
      }
    }

    public static object GetRegistryValue(string valueName, string appCompanyName)
    {
      if (appCompanyName == null)
        appCompanyName = "Ellie Mae";
      object registryValue = (object) null;
      using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\" + appCompanyName + "\\SmartClient"))
      {
        if (registryKey != null)
          registryValue = registryKey.GetValue(valueName);
      }
      if (registryValue == null)
      {
        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\" + appCompanyName + "\\SmartClient"))
        {
          if (registryKey == null)
            return (object) null;
          registryValue = registryKey.GetValue(valueName);
        }
      }
      return registryValue;
    }

    public static int DisableSmartClient
    {
      get
      {
        switch (((string) BasicUtils.GetRegistryValue(nameof (DisableSmartClient), (string) null) ?? "").Trim().ToLower())
        {
          case "1":
          case "true":
            return 1;
          case "0":
          case "false":
            return -1;
          default:
            return 0;
        }
      }
    }

    public static bool IsAllUsersInstall(string appCompanyName)
    {
      if (appCompanyName == null)
        appCompanyName = "Ellie Mae";
      bool flag = false;
      using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\" + appCompanyName + "\\SmartClient"))
      {
        if (registryKey != null)
        {
          if (((string) registryKey.GetValue("AllUsersInstall") ?? "").Trim() == "1")
            flag = true;
        }
      }
      return flag;
    }

    public static RegistryKey GetRegistryHive(string appCompanyName)
    {
      if (appCompanyName == null)
        appCompanyName = "Ellie Mae";
      return BasicUtils.IsAllUsersInstall(appCompanyName) ? Registry.LocalMachine : Registry.CurrentUser;
    }
  }
}
