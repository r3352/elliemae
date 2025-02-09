// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SecondaryMarketing.LockComparisonToolForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using Elli.Web.Host.SSF.Context;
using Elli.Web.Host.SSF.UI;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.SecondaryMarketing
{
  public class LockComparisonToolForm : UserControl
  {
    private LoanDataMgr loanMgr;
    private IContainer components;
    private Label labelTitle;
    private GroupContainer groupContainer;

    public LockComparisonToolForm(Sessions.Session session, LoanDataMgr loanMgr)
    {
      this.loanMgr = loanMgr;
      if (loanMgr.Dirty && !loanMgr.Save())
      {
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "Lock Comparison Tool result may not be accurate because the loan could not be saved.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      string tapeThinClientUrl = Session.SessionObjects.ConfigurationManager.GetBidTapeThinClientURL();
      string appSetting = EnConfigurationSettings.AppSettings["ThinClientBidTape.Url"];
      string str = (string.IsNullOrEmpty(appSetting) ? tapeThinClientUrl : appSetting) + "/lockComparisonTool";
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
      this.groupContainer.Controls.Add((Control) ssfControl);
      ssfControl.Dock = DockStyle.Fill;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.groupContainer = new GroupContainer();
      this.labelTitle = new Label();
      this.groupContainer.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer.Controls.Add((Control) this.labelTitle);
      this.groupContainer.Dock = DockStyle.Fill;
      this.groupContainer.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer.Location = new Point(0, 0);
      this.groupContainer.Name = "groupContainer";
      this.groupContainer.Size = new Size(800, 450);
      this.groupContainer.TabIndex = 3;
      this.labelTitle.AutoSize = true;
      this.labelTitle.BackColor = Color.Transparent;
      this.labelTitle.Font = new Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.labelTitle.Location = new Point(4, 5);
      this.labelTitle.Name = "labelTitle";
      this.labelTitle.Size = new Size(133, 15);
      this.labelTitle.TabIndex = 0;
      this.labelTitle.Text = "Lock Comparison Tool";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupContainer);
      this.Name = nameof (LockComparisonToolForm);
      this.Size = new Size(800, 450);
      this.groupContainer.ResumeLayout(false);
      this.groupContainer.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
