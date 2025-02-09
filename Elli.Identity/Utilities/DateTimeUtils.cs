// Decompiled with JetBrains decompiler
// Type: Elli.Identity.Utilities.DateTimeUtils
// Assembly: Elli.Identity, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D0ECC974-8E0E-42B3-88D3-2EAE5F37B212
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Identity.dll

using System;

#nullable disable
namespace Elli.Identity.Utilities
{
  public class DateTimeUtils
  {
    public static int ToEpochTime(DateTime dateTime)
    {
      return (int) (dateTime - new DateTime(1970, 1, 1)).TotalSeconds;
    }

    public static DateTime FromEpochTime(int epochTime)
    {
      return new DateTime(1970, 1, 1).AddSeconds((double) epochTime);
    }
  }
}
