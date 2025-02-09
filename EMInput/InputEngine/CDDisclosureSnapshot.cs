// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.CDDisclosureSnapshot
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
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class CDDisclosureSnapshot : Form, IOnlineHelpTarget, IHelp
  {
    private IDisclosureTracking2015Log[] logs;
    private IDisclosureTracking2015Log intentToProceed;
    private Sessions.Session session;
    private string[] discloseMethod = new string[6]
    {
      "U.S. Mail",
      "In Person",
      "Fax",
      "Other",
      "eFolder eDisclosures",
      "Email"
    };
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
    private DisclosureTracking2015Log log;
    private System.Runtime.InteropServices.ComTypes.IConnectionPoint conPt;
    private int cookie;
    private IHtmlInput input;
    private InputFormList inputList;
    private static object nobj = (object) Missing.Value;
    private bool[] loaded = new bool[5];
    private IContainer components;
    private GroupContainer groupContainer1;
    private TabControl tabCD;
    private TabPage tpLEPg1;
    private AxWebBrowser axWebBrowser;
    private TabPage tpLEPg2;
    private AxWebBrowser axWebBrowser1;
    private TabPage tpLEPg3;
    private AxWebBrowser axWebBrowser2;
    private GridView gvDisclosedCD;
    private Label label1;
    private Button btnOK;
    private Button btnCancel;
    private TabPage tabPage1;
    private TabPage tabPage2;
    private AxWebBrowser axWebBrowser3;
    private AxWebBrowser axWebBrowser4;

    public CDDisclosureSnapshot(Sessions.Session session)
    {
      this.InitializeComponent();
      this.session = session;
      this.logs = session.LoanData.GetLogList().GetAllIDisclosureTracking2015Log(true);
      this.inputList = new InputFormList(Session.SessionObjects);
      this.findIntentToProceed();
      this.populateGrid();
      if (this.gvDisclosedCD.SelectedItems.Count != 0 || this.gvDisclosedCD.Items.Count <= 0)
        return;
      this.gvDisclosedCD.Items[0].Selected = true;
    }

    public void DefaultSelection(string selectLog)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvDisclosedCD.Items)
        gvItem.Selected = ((LogRecordBase) gvItem.Tag).Guid == selectLog;
    }

    private void findIntentToProceed()
    {
      foreach (DisclosureTracking2015Log log in this.logs)
      {
        if (log.DisclosedForCD && log.IntentToProceed)
          this.intentToProceed = (IDisclosureTracking2015Log) log;
      }
      if (this.intentToProceed != null)
        return;
      IDisclosureTracking2015Log idisclosureTracking2015Log = this.session.LoanData.GetLogList().GetLatestIDisclosureTracking2015Log(DisclosureTracking2015Log.DisclosureTrackingType.CD);
      if (idisclosureTracking2015Log == null || (!(idisclosureTracking2015Log.BorrowerActualReceivedDate != DateTime.MinValue) || !(idisclosureTracking2015Log.BorrowerActualReceivedDate <= DateTime.Now)) && (!(idisclosureTracking2015Log.BorrowerPresumedReceivedDate != DateTime.MinValue) || !(idisclosureTracking2015Log.BorrowerPresumedReceivedDate <= DateTime.Now)) && (!idisclosureTracking2015Log.IsBorrowerPresumedDateLocked || !(idisclosureTracking2015Log.LockedBorrowerPresumedReceivedDate != DateTime.MinValue) || !(idisclosureTracking2015Log.LockedBorrowerPresumedReceivedDate <= DateTime.Now)) && (!(idisclosureTracking2015Log.CoBorrowerActualReceivedDate != DateTime.MinValue) || !(idisclosureTracking2015Log.CoBorrowerActualReceivedDate <= DateTime.Now)) && (!(idisclosureTracking2015Log.CoBorrowerPresumedReceivedDate != DateTime.MinValue) || !(idisclosureTracking2015Log.CoBorrowerPresumedReceivedDate <= DateTime.Now)) && (!idisclosureTracking2015Log.IsCoBorrowerPresumedDateLocked || !(idisclosureTracking2015Log.LockedCoBorrowerPresumedReceivedDate != DateTime.MinValue) || !(idisclosureTracking2015Log.LockedCoBorrowerPresumedReceivedDate <= DateTime.Now)))
        return;
      this.intentToProceed = idisclosureTracking2015Log;
    }

    private void populateGrid()
    {
      foreach (DisclosureTracking2015Log log in this.logs)
      {
        if (log.DisclosedForCD)
        {
          GVItem gvItem = new GVItem();
          gvItem.Tag = (object) log;
          GVSubItemCollection subItems1 = gvItem.SubItems;
          DateTime dateTime = log.DisclosedDate;
          string str1 = dateTime.ToString("d");
          subItems1.Add((object) str1);
          if (log.IsDisclosedByLocked)
            gvItem.SubItems.Add((object) log.LockedDisclosedByField);
          else
            gvItem.SubItems.Add((object) (log.DisclosedByFullName + "(" + log.DisclosedBy + ")"));
          switch (log.DisclosureMethod)
          {
            case DisclosureTrackingBase.DisclosedMethod.ByMail:
              gvItem.SubItems.Add((object) this.discloseMethod[0]);
              break;
            case DisclosureTrackingBase.DisclosedMethod.eDisclosure:
              gvItem.SubItems.Add((object) this.discloseMethod[4]);
              break;
            case DisclosureTrackingBase.DisclosedMethod.Fax:
              gvItem.SubItems.Add((object) this.discloseMethod[2]);
              break;
            case DisclosureTrackingBase.DisclosedMethod.InPerson:
              gvItem.SubItems.Add((object) this.discloseMethod[1]);
              break;
            case DisclosureTrackingBase.DisclosedMethod.Other:
              gvItem.SubItems.Add((object) this.discloseMethod[3]);
              break;
            case DisclosureTrackingBase.DisclosedMethod.Email:
              gvItem.SubItems.Add((object) this.discloseMethod[5]);
              break;
            default:
              gvItem.SubItems.Add((object) this.discloseMethod[0]);
              break;
          }
          if (log.BorrowerActualReceivedDate != DateTime.MinValue)
          {
            GVSubItemCollection subItems2 = gvItem.SubItems;
            dateTime = log.BorrowerActualReceivedDate;
            string str2 = dateTime.ToString("d");
            subItems2.Add((object) str2);
          }
          else
            gvItem.SubItems.Add((object) "");
          if (log.IsBorrowerPresumedDateLocked)
          {
            if (log.LockedBorrowerPresumedReceivedDate != DateTime.MinValue)
            {
              GVSubItemCollection subItems3 = gvItem.SubItems;
              dateTime = log.LockedBorrowerPresumedReceivedDate;
              string str3 = dateTime.ToString("d");
              subItems3.Add((object) str3);
            }
            else
              gvItem.SubItems.Add((object) "");
          }
          else if (log.BorrowerPresumedReceivedDate != DateTime.MinValue)
          {
            GVSubItemCollection subItems4 = gvItem.SubItems;
            dateTime = log.BorrowerPresumedReceivedDate;
            string str4 = dateTime.ToString("d");
            subItems4.Add((object) str4);
          }
          else
          {
            GVSubItemCollection subItems5 = gvItem.SubItems;
            dateTime = this.getReceivedDate(log.DisclosedDate);
            string str5 = dateTime.ToString("d");
            subItems5.Add((object) str5);
          }
          this.gvDisclosedCD.Items.Add(gvItem);
        }
      }
    }

    private DateTime getReceivedDate(DateTime disclosedDate)
    {
      return Session.SessionObjects.GetBusinessCalendar(CalendarType.RegZ).AddBusinessDays(disclosedDate, 3, true);
    }

    public IDisclosureTracking2015Log SelectedDT => this.intentToProceed;

    public string GetHelpTargetName() => "";

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, this.GetHelpTargetName());
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvDisclosedCD.Items)
      {
        if (gvItem.SubItems[0].Checked)
          this.intentToProceed = (IDisclosureTracking2015Log) gvItem.Tag;
      }
      this.DialogResult = DialogResult.OK;
    }

    private void gvDisclosedCD_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      this.btnOK.Enabled = ((IEnumerable<GVItem>) this.gvDisclosedCD.GetCheckedItems(0)).Count<GVItem>() > 0;
      this.gvDisclosedCD.SubItemCheck -= new GVSubItemEventHandler(this.gvDisclosedCD_SubItemCheck);
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvDisclosedCD.Items)
      {
        if (e.SubItem.Item != gvItem)
          gvItem.SubItems[0].Checked = false;
      }
      this.gvDisclosedCD.SubItemCheck += new GVSubItemEventHandler(this.gvDisclosedCD_SubItemCheck);
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

    private void tabCD_SelectedIndexChanged(object sender, EventArgs e)
    {
      InputFormInfo formByName = this.inputList.GetFormByName(this.formName[0]);
      if (this.log == null)
        return;
      this.input = (IHtmlInput) new DisclosedCDHandler((IDisclosureTracking2015Log) this.log, LoanReportFieldDefs.GetFieldDefs(Session.DefaultInstance, LoanReportFieldFlags.AllLoanDataFields));
      AxWebBrowser brw = this.axWebBrowser;
      bool flag = false;
      if (this.tabCD.SelectedIndex == 0 && !this.loaded[0])
      {
        formByName = this.inputList.GetFormByName(this.formName[0]);
        brw = this.axWebBrowser;
        this.loaded[0] = true;
        flag = true;
      }
      else if (this.tabCD.SelectedIndex == 1 && !this.loaded[1])
      {
        formByName = this.inputList.GetFormByName(this.formName[1]);
        brw = this.axWebBrowser1;
        this.loaded[1] = true;
        flag = true;
      }
      else if (this.tabCD.SelectedIndex == 2 && !this.loaded[2])
      {
        formByName = this.inputList.GetFormByName(this.formName[2]);
        brw = this.axWebBrowser2;
        this.loaded[2] = true;
        flag = true;
      }
      else if (this.tabCD.SelectedIndex == 3 && !this.loaded[3])
      {
        formByName = this.inputList.GetFormByName(this.formName[3]);
        brw = this.axWebBrowser3;
        this.loaded[3] = true;
        flag = true;
      }
      else if (this.tabCD.SelectedIndex == 4 && !this.loaded[4])
      {
        formByName = this.inputList.GetFormByName(this.formName[4]);
        brw = this.axWebBrowser4;
        this.loaded[4] = true;
        flag = true;
      }
      if (!flag)
        return;
      this.hookUpBrowserHandler(brw);
      string formHtmlPath = FormStore.GetFormHTMLPath(Session.DefaultInstance, formByName);
      brw.Navigate(formHtmlPath, ref CDDisclosureSnapshot.nobj, ref CDDisclosureSnapshot.nobj, ref CDDisclosureSnapshot.nobj, ref CDDisclosureSnapshot.nobj);
    }

    private void gvDisclosedCD_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.gvDisclosedCD.SelectedItems.Count == 0)
        return;
      this.log = (DisclosureTracking2015Log) this.gvDisclosedCD.SelectedItems[0].Tag;
      if (!this.log.IsLoanDataListExist)
        this.log.PopulateLoanDataList(Session.LoanDataMgr.GetLoanSnapshot(LogSnapshotType.DisclosureTracking, new Guid(this.log.Guid), !this.log.UCDCreationError && (this.log.DisclosedForCD || this.log.DisclosedForLE)));
      this.loaded = new bool[5];
      this.tabCD.SelectedIndex = 0;
      this.tabCD_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (CDDisclosureSnapshot));
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      this.groupContainer1 = new GroupContainer();
      this.tabCD = new TabControl();
      this.tpLEPg1 = new TabPage();
      this.axWebBrowser = new AxWebBrowser();
      this.tpLEPg2 = new TabPage();
      this.axWebBrowser1 = new AxWebBrowser();
      this.tpLEPg3 = new TabPage();
      this.axWebBrowser2 = new AxWebBrowser();
      this.tabPage1 = new TabPage();
      this.axWebBrowser3 = new AxWebBrowser();
      this.tabPage2 = new TabPage();
      this.axWebBrowser4 = new AxWebBrowser();
      this.gvDisclosedCD = new GridView();
      this.label1 = new Label();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.groupContainer1.SuspendLayout();
      this.tabCD.SuspendLayout();
      this.tpLEPg1.SuspendLayout();
      this.axWebBrowser.BeginInit();
      this.tpLEPg2.SuspendLayout();
      this.axWebBrowser1.BeginInit();
      this.tpLEPg3.SuspendLayout();
      this.axWebBrowser2.BeginInit();
      this.tabPage1.SuspendLayout();
      this.axWebBrowser3.BeginInit();
      this.tabPage2.SuspendLayout();
      this.axWebBrowser4.BeginInit();
      this.SuspendLayout();
      this.groupContainer1.Controls.Add((Control) this.tabCD);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(14, 256);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(756, 320);
      this.groupContainer1.TabIndex = 8;
      this.groupContainer1.Text = "View the snapshot of the highlighted Closing Disclosure below.";
      this.tabCD.Controls.Add((Control) this.tpLEPg1);
      this.tabCD.Controls.Add((Control) this.tpLEPg2);
      this.tabCD.Controls.Add((Control) this.tpLEPg3);
      this.tabCD.Controls.Add((Control) this.tabPage1);
      this.tabCD.Controls.Add((Control) this.tabPage2);
      this.tabCD.Dock = DockStyle.Fill;
      this.tabCD.Location = new Point(1, 26);
      this.tabCD.Name = "tabCD";
      this.tabCD.SelectedIndex = 0;
      this.tabCD.Size = new Size(754, 293);
      this.tabCD.TabIndex = 4;
      this.tabCD.SelectedIndexChanged += new EventHandler(this.tabCD_SelectedIndexChanged);
      this.tpLEPg1.Controls.Add((Control) this.axWebBrowser);
      this.tpLEPg1.Location = new Point(4, 22);
      this.tpLEPg1.Name = "tpLEPg1";
      this.tpLEPg1.Padding = new Padding(3);
      this.tpLEPg1.Size = new Size(746, 267);
      this.tpLEPg1.TabIndex = 0;
      this.tpLEPg1.Text = "CD Page 1";
      this.tpLEPg1.UseVisualStyleBackColor = true;
      this.axWebBrowser.Dock = DockStyle.Fill;
      this.axWebBrowser.Enabled = true;
      this.axWebBrowser.Location = new Point(3, 3);
      this.axWebBrowser.OcxState = (AxHost.State) componentResourceManager.GetObject("axWebBrowser.OcxState");
      this.axWebBrowser.Size = new Size(740, 261);
      this.axWebBrowser.TabIndex = 7;
      this.tpLEPg2.Controls.Add((Control) this.axWebBrowser1);
      this.tpLEPg2.Location = new Point(4, 22);
      this.tpLEPg2.Name = "tpLEPg2";
      this.tpLEPg2.Padding = new Padding(3);
      this.tpLEPg2.Size = new Size(746, 267);
      this.tpLEPg2.TabIndex = 1;
      this.tpLEPg2.Text = "CD Page 2";
      this.tpLEPg2.UseVisualStyleBackColor = true;
      this.axWebBrowser1.Dock = DockStyle.Fill;
      this.axWebBrowser1.Enabled = true;
      this.axWebBrowser1.Location = new Point(3, 3);
      this.axWebBrowser1.OcxState = (AxHost.State) componentResourceManager.GetObject("axWebBrowser1.OcxState");
      this.axWebBrowser1.Size = new Size(740, 261);
      this.axWebBrowser1.TabIndex = 7;
      this.tpLEPg3.Controls.Add((Control) this.axWebBrowser2);
      this.tpLEPg3.Location = new Point(4, 22);
      this.tpLEPg3.Name = "tpLEPg3";
      this.tpLEPg3.Size = new Size(746, 267);
      this.tpLEPg3.TabIndex = 2;
      this.tpLEPg3.Text = "CD Page 3";
      this.tpLEPg3.UseVisualStyleBackColor = true;
      this.axWebBrowser2.Dock = DockStyle.Fill;
      this.axWebBrowser2.Enabled = true;
      this.axWebBrowser2.Location = new Point(0, 0);
      this.axWebBrowser2.MaximumSize = new Size(941, 734);
      this.axWebBrowser2.OcxState = (AxHost.State) componentResourceManager.GetObject("axWebBrowser2.OcxState");
      this.axWebBrowser2.Size = new Size(746, 267);
      this.axWebBrowser2.TabIndex = 8;
      this.tabPage1.Controls.Add((Control) this.axWebBrowser3);
      this.tabPage1.Location = new Point(4, 22);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new Padding(3);
      this.tabPage1.Size = new Size(746, 267);
      this.tabPage1.TabIndex = 3;
      this.tabPage1.Text = "CD Page 4";
      this.tabPage1.UseVisualStyleBackColor = true;
      this.axWebBrowser3.Dock = DockStyle.Fill;
      this.axWebBrowser3.Enabled = true;
      this.axWebBrowser3.Location = new Point(3, 3);
      this.axWebBrowser3.MaximumSize = new Size(941, 734);
      this.axWebBrowser3.OcxState = (AxHost.State) componentResourceManager.GetObject("axWebBrowser3.OcxState");
      this.axWebBrowser3.Size = new Size(740, 261);
      this.axWebBrowser3.TabIndex = 9;
      this.tabPage2.Controls.Add((Control) this.axWebBrowser4);
      this.tabPage2.Location = new Point(4, 22);
      this.tabPage2.Name = "tabPage2";
      this.tabPage2.Padding = new Padding(3);
      this.tabPage2.Size = new Size(746, 267);
      this.tabPage2.TabIndex = 4;
      this.tabPage2.Text = "CD Page 5";
      this.tabPage2.UseVisualStyleBackColor = true;
      this.axWebBrowser4.Dock = DockStyle.Fill;
      this.axWebBrowser4.Enabled = true;
      this.axWebBrowser4.Location = new Point(3, 3);
      this.axWebBrowser4.MaximumSize = new Size(941, 734);
      this.axWebBrowser4.OcxState = (AxHost.State) componentResourceManager.GetObject("axWebBrowser4.OcxState");
      this.axWebBrowser4.Size = new Size(740, 261);
      this.axWebBrowser4.TabIndex = 9;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column2";
      gvColumn1.Text = "Sent Date";
      gvColumn1.Width = 150;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column3";
      gvColumn2.Text = "Sent By";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column4";
      gvColumn3.Text = "Sent Method";
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column5";
      gvColumn4.Text = "Actual Received Date";
      gvColumn4.Width = 150;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column6";
      gvColumn5.Text = "Presumed Received Date";
      gvColumn5.Width = 150;
      this.gvDisclosedCD.Columns.AddRange(new GVColumn[5]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5
      });
      this.gvDisclosedCD.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvDisclosedCD.Location = new Point(14, 61);
      this.gvDisclosedCD.Name = "gvDisclosedCD";
      this.gvDisclosedCD.Size = new Size(756, 189);
      this.gvDisclosedCD.TabIndex = 7;
      this.gvDisclosedCD.SelectedIndexChanged += new EventHandler(this.gvDisclosedCD_SelectedIndexChanged);
      this.gvDisclosedCD.SubItemCheck += new GVSubItemEventHandler(this.gvDisclosedCD_SubItemCheck);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(11, 27);
      this.label1.MaximumSize = new Size(775, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(614, 13);
      this.label1.TabIndex = 6;
      this.label1.Text = "Each Closing Disclosure (CD) disclosed to the borrower is listed below. Select a CD to view key dates and disclosure information. ";
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(611, 578);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 10;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(692, 578);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 9;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(781, 603);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.gvDisclosedCD);
      this.Controls.Add((Control) this.label1);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (CDDisclosureSnapshot);
      this.Text = "Disclosed CD Snapshot";
      this.groupContainer1.ResumeLayout(false);
      this.tabCD.ResumeLayout(false);
      this.tpLEPg1.ResumeLayout(false);
      this.axWebBrowser.EndInit();
      this.tpLEPg2.ResumeLayout(false);
      this.axWebBrowser1.EndInit();
      this.tpLEPg3.ResumeLayout(false);
      this.axWebBrowser2.EndInit();
      this.tabPage1.ResumeLayout(false);
      this.axWebBrowser3.EndInit();
      this.tabPage2.ResumeLayout(false);
      this.axWebBrowser4.EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
