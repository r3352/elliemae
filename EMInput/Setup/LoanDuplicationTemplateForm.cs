// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LoanDuplicationTemplateForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.Encompass.AsmResolver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class LoanDuplicationTemplateForm : Form, IHelp
  {
    private Sessions.Session session;
    private LoanDuplicationTemplate currentTemplate;
    private IContainer components;
    private Button btnSave;
    private Button btnCancel;
    private Label label1;
    private TextBox txtName;
    private TextBox txtDescription;
    private Label label2;
    private GridView gridViewBorInfo;
    private GroupContainer groupContainer1;
    private CheckBox chkBorInfo;
    private GroupContainer groupContainer2;
    private CheckBox chkCobInfo;
    private GroupContainer groupContainer3;
    private CheckBox chkBorEmpInfo;
    private GroupContainer groupContainer4;
    private CheckBox chkCobEmpInfo;
    private GroupContainer groupContainer5;
    private CheckBox chkProperty;
    private GroupContainer groupContainer6;
    private CheckBox chkBorAddr;
    private GroupContainer groupContainer7;
    private CheckBox chkCobAddr;
    private ComboBox cboProperty;
    private Label label3;
    private ComboBox cboBorPresentAddr;
    private Label label4;
    private ComboBox cboCoBorPresentAddr;
    private Label label5;
    private GridView gridViewCobInfo;
    private GridView gridViewBorEmpInfo;
    private GridView gridViewCobEmpInfo;
    private GridView gridViewPropertyAddr;
    private GridView gridViewBorPresentAddr;
    private GridView gridViewCoBorPresentAddr;
    private GroupContainer groupContainer8;
    private GridView gridViewAdditional;
    private Label label6;
    private StandardIconButton btnNew;
    private StandardIconButton btnDelete;

    public event EventHandler AdditionalFieldButtonClicked;

    public LoanDuplicationTemplateForm(
      Sessions.Session session,
      LoanDuplicationTemplate currentTemplate)
    {
      this.session = session;
      this.currentTemplate = currentTemplate;
      this.InitializeComponent();
      this.initForm();
      this.gridViewAdditional_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    private void initForm()
    {
      string empty = string.Empty;
      using (BinaryObject binaryObject = new BinaryObject(!AssemblyResolver.IsSmartClient ? Path.Combine(SystemSettings.DocDirAbsPath, "LoanDuplicationDefaultTemplate.xml") : AssemblyResolver.GetResourceFileFullPath(SystemSettings.DocDirRelPath + "LoanDuplicationDefaultTemplate.xml")))
      {
        LoanDuplicationDefaultTemplate duplicationDefaultTemplate = (LoanDuplicationDefaultTemplate) binaryObject;
        bool boolean = Convert.ToBoolean(this.session.ServerManager.GetServerSetting("FEATURE.ENABLEURLA2020", false));
        this.buildFieldList(this.gridViewBorInfo, duplicationDefaultTemplate.GetFields(LoanDuplicationDefaultTemplate.FieldTypes.BorrowerInformation, boolean));
        this.buildFieldList(this.gridViewCobInfo, duplicationDefaultTemplate.GetFields(LoanDuplicationDefaultTemplate.FieldTypes.CoBorrowerInformation, boolean));
        this.buildFieldList(this.gridViewBorEmpInfo, duplicationDefaultTemplate.GetFields(LoanDuplicationDefaultTemplate.FieldTypes.BorrowerEmployerInformation, boolean));
        this.buildFieldList(this.gridViewCobEmpInfo, duplicationDefaultTemplate.GetFields(LoanDuplicationDefaultTemplate.FieldTypes.CoBorrowerEmployerInformation, boolean));
        this.buildFieldList(this.gridViewPropertyAddr, duplicationDefaultTemplate.GetFields(LoanDuplicationDefaultTemplate.FieldTypes.PropertyAddress, boolean));
        this.buildFieldList(this.gridViewBorPresentAddr, duplicationDefaultTemplate.GetFields(LoanDuplicationDefaultTemplate.FieldTypes.BorrowerPresentAddress, boolean));
        this.buildFieldList(this.gridViewCoBorPresentAddr, duplicationDefaultTemplate.GetFields(LoanDuplicationDefaultTemplate.FieldTypes.CoBorrowerPresentAddress, boolean));
        this.cboProperty.SelectedIndex = this.cboBorPresentAddr.SelectedIndex = this.cboCoBorPresentAddr.SelectedIndex = 0;
        if (this.currentTemplate == null)
          return;
        this.txtName.Text = this.currentTemplate.TemplateName;
        this.txtDescription.Text = this.currentTemplate.Description;
        this.chkBorInfo.Checked = this.currentTemplate.GetField("BorrowerInformation") == "1";
        this.chkBorEmpInfo.Checked = this.currentTemplate.GetField("BorrowerEmployerInformation") == "1";
        this.chkBorAddr.Checked = this.currentTemplate.GetField("BorrowerPresentAddress") == "1";
        this.chkCobInfo.Checked = this.currentTemplate.GetField("Co-BorrowerInformation") == "1";
        this.chkCobEmpInfo.Checked = this.currentTemplate.GetField("Co-BorrowerEmployerInformation") == "1";
        this.chkCobAddr.Checked = this.currentTemplate.GetField("Co-BorrowerPresentAddress") == "1";
        this.chkProperty.Checked = this.currentTemplate.GetField("Property") == "1";
        this.cboBorPresentAddr.SelectedItem = (object) this.currentTemplate.GetField("BorrowerAddressTo");
        this.cboCoBorPresentAddr.SelectedItem = (object) this.currentTemplate.GetField("CoBorrowerAddressTo");
        this.cboProperty.SelectedItem = (object) this.currentTemplate.GetField("PropertyAddressTo");
        this.AddAdditionalFields(this.currentTemplate.GetAdditionalFields());
      }
    }

    private void buildFieldList(GridView view, List<string[]> fields)
    {
      view.Items.Clear();
      view.BeginUpdate();
      for (int index = 0; index < fields.Count; ++index)
        view.Items.Add(new GVItem(fields[index][1])
        {
          SubItems = {
            (object) fields[index][0]
          }
        });
      view.EndUpdate();
    }

    private void gridViewAdditional_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnDelete.Enabled = this.gridViewAdditional.SelectedItems.Count > 0;
    }

    private void btnNew_Click(object sender, EventArgs e)
    {
      if (this.AdditionalFieldButtonClicked == null)
        return;
      this.AdditionalFieldButtonClicked((object) this, e);
    }

    public void AddAdditionalFields(string[] fields)
    {
      if (fields == null)
        return;
      this.gridViewAdditional.BeginUpdate();
      for (int index = 0; index < fields.Length; ++index)
        this.gridViewAdditional.Items.Add(new GVItem(EncompassFields.GetDescription(fields[index]))
        {
          SubItems = {
            (object) fields[index]
          }
        });
      this.gridViewAdditional.EndUpdate();
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to delete " + (this.gridViewAdditional.SelectedItems.Count > 1 ? "these fields" : "the field") + "?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      int index = this.gridViewAdditional.SelectedItems[0].Index;
      for (int nItemIndex = this.gridViewAdditional.Items.Count - 1; nItemIndex >= 0; --nItemIndex)
      {
        if (this.gridViewAdditional.Items[nItemIndex].Selected)
          this.gridViewAdditional.Items.RemoveAt(nItemIndex);
      }
      if (this.gridViewAdditional.Items.Count == 0 || index >= this.gridViewAdditional.Items.Count)
        return;
      this.gridViewAdditional.Items[index].Selected = true;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (this.txtName.Text.Trim() == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must enter a name for this template.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtName.Focus();
      }
      else if (!this.chkBorInfo.Checked && !this.chkCobAddr.Checked && !this.chkBorEmpInfo.Checked && !this.chkCobEmpInfo.Checked && !this.chkProperty.Checked && !this.chkBorAddr.Checked && !this.chkCobAddr.Checked && this.gridViewAdditional.Items.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must select a field group or an additional field to duplicate a loan.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.chkBorInfo.Focus();
      }
      else if (this.chkBorAddr.Checked && this.cboBorPresentAddr.SelectedItem == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must select a field group to copy borrower address.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.chkBorAddr.Focus();
      }
      else if (this.chkCobAddr.Checked && this.cboCoBorPresentAddr.SelectedItem == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must select a field group to copy co-borrower address.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.chkCobAddr.Focus();
      }
      else if (this.chkProperty.Checked && this.cboProperty.SelectedItem == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must select a field group to copy property address.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.chkCobAddr.Focus();
      }
      else
      {
        if (this.currentTemplate != null)
          this.currentTemplate.SetAdditionalFields(new string[0]);
        else
          this.currentTemplate = new LoanDuplicationTemplate();
        this.currentTemplate.Description = this.txtDescription.Text.Trim();
        this.currentTemplate.SetCurrentField("BorrowerInformation", this.chkBorInfo.Checked ? "1" : "");
        this.currentTemplate.SetCurrentField("BorrowerEmployerInformation", this.chkBorEmpInfo.Checked ? "1" : "");
        this.currentTemplate.SetCurrentField("BorrowerPresentAddress", this.chkBorAddr.Checked ? "1" : "");
        this.currentTemplate.SetCurrentField("Co-BorrowerInformation", this.chkCobInfo.Checked ? "1" : "");
        this.currentTemplate.SetCurrentField("Co-BorrowerEmployerInformation", this.chkCobEmpInfo.Checked ? "1" : "");
        this.currentTemplate.SetCurrentField("Co-BorrowerPresentAddress", this.chkCobAddr.Checked ? "1" : "");
        this.currentTemplate.SetCurrentField("Property", this.chkProperty.Checked ? "1" : "");
        this.currentTemplate.SetCurrentField("BorrowerAddressTo", this.cboBorPresentAddr.SelectedItem.ToString());
        this.currentTemplate.SetCurrentField("CoBorrowerAddressTo", this.cboCoBorPresentAddr.SelectedItem.ToString());
        this.currentTemplate.SetCurrentField("PropertyAddressTo", this.cboProperty.SelectedItem.ToString());
        if (this.gridViewAdditional.Items.Count > 0)
        {
          List<string> stringList = new List<string>();
          for (int nItemIndex = 0; nItemIndex < this.gridViewAdditional.Items.Count; ++nItemIndex)
            stringList.Add(this.gridViewAdditional.Items[nItemIndex].SubItems[1].Text);
          this.currentTemplate.SetAdditionalFields(stringList.ToArray());
        }
        this.DialogResult = DialogResult.OK;
      }
    }

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, "Setup\\Loan Duplication");
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
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      GVColumn gvColumn8 = new GVColumn();
      GVColumn gvColumn9 = new GVColumn();
      GVColumn gvColumn10 = new GVColumn();
      GVColumn gvColumn11 = new GVColumn();
      GVColumn gvColumn12 = new GVColumn();
      GVColumn gvColumn13 = new GVColumn();
      GVColumn gvColumn14 = new GVColumn();
      GVColumn gvColumn15 = new GVColumn();
      GVColumn gvColumn16 = new GVColumn();
      this.btnSave = new Button();
      this.btnCancel = new Button();
      this.label1 = new Label();
      this.txtName = new TextBox();
      this.txtDescription = new TextBox();
      this.label2 = new Label();
      this.groupContainer8 = new GroupContainer();
      this.btnNew = new StandardIconButton();
      this.btnDelete = new StandardIconButton();
      this.label6 = new Label();
      this.gridViewAdditional = new GridView();
      this.groupContainer7 = new GroupContainer();
      this.gridViewCoBorPresentAddr = new GridView();
      this.cboCoBorPresentAddr = new ComboBox();
      this.label5 = new Label();
      this.chkCobAddr = new CheckBox();
      this.groupContainer6 = new GroupContainer();
      this.gridViewBorPresentAddr = new GridView();
      this.cboBorPresentAddr = new ComboBox();
      this.label4 = new Label();
      this.chkBorAddr = new CheckBox();
      this.groupContainer5 = new GroupContainer();
      this.gridViewPropertyAddr = new GridView();
      this.cboProperty = new ComboBox();
      this.label3 = new Label();
      this.chkProperty = new CheckBox();
      this.groupContainer4 = new GroupContainer();
      this.gridViewCobEmpInfo = new GridView();
      this.chkCobEmpInfo = new CheckBox();
      this.groupContainer3 = new GroupContainer();
      this.gridViewBorEmpInfo = new GridView();
      this.chkBorEmpInfo = new CheckBox();
      this.groupContainer2 = new GroupContainer();
      this.gridViewCobInfo = new GridView();
      this.chkCobInfo = new CheckBox();
      this.groupContainer1 = new GroupContainer();
      this.chkBorInfo = new CheckBox();
      this.gridViewBorInfo = new GridView();
      this.groupContainer8.SuspendLayout();
      ((ISupportInitialize) this.btnNew).BeginInit();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      this.groupContainer7.SuspendLayout();
      this.groupContainer6.SuspendLayout();
      this.groupContainer5.SuspendLayout();
      this.groupContainer4.SuspendLayout();
      this.groupContainer3.SuspendLayout();
      this.groupContainer2.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSave.Location = new Point(836, 624);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 23);
      this.btnSave.TabIndex = 13;
      this.btnSave.Text = "&Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(917, 624);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 14;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(21, 18);
      this.label1.Name = "label1";
      this.label1.Size = new Size(82, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Template Name";
      this.txtName.Location = new Point(109, 15);
      this.txtName.Name = "txtName";
      this.txtName.ReadOnly = true;
      this.txtName.Size = new Size(177, 20);
      this.txtName.TabIndex = 0;
      this.txtName.TabStop = false;
      this.txtDescription.Location = new Point(380, 15);
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.Size = new Size(565, 20);
      this.txtDescription.TabIndex = 0;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(314, 18);
      this.label2.Name = "label2";
      this.label2.Size = new Size(60, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "Description";
      this.groupContainer8.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      this.groupContainer8.Controls.Add((Control) this.btnNew);
      this.groupContainer8.Controls.Add((Control) this.btnDelete);
      this.groupContainer8.Controls.Add((Control) this.label6);
      this.groupContainer8.Controls.Add((Control) this.gridViewAdditional);
      this.groupContainer8.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer8.Location = new Point(12, 473);
      this.groupContainer8.Name = "groupContainer8";
      this.groupContainer8.Size = new Size(553, 174);
      this.groupContainer8.TabIndex = 12;
      this.btnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnNew.BackColor = Color.Transparent;
      this.btnNew.Location = new Point(511, 5);
      this.btnNew.MouseDownImage = (Image) null;
      this.btnNew.Name = "btnNew";
      this.btnNew.Size = new Size(16, 17);
      this.btnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnNew.TabIndex = 29;
      this.btnNew.TabStop = false;
      this.btnNew.Click += new EventHandler(this.btnNew_Click);
      this.btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Location = new Point(533, 5);
      this.btnDelete.MouseDownImage = (Image) null;
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(16, 17);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabIndex = 28;
      this.btnDelete.TabStop = false;
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.label6.AutoSize = true;
      this.label6.BackColor = Color.Transparent;
      this.label6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label6.Location = new Point(7, 7);
      this.label6.Name = "label6";
      this.label6.Size = new Size(100, 13);
      this.label6.TabIndex = 10;
      this.label6.Text = "Additional Fields";
      this.gridViewAdditional.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Field Name";
      gvColumn1.Width = 350;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "Field ID";
      gvColumn2.Width = 201;
      this.gridViewAdditional.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gridViewAdditional.Dock = DockStyle.Fill;
      this.gridViewAdditional.Location = new Point(1, 26);
      this.gridViewAdditional.Name = "gridViewAdditional";
      this.gridViewAdditional.Size = new Size(551, 147);
      this.gridViewAdditional.TabIndex = 9;
      this.gridViewAdditional.SelectedIndexChanged += new EventHandler(this.gridViewAdditional_SelectedIndexChanged);
      this.groupContainer7.Controls.Add((Control) this.gridViewCoBorPresentAddr);
      this.groupContainer7.Controls.Add((Control) this.cboCoBorPresentAddr);
      this.groupContainer7.Controls.Add((Control) this.label5);
      this.groupContainer7.Controls.Add((Control) this.chkCobAddr);
      this.groupContainer7.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer7.Location = new Point(571, 366);
      this.groupContainer7.Name = "groupContainer7";
      this.groupContainer7.Size = new Size(421, 138);
      this.groupContainer7.TabIndex = 13;
      this.gridViewCoBorPresentAddr.BorderStyle = BorderStyle.None;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column1";
      gvColumn3.Text = "Field Name";
      gvColumn3.Width = 250;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column2";
      gvColumn4.SpringToFit = true;
      gvColumn4.Text = "Field ID";
      gvColumn4.Width = 169;
      this.gridViewCoBorPresentAddr.Columns.AddRange(new GVColumn[2]
      {
        gvColumn3,
        gvColumn4
      });
      this.gridViewCoBorPresentAddr.Dock = DockStyle.Fill;
      this.gridViewCoBorPresentAddr.Location = new Point(1, 26);
      this.gridViewCoBorPresentAddr.Name = "gridViewCoBorPresentAddr";
      this.gridViewCoBorPresentAddr.Size = new Size(419, 111);
      this.gridViewCoBorPresentAddr.TabIndex = 12;
      this.cboCoBorPresentAddr.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboCoBorPresentAddr.FormattingEnabled = true;
      this.cboCoBorPresentAddr.Items.AddRange(new object[2]
      {
        (object) "Co-Borrower's Present Address",
        (object) "Co-Borrower's Prior Residence"
      });
      this.cboCoBorPresentAddr.Location = new Point(220, 2);
      this.cboCoBorPresentAddr.Name = "cboCoBorPresentAddr";
      this.cboCoBorPresentAddr.Size = new Size(197, 21);
      this.cboCoBorPresentAddr.TabIndex = 11;
      this.label5.AutoSize = true;
      this.label5.BackColor = Color.Transparent;
      this.label5.Location = new Point(198, 6);
      this.label5.Name = "label5";
      this.label5.Size = new Size(16, 13);
      this.label5.TabIndex = 9;
      this.label5.Text = "to";
      this.chkCobAddr.AutoSize = true;
      this.chkCobAddr.BackColor = Color.Transparent;
      this.chkCobAddr.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkCobAddr.Location = new Point(8, 5);
      this.chkCobAddr.Name = "chkCobAddr";
      this.chkCobAddr.Size = new Size(191, 17);
      this.chkCobAddr.TabIndex = 10;
      this.chkCobAddr.Text = "Co-Borrower Present Address";
      this.chkCobAddr.UseVisualStyleBackColor = false;
      this.groupContainer6.Controls.Add((Control) this.gridViewBorPresentAddr);
      this.groupContainer6.Controls.Add((Control) this.cboBorPresentAddr);
      this.groupContainer6.Controls.Add((Control) this.label4);
      this.groupContainer6.Controls.Add((Control) this.chkBorAddr);
      this.groupContainer6.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer6.Location = new Point(572, 221);
      this.groupContainer6.Name = "groupContainer6";
      this.groupContainer6.Size = new Size(420, 139);
      this.groupContainer6.TabIndex = 12;
      this.gridViewBorPresentAddr.BorderStyle = BorderStyle.None;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column1";
      gvColumn5.Text = "Field Name";
      gvColumn5.Width = 250;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column2";
      gvColumn6.SpringToFit = true;
      gvColumn6.Text = "Field ID";
      gvColumn6.Width = 168;
      this.gridViewBorPresentAddr.Columns.AddRange(new GVColumn[2]
      {
        gvColumn5,
        gvColumn6
      });
      this.gridViewBorPresentAddr.Dock = DockStyle.Fill;
      this.gridViewBorPresentAddr.Location = new Point(1, 26);
      this.gridViewBorPresentAddr.Name = "gridViewBorPresentAddr";
      this.gridViewBorPresentAddr.Size = new Size(418, 112);
      this.gridViewBorPresentAddr.TabIndex = 11;
      this.cboBorPresentAddr.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboBorPresentAddr.FormattingEnabled = true;
      this.cboBorPresentAddr.Items.AddRange(new object[2]
      {
        (object) "Borrower's Present Address",
        (object) "Borrower's Prior Residence"
      });
      this.cboBorPresentAddr.Location = new Point(200, 2);
      this.cboBorPresentAddr.Name = "cboBorPresentAddr";
      this.cboBorPresentAddr.Size = new Size(216, 21);
      this.cboBorPresentAddr.TabIndex = 9;
      this.label4.AutoSize = true;
      this.label4.BackColor = Color.Transparent;
      this.label4.Location = new Point(178, 6);
      this.label4.Name = "label4";
      this.label4.Size = new Size(16, 13);
      this.label4.TabIndex = 9;
      this.label4.Text = "to";
      this.chkBorAddr.AutoSize = true;
      this.chkBorAddr.BackColor = Color.Transparent;
      this.chkBorAddr.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkBorAddr.Location = new Point(8, 5);
      this.chkBorAddr.Name = "chkBorAddr";
      this.chkBorAddr.Size = new Size(176, 17);
      this.chkBorAddr.TabIndex = 8;
      this.chkBorAddr.Text = "Borrower Present Address ";
      this.chkBorAddr.UseVisualStyleBackColor = false;
      this.groupContainer5.Controls.Add((Control) this.gridViewPropertyAddr);
      this.groupContainer5.Controls.Add((Control) this.cboProperty);
      this.groupContainer5.Controls.Add((Control) this.label3);
      this.groupContainer5.Controls.Add((Control) this.chkProperty);
      this.groupContainer5.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer5.Location = new Point(572, 41);
      this.groupContainer5.Name = "groupContainer5";
      this.groupContainer5.Size = new Size(420, 174);
      this.groupContainer5.TabIndex = 11;
      this.gridViewPropertyAddr.BorderStyle = BorderStyle.None;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column1";
      gvColumn7.Text = "Field Name";
      gvColumn7.Width = 250;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Column2";
      gvColumn8.SpringToFit = true;
      gvColumn8.Text = "Field ID";
      gvColumn8.Width = 168;
      this.gridViewPropertyAddr.Columns.AddRange(new GVColumn[2]
      {
        gvColumn7,
        gvColumn8
      });
      this.gridViewPropertyAddr.Dock = DockStyle.Fill;
      this.gridViewPropertyAddr.Location = new Point(1, 26);
      this.gridViewPropertyAddr.Name = "gridViewPropertyAddr";
      this.gridViewPropertyAddr.Size = new Size(418, 147);
      this.gridViewPropertyAddr.TabIndex = 10;
      this.cboProperty.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboProperty.FormattingEnabled = true;
      this.cboProperty.Items.AddRange(new object[4]
      {
        (object) "Property Address",
        (object) "Borrower's and Co-Borrower's Present Address",
        (object) "Borrower's Present Address",
        (object) "Co-Borrower's Present Address"
      });
      this.cboProperty.Location = new Point(153, 2);
      this.cboProperty.Name = "cboProperty";
      this.cboProperty.Size = new Size(263, 21);
      this.cboProperty.TabIndex = 7;
      this.label3.AutoSize = true;
      this.label3.BackColor = Color.Transparent;
      this.label3.Location = new Point(130, 6);
      this.label3.Name = "label3";
      this.label3.Size = new Size(16, 13);
      this.label3.TabIndex = 8;
      this.label3.Text = "to";
      this.chkProperty.AutoSize = true;
      this.chkProperty.BackColor = Color.Transparent;
      this.chkProperty.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkProperty.Location = new Point(8, 5);
      this.chkProperty.Name = "chkProperty";
      this.chkProperty.Size = new Size(122, 17);
      this.chkProperty.TabIndex = 6;
      this.chkProperty.Text = "Property Address";
      this.chkProperty.UseVisualStyleBackColor = false;
      this.groupContainer4.Controls.Add((Control) this.gridViewCobEmpInfo);
      this.groupContainer4.Controls.Add((Control) this.chkCobEmpInfo);
      this.groupContainer4.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer4.Location = new Point(292, 274);
      this.groupContainer4.Name = "groupContainer4";
      this.groupContainer4.Size = new Size(274, 193);
      this.groupContainer4.TabIndex = 10;
      this.gridViewCobEmpInfo.BorderStyle = BorderStyle.None;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "Column1";
      gvColumn9.Text = "Field Name";
      gvColumn9.Width = 150;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "Column2";
      gvColumn10.SpringToFit = true;
      gvColumn10.Text = "Field ID";
      gvColumn10.Width = 122;
      this.gridViewCobEmpInfo.Columns.AddRange(new GVColumn[2]
      {
        gvColumn9,
        gvColumn10
      });
      this.gridViewCobEmpInfo.Dock = DockStyle.Fill;
      this.gridViewCobEmpInfo.Location = new Point(1, 26);
      this.gridViewCobEmpInfo.Name = "gridViewCobEmpInfo";
      this.gridViewCobEmpInfo.Size = new Size(272, 166);
      this.gridViewCobEmpInfo.TabIndex = 9;
      this.chkCobEmpInfo.AutoSize = true;
      this.chkCobEmpInfo.BackColor = Color.Transparent;
      this.chkCobEmpInfo.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkCobEmpInfo.Location = new Point(8, 5);
      this.chkCobEmpInfo.Name = "chkCobEmpInfo";
      this.chkCobEmpInfo.Size = new Size(217, 17);
      this.chkCobEmpInfo.TabIndex = 5;
      this.chkCobEmpInfo.Text = "Co-Borrower Employer Information";
      this.chkCobEmpInfo.UseVisualStyleBackColor = false;
      this.groupContainer3.Controls.Add((Control) this.gridViewBorEmpInfo);
      this.groupContainer3.Controls.Add((Control) this.chkBorEmpInfo);
      this.groupContainer3.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer3.Location = new Point(12, 274);
      this.groupContainer3.Name = "groupContainer3";
      this.groupContainer3.Size = new Size(273, 193);
      this.groupContainer3.TabIndex = 9;
      this.gridViewBorEmpInfo.BorderStyle = BorderStyle.None;
      gvColumn11.ImageIndex = -1;
      gvColumn11.Name = "Column1";
      gvColumn11.Text = "Field Name";
      gvColumn11.Width = 150;
      gvColumn12.ImageIndex = -1;
      gvColumn12.Name = "Column2";
      gvColumn12.SpringToFit = true;
      gvColumn12.Text = "Field ID";
      gvColumn12.Width = 121;
      this.gridViewBorEmpInfo.Columns.AddRange(new GVColumn[2]
      {
        gvColumn11,
        gvColumn12
      });
      this.gridViewBorEmpInfo.Dock = DockStyle.Fill;
      this.gridViewBorEmpInfo.Location = new Point(1, 26);
      this.gridViewBorEmpInfo.Name = "gridViewBorEmpInfo";
      this.gridViewBorEmpInfo.Size = new Size(271, 166);
      this.gridViewBorEmpInfo.TabIndex = 8;
      this.chkBorEmpInfo.AutoSize = true;
      this.chkBorEmpInfo.BackColor = Color.Transparent;
      this.chkBorEmpInfo.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkBorEmpInfo.Location = new Point(8, 5);
      this.chkBorEmpInfo.Name = "chkBorEmpInfo";
      this.chkBorEmpInfo.Size = new Size(198, 17);
      this.chkBorEmpInfo.TabIndex = 3;
      this.chkBorEmpInfo.Text = "Borrower Employer Information";
      this.chkBorEmpInfo.UseVisualStyleBackColor = false;
      this.groupContainer2.Controls.Add((Control) this.gridViewCobInfo);
      this.groupContainer2.Controls.Add((Control) this.chkCobInfo);
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(292, 41);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(274, 228);
      this.groupContainer2.TabIndex = 8;
      this.gridViewCobInfo.BorderStyle = BorderStyle.None;
      gvColumn13.ImageIndex = -1;
      gvColumn13.Name = "Column1";
      gvColumn13.Text = "Field Name";
      gvColumn13.Width = 150;
      gvColumn14.ImageIndex = -1;
      gvColumn14.Name = "Column2";
      gvColumn14.SpringToFit = true;
      gvColumn14.Text = "Field ID";
      gvColumn14.Width = 122;
      this.gridViewCobInfo.Columns.AddRange(new GVColumn[2]
      {
        gvColumn13,
        gvColumn14
      });
      this.gridViewCobInfo.Dock = DockStyle.Fill;
      this.gridViewCobInfo.Location = new Point(1, 26);
      this.gridViewCobInfo.Name = "gridViewCobInfo";
      this.gridViewCobInfo.Size = new Size(272, 201);
      this.gridViewCobInfo.TabIndex = 8;
      this.chkCobInfo.AutoSize = true;
      this.chkCobInfo.BackColor = Color.Transparent;
      this.chkCobInfo.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkCobInfo.Location = new Point(8, 5);
      this.chkCobInfo.Name = "chkCobInfo";
      this.chkCobInfo.Size = new Size(162, 17);
      this.chkCobInfo.TabIndex = 4;
      this.chkCobInfo.Text = "Co-Borrower Information";
      this.chkCobInfo.UseVisualStyleBackColor = false;
      this.groupContainer1.Controls.Add((Control) this.chkBorInfo);
      this.groupContainer1.Controls.Add((Control) this.gridViewBorInfo);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(12, 41);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(274, 228);
      this.groupContainer1.TabIndex = 7;
      this.chkBorInfo.AutoSize = true;
      this.chkBorInfo.BackColor = Color.Transparent;
      this.chkBorInfo.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkBorInfo.Location = new Point(8, 5);
      this.chkBorInfo.Name = "chkBorInfo";
      this.chkBorInfo.Size = new Size(143, 17);
      this.chkBorInfo.TabIndex = 2;
      this.chkBorInfo.Text = "Borrower Information";
      this.chkBorInfo.UseVisualStyleBackColor = false;
      this.gridViewBorInfo.BorderStyle = BorderStyle.None;
      gvColumn15.ImageIndex = -1;
      gvColumn15.Name = "Column1";
      gvColumn15.Text = "Field Name";
      gvColumn15.Width = 150;
      gvColumn16.ImageIndex = -1;
      gvColumn16.Name = "Column2";
      gvColumn16.SpringToFit = true;
      gvColumn16.Text = "Field ID";
      gvColumn16.Width = 122;
      this.gridViewBorInfo.Columns.AddRange(new GVColumn[2]
      {
        gvColumn15,
        gvColumn16
      });
      this.gridViewBorInfo.Dock = DockStyle.Fill;
      this.gridViewBorInfo.Location = new Point(1, 26);
      this.gridViewBorInfo.Name = "gridViewBorInfo";
      this.gridViewBorInfo.Size = new Size(272, 201);
      this.gridViewBorInfo.TabIndex = 6;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1004, 659);
      this.Controls.Add((Control) this.groupContainer8);
      this.Controls.Add((Control) this.groupContainer7);
      this.Controls.Add((Control) this.groupContainer6);
      this.Controls.Add((Control) this.groupContainer5);
      this.Controls.Add((Control) this.groupContainer4);
      this.Controls.Add((Control) this.groupContainer3);
      this.Controls.Add((Control) this.groupContainer2);
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.txtDescription);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.txtName);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSave);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MinimizeBox = false;
      this.Name = nameof (LoanDuplicationTemplateForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Loan Duplication Template";
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.groupContainer8.ResumeLayout(false);
      this.groupContainer8.PerformLayout();
      ((ISupportInitialize) this.btnNew).EndInit();
      ((ISupportInitialize) this.btnDelete).EndInit();
      this.groupContainer7.ResumeLayout(false);
      this.groupContainer7.PerformLayout();
      this.groupContainer6.ResumeLayout(false);
      this.groupContainer6.PerformLayout();
      this.groupContainer5.ResumeLayout(false);
      this.groupContainer5.PerformLayout();
      this.groupContainer4.ResumeLayout(false);
      this.groupContainer4.PerformLayout();
      this.groupContainer3.ResumeLayout(false);
      this.groupContainer3.PerformLayout();
      this.groupContainer2.ResumeLayout(false);
      this.groupContainer2.PerformLayout();
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
