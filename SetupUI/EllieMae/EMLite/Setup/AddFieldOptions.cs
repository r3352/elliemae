// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AddFieldOptions
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  [Flags]
  public enum AddFieldOptions
  {
    None = 0,
    AllowCustomFields = 1,
    AllowVirtualFields = 2,
    AllowButtons = 4,
    AllowHiddenFields = 8,
    AllowAnyField = AllowHiddenFields | AllowVirtualFields | AllowCustomFields, // 0x0000000B
    AllowAll = AllowAnyField | AllowButtons, // 0x0000000F
  }
}
