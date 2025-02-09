// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.ContactFormat
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System.Globalization;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class ContactFormat
  {
    public static NumberFormatInfo NFI_AV = new NumberFormatInfo();
    public static NumberFormatInfo NFI_LA = new NumberFormatInfo();
    public static NumberFormatInfo NFI_IR = new NumberFormatInfo();
    public static NumberFormatInfo NFI_DP = new NumberFormatInfo();

    static ContactFormat()
    {
      ContactFormat.NFI_AV.CurrencySymbol = "$";
      ContactFormat.NFI_AV.CurrencyGroupSeparator = ",";
      ContactFormat.NFI_AV.CurrencyDecimalDigits = 0;
      ContactFormat.NFI_LA.CurrencySymbol = "$";
      ContactFormat.NFI_LA.CurrencyGroupSeparator = ",";
      ContactFormat.NFI_LA.CurrencyDecimalDigits = 0;
      ContactFormat.NFI_IR.CurrencySymbol = "";
      ContactFormat.NFI_IR.CurrencyGroupSeparator = "";
      ContactFormat.NFI_IR.CurrencyDecimalDigits = 3;
      ContactFormat.NFI_DP.CurrencySymbol = "";
      ContactFormat.NFI_DP.CurrencyGroupSeparator = "";
      ContactFormat.NFI_DP.CurrencyDecimalDigits = 3;
    }
  }
}
