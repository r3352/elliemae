// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.ILicenseMonitorEventArgs
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Client
{
  [Guid("8DA7011A-217B-4d12-A820-D536417FDCC4")]
  public interface ILicenseMonitorEventArgs
  {
    LicenseMonitorEventType EventType { get; }

    string UserID { get; }
  }
}
