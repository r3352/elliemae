// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.IPRestrictionForm
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools
{
  public class IPRestrictionForm : Form
  {
    private const string IP_Restriction_All_Applications = "Policies.IPRestrictionAllApplications";
    private IContainer components;
    private GroupContainer groupContainer1;
    private Button btnClose;
    private GridView gridViewIPRanges;
    private StandardIconButton stdIconBtnEdit;
    private StandardIconButton stdIconBtnAdd;
    private StandardIconButton stdIconBtnDelete;
    private CheckBox cbApplicationRestriction;

    public IPRestrictionForm()
    {
      this.InitializeComponent();
      this.refreshGridView();
      this.cbApplicationRestriction.Checked = Convert.ToBoolean(Session.ServerManager.GetServerSetting("Policies.IPRestrictionAllApplications", false));
    }

    public void refreshGridView()
    {
      IPRange[] allowedIpRanges = Session.ConfigurationManager.GetAllowedIPRanges();
      this.gridViewIPRanges.Items.Clear();
      if (allowedIpRanges == null)
        return;
      foreach (IPRange ipRange in allowedIpRanges)
        this.gridViewIPRanges.Items.Add(new GVItem(new string[3]
        {
          string.IsNullOrEmpty(ipRange.Userid) ? "<Everyone>" : ipRange.Userid,
          ipRange.StartIP,
          ipRange.EndIP
        })
        {
          Tag = (object) ipRange
        });
    }

    private void stdIconBtnAdd_Click(object sender, EventArgs e)
    {
      if (new IPAddressesForm((IPRange) null).ShowDialog((IWin32Window) this) == DialogResult.Cancel)
        return;
      this.refreshGridView();
    }

    private void stdIconBtnEdit_Click(object sender, EventArgs e) => this.edit();

    private void gridViewIPRanges_DoubleClick(object sender, EventArgs e) => this.edit();

    private void edit()
    {
      if (this.gridViewIPRanges.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select an item to edit.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
      }
      else
      {
        if (new IPAddressesForm(this.gridViewIPRanges.SelectedItems[0].Tag as IPRange).ShowDialog((IWin32Window) this) == DialogResult.Cancel)
          return;
        this.refreshGridView();
      }
    }

    private void stdIconBtnDelete_Click(object sender, EventArgs e)
    {
      int[] oids = new int[this.gridViewIPRanges.SelectedItems.Count];
      for (int index = 0; index < this.gridViewIPRanges.SelectedItems.Count; ++index)
        oids[index] = ((IPRange) this.gridViewIPRanges.SelectedItems[index].Tag).OID;
      Session.ConfigurationManager.DeleteAllowedIPRanges(oids);
      this.refreshGridView();
    }

    private void gridViewIPRanges_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.stdIconBtnEdit.Enabled = this.gridViewIPRanges.SelectedItems.Count == 1;
      this.stdIconBtnDelete.Enabled = this.gridViewIPRanges.SelectedItems.Count >= 1;
    }

    private void cbApplicationRestriction_CheckedChanged(object sender, EventArgs e)
    {
      Session.ServerManager.UpdateServerSetting("Policies.IPRestrictionAllApplications", (object) this.cbApplicationRestriction.Checked, false);
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
      GVColumn gvColumn3 = new GVColumn();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (IPRestrictionForm));
      this.groupContainer1 = new GroupContainer();
      this.cbApplicationRestriction = new CheckBox();
      this.stdIconBtnEdit = new StandardIconButton();
      this.stdIconBtnAdd = new StandardIconButton();
      this.stdIconBtnDelete = new StandardIconButton();
      this.btnClose = new Button();
      this.gridViewIPRanges = new GridView();
      this.groupContainer1.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnEdit).BeginInit();
      ((ISupportInitialize) this.stdIconBtnAdd).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDelete).BeginInit();
      this.SuspendLayout();
      this.groupContainer1.Controls.Add((Control) this.cbApplicationRestriction);
      this.groupContainer1.Controls.Add((Control) this.stdIconBtnEdit);
      this.groupContainer1.Controls.Add((Control) this.stdIconBtnAdd);
      this.groupContainer1.Controls.Add((Control) this.stdIconBtnDelete);
      this.groupContainer1.Controls.Add((Control) this.btnClose);
      this.groupContainer1.Controls.Add((Control) this.gridViewIPRanges);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(457, 381);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "Allowed IP Addresses";
      this.cbApplicationRestriction.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cbApplicationRestriction.Location = new Point(12, 27);
      this.cbApplicationRestriction.Name = "cbApplicationRestriction";
      this.cbApplicationRestriction.Size = new Size(441, 38);
      this.cbApplicationRestriction.TabIndex = 65;
      this.cbApplicationRestriction.Text = "Apply to Encompass Connect products (Applies to Encompass SmartClient by default)";
      this.cbApplicationRestriction.UseVisualStyleBackColor = true;
      this.cbApplicationRestriction.CheckedChanged += new EventHandler(this.cbApplicationRestriction_CheckedChanged);
      this.stdIconBtnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnEdit.BackColor = Color.Transparent;
      this.stdIconBtnEdit.Location = new Point(410, 5);
      this.stdIconBtnEdit.MouseDownImage = (Image) null;
      this.stdIconBtnEdit.Name = "stdIconBtnEdit";
      this.stdIconBtnEdit.Size = new Size(16, 16);
      this.stdIconBtnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdIconBtnEdit.TabIndex = 64;
      this.stdIconBtnEdit.TabStop = false;
      this.stdIconBtnEdit.Click += new EventHandler(this.stdIconBtnEdit_Click);
      this.stdIconBtnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnAdd.BackColor = Color.Transparent;
      this.stdIconBtnAdd.Location = new Point(388, 5);
      this.stdIconBtnAdd.MouseDownImage = (Image) null;
      this.stdIconBtnAdd.Name = "stdIconBtnAdd";
      this.stdIconBtnAdd.Size = new Size(16, 16);
      this.stdIconBtnAdd.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnAdd.TabIndex = 63;
      this.stdIconBtnAdd.TabStop = false;
      this.stdIconBtnAdd.Click += new EventHandler(this.stdIconBtnAdd_Click);
      this.stdIconBtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDelete.BackColor = Color.Transparent;
      this.stdIconBtnDelete.Location = new Point(432, 5);
      this.stdIconBtnDelete.MouseDownImage = (Image) null;
      this.stdIconBtnDelete.Name = "stdIconBtnDelete";
      this.stdIconBtnDelete.Size = new Size(16, 16);
      this.stdIconBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDelete.TabIndex = 62;
      this.stdIconBtnDelete.TabStop = false;
      this.stdIconBtnDelete.Click += new EventHandler(this.stdIconBtnDelete_Click);
      this.btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnClose.DialogResult = DialogResult.OK;
      this.btnClose.Location = new Point(370, 351);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 23);
      this.btnClose.TabIndex = 41;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.gridViewIPRanges.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "ColumnUserID";
      gvColumn1.Text = "User ID";
      gvColumn1.Width = 120;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "ColumnFrom";
      gvColumn2.Text = "From";
      gvColumn2.Width = 150;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "ColumnTo";
      gvColumn3.Text = "To";
      gvColumn3.Width = 150;
      this.gridViewIPRanges.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gridViewIPRanges.Location = new Point(0, 65);
      this.gridViewIPRanges.Name = "gridViewIPRanges";
      this.gridViewIPRanges.Size = new Size(457, 280);
      this.gridViewIPRanges.TabIndex = 40;
      this.gridViewIPRanges.SelectedIndexChanged += new EventHandler(this.gridViewIPRanges_SelectedIndexChanged);
      this.gridViewIPRanges.DoubleClick += new EventHandler(this.gridViewIPRanges_DoubleClick);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(457, 381);
      this.Controls.Add((Control) this.groupContainer1);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (IPRestrictionForm);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Remote Access IP Restriction";
      this.groupContainer1.ResumeLayout(false);
      ((ISupportInitialize) this.stdIconBtnEdit).EndInit();
      ((ISupportInitialize) this.stdIconBtnAdd).EndInit();
      ((ISupportInitialize) this.stdIconBtnDelete).EndInit();
      this.ResumeLayout(false);
    }
  }
}
