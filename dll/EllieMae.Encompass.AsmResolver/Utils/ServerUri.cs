// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.Utils.ServerUri
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using System;

#nullable disable
namespace EllieMae.Encompass.AsmResolver.Utils
{
  internal class ServerUri
  {
    private Uri uri;
    private bool webService;

    internal ServerUri(string uriString, bool webService)
    {
      this.uri = new Uri(uriString);
      this.webService = webService;
    }

    internal ServerUri(string uriString)
      : this(uriString, false)
    {
    }

    public override string ToString()
    {
      if (!(this.uri.AbsolutePath == "/"))
        return this.uri.AbsoluteUri;
      return this.webService ? this.uri.AbsoluteUri + "AuthenticationWS/AuthenticationService.asmx" : this.uri.AbsoluteUri + "EncompassSC/Default.aspx";
    }
  }
}
