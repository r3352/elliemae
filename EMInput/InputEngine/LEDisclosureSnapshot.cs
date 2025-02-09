// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.LEDisclosureSnapshot
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
  public class LEDisclosureSnapshot : Form, IOnlineHelpTarget, IHelp
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
    private const string className = "DisclosedLEDialog";
    private static string sw = Tracing.SwInputEngine;
    private string[] formName = new string[3]
    {
      "Loan Estimate Page 1",
      "Loan Estimate Page 2",
      "Loan Estimate Page 3"
    };
    private DisclosureTracking2015Log log;
    private System.Runtime.InteropServices.ComTypes.IConnectionPoint conPt;
    private int cookie;
    private IHtmlInput input;
    private InputFormList inputList;
    private static object nobj = (object) Missing.Value;
    private bool[] loaded = new bool[3];
    private bool ShowAll;
    private IContainer components;
    private Label label1;
    private Button btnCancel;
    private Button btnOK;
    private GridView gvDisclosedLE;
    private GroupContainer groupContainer1;
    private TabControl tabLE;
    private TabPage tpLEPg1;
    private AxWebBrowser axWebBrowser;
    private TabPage tpLEPg2;
    private AxWebBrowser axWebBrowser1;
    private TabPage tpLEPg3;
    private AxWebBrowser axWebBrowser2;

    public LEDisclosureSnapshot(Sessions.Session session, bool showAll = false)
    {
      this.InitializeComponent();
      this.ShowAll = showAll;
      if (showAll)
        this.label1.Text = "Each Loan Estimate (LE) disclosed to the borrower is listed below. Select an LE to view key dates and disclosure information.";
      this.session = session;
      this.logs = session.LoanData.GetLogList().GetAllIDisclosureTracking2015Log(true);
      this.inputList = new InputFormList(Session.SessionObjects);
      this.findIntentToProceed(!showAll);
      this.populateGrid();
      if (this.gvDisclosedLE.SelectedItems.Count != 0 || this.gvDisclosedLE.Items.Count <= 0)
        return;
      this.gvDisclosedLE.Items[0].Selected = true;
    }

    public void DefaultSelection(string selectLog)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvDisclosedLE.Items)
        gvItem.Selected = ((LogRecordBase) gvItem.Tag).Guid == selectLog;
    }

    private void findIntentToProceed(bool findDefault)
    {
      foreach (DisclosureTracking2015Log log in this.logs)
      {
        if (log.DisclosedForLE && log.IntentToProceed)
          this.intentToProceed = (IDisclosureTracking2015Log) log;
      }
      if (!(this.intentToProceed == null & findDefault))
        return;
      IDisclosureTracking2015Log idisclosureTracking2015Log = this.session.LoanData.GetLogList().GetLatestIDisclosureTracking2015Log(DisclosureTracking2015Log.DisclosureTrackingType.LE);
      if (idisclosureTracking2015Log == null || (!(idisclosureTracking2015Log.BorrowerActualReceivedDate != DateTime.MinValue) || !(idisclosureTracking2015Log.BorrowerActualReceivedDate <= DateTime.Now)) && (!(idisclosureTracking2015Log.BorrowerPresumedReceivedDate != DateTime.MinValue) || !(idisclosureTracking2015Log.BorrowerPresumedReceivedDate <= DateTime.Now)) && (!idisclosureTracking2015Log.IsBorrowerPresumedDateLocked || !(idisclosureTracking2015Log.LockedBorrowerPresumedReceivedDate != DateTime.MinValue) || !(idisclosureTracking2015Log.LockedBorrowerPresumedReceivedDate <= DateTime.Now)) && (!(idisclosureTracking2015Log.CoBorrowerActualReceivedDate != DateTime.MinValue) || !(idisclosureTracking2015Log.CoBorrowerActualReceivedDate <= DateTime.Now)) && (!(idisclosureTracking2015Log.CoBorrowerPresumedReceivedDate != DateTime.MinValue) || !(idisclosureTracking2015Log.CoBorrowerPresumedReceivedDate <= DateTime.Now)) && (!idisclosureTracking2015Log.IsCoBorrowerPresumedDateLocked || !(idisclosureTracking2015Log.LockedCoBorrowerPresumedReceivedDate != DateTime.MinValue) || !(idisclosureTracking2015Log.LockedCoBorrowerPresumedReceivedDate <= DateTime.Now)))
        return;
      this.intentToProceed = idisclosureTracking2015Log;
    }

    private void populateGrid()
    {
      foreach (IDisclosureTracking2015Log log in this.logs)
      {
        if (log.DisclosedForLE && (log.BorrowerActualReceivedDate != DateTime.MinValue && log.BorrowerActualReceivedDate <= DateTime.Now || log.BorrowerPresumedReceivedDate != DateTime.MinValue && log.BorrowerPresumedReceivedDate <= DateTime.Now || log.IsBorrowerPresumedDateLocked && log.LockedBorrowerPresumedReceivedDate != DateTime.MinValue && log.LockedBorrowerPresumedReceivedDate <= DateTime.Now || log.CoBorrowerActualReceivedDate != DateTime.MinValue && log.CoBorrowerActualReceivedDate <= DateTime.Now || log.CoBorrowerPresumedReceivedDate != DateTime.MinValue && log.CoBorrowerPresumedReceivedDate <= DateTime.Now || log.IsCoBorrowerPresumedDateLocked && log.LockedCoBorrowerPresumedReceivedDate != DateTime.MinValue && log.LockedCoBorrowerPresumedReceivedDate <= DateTime.Now || this.ShowAll))
        {
          GVItem gvItem = new GVItem();
          gvItem.Tag = (object) log;
          gvItem.SubItems.Add((object) "");
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
          if (log == this.intentToProceed)
          {
            gvItem.SubItems[0].Checked = true;
            gvItem.Selected = true;
          }
          if (this.ShowAll)
            gvItem.SubItems[0].CheckBoxEnabled = false;
          this.gvDisclosedLE.Items.Add(gvItem);
        }
      }
    }

    private DateTime getReceivedDate(DateTime disclosedDate)
    {
      return Session.SessionObjects.GetBusinessCalendar(CalendarType.RegZ).AddBusinessDays(disclosedDate, 3, true);
    }

    public IDisclosureTracking2015Log SelectedDT => this.intentToProceed;

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvDisclosedLE.Items)
      {
        if (gvItem.SubItems[0].Checked)
          this.intentToProceed = (IDisclosureTracking2015Log) gvItem.Tag;
      }
      this.DialogResult = DialogResult.OK;
    }

    private void gvDisclosedLE_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      this.btnOK.Enabled = ((IEnumerable<GVItem>) this.gvDisclosedLE.GetCheckedItems(0)).Count<GVItem>() > 0;
      this.gvDisclosedLE.SubItemCheck -= new GVSubItemEventHandler(this.gvDisclosedLE_SubItemCheck);
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvDisclosedLE.Items)
      {
        if (e.SubItem.Item != gvItem)
          gvItem.SubItems[0].Checked = false;
      }
      this.gvDisclosedLE.SubItemCheck += new GVSubItemEventHandler(this.gvDisclosedLE_SubItemCheck);
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

    private void gvDisclosedLE_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.gvDisclosedLE.SelectedItems.Count == 0)
        return;
      this.log = (DisclosureTracking2015Log) this.gvDisclosedLE.SelectedItems[0].Tag;
      if (!this.log.IsLoanDataListExist)
        this.log.PopulateLoanDataList(Session.LoanDataMgr.GetLoanSnapshot(LogSnapshotType.DisclosureTracking, new Guid(this.log.Guid), !this.log.UCDCreationError && (this.log.DisclosedForCD || this.log.DisclosedForLE)));
      this.loaded = new bool[3];
      this.tabLE.SelectedIndex = 0;
      this.tabLE_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    private void tabLE_SelectedIndexChanged(object sender, EventArgs e)
    {
      InputFormInfo formByName = this.inputList.GetFormByName(this.formName[0]);
      if (this.log == null)
        return;
      this.input = (IHtmlInput) new DisclosedLEHandler((IDisclosureTracking2015Log) this.log, LoanReportFieldDefs.GetFieldDefs(Session.DefaultInstance, LoanReportFieldFlags.AllLoanDataFields));
      AxWebBrowser brw = this.axWebBrowser;
      bool flag = false;
      if (this.tabLE.SelectedIndex == 0 && !this.loaded[0])
      {
        formByName = this.inputList.GetFormByName(this.formName[0]);
        brw = this.axWebBrowser;
        this.loaded[0] = true;
        flag = true;
      }
      else if (this.tabLE.SelectedIndex == 1 && !this.loaded[1])
      {
        formByName = this.inputList.GetFormByName(this.formName[1]);
        brw = this.axWebBrowser1;
        this.loaded[1] = true;
        flag = true;
      }
      else if (this.tabLE.SelectedIndex == 2 && !this.loaded[2])
      {
        formByName = this.inputList.GetFormByName(this.formName[2]);
        brw = this.axWebBrowser2;
        this.loaded[2] = true;
        flag = true;
      }
      if (!flag)
        return;
      this.hookUpBrowserHandler(brw);
      string formHtmlPath = FormStore.GetFormHTMLPath(Session.DefaultInstance, formByName);
      brw.Navigate(formHtmlPath, ref LEDisclosureSnapshot.nobj, ref LEDisclosureSnapshot.nobj, ref LEDisclosureSnapshot.nobj, ref LEDisclosureSnapshot.nobj);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (LEDisclosureSnapshot));
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      this.label1 = new Label();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.gvDisclosedLE = new GridView();
      this.groupContainer1 = new GroupContainer();
      this.tabLE = new TabControl();
      this.tpLEPg1 = new TabPage();
      this.axWebBrowser = new AxWebBrowser();
      this.tpLEPg2 = new TabPage();
      this.axWebBrowser1 = new AxWebBrowser();
      this.tpLEPg3 = new TabPage();
      this.axWebBrowser2 = new AxWebBrowser();
      this.groupContainer1.SuspendLayout();
      this.tabLE.SuspendLayout();
      this.tpLEPg1.SuspendLayout();
      this.axWebBrowser.BeginInit();
      this.tpLEPg2.SuspendLayout();
      this.axWebBrowser1.BeginInit();
      this.tpLEPg3.SuspendLayout();
      this.axWebBrowser2.BeginInit();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(13, 10);
      this.label1.MaximumSize = new Size(775, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(757, 26);
      this.label1.TabIndex = 0;
      this.label1.Text = componentResourceManager.GetString("label1.Text");
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(693, 568);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(612, 568);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 2;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      gvColumn1.CheckBoxes = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Intent to Proceed";
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Sent Date";
      gvColumn2.Width = 150;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Sent By";
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "Sent Method";
      gvColumn4.Width = 100;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.Text = "Actual Received Date";
      gvColumn5.Width = 150;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column6";
      gvColumn6.Text = "Presumed Received Date";
      gvColumn6.Width = 150;
      this.gvDisclosedLE.Columns.AddRange(new GVColumn[6]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6
      });
      this.gvDisclosedLE.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvDisclosedLE.Location = new Point(16, 44);
      this.gvDisclosedLE.Name = "gvDisclosedLE";
      this.gvDisclosedLE.Size = new Size(756, 189);
      this.gvDisclosedLE.TabIndex = 4;
      this.gvDisclosedLE.SelectedIndexChanged += new EventHandler(this.gvDisclosedLE_SelectedIndexChanged);
      this.gvDisclosedLE.SubItemCheck += new GVSubItemEventHandler(this.gvDisclosedLE_SubItemCheck);
      this.groupContainer1.Controls.Add((Control) this.tabLE);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(16, 239);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(756, 320);
      this.groupContainer1.TabIndex = 5;
      this.groupContainer1.Text = "View the snapshot of the highlighted Loan Estimate below.";
      this.tabLE.Controls.Add((Control) this.tpLEPg1);
      this.tabLE.Controls.Add((Control) this.tpLEPg2);
      this.tabLE.Controls.Add((Control) this.tpLEPg3);
      this.tabLE.Dock = DockStyle.Fill;
      this.tabLE.Location = new Point(1, 26);
      this.tabLE.Name = "tabLE";
      this.tabLE.SelectedIndex = 0;
      this.tabLE.Size = new Size(754, 293);
      this.tabLE.TabIndex = 4;
      this.tabLE.SelectedIndexChanged += new EventHandler(this.tabLE_SelectedIndexChanged);
      this.tpLEPg1.Controls.Add((Control) this.axWebBrowser);
      this.tpLEPg1.Location = new Point(4, 22);
      this.tpLEPg1.Name = "tpLEPg1";
      this.tpLEPg1.Padding = new Padding(3);
      this.tpLEPg1.Size = new Size(746, 267);
      this.tpLEPg1.TabIndex = 0;
      this.tpLEPg1.Text = "LE Page 1";
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
      this.tpLEPg2.Text = "LE Page 2";
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
      this.tpLEPg3.Text = "LE Page 3";
      this.tpLEPg3.UseVisualStyleBackColor = true;
      this.axWebBrowser2.Dock = DockStyle.Fill;
      this.axWebBrowser2.Enabled = true;
      this.axWebBrowser2.Location = new Point(0, 0);
      this.axWebBrowser2.MaximumSize = new Size(941, 734);
      this.axWebBrowser2.OcxState = (AxHost.State) componentResourceManager.GetObject("axWebBrowser2.OcxState");
      this.axWebBrowser2.Size = new Size(746, 267);
      this.axWebBrowser2.TabIndex = 8;
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(781, 603);
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.gvDisclosedLE);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LEDisclosureSnapshot);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Disclosed LE Snapshot";
      this.groupContainer1.ResumeLayout(false);
      this.tabLE.ResumeLayout(false);
      this.tpLEPg1.ResumeLayout(false);
      this.axWebBrowser.EndInit();
      this.tpLEPg2.ResumeLayout(false);
      this.axWebBrowser1.EndInit();
      this.tpLEPg3.ResumeLayout(false);
      this.axWebBrowser2.EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
