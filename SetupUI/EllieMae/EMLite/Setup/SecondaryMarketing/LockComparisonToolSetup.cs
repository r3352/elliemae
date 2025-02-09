// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SecondaryMarketing.LockComparisonToolSetup
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using Elli.Web.Host.SSF.Context;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.SecondaryMarketing
{
  public class LockComparisonToolSetup : SettingsUserControl, ILockComparisonConsole
  {
    private bool isDirty;
    private IContainer components;

    public LockComparisonToolSetup(SetUpContainer setupContainer)
      : this(setupContainer, Session.DefaultInstance)
    {
      Session.Application.RegisterService((object) this, typeof (ILockComparisonConsole));
    }

    public void SetIsDirty(bool isDirty) => this.isDirty = isDirty;

    public override bool IsDirty => this.isDirty;

    public LockComparisonToolSetup(SetUpContainer setupContainer, Sessions.Session session)
      : base(setupContainer)
    {
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      string tapeThinClientUrl = Session.SessionObjects.ConfigurationManager.GetBidTapeThinClientURL();
      string appSetting = EnConfigurationSettings.AppSettings["ThinClientBidTape.Url"];
      string str = (string.IsNullOrEmpty(appSetting) ? tapeThinClientUrl : appSetting) + "/settings/lockComparisonFields";
      SSFContext context = SSFContext.Create(Session.LoanDataMgr, SSFHostType.Network, new SSFGuest()
      {
        uri = str,
        scope = "sec",
        clientId = "04gkefdw"
      });
      context.unloadHandler = new Action(((SettingsUserControl) this).unloadHandler);
      context.parameters = new Dictionary<string, object>()
      {
        {
          "oapiBaseUrl",
          (object) Session.StartupInfo.OAPIGatewayBaseUri
        }
      };
      this.ssfControl.LoadApp(context);
      this.Controls.Add((Control) this.ssfControl);
      this.ssfControl.Dock = DockStyle.Fill;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(800, 450);
      this.Text = nameof (LockComparisonToolSetup);
    }
  }
}
