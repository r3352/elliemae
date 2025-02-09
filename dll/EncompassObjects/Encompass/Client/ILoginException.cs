// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.ILoginException
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Client
{
  [Guid("7AB0DF15-FB4D-493e-A879-81FBDD749859")]
  public interface ILoginException
  {
    string UserID { get; }

    string ClientHostname { get; }

    string ClientIPAddress { get; }

    string ApplicationName { get; }

    string WindowsUserName { get; }

    LoginErrorType ErrorType { get; }

    string Message { get; }
  }
}
