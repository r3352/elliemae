// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.Extension
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  internal static class Extension
  {
    internal static string GetMessageCount(
      this List<RepAndWarrantTracker.MessageDetails> messageList)
    {
      int num = messageList.Count<RepAndWarrantTracker.MessageDetails>((Func<RepAndWarrantTracker.MessageDetails, bool>) (m => m.Message != string.Empty));
      return num != 0 ? num.ToString() : string.Empty;
    }

    internal static string FormatDatetime(this string dateValue, string format)
    {
      try
      {
        DateTime localTime = DateTime.Parse(dateValue);
        if (dateValue.Contains("Z"))
          localTime = localTime.ToLocalTime();
        dateValue = localTime.ToString(format);
      }
      catch
      {
      }
      return dateValue;
    }
  }
}
