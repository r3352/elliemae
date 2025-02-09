// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.BorrowerReportFieldFlags
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  [Flags]
  public enum BorrowerReportFieldFlags
  {
    IncludeBasicFields = 1,
    IncludeHistoryFields = 2,
    IncludeNonSelectableFields = 4,
    SelectableWithHistoryFields = IncludeHistoryFields | IncludeBasicFields, // 0x00000003
    AllFields = SelectableWithHistoryFields | IncludeNonSelectableFields, // 0x00000007
  }
}
