// Decompiled with JetBrains decompiler
// Type: EllieMae.EncompassAPI.WebServices.GetAttributeCompletedEventArgs
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EncompassAPI.WebServices
{
  /// <remarks />
  [GeneratedCode("System.Web.Services", "4.8.3752.0")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  public class GetAttributeCompletedEventArgs : AsyncCompletedEventArgs
  {
    private object[] results;

    internal GetAttributeCompletedEventArgs(
      object[] results,
      Exception exception,
      bool cancelled,
      object userState)
      : base(exception, cancelled, userState)
    {
      this.results = results;
    }

    /// <remarks />
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
