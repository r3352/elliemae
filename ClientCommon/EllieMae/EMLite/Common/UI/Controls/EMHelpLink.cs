// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.Controls.EMHelpLink
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;

#nullable disable
namespace EllieMae.EMLite.Common.UI.Controls
{
  [DefaultProperty("HelpTag")]
  public class EMHelpLink : HelpLink
  {
    private string helpTag = string.Empty;
    private Sessions.Session session;

    [Category("Behavior")]
    [DefaultValue("")]
    public string HelpTag
    {
      get => this.helpTag;
      set => this.helpTag = value;
    }

    public void AssignSession(Sessions.Session session) => this.session = session;

    protected override void OnHelp(EventArgs e)
    {
      if (this.session == null)
        Session.Application.DisplayHelp(this.helpTag);
      else
        this.session.Application.DisplayHelp(this.helpTag);
    }
  }
}
