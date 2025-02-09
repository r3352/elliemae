// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Documents.RequestEpassDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Documents
{
  public class RequestEpassDialog : Form
  {
    private LoanDataMgr loanDataMgr;
    private DocumentLog[] docList;
    private GridViewDataManager gvEpassMgr;
    private DocumentLog doc;
    private IContainer components;
    private ToolTip tooltip;
    private GroupContainer gcEpass;
    private GridView gvEpass;
    private Button btnOrder;
    private Button btnCancel;
    private EMHelpLink helpLink;

    public RequestEpassDialog(LoanDataMgr loanDataMgr, DocumentLog[] docList)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
      this.docList = docList;
      this.initEpassList();
      this.loadEpassList();
    }

    public DocumentLog Document => this.doc;

    private void initEpassList()
    {
      this.gvEpassMgr = new GridViewDataManager(this.gvEpass, this.loanDataMgr);
      this.gvEpassMgr.CreateLayout(new TableLayout.Column[1]
      {
        GridViewDataManager.NameColumn
      });
    }

    private void loadEpassList()
    {
      foreach (DocumentLog doc in this.docList)
        this.gvEpassMgr.AddItem(doc);
      this.gvEpass.Sort(0, SortOrder.Ascending);
    }

    private void gvEpass_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.btnOrder_Click(source, EventArgs.Empty);
    }

    private void gvEpass_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnOrder.Enabled = this.gvEpass.SelectedItems.Count > 0;
    }

    private void btnOrder_Click(object sender, EventArgs e)
    {
      this.doc = (DocumentLog) this.gvEpass.SelectedItems[0].Tag;
      this.DialogResult = DialogResult.OK;
    }

    private void RequestEpassDialog_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      Session.Application.DisplayHelp(this.helpLink.HelpTag);
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
      this.tooltip = new ToolTip(this.components);
      this.gcEpass = new GroupContainer();
      this.gvEpass = new GridView();
      this.btnOrder = new Button();
      this.btnCancel = new Button();
      this.helpLink = new EMHelpLink();
      this.gcEpass.SuspendLayout();
      this.SuspendLayout();
      this.gcEpass.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gcEpass.Controls.Add((Control) this.gvEpass);
      this.gcEpass.Location = new Point(8, 8);
      this.gcEpass.Name = "gcEpass";
      this.gcEpass.Size = new Size(594, 444);
      this.gcEpass.TabIndex = 0;
      this.gcEpass.Text = "Request from Service Providers";
      this.gvEpass.AllowMultiselect = false;
      this.gvEpass.BorderStyle = BorderStyle.None;
      this.gvEpass.ClearSelectionsOnEmptyRowClick = false;
      this.gvEpass.Dock = DockStyle.Fill;
      this.gvEpass.HeaderHeight = 0;
      this.gvEpass.HeaderVisible = false;
      this.gvEpass.HoverToolTip = this.tooltip;
      this.gvEpass.Location = new Point(1, 26);
      this.gvEpass.Name = "gvEpass";
      this.gvEpass.Size = new Size(592, 417);
      this.gvEpass.TabIndex = 1;
      this.gvEpass.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvEpass.SelectedIndexChanged += new EventHandler(this.gvEpass_SelectedIndexChanged);
      this.gvEpass.ItemDoubleClick += new GVItemEventHandler(this.gvEpass_ItemDoubleClick);
      this.btnOrder.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOrder.Enabled = false;
      this.btnOrder.Location = new Point(452, 460);
      this.btnOrder.Name = "btnOrder";
      this.btnOrder.Size = new Size(75, 22);
      this.btnOrder.TabIndex = 2;
      this.btnOrder.Text = "Order";
      this.btnOrder.UseVisualStyleBackColor = true;
      this.btnOrder.Click += new EventHandler(this.btnOrder_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.CausesValidation = false;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(528, 460);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.helpLink.BackColor = Color.Transparent;
      this.helpLink.Cursor = Cursors.Hand;
      this.helpLink.HelpTag = "Request Documents and Settlement Services";
      this.helpLink.Location = new Point(8, 462);
      this.helpLink.Name = "helpLink";
      this.helpLink.Size = new Size(90, 18);
      this.helpLink.TabIndex = 4;
      this.helpLink.TabStop = false;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(610, 491);
      this.Controls.Add((Control) this.helpLink);
      this.Controls.Add((Control) this.btnOrder);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.gcEpass);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (RequestEpassDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Request";
      this.KeyDown += new KeyEventHandler(this.RequestEpassDialog_KeyDown);
      this.gcEpass.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
