// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.PII.MaskUtilities
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Encompass.Diagnostics.Logging.Schema;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace Encompass.Diagnostics.PII
{
  public static class MaskUtilities
  {
    private static Regex PwdRegex = new Regex("PwdCompare\\(\\'([A-Za-z0-9!@#$%^&*$\\-]+)", RegexOptions.Compiled);
    private static Regex PwdEncryptRegex = new Regex("PwdEncrypt\\(\\'([A-Za-z0-9!@#$%^&*$\\-]+)", RegexOptions.Compiled);
    private static Regex ERDBPasswordRegex = new Regex("with PASSWORD = \\'[A-Za-z0-9!@#$%^&*$\\-]+", RegexOptions.Compiled);

    public static string MaskPII(string message)
    {
      char[] charArray = message.ToCharArray();
      return MaskUtilities.MaskSSNs(charArray) + MaskUtilities.MaskDates(charArray) + MaskUtilities.MaskEmailAddresses(charArray) + MaskUtilities.MaskPhoneNumbers(charArray) > 0 ? new string(charArray) : message;
    }

    public static string MaskPII(string message, bool requiresMaskingForSQL)
    {
      string input1 = string.Empty;
      try
      {
        input1 = MaskUtilities.MaskPII(message);
      }
      catch
      {
      }
      if (requiresMaskingForSQL)
      {
        string input2 = MaskUtilities.PwdRegex.Replace(input1, "PwdCompare('***");
        string input3 = MaskUtilities.PwdEncryptRegex.Replace(input2, "PwdEncrypt('***");
        input1 = MaskUtilities.ERDBPasswordRegex.Replace(input3, "with PASSWORD ='***");
      }
      return input1;
    }

    public static bool RequiresMaskingForSQL(this Log log)
    {
      return "SQLTrace".Equals(log.Logger, StringComparison.OrdinalIgnoreCase);
    }

    public static string MaskDates(string message)
    {
      char[] charArray = message.ToCharArray();
      return MaskUtilities.MaskDates(charArray) > 0 ? new string(charArray) : message;
    }

    public static int MaskDates(char[] arr)
    {
      int num = 0;
      for (int index = 0; index < arr.Length; ++index)
      {
        if (arr[index] == '/')
        {
          if (index + 5 <= arr.Length && index >= 2 && arr[index + 3] == '/')
          {
            if (char.IsDigit(arr[index - 1]) && (arr[index - 2] == '0' || arr[index - 2] == '1') && (index - 3 < 0 || !char.IsDigit(arr[index - 3])) && (arr[index + 1] == '0' || arr[index + 1] == '1' || arr[index + 1] == '2' || arr[index + 1] == '3' && char.IsDigit(arr[index + 2])) && char.IsDigit(arr[index + 4]) && char.IsDigit(arr[index + 5]) && char.IsDigit(arr[index + 6]) && char.IsDigit(arr[index + 7]) && (index + 8 >= arr.Length || !char.IsDigit(arr[index + 8])))
            {
              arr[index - 2] = '*';
              arr[index - 1] = '*';
              arr[index + 1] = '*';
              arr[index + 2] = '*';
              arr[index + 4] = '*';
              arr[index + 5] = '*';
              arr[index + 6] = '*';
              arr[index + 7] = '*';
              index += 7;
              ++num;
            }
          }
          else
            continue;
        }
        if (arr[index] == ':' && index + 2 <= arr.Length && index >= 11 && arr[index - 1] == '0' && arr[index + 1] == '0' && arr[index + 2] == '0' && arr[index - 2] == '0' && MaskUtilities.IsDayPart(arr[index - 5], arr[index - 4]) && MaskUtilities.IsMonthPart(arr[index - 7], arr[index - 6]) && char.IsDigit(arr[index - 8]) && char.IsDigit(arr[index - 9]) && char.IsDigit(arr[index - 10]) && char.IsDigit(arr[index - 11]) && (index - 12 < 0 || !char.IsDigit(arr[index - 12])))
        {
          arr[index - 11] = '*';
          arr[index - 10] = '*';
          arr[index - 9] = '*';
          arr[index - 8] = '*';
          arr[index - 7] = '*';
          arr[index - 6] = '*';
          arr[index - 5] = '*';
          arr[index - 4] = '*';
          index += 2;
          ++num;
        }
      }
      return num;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsMonthPart(char a, char b)
    {
      int num;
      switch (a)
      {
        case '0':
          num = 1;
          break;
        case '1':
          num = char.IsDigit(b) ? 1 : 0;
          break;
        default:
          num = 0;
          break;
      }
      return num != 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsDayPart(char a, char b)
    {
      int num;
      switch (a)
      {
        case '0':
        case '1':
        case '2':
          num = 1;
          break;
        case '3':
          num = char.IsDigit(b) ? 1 : 0;
          break;
        default:
          num = 0;
          break;
      }
      return num != 0;
    }

    public static string MaskPhoneNumbers(string message)
    {
      char[] charArray = message.ToCharArray();
      return MaskUtilities.MaskPhoneNumbers(charArray) > 0 ? new string(charArray) : message;
    }

    public static int MaskPhoneNumbers(char[] arr)
    {
      int num = 0;
      for (int index = 0; index < arr.Length; ++index)
      {
        if (arr[index] == '-' && index + 8 <= arr.Length && index >= 3 && arr[index + 4] == '-' && char.IsDigit(arr[index - 1]) && char.IsDigit(arr[index - 2]) && char.IsDigit(arr[index - 3]) && (index - 4 < 0 || !char.IsDigit(arr[index - 4])) && char.IsDigit(arr[index + 1]) && char.IsDigit(arr[index + 2]) && char.IsDigit(arr[index + 3]) && char.IsDigit(arr[index + 5]) && char.IsDigit(arr[index + 6]) && char.IsDigit(arr[index + 7]) && char.IsDigit(arr[index + 8]) && (index + 9 >= arr.Length || !char.IsDigit(arr[index + 9])))
        {
          arr[index - 3] = '*';
          arr[index - 2] = '*';
          arr[index - 1] = '*';
          arr[index + 1] = '*';
          arr[index + 2] = '*';
          arr[index + 3] = '*';
          arr[index + 5] = '*';
          arr[index + 6] = '*';
          arr[index + 7] = '*';
          arr[index + 8] = '*';
          index += 8;
          ++num;
        }
      }
      return num;
    }

    public static string MaskSSNs(string message)
    {
      char[] charArray = message.ToCharArray();
      return MaskUtilities.MaskSSNs(charArray) > 0 ? new string(charArray) : message;
    }

    public static int MaskSSNs(char[] arr)
    {
      int num = 0;
      for (int index = 0; index < arr.Length; ++index)
      {
        if (arr[index] == '-' && index + 7 <= arr.Length && index >= 3 && arr[index + 3] == '-' && char.IsDigit(arr[index - 1]) && char.IsDigit(arr[index - 2]) && char.IsDigit(arr[index - 3]) && (index - 4 < 0 || !char.IsDigit(arr[index - 4])) && char.IsDigit(arr[index + 1]) && char.IsDigit(arr[index + 2]) && char.IsDigit(arr[index + 4]) && char.IsDigit(arr[index + 5]) && char.IsDigit(arr[index + 6]) && char.IsDigit(arr[index + 7]) && (index + 8 >= arr.Length || !char.IsDigit(arr[index + 8])))
        {
          arr[index - 3] = '*';
          arr[index - 2] = '*';
          arr[index - 1] = '*';
          arr[index + 1] = '*';
          arr[index + 2] = '*';
          arr[index + 4] = '*';
          arr[index + 5] = '*';
          arr[index + 6] = '*';
          arr[index + 7] = '*';
          index += 7;
          ++num;
        }
      }
      return num;
    }

    public static string MaskEmailAddresses(string message)
    {
      char[] charArray = message.ToCharArray();
      return MaskUtilities.MaskEmailAddresses(charArray) > 0 ? new string(charArray) : message;
    }

    public static int MaskEmailAddresses(char[] arr)
    {
      int num1 = 0;
      for (int index1 = 0; index1 < arr.Length; ++index1)
      {
        if (arr[index1] == '@')
        {
          int num2 = 1;
          int num3 = 1;
          int num4 = index1 < 64 ? index1 : 64;
          while (num2 <= num4 && (char.IsLetterOrDigit(arr[index1 - num2]) || arr[index1 - num2] == '.' || arr[index1 - num2] == '_'))
            ++num2;
          if (num2 >= 2 && num2 != 64)
          {
            int num5 = arr.Length - index1 < (int) byte.MaxValue ? arr.Length - index1 : (int) byte.MaxValue;
            while (num3 < num5 && (char.IsLetterOrDigit(arr[index1 + num3]) || arr[index1 + num3] == '.' || arr[index1 + num3] == '_'))
              ++num3;
            if (num3 >= 2 && num3 != 254)
            {
              int index2 = index1 - num2 + 1;
              for (int index3 = 0; index3 < num2 + num3 - 1; ++index3)
              {
                if (arr[index2] != '.' && arr[index2] != '@')
                  arr[index2] = '*';
                ++index2;
              }
              ++num1;
              index1 += num3;
            }
          }
        }
      }
      return num1;
    }

    public static string SerializeObject(
      object value,
      Formatting formatting,
      JsonSerializerSettings settings,
      bool maskSQL)
    {
      JsonSerializer jsonSerializer = JsonSerializer.CreateDefault(settings);
      jsonSerializer.Formatting = formatting;
      return MaskUtilities.SerializeObjectInternal(value, jsonSerializer, maskSQL);
    }

    private static string SerializeObjectInternal(
      object value,
      JsonSerializer jsonSerializer,
      bool maskSQL)
    {
      StringBuilder sb = new StringBuilder(256);
      using (StringWriter sw = new StringWriter(sb, (IFormatProvider) CultureInfo.InvariantCulture))
        MaskUtilities.SerializeObjectInternal((TextWriter) sw, value, jsonSerializer, maskSQL);
      return sb.ToString();
    }

    private static void SerializeObjectInternal(
      TextWriter sw,
      object value,
      JsonSerializer jsonSerializer,
      bool maskSQL)
    {
      using (JsonTextWriter jsonTextWriter = (JsonTextWriter) new MaskPIIJsonTextWriter(sw, maskSQL))
      {
        jsonTextWriter.CloseOutput = false;
        jsonTextWriter.Formatting = jsonSerializer.Formatting;
        jsonSerializer.Serialize((JsonWriter) jsonTextWriter, value, (Type) null);
      }
    }

    public static void SerializeObject(
      TextWriter sw,
      object value,
      Formatting formatting,
      JsonSerializerSettings settings,
      bool maskSQL)
    {
      JsonSerializer jsonSerializer = JsonSerializer.CreateDefault(settings);
      jsonSerializer.Formatting = formatting;
      MaskUtilities.SerializeObjectInternal(sw, value, jsonSerializer, maskSQL);
    }
  }
}
