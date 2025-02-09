// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.MessageCodes
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public class MessageCodes
  {
    public const int DATATABLEIMPORT_NOERROR = 0;
    public const int IMPORT_NODATA = 1;
    public const int DATATABLEIMPORT_ERRORINVALIDVALUE = 2;
    public const int DATATABLEIMPORT_ERRORINVALIDRANGE = 3;
    public const int DATATABLEIMPORT_ADVANCEDCODE_EMPTY = 4;
    public const int DATATABLEIMPORT_ADVANCEDCODE_INVALIDCHARACTERS = 5;
    public const int DATATABLEIMPORT_ADVANCEDCODE_NOTVALIDFORMULA = 6;
    public const int DATATABLEIMPORT_ADVANCEDCODE_UNKNOWNERROR = 7;

    public static List<int> NonErrors
    {
      get => new List<int>() { 0 };
    }
  }
}
