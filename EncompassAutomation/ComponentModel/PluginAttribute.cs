// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.ComponentModel.PluginAttribute
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using System;

#nullable disable
namespace EllieMae.Encompass.ComponentModel
{
  /// <summary>
  /// Marks a class as being an Encompass plugin which should be loaded and initialized at runtime.
  /// </summary>
  [AttributeUsage(AttributeTargets.Class)]
  public class PluginAttribute : Attribute
  {
  }
}
