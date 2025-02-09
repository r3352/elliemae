// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.WebServices.GetSignedDocumentCompletedEventArgs
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
  [GeneratedCode("wsdl", "4.0.30319.1")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  public class GetSignedDocumentCompletedEventArgs : AsyncCompletedEventArgs
  {
    private object[] results;

    internal GetSignedDocumentCompletedEventArgs(
      object[] results,
      Exception exception,
      bool cancelled,
      object userState)
      : base(exception, cancelled, userState)
    {
      this.results = results;
    }

    public SignedDocument Result
    {
      get
      {
        this.RaiseExceptionIfNecessary();
        return (SignedDocument) this.results[0];
      }
    }
  }
}
