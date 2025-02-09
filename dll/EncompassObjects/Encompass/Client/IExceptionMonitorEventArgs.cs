// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.IExceptionMonitorEventArgs
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Client
{
  [Guid("B46164D8-F926-4640-8572-2A9ADC167CF0")]
  public interface IExceptionMonitorEventArgs
  {
    Exception Exception { get; }
  }
}
