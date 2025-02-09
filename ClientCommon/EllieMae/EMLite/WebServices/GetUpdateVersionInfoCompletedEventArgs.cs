// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.WebServices.GetUpdateVersionInfoCompletedEventArgs
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.WebServices
{
  [GeneratedCode("wsdl", "2.0.50727.42")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  public class GetUpdateVersionInfoCompletedEventArgs : AsyncCompletedEventArgs
  {
    private object[] results;

    internal GetUpdateVersionInfoCompletedEventArgs(
      object[] results,
      Exception exception,
      bool cancelled,
      object userState)
      : base(exception, cancelled, userState)
    {
      this.results = results;
    }

    public object[] Result
    {
      get
      {
        this.RaiseExceptionIfNecessary();
        return (object[]) this.results[0];
      }
    }
  }
}
