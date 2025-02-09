// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.eFolder.ConfigurableWorkFlow
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using Elli.Web.Host.SSF.Context;
using Elli.Web.Host.SSF.UI;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.eFolder
{
  public class ConfigurableWorkFlow : SettingsUserControl, IConfigurableWorkFlow
  {
    private bool isDirty;
    private IContainer components;
    private Label label1;

    public ConfigurableWorkFlow(SetUpContainer setupContainer)
      : this(setupContainer, Session.DefaultInstance)
    {
      Session.Application.RegisterService((object) this, typeof (IConfigurableWorkFlow));
    }

    public void SetIsDirty(bool isDirty) => this.isDirty = isDirty;

    public override bool IsDirty => this.isDirty;

    public ConfigurableWorkFlow(SetUpContainer setupContainer, Sessions.Session session)
      : base(setupContainer)
    {
      this.InitializeComponent();
      string companySetting = Session.ConfigurationManager.GetCompanySetting("CHANNELOPTION", "DISPLAY");
      bool flag = companySetting.Contains("1") || companySetting.Contains("3");
      if (Session.ConfigurationManager.CheckIfAnyTPOSiteExists() & flag)
      {
        this.label1.Text = "";
        this.Dock = DockStyle.Fill;
        string tapeThinClientUrl = Session.SessionObjects.ConfigurationManager.GetBidTapeThinClientURL();
        string appSetting = EnConfigurationSettings.AppSettings["ThinClientBidTape.Url"];
        string str = (string.IsNullOrEmpty(appSetting) ? tapeThinClientUrl : appSetting) + "/settings/configurableWorkflowTemplates";
        SSFContext context = SSFContext.Create(Session.LoanDataMgr, SSFHostType.Network, new SSFGuest()
        {
          uri = str,
          scope = "sec",
          clientId = "04gkefdw"
        });
        context.parameters = new Dictionary<string, object>()
        {
          {
            "oapiBaseUrl",
            (object) Session.StartupInfo.OAPIGatewayBaseUri
          }
        };
        SSFControl ssfControl = new SSFControl();
        ssfControl.LoadApp(context);
        this.Controls.Add((Control) ssfControl);
        ssfControl.Dock = DockStyle.Fill;
      }
      else
        this.label1.Text = "To use the Configurable Workflow Template settings, you must first configure a TPO Connect site and at least one channel setting for Wholesale or Correspondent.";
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(0, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(35, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "label1";
      this.Controls.Add((Control) this.label1);
      this.Name = nameof (ConfigurableWorkFlow);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
