// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.ISessionMonitorEventArgs
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Client
{
  [Guid("FE40EC91-16EA-4dbc-B969-849BDB44D936")]
  public interface ISessionMonitorEventArgs
  {
    SessionMonitorEventType EventType { get; }

    SessionInformation SessionInformation { get; }
  }
}
