// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.ResizeMethod
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using System;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>Enumeration of the allow control resizing methods.</summary>
  [Flags]
  public enum ResizeMethod
  {
    /// <summary>No resizing method specified.</summary>
    None = 0,
    /// <summary>Resize controls to have same width.</summary>
    Width = 1,
    /// <summary>Resize controls to have same height.</summary>
    Height = 2,
    /// <summary>Resize controls to have same width and height.</summary>
    Both = Height | Width, // 0x00000003
  }
}
