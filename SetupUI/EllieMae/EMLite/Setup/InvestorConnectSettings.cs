// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.InvestorConnectSettings
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using Elli.Web.Host;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class InvestorConnectSettings : UserControl
  {
    public LoadWebPageForm investorForm;
    private IContainer components;

    public InvestorConnectSettings(Sessions.Session session, string pageTitle)
    {
      this.InitializeComponent();
      this.investorForm = this.loadWebPage(pageTitle);
      if (this.investorForm == null)
        return;
      this.investorForm.TopLevel = false;
      this.investorForm.FormBorderStyle = FormBorderStyle.None;
      this.investorForm.Visible = true;
      this.investorForm.Dock = DockStyle.Fill;
      this.Controls.Add((Control) this.investorForm);
    }

    private LoadWebPageForm loadWebPage(string pageTitle)
    {
      try
      {
        string str = "lender";
        string title = "Loan Delivery Status";
        switch (pageTitle)
        {
          case "Deliver Loans":
            str = "lender";
            title = "Loan Delivery Status";
            break;
          case "Partner Setup":
            str = "loans";
            title = "Partner Setup";
            break;
        }
        string webPageURL = Session.DefaultInstance.StartupInfo.InvestorConnectAppUrl + "/" + str + "/settings";
        Dictionary<string, object> webPageParams = new Dictionary<string, object>();
        string scope = "sc";
        var data = new
        {
          version = Session.DefaultInstance.UserInfo.EncompassVersion,
          user = new
          {
            id = string.Format("encompass\\{0}\\{1}", (object) Session.DefaultInstance.ServerIdentity.InstanceName, (object) Session.DefaultInstance.UserInfo.Userid)
          }
        };
        webPageParams.Add("encompass", (object) data);
        webPageParams.Add("errorMessages", (object) new List<string>());
        return new LoadWebPageForm(webPageURL, webPageParams, scope, title);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Error requesting for " + pageTitle + " Page.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return (LoadWebPageForm) null;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.SuspendLayout();
      this.AutoScaleDimensions = new SizeF(9f, 20f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Margin = new Padding(4, 5, 4, 5);
      this.Name = nameof (InvestorConnectSettings);
      this.Size = new Size(790, 562);
      this.ResumeLayout(false);
    }
  }
}
