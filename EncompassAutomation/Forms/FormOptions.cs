// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.FormOptions
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using System;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>
  /// Enumeration of the form management options when opening a Form.
  /// </summary>
  /// <remarks>This enumeration is for use by the Encompass applicaiton only.</remarks>
  [Flags]
  public enum FormOptions
  {
    /// <summary>No options selected.</summary>
    None = 0,
    /// <summary>Allow for internal management of the native HTML events.</summary>
    ManageEvents = 1,
    /// <summary>Allow for editing mode to be enabled in the form.</summary>
    AllowEditing = 2,
    /// <summary>Enables all options.</summary>
    All = 1023, // 0x000003FF
  }
}
