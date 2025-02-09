// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.ComponentModel.RequiredAttribute
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using System;

#nullable disable
namespace EllieMae.Encompass.ComponentModel
{
  /// <summary>
  /// Attribute used to indicate that a parameter is required.
  /// </summary>
  /// <remarks>For string-based parameters, this also means non-empty.</remarks>
  /// <exclude />
  [AttributeUsage(AttributeTargets.Parameter)]
  public class RequiredAttribute : Attribute
  {
  }
}
