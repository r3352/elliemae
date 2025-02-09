// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Overflow
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>
  /// Defines the different options for how text will overflow a control's boundaries.
  /// </summary>
  public enum Overflow
  {
    /// <summary>Specifies that overflow will be handled automatically by the control.</summary>
    Auto,
    /// <summary>Any overflow content will be clipped off at the edge of the control.</summary>
    Clip,
    /// <summary>The text will be truncated and elipses automatically added.</summary>
    Ellipses,
  }
}
