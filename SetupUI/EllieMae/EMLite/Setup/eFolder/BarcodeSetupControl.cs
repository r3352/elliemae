// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.eFolder.BarcodeSetupControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.eFolder;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.eFolder
{
  public class BarcodeSetupControl : SettingsUserControl
  {
    private BarcodeSetup barcodeSetup;
    private IContainer components;
    private GroupContainer gcBarcodes;
    private GroupContainer gcDocuments;
    private Splitter splitter;
    private GroupContainer gcStates;
    private GridView gvStates;
    private CheckBox chkEnabled;
    private CheckBox chkClosingDocs;
    private CheckBox chkOpeningDocs;
    private CheckBox chkRequestedDocs;
    private CheckBox chkPreClosingDocs;
    private CheckBox chkExistingAttachmentsFromEFolder;
    private Label label1;
    private CheckBox chkOpeningDocsInfoDocs;
    private CheckBox chkPreClosingInfoDocs;
    private CheckBox chkClosingInfoDocs;
    private CheckBox chkReqDocInfoDocs;

    public BarcodeSetupControl(SetUpContainer setupContainer)
      : base(setupContainer)
    {
      this.InitializeComponent();
      this.barcodeSetup = BarcodeSetup.GetBarcodeSetup(Session.ISession, false);
      this.initStateList();
      this.Reset();
    }

    private void initStateList()
    {
      string[] strArray = new string[53]
      {
        "AK",
        "AL",
        "AZ",
        "AR",
        "CA",
        "CO",
        "CT",
        "DE",
        "DC",
        "FL",
        "GA",
        "HI",
        "ID",
        "IL",
        "IN",
        "IA",
        "KS",
        "KY",
        "LA",
        "ME",
        "MD",
        "MA",
        "MI",
        "MN",
        "MS",
        "MO",
        "MT",
        "NE",
        "NV",
        "NH",
        "NJ",
        "NM",
        "NY",
        "NC",
        "ND",
        "OH",
        "OK",
        "OR",
        "PA",
        "PR",
        "RI",
        "SC",
        "SD",
        "TN",
        "TX",
        "UT",
        "VT",
        "VA",
        "VI",
        "WA",
        "WV",
        "WI",
        "WY"
      };
      foreach (string str in strArray)
        this.gvStates.Items.Add(new GVItem()
        {
          SubItems = {
            [1] = {
              Value = (object) str
            }
          },
          Tag = (object) str
        });
      this.gvStates.Sort(1, SortOrder.Ascending);
    }

    private void chkEnabled_CheckedChanged(object sender, EventArgs e)
    {
      this.gcStates.Enabled = this.chkEnabled.Checked;
      this.gcDocuments.Enabled = this.chkEnabled.Checked;
    }

    private void checkbox_Click(object sender, EventArgs e) => this.setDirtyFlag(true);

    private void gvStates_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      this.setDirtyFlag(true);
    }

    private void gcBarcodes_ClientSizeChanged(object sender, EventArgs e)
    {
      this.gcStates.Width = (this.gcBarcodes.ClientSize.Width - this.gcBarcodes.Padding.Left - this.splitter.Width - this.gcBarcodes.Padding.Right) / 2;
    }

    public override void Reset()
    {
      this.chkEnabled.Checked = this.barcodeSetup.Enabled;
      this.chkRequestedDocs.Checked = this.barcodeSetup.RequestedDocuments;
      this.chkOpeningDocs.Checked = this.barcodeSetup.OpeningDocuments;
      this.chkPreClosingDocs.Checked = this.barcodeSetup.PreClosingDocuments;
      this.chkClosingDocs.Checked = this.barcodeSetup.ClosingDocuments;
      this.chkExistingAttachmentsFromEFolder.Checked = this.barcodeSetup.ExistingAttachmentsFromEFolder;
      this.chkReqDocInfoDocs.Checked = this.barcodeSetup.RequestedDocumentsInfoDocs;
      this.chkOpeningDocsInfoDocs.Checked = this.barcodeSetup.OpeningDocumentsInfoDocs;
      this.chkPreClosingInfoDocs.Checked = this.barcodeSetup.PreClosingDocumentsInfoDocs;
      this.chkClosingInfoDocs.Checked = this.barcodeSetup.ClosingDocumentsInfoDocs;
      this.gvStates.BeginUpdate();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvStates.Items)
      {
        bool flag = true;
        if (this.barcodeSetup.PropertyStates != null && Array.IndexOf<object>((object[]) this.barcodeSetup.PropertyStates, gvItem.Tag) < 0)
          flag = false;
        gvItem.Checked = flag;
      }
      this.gvStates.EndUpdate();
      base.Reset();
    }

    public override void Save()
    {
      this.barcodeSetup.Enabled = this.chkEnabled.Checked;
      this.barcodeSetup.RequestedDocuments = this.chkRequestedDocs.Checked;
      this.barcodeSetup.OpeningDocuments = this.chkOpeningDocs.Checked;
      this.barcodeSetup.PreClosingDocuments = this.chkPreClosingDocs.Checked;
      this.barcodeSetup.ClosingDocuments = this.chkClosingDocs.Checked;
      this.barcodeSetup.ExistingAttachmentsFromEFolder = this.chkExistingAttachmentsFromEFolder.Checked;
      this.barcodeSetup.RequestedDocumentsInfoDocs = this.chkRequestedDocs.Checked && this.chkReqDocInfoDocs.Checked;
      this.barcodeSetup.OpeningDocumentsInfoDocs = this.chkOpeningDocs.Checked && this.chkOpeningDocsInfoDocs.Checked;
      this.barcodeSetup.PreClosingDocumentsInfoDocs = this.chkPreClosingDocs.Checked && this.chkPreClosingInfoDocs.Checked;
      this.barcodeSetup.ClosingDocumentsInfoDocs = this.chkClosingDocs.Checked && this.chkClosingInfoDocs.Checked;
      List<string> stringList = new List<string>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvStates.Items)
      {
        if (gvItem.Checked)
          stringList.Add(gvItem.Tag as string);
      }
      this.barcodeSetup.PropertyStates = stringList.Count >= this.gvStates.Items.Count ? (string[]) null : stringList.ToArray();
      BarcodeSetup.SaveBarcodeSetup(Session.ISession, this.barcodeSetup);
      base.Save();
    }

    private void chkRequestedDocs_CheckedChanged(object sender, EventArgs e)
    {
      this.chkReqDocInfoDocs.Enabled = this.chkRequestedDocs.Checked;
      if (this.chkRequestedDocs.Checked)
        return;
      this.chkReqDocInfoDocs.Checked = false;
    }

    private void chkOpeningDocs_CheckedChanged(object sender, EventArgs e)
    {
      this.chkOpeningDocsInfoDocs.Enabled = this.chkOpeningDocs.Checked;
      if (this.chkOpeningDocs.Checked)
        return;
      this.chkOpeningDocsInfoDocs.Checked = false;
    }

    private void chkPreClosingDocs_CheckedChanged(object sender, EventArgs e)
    {
      this.chkPreClosingInfoDocs.Enabled = this.chkPreClosingDocs.Checked;
      if (this.chkPreClosingDocs.Checked)
        return;
      this.chkPreClosingInfoDocs.Checked = false;
    }

    private void chkClosingDocs_CheckedChanged(object sender, EventArgs e)
    {
      this.chkClosingInfoDocs.Enabled = this.chkClosingDocs.Checked;
      if (this.chkClosingDocs.Checked)
        return;
      this.chkClosingInfoDocs.Checked = false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      this.gcBarcodes = new GroupContainer();
      this.chkEnabled = new CheckBox();
      this.gcDocuments = new GroupContainer();
      this.chkOpeningDocsInfoDocs = new CheckBox();
      this.chkPreClosingInfoDocs = new CheckBox();
      this.chkClosingInfoDocs = new CheckBox();
      this.chkReqDocInfoDocs = new CheckBox();
      this.label1 = new Label();
      this.chkExistingAttachmentsFromEFolder = new CheckBox();
      this.chkPreClosingDocs = new CheckBox();
      this.chkClosingDocs = new CheckBox();
      this.chkOpeningDocs = new CheckBox();
      this.chkRequestedDocs = new CheckBox();
      this.splitter = new Splitter();
      this.gcStates = new GroupContainer();
      this.gvStates = new GridView();
      this.gcBarcodes.SuspendLayout();
      this.gcDocuments.SuspendLayout();
      this.gcStates.SuspendLayout();
      this.SuspendLayout();
      this.gcBarcodes.Controls.Add((Control) this.chkEnabled);
      this.gcBarcodes.Controls.Add((Control) this.gcDocuments);
      this.gcBarcodes.Controls.Add((Control) this.splitter);
      this.gcBarcodes.Controls.Add((Control) this.gcStates);
      this.gcBarcodes.Dock = DockStyle.Fill;
      this.gcBarcodes.HeaderForeColor = SystemColors.ControlText;
      this.gcBarcodes.Location = new Point(0, 0);
      this.gcBarcodes.Name = "gcBarcodes";
      this.gcBarcodes.Padding = new Padding(3);
      this.gcBarcodes.Size = new Size(554, 352);
      this.gcBarcodes.TabIndex = 0;
      this.gcBarcodes.Text = "Document Bar Coding";
      this.gcBarcodes.ClientSizeChanged += new EventHandler(this.gcBarcodes_ClientSizeChanged);
      this.chkEnabled.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkEnabled.AutoSize = true;
      this.chkEnabled.BackColor = Color.Transparent;
      this.chkEnabled.Checked = true;
      this.chkEnabled.CheckState = CheckState.Checked;
      this.chkEnabled.Location = new Point(392, 6);
      this.chkEnabled.Name = "chkEnabled";
      this.chkEnabled.Size = new Size(158, 17);
      this.chkEnabled.TabIndex = 0;
      this.chkEnabled.Text = "Enable the bar code feature";
      this.chkEnabled.UseVisualStyleBackColor = false;
      this.chkEnabled.CheckedChanged += new EventHandler(this.chkEnabled_CheckedChanged);
      this.chkEnabled.Click += new EventHandler(this.checkbox_Click);
      this.gcDocuments.Controls.Add((Control) this.chkOpeningDocsInfoDocs);
      this.gcDocuments.Controls.Add((Control) this.chkPreClosingInfoDocs);
      this.gcDocuments.Controls.Add((Control) this.chkClosingInfoDocs);
      this.gcDocuments.Controls.Add((Control) this.chkReqDocInfoDocs);
      this.gcDocuments.Controls.Add((Control) this.label1);
      this.gcDocuments.Controls.Add((Control) this.chkExistingAttachmentsFromEFolder);
      this.gcDocuments.Controls.Add((Control) this.chkPreClosingDocs);
      this.gcDocuments.Controls.Add((Control) this.chkClosingDocs);
      this.gcDocuments.Controls.Add((Control) this.chkOpeningDocs);
      this.gcDocuments.Controls.Add((Control) this.chkRequestedDocs);
      this.gcDocuments.Dock = DockStyle.Fill;
      this.gcDocuments.HeaderForeColor = SystemColors.ControlText;
      this.gcDocuments.Location = new Point(279, 29);
      this.gcDocuments.Name = "gcDocuments";
      this.gcDocuments.Size = new Size(271, 319);
      this.gcDocuments.TabIndex = 3;
      this.gcDocuments.Text = "Add Bar Codes on the Following Documents";
      this.chkOpeningDocsInfoDocs.AutoSize = true;
      this.chkOpeningDocsInfoDocs.Checked = true;
      this.chkOpeningDocsInfoDocs.CheckState = CheckState.Checked;
      this.chkOpeningDocsInfoDocs.Location = new Point(31, 96);
      this.chkOpeningDocsInfoDocs.Name = "chkOpeningDocsInfoDocs";
      this.chkOpeningDocsInfoDocs.Size = new Size(143, 17);
      this.chkOpeningDocsInfoDocs.TabIndex = 9;
      this.chkOpeningDocsInfoDocs.Text = "Informational Documents";
      this.chkOpeningDocsInfoDocs.UseVisualStyleBackColor = true;
      this.chkOpeningDocsInfoDocs.Click += new EventHandler(this.checkbox_Click);
      this.chkPreClosingInfoDocs.AutoSize = true;
      this.chkPreClosingInfoDocs.Checked = true;
      this.chkPreClosingInfoDocs.CheckState = CheckState.Checked;
      this.chkPreClosingInfoDocs.Location = new Point(31, 136);
      this.chkPreClosingInfoDocs.Name = "chkPreClosingInfoDocs";
      this.chkPreClosingInfoDocs.Size = new Size(143, 17);
      this.chkPreClosingInfoDocs.TabIndex = 8;
      this.chkPreClosingInfoDocs.Text = "Informational Documents";
      this.chkPreClosingInfoDocs.UseVisualStyleBackColor = true;
      this.chkPreClosingInfoDocs.Click += new EventHandler(this.checkbox_Click);
      this.chkClosingInfoDocs.AutoSize = true;
      this.chkClosingInfoDocs.Checked = true;
      this.chkClosingInfoDocs.CheckState = CheckState.Checked;
      this.chkClosingInfoDocs.Location = new Point(31, 176);
      this.chkClosingInfoDocs.Name = "chkClosingInfoDocs";
      this.chkClosingInfoDocs.Size = new Size(143, 17);
      this.chkClosingInfoDocs.TabIndex = 7;
      this.chkClosingInfoDocs.Text = "Informational Documents";
      this.chkClosingInfoDocs.UseVisualStyleBackColor = true;
      this.chkClosingInfoDocs.Click += new EventHandler(this.checkbox_Click);
      this.chkReqDocInfoDocs.AutoSize = true;
      this.chkReqDocInfoDocs.Checked = true;
      this.chkReqDocInfoDocs.CheckState = CheckState.Checked;
      this.chkReqDocInfoDocs.Location = new Point(31, 56);
      this.chkReqDocInfoDocs.Name = "chkReqDocInfoDocs";
      this.chkReqDocInfoDocs.Size = new Size(143, 17);
      this.chkReqDocInfoDocs.TabIndex = 6;
      this.chkReqDocInfoDocs.Text = "Informational Documents";
      this.chkReqDocInfoDocs.UseVisualStyleBackColor = true;
      this.chkReqDocInfoDocs.Click += new EventHandler(this.checkbox_Click);
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 8f);
      this.label1.Location = new Point(28, 211);
      this.label1.Name = "label1";
      this.label1.Size = new Size(257, 13);
      this.label1.TabIndex = 5;
      this.label1.Text = "( for eDisclosure, Pre-Closing, and Closing Packages)";
      this.chkExistingAttachmentsFromEFolder.AutoSize = true;
      this.chkExistingAttachmentsFromEFolder.Checked = true;
      this.chkExistingAttachmentsFromEFolder.CheckState = CheckState.Checked;
      this.chkExistingAttachmentsFromEFolder.Location = new Point(12, 196);
      this.chkExistingAttachmentsFromEFolder.Name = "chkExistingAttachmentsFromEFolder";
      this.chkExistingAttachmentsFromEFolder.Size = new Size(185, 17);
      this.chkExistingAttachmentsFromEFolder.TabIndex = 4;
      this.chkExistingAttachmentsFromEFolder.Text = "Existing Attachments from eFolder";
      this.chkExistingAttachmentsFromEFolder.UseVisualStyleBackColor = true;
      this.chkExistingAttachmentsFromEFolder.Click += new EventHandler(this.checkbox_Click);
      this.chkPreClosingDocs.AutoSize = true;
      this.chkPreClosingDocs.Checked = true;
      this.chkPreClosingDocs.CheckState = CheckState.Checked;
      this.chkPreClosingDocs.Location = new Point(12, 116);
      this.chkPreClosingDocs.Name = "chkPreClosingDocs";
      this.chkPreClosingDocs.Size = new Size(136, 17);
      this.chkPreClosingDocs.TabIndex = 2;
      this.chkPreClosingDocs.Text = "Pre-Closing Documents";
      this.chkPreClosingDocs.UseVisualStyleBackColor = true;
      this.chkPreClosingDocs.CheckedChanged += new EventHandler(this.chkPreClosingDocs_CheckedChanged);
      this.chkPreClosingDocs.Click += new EventHandler(this.checkbox_Click);
      this.chkClosingDocs.AutoSize = true;
      this.chkClosingDocs.Checked = true;
      this.chkClosingDocs.CheckState = CheckState.Checked;
      this.chkClosingDocs.Location = new Point(12, 156);
      this.chkClosingDocs.Name = "chkClosingDocs";
      this.chkClosingDocs.Size = new Size(117, 17);
      this.chkClosingDocs.TabIndex = 3;
      this.chkClosingDocs.Text = "Closing Documents";
      this.chkClosingDocs.UseVisualStyleBackColor = true;
      this.chkClosingDocs.CheckedChanged += new EventHandler(this.chkClosingDocs_CheckedChanged);
      this.chkClosingDocs.Click += new EventHandler(this.checkbox_Click);
      this.chkOpeningDocs.AutoSize = true;
      this.chkOpeningDocs.Checked = true;
      this.chkOpeningDocs.CheckState = CheckState.Checked;
      this.chkOpeningDocs.Location = new Point(12, 76);
      this.chkOpeningDocs.Name = "chkOpeningDocs";
      this.chkOpeningDocs.Size = new Size(86, 17);
      this.chkOpeningDocs.TabIndex = 1;
      this.chkOpeningDocs.Text = "eDisclosures";
      this.chkOpeningDocs.UseVisualStyleBackColor = true;
      this.chkOpeningDocs.CheckedChanged += new EventHandler(this.chkOpeningDocs_CheckedChanged);
      this.chkOpeningDocs.Click += new EventHandler(this.checkbox_Click);
      this.chkRequestedDocs.AutoSize = true;
      this.chkRequestedDocs.Checked = true;
      this.chkRequestedDocs.CheckState = CheckState.Checked;
      this.chkRequestedDocs.Location = new Point(12, 36);
      this.chkRequestedDocs.Name = "chkRequestedDocs";
      this.chkRequestedDocs.Size = new Size(135, 17);
      this.chkRequestedDocs.TabIndex = 0;
      this.chkRequestedDocs.Text = "Requested Documents";
      this.chkRequestedDocs.UseVisualStyleBackColor = true;
      this.chkRequestedDocs.CheckedChanged += new EventHandler(this.chkRequestedDocs_CheckedChanged);
      this.chkRequestedDocs.Click += new EventHandler(this.checkbox_Click);
      this.splitter.Location = new Point(276, 29);
      this.splitter.Name = "splitter";
      this.splitter.Size = new Size(3, 319);
      this.splitter.TabIndex = 2;
      this.splitter.TabStop = false;
      this.gcStates.Controls.Add((Control) this.gvStates);
      this.gcStates.Dock = DockStyle.Left;
      this.gcStates.HeaderForeColor = SystemColors.ControlText;
      this.gcStates.Location = new Point(4, 29);
      this.gcStates.Name = "gcStates";
      this.gcStates.Size = new Size(272, 319);
      this.gcStates.TabIndex = 1;
      this.gcStates.Text = "Use Bar Codes in the Following Subject Property States";
      this.gvStates.AllowDrop = true;
      this.gvStates.BorderStyle = BorderStyle.None;
      this.gvStates.ClearSelectionsOnEmptyRowClick = false;
      gvColumn1.CheckBoxes = true;
      gvColumn1.ColumnHeaderCheckbox = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "colCheckbox";
      gvColumn1.Text = "";
      gvColumn1.Width = 24;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "colState";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "State";
      gvColumn2.Width = 246;
      this.gvStates.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvStates.Dock = DockStyle.Fill;
      this.gvStates.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvStates.Location = new Point(1, 26);
      this.gvStates.Name = "gvStates";
      this.gvStates.Size = new Size(270, 292);
      this.gvStates.TabIndex = 0;
      this.gvStates.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvStates.SubItemCheck += new GVSubItemEventHandler(this.gvStates_SubItemCheck);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcBarcodes);
      this.Name = nameof (BarcodeSetupControl);
      this.Size = new Size(554, 352);
      this.gcBarcodes.ResumeLayout(false);
      this.gcBarcodes.PerformLayout();
      this.gcDocuments.ResumeLayout(false);
      this.gcDocuments.PerformLayout();
      this.gcStates.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
