// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AnalysisToolsUserControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using Elli.Web.Host.BrowserControls;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.eFolder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AnalysisToolsUserControl : UserControl
  {
    private EncWebFormBrowserControl analysistoolsBrowser;
    private IContainer components;
    private SetUpContainer setupContainer;
    private const string THINHOMEURL = "http://10.112.104.11/app/index.html#/AnalysisToolsUsingKendo";
    private const string THINSPECIALURL = "http://fieldsearch.elliemae.com";

    public AnalysisToolsUserControl(SetUpContainer setupContainer)
    {
      this.InitializeComponent();
      this.browser_navigate("http://10.112.104.11/app/index.html#/AnalysisToolsUsingKendo");
      this.setupContainer = setupContainer;
    }

    public void browser_navigate(string url)
    {
      this.analysistoolsBrowser.Refresh();
      this.analysistoolsBrowser.Navigate(url, (string) null, new Dictionary<string, string>()
      {
        {
          "User-Agent",
          "WebBrowserControl"
        }
      });
    }

    private string getBusRuleType(string webUrl) => webUrl.Split(';')[1].Split(':')[1];

    private string getBusRuleParams(string webUrl) => webUrl.Split(';')[2].Split(':')[1];

    private void invokeBusRuleDialog(string type, string webUrl)
    {
      switch (type)
      {
        case "Field Triggers":
          new TriggerListPanel(Session.DefaultInstance, false).newBtn_Click((object) null, (EventArgs) null);
          break;
        case "Input Form List":
          new InputFormRulePanel(Session.DefaultInstance, true).newBtn_Click((object) null, (EventArgs) null);
          break;
        case "Persona Access to Fields":
          new FieldAccessRulePanel(Session.DefaultInstance, true).newBtn_Click((object) null, (EventArgs) null);
          break;
        case "Persona Access to Loans":
          new LoanAccessRulePanel(Session.DefaultInstance, true).newBtn_Click((object) null, (EventArgs) null);
          break;
        case "Automated Conditions":
          new AutomatedConditionsListPanel(Session.DefaultInstance, false).newBtn_Click((object) null, (EventArgs) null);
          break;
        case "Field Data Entry":
          new FieldRulePanel(Session.DefaultInstance, false).newBtn_Click((object) null, (EventArgs) null);
          break;
        case "Milestone Completion":
          new ConditionRulePanel(Session.DefaultInstance, false).newBtn_Click((object) null, (EventArgs) null);
          break;
        case "Loan Form Printing":
          new PrintFormRulePanel(Session.DefaultInstance, false).newBtn_Click((object) null, (EventArgs) null);
          break;
        case "Print Auto Selection":
          new PrintSelectionRuleListPanel(Session.DefaultInstance, false).newBtn_Click((object) null, (EventArgs) null);
          break;
        case "HTML Email Templates":
          new EDMEmailControl(Session.DefaultInstance, (SetUpContainer) null, false).btnAdd_Click((object) null, (EventArgs) null);
          break;
        case "Piggyback Loan Synchronization":
          new PiggybackSetupPanel(Session.DefaultInstance, this.setupContainer).addFieldbtn_Click((object) null, (EventArgs) null);
          break;
        case "Loan Custom Fields":
          new CustomFieldsEditor(Session.DefaultInstance, false).btnAddField_Click((object) null, (EventArgs) null);
          break;
        case "Alerts":
          new Alerts(Session.DefaultInstance, (SetUpContainer) null, false).stdIconBtnAdd_Click((object) null, (EventArgs) null);
          break;
        case "FindFieldID":
          BusinessRuleFindFieldDialog ruleFindFieldDialog = new BusinessRuleFindFieldDialog(Session.DefaultInstance, (string[]) null, true, string.Empty, true, true);
          ruleFindFieldDialog.ShowDialog((IWin32Window) this);
          if (ruleFindFieldDialog.SelectedRequiredFields.Length == 0)
            break;
          string selectedRequiredField = ruleFindFieldDialog.SelectedRequiredFields[ruleFindFieldDialog.SelectedRequiredFields.Length - 1];
          this.browser_navigate("http://10.112.104.11/app/index.html#/AnalysisToolsUsingKendo" + this.getBusRuleParams(webUrl) + "/" + selectedRequiredField);
          break;
      }
    }

    private void analysistoolsBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
    {
      try
      {
        string webUrl = e.Url.ToString();
        if (string.IsNullOrEmpty(webUrl) || !webUrl.Contains("http://fieldsearch.elliemae.com"))
          return;
        e.Cancel = true;
        this.invokeBusRuleDialog(this.getBusRuleType(webUrl), webUrl);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
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
      this.components = (IContainer) new System.ComponentModel.Container();
      this.analysistoolsBrowser = BrowserFactory.GetWebBrowserInstance();
      this.analysistoolsBrowser.Dock = DockStyle.Fill;
      this.analysistoolsBrowser.Location = new Point(0, 0);
      this.analysistoolsBrowser.Name = "analysistoolsBrowser";
      this.analysistoolsBrowser.Size = new Size(762, 444);
      this.analysistoolsBrowser.TabIndex = 0;
      this.Controls.Add((Control) this.analysistoolsBrowser);
    }
  }
}
