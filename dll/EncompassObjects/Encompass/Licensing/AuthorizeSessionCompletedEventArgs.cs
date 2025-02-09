// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Licensing.AuthorizeSessionCompletedEventArgs
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

#nullable disable
namespace EllieMae.Encompass.Licensing
{
  [GeneratedCode("wsdl", "2.0.50727.42")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  internal class AuthorizeSessionCompletedEventArgs : AsyncCompletedEventArgs
  {
    private object[] results;

    internal AuthorizeSessionCompletedEventArgs(
      object[] results,
      Exception exception,
      bool cancelled,
      object userState)
      : base(exception, cancelled, userState)
    {
      this.results = results;
    }

    public string Result
    {
      get
      {
        this.RaiseExceptionIfNecessary();
        return (string) this.results[0];
      }
    }
  }
}
