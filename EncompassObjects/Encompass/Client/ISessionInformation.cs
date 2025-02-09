// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.ISessionInformation
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Client
{
  /// <summary>Interface for the SessionInformation class.</summary>
  /// <exclude />
  [Guid("60276403-09C8-40b8-B840-D111E226754F")]
  public interface ISessionInformation
  {
    string SessionID { get; }

    string UserID { get; }

    string ClientHostname { get; }

    string ClientIPAddress { get; }

    DateTime LoginTime { get; }
  }
}
