// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.WebServices.GetDisclosureTrackingDetailsCompletedEventArgs
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

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
  public class GetDisclosureTrackingDetailsCompletedEventArgs : AsyncCompletedEventArgs
  {
    private object[] results;

    internal GetDisclosureTrackingDetailsCompletedEventArgs(
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
