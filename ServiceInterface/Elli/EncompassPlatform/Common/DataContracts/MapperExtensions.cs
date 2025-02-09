// Decompiled with JetBrains decompiler
// Type: Elli.EncompassPlatform.Common.DataContracts.MapperExtensions
// Assembly: ServiceInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF0C2B89-A027-4FA0-9669-4D2AA36A4D74
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ServiceInterface.dll

using System;

#nullable disable
namespace Elli.EncompassPlatform.Common.DataContracts
{
  public static class MapperExtensions
  {
    public static DateTime? ToDateTimeOrDefault(this DateTime dateValue)
    {
      return !(dateValue.Date == DateTime.MinValue.Date) ? new DateTime?(dateValue) : new DateTime?();
    }

    public static int? ToRoleIdValue(this int roleId)
    {
      return roleId != -1 ? new int?(roleId) : new int?();
    }

    public static bool ToBoolValue(this bool? value) => value.HasValue && value.Value;
  }
}
