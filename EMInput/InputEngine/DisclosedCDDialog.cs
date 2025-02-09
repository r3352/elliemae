// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.DisclosedCDDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using AxSHDocVw;
using Elli.ElliEnum;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.InputEngine.Forms;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class DisclosedCDDialog : Form, IOnlineHelpTarget, IHelp
  {
    private const string className = "DisclosedCDDialog";
    private static string sw = Tracing.SwInputEngine;
    private string[] formName = new string[5]
    {
      "Closing Disclosure Page 1",
      "Closing Disclosure Page 2",
      "Closing Disclosure Page 3",
      "Closing Disclosure Page 4",
      "Closing Disclosure Page 5"
    };
    private IDisclosureTracking2015Log log;
    private System.Runtime.InteropServices.ComTypes.IConnectionPoint conPt;
    private int cookie;
    private IHtmlInput input;
    private InputFormList inputList;
    private static object nobj = (object) Missing.Value;
    private bool[] loaded = new bool[5];
    private IContainer components;
    private Button btnCancel;
    private TabControl tabControl1;
    private TabPage tpCDPg1;
    private Panel panel1;
    private AxWebBrowser axWebBrowser;
    private TabPage tpCDPg2;
    private AxWebBrowser axWebBrowser1;
    private TabPage tpCDPg3;
    private AxWebBrowser axWebBrowser2;
    private TabPage tpCDPg4;
    private AxWebBrowser axWebBrowser3;
    private TabPage tpCDPg5;
    private AxWebBrowser axWebBrowser4;
    private TabPage tabFeeChange;
    private GradientPanel gradientPanel1;
    private Label label1;
    private GridView feeDetailsGV;

    public DisclosedCDDialog(IDisclosureTracking2015Log log)
    {
      this.InitializeComponent();
      this.log = log;
      this.inputList = new InputFormList(Session.SessionObjects);
      if (!this.log.IsLoanDataListExist)
        this.log.PopulateLoanDataList(Session.LoanDataMgr.GetLoanSnapshot(LogSnapshotType.DisclosureTracking, new Guid(this.log.Guid), !this.log.UCDCreationError && (this.log.DisclosedForCD || this.log.DisclosedForLE)));
      bool flag = this.log.LogLoanData.GetField("4461") == "Y";
      if (this.log.AttributeList.ContainsKey("XCOCFeeLevelIndicator"))
        flag = this.log.AttributeList["XCOCFeeLevelIndicator"] == "Y";
      if (!(Session.StartupInfo.EnableCoC & flag))
        this.tabControl1.TabPages.Remove(this.tabFeeChange);
      else if (!this.tabControl1.TabPages.Contains(this.tabFeeChange))
        this.tabControl1.TabPages.Add(this.tabFeeChange);
      this.tabControl1_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    protected void hookUpBrowserHandler(AxWebBrowser brw)
    {
      System.Runtime.InteropServices.ComTypes.IConnectionPointContainer ocx = (System.Runtime.InteropServices.ComTypes.IConnectionPointContainer) brw.GetOcx();
      Guid guid = typeof (SHDocVw.DWebBrowserEvents2).GUID;
      ocx.FindConnectionPoint(ref guid, out this.conPt);
      FrmBrowserHandler pUnkSink = new FrmBrowserHandler(Session.DefaultInstance, (IWin32Window) this, this.input);
      this.conPt.Advise((object) pUnkSink, out this.cookie);
      pUnkSink.SetHelpTarget((IOnlineHelpTarget) this);
    }

    public string GetHelpTargetName() => "";

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, this.GetHelpTargetName());
    }

    private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
    {
      InputFormInfo formByName = this.inputList.GetFormByName(this.formName[0]);
      this.input = (IHtmlInput) new DisclosedCDHandler(this.log, LoanReportFieldDefs.GetFieldDefs(Session.DefaultInstance, LoanReportFieldFlags.AllLoanDataFields));
      AxWebBrowser brw = this.axWebBrowser;
      bool flag = false;
      if (this.tabControl1.SelectedIndex == 0 && !this.loaded[0])
      {
        formByName = this.inputList.GetFormByName(this.formName[0]);
        brw = this.axWebBrowser;
        this.loaded[0] = true;
        flag = true;
      }
      else if (this.tabControl1.SelectedIndex == 1 && !this.loaded[1])
      {
        formByName = this.inputList.GetFormByName(this.formName[1]);
        brw = this.axWebBrowser1;
        this.loaded[1] = true;
        flag = true;
      }
      else if (this.tabControl1.SelectedIndex == 2 && !this.loaded[2])
      {
        formByName = this.inputList.GetFormByName(this.formName[2]);
        brw = this.axWebBrowser2;
        this.loaded[2] = true;
        flag = true;
      }
      else if (this.tabControl1.SelectedIndex == 3 && !this.loaded[3])
      {
        formByName = this.inputList.GetFormByName(this.formName[3]);
        brw = this.axWebBrowser3;
        this.loaded[3] = true;
        flag = true;
      }
      else if (this.tabControl1.SelectedIndex == 4 && !this.loaded[4])
      {
        formByName = this.inputList.GetFormByName(this.formName[4]);
        brw = this.axWebBrowser4;
        this.loaded[4] = true;
        flag = true;
      }
      else if (this.tabControl1.SelectedIndex == 5 && this.feeDetailsGV.Items.Count == 0)
        this.addCOCFields();
      if (!flag)
        return;
      this.hookUpBrowserHandler(brw);
      string formHtmlPath = FormStore.GetFormHTMLPath(Session.DefaultInstance, formByName);
      brw.Navigate(formHtmlPath, ref DisclosedCDDialog.nobj, ref DisclosedCDDialog.nobj, ref DisclosedCDDialog.nobj, ref DisclosedCDDialog.nobj);
    }

    private void addCOCFields()
    {
      int num = Utils.ParseInt(this.log.AttributeList.ContainsKey("XCOCcount") ? (object) this.log.AttributeList["XCOCcount"] : (object) "0");
      if (num <= 0)
        return;
      for (int index = 1; index <= num; ++index)
      {
        List<string> stringList = new List<string>();
        string key1 = "XCOC" + index.ToString("00") + "01";
        string key2 = "XCOC" + index.ToString("00") + "_Description";
        string key3 = "XCOC" + index.ToString("00") + "05";
        string key4 = "XCOC" + index.ToString("00") + "06";
        string key5 = "XCOC" + index.ToString("00") + "_Amount";
        string key6 = "XCOC" + index.ToString("00") + "07";
        string key7 = "XCOC" + index.ToString("00") + "08";
        string key8 = "XCOC" + index.ToString("00") + "03";
        if (this.log.AttributeList.ContainsKey(key1) && this.log.AttributeList[key1] != "")
        {
          GVItem gvItem = new GVItem(this.log.AttributeList.ContainsKey(key2) ? this.log.AttributeList[key2] : "");
          gvItem.SubItems.Add(this.log.AttributeList.ContainsKey(key5) ? (object) this.log.AttributeList[key5] : (object) "");
          gvItem.SubItems.Add(this.log.AttributeList.ContainsKey(key3) ? (object) this.log.AttributeList[key3] : (object) "");
          gvItem.SubItems.Add(this.log.AttributeList.ContainsKey(key4) ? (object) this.log.AttributeList[key4] : (object) "");
          if (this.log.AttributeList.ContainsKey(key6))
          {
            string str = this.log.AttributeList[key6];
            if (string.Compare(str.ToLowerInvariant(), "other") == 0 && this.log.AttributeList.ContainsKey(key7))
              str = str + " - " + this.log.AttributeList[key7];
            gvItem.SubItems.Add((object) str);
            gvItem.SubItems.Add((object) this.log.AttributeList[key8]);
          }
          else
          {
            gvItem.SubItems.Add((object) "");
            gvItem.SubItems.Add((object) "");
          }
          this.feeDetailsGV.Items.Add(gvItem);
        }
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DisclosedCDDialog));
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      this.btnCancel = new Button();
      this.tabControl1 = new TabControl();
      this.tpCDPg1 = new TabPage();
      this.panel1 = new Panel();
      this.axWebBrowser = new AxWebBrowser();
      this.tpCDPg2 = new TabPage();
      this.axWebBrowser1 = new AxWebBrowser();
      this.tpCDPg3 = new TabPage();
      this.axWebBrowser2 = new AxWebBrowser();
      this.tpCDPg4 = new TabPage();
      this.axWebBrowser3 = new AxWebBrowser();
      this.tpCDPg5 = new TabPage();
      this.axWebBrowser4 = new AxWebBrowser();
      this.tabFeeChange = new TabPage();
      this.feeDetailsGV = new GridView();
      this.gradientPanel1 = new GradientPanel();
      this.label1 = new Label();
      this.tabControl1.SuspendLayout();
      this.tpCDPg1.SuspendLayout();
      this.panel1.SuspendLayout();
      this.axWebBrowser.BeginInit();
      this.tpCDPg2.SuspendLayout();
      this.axWebBrowser1.BeginInit();
      this.tpCDPg3.SuspendLayout();
      this.axWebBrowser2.BeginInit();
      this.tpCDPg4.SuspendLayout();
      this.axWebBrowser3.BeginInit();
      this.tpCDPg5.SuspendLayout();
      this.axWebBrowser4.BeginInit();
      this.tabFeeChange.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(874, 780);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 7;
      this.btnCancel.Text = "Close";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tabControl1.Controls.Add((Control) this.tpCDPg1);
      this.tabControl1.Controls.Add((Control) this.tpCDPg2);
      this.tabControl1.Controls.Add((Control) this.tpCDPg3);
      this.tabControl1.Controls.Add((Control) this.tpCDPg4);
      this.tabControl1.Controls.Add((Control) this.tpCDPg5);
      this.tabControl1.Controls.Add((Control) this.tabFeeChange);
      this.tabControl1.Location = new Point(0, 2);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new Size(949, 760);
      this.tabControl1.TabIndex = 7;
      this.tabControl1.SelectedIndexChanged += new EventHandler(this.tabControl1_SelectedIndexChanged);
      this.tpCDPg1.Controls.Add((Control) this.panel1);
      this.tpCDPg1.Location = new Point(4, 22);
      this.tpCDPg1.Name = "tpCDPg1";
      this.tpCDPg1.Padding = new Padding(3);
      this.tpCDPg1.Size = new Size(941, 734);
      this.tpCDPg1.TabIndex = 0;
      this.tpCDPg1.Text = "CD Page 1";
      this.tpCDPg1.UseVisualStyleBackColor = true;
      this.panel1.Controls.Add((Control) this.axWebBrowser);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(3, 3);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(935, 728);
      this.panel1.TabIndex = 7;
      this.axWebBrowser.Dock = DockStyle.Fill;
      this.axWebBrowser.Enabled = true;
      this.axWebBrowser.Location = new Point(0, 0);
      this.axWebBrowser.OcxState = (AxHost.State) componentResourceManager.GetObject("axWebBrowser.OcxState");
      this.axWebBrowser.Size = new Size(935, 728);
      this.axWebBrowser.TabIndex = 6;
      this.tpCDPg2.Controls.Add((Control) this.axWebBrowser1);
      this.tpCDPg2.Location = new Point(4, 22);
      this.tpCDPg2.Name = "tpCDPg2";
      this.tpCDPg2.Padding = new Padding(3);
      this.tpCDPg2.Size = new Size(941, 734);
      this.tpCDPg2.TabIndex = 1;
      this.tpCDPg2.Text = "CD Page 2";
      this.tpCDPg2.UseVisualStyleBackColor = true;
      this.axWebBrowser1.Dock = DockStyle.Fill;
      this.axWebBrowser1.Enabled = true;
      this.axWebBrowser1.Location = new Point(3, 3);
      this.axWebBrowser1.OcxState = (AxHost.State) componentResourceManager.GetObject("axWebBrowser1.OcxState");
      this.axWebBrowser1.Size = new Size(935, 728);
      this.axWebBrowser1.TabIndex = 7;
      this.tpCDPg3.Controls.Add((Control) this.axWebBrowser2);
      this.tpCDPg3.Location = new Point(4, 22);
      this.tpCDPg3.Name = "tpCDPg3";
      this.tpCDPg3.Padding = new Padding(3);
      this.tpCDPg3.Size = new Size(941, 734);
      this.tpCDPg3.TabIndex = 2;
      this.tpCDPg3.Text = "CD Page 3";
      this.tpCDPg3.UseVisualStyleBackColor = true;
      this.axWebBrowser2.Dock = DockStyle.Fill;
      this.axWebBrowser2.Enabled = true;
      this.axWebBrowser2.Location = new Point(3, 3);
      this.axWebBrowser2.MaximumSize = new Size(941, 734);
      this.axWebBrowser2.OcxState = (AxHost.State) componentResourceManager.GetObject("axWebBrowser2.OcxState");
      this.axWebBrowser2.Size = new Size(935, 728);
      this.axWebBrowser2.TabIndex = 7;
      this.tpCDPg4.Controls.Add((Control) this.axWebBrowser3);
      this.tpCDPg4.Location = new Point(4, 22);
      this.tpCDPg4.Name = "tpCDPg4";
      this.tpCDPg4.Padding = new Padding(3);
      this.tpCDPg4.Size = new Size(941, 734);
      this.tpCDPg4.TabIndex = 3;
      this.tpCDPg4.Text = "CD Page 4";
      this.tpCDPg4.UseVisualStyleBackColor = true;
      this.axWebBrowser3.Dock = DockStyle.Fill;
      this.axWebBrowser3.Enabled = true;
      this.axWebBrowser3.Location = new Point(3, 3);
      this.axWebBrowser3.OcxState = (AxHost.State) componentResourceManager.GetObject("axWebBrowser3.OcxState");
      this.axWebBrowser3.Size = new Size(935, 728);
      this.axWebBrowser3.TabIndex = 7;
      this.tpCDPg5.Controls.Add((Control) this.axWebBrowser4);
      this.tpCDPg5.Location = new Point(4, 22);
      this.tpCDPg5.Name = "tpCDPg5";
      this.tpCDPg5.Padding = new Padding(3);
      this.tpCDPg5.Size = new Size(941, 734);
      this.tpCDPg5.TabIndex = 4;
      this.tpCDPg5.Text = "CD Page 5";
      this.tpCDPg5.UseVisualStyleBackColor = true;
      this.axWebBrowser4.Dock = DockStyle.Fill;
      this.axWebBrowser4.Enabled = true;
      this.axWebBrowser4.Location = new Point(3, 3);
      this.axWebBrowser4.OcxState = (AxHost.State) componentResourceManager.GetObject("axWebBrowser4.OcxState");
      this.axWebBrowser4.Size = new Size(935, 728);
      this.axWebBrowser4.TabIndex = 7;
      this.tabFeeChange.Controls.Add((Control) this.feeDetailsGV);
      this.tabFeeChange.Controls.Add((Control) this.gradientPanel1);
      this.tabFeeChange.Location = new Point(4, 22);
      this.tabFeeChange.Name = "tabFeeChange";
      this.tabFeeChange.Padding = new Padding(3);
      this.tabFeeChange.Size = new Size(941, 734);
      this.tabFeeChange.TabIndex = 5;
      this.tabFeeChange.Text = "Fee Changes";
      this.tabFeeChange.UseVisualStyleBackColor = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "FeeDesc";
      gvColumn1.SpringToFit = true;
      gvColumn1.Text = "Fee Description";
      gvColumn1.Width = 156;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "NewAmt";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "New Amount $";
      gvColumn2.Width = 140;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Description";
      gvColumn3.SpringToFit = true;
      gvColumn3.Text = "Description";
      gvColumn3.Width = 180;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Comments";
      gvColumn4.SpringToFit = true;
      gvColumn4.Text = "Comments";
      gvColumn4.Width = 180;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Reason";
      gvColumn5.SpringToFit = true;
      gvColumn5.Text = "Reason";
      gvColumn5.Width = 140;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "changesReceivedDate";
      gvColumn6.SpringToFit = true;
      gvColumn6.Text = "Changes Received Date";
      gvColumn6.Width = 140;
      this.feeDetailsGV.Columns.AddRange(new GVColumn[6]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6
      });
      this.feeDetailsGV.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.feeDetailsGV.Location = new Point(3, 26);
      this.feeDetailsGV.Name = "feeDetailsGV";
      this.feeDetailsGV.Size = new Size(938, 201);
      this.feeDetailsGV.TabIndex = 45;
      this.gradientPanel1.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gradientPanel1.Controls.Add((Control) this.label1);
      this.gradientPanel1.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Location = new Point(3, 3);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(940, 25);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.TableFooter;
      this.gradientPanel1.TabIndex = 18;
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(4, 6);
      this.label1.Name = "label1";
      this.label1.Size = new Size(79, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "Fee Changes";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(959, 817);
      this.Controls.Add((Control) this.tabControl1);
      this.Controls.Add((Control) this.btnCancel);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (DisclosedCDDialog);
      this.Text = "Disclosed CD Snapshot";
      this.tabControl1.ResumeLayout(false);
      this.tpCDPg1.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.axWebBrowser.EndInit();
      this.tpCDPg2.ResumeLayout(false);
      this.axWebBrowser1.EndInit();
      this.tpCDPg3.ResumeLayout(false);
      this.axWebBrowser2.EndInit();
      this.tpCDPg4.ResumeLayout(false);
      this.axWebBrowser3.EndInit();
      this.tpCDPg5.ResumeLayout(false);
      this.axWebBrowser4.EndInit();
      this.tabFeeChange.ResumeLayout(false);
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
