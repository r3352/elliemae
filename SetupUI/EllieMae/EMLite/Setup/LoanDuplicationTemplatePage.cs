// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LoanDuplicationTemplatePage
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.RemotingServices.Acl;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class LoanDuplicationTemplatePage : Form
  {
    private Sessions.Session session;
    private bool dirty;
    private bool readOnly;
    private int selectOption = 2;
    private int personaID = -1;
    private Persona[] personaList;
    private LoanDuplicationAclManager aclMgr;
    private FeaturesAclManager featMgr;
    private List<string> templateWithPermission;
    private string userID;
    private bool showLinkIcons;
    private bool IncludeBlankLoanChanged;
    private bool blnIncludeBlankLoan;
    private bool multiplePersonas;
    private ArrayList previousView;
    private IContainer components;
    private GroupContainer groupContainerTemplate;
    private GridView gridViewTemplates;
    private Button btnCancel;
    private Button btnOK;
    private CheckBox chkAll;
    private ImageList imgListTv;
    private Label lblDisconnected;
    private Label lblLinked;
    private CheckBox chkIncludeBlankLoan;

    public LoanDuplicationTemplatePage(
      Sessions.Session session,
      int personaID,
      bool readOnly,
      int option)
    {
      this.session = session;
      this.InitializeComponent();
      this.userID = (string) null;
      this.showLinkIcons = this.multiplePersonas = this.lblDisconnected.Visible = this.lblLinked.Visible = false;
      this.personaID = personaID;
      this.chkIncludeBlankLoan.Visible = true;
      this.chkIncludeBlankLoan.Location = this.lblDisconnected.Location;
      this.selectOption = option;
      this.readOnly = readOnly;
      this.MakeReadOnly(this.readOnly);
      this.aclMgr = (LoanDuplicationAclManager) this.session.ACL.GetAclManager(AclCategory.LoanDuplicationTemplates);
      this.featMgr = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
      this.readLastSettingFromSQL();
      this.InitForm();
    }

    public LoanDuplicationTemplatePage(
      Sessions.Session session,
      string userID,
      Persona[] personaList,
      bool readOnly,
      int option)
    {
      this.session = session;
      this.personaList = personaList;
      this.InitializeComponent();
      this.userID = userID;
      this.selectOption = option;
      this.readOnly = readOnly && this.userID == null;
      this.MakeReadOnly(this.readOnly);
      this.aclMgr = (LoanDuplicationAclManager) this.session.ACL.GetAclManager(AclCategory.LoanDuplicationTemplates);
      this.featMgr = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
      if (this.personaList.Length == 1)
      {
        this.personaID = this.personaList[0].ID;
      }
      else
      {
        this.multiplePersonas = true;
        this.chkIncludeBlankLoan.Enabled = false;
      }
      this.showLinkIcons = this.lblDisconnected.Visible = this.lblLinked.Visible = this.chkIncludeBlankLoan.Visible = true;
      this.chkIncludeBlankLoan.Checked = this.featMgr.GetPermission(AclFeature.LoanMgmt_Duplicate_Blank, this.personaList[0].ID);
      this.gridViewTemplates.Columns[0].Width = !this.showLinkIcons ? 30 : 70;
      this.readLastSettingFromSQL();
      this.InitForm();
      this.chkAll.Visible = !this.readOnly;
    }

    private void readLastSettingFromSQL()
    {
      this.templateWithPermission = new List<string>();
      LoanDuplicationTemplateAclInfo[] duplicationTemplateAclInfoArray = (LoanDuplicationTemplateAclInfo[]) null;
      if (this.personaID != -1)
      {
        duplicationTemplateAclInfoArray = this.aclMgr.GetAccessibleLoanDuplicationTemplates(this.personaID);
        this.chkIncludeBlankLoan.Checked = this.featMgr.GetPermission(AclFeature.LoanMgmt_Duplicate_Blank, this.personaID);
        this.blnIncludeBlankLoan = this.chkIncludeBlankLoan.Checked;
      }
      else if (this.personaList != null && this.personaList.Length != 0)
        duplicationTemplateAclInfoArray = this.aclMgr.GetAccessibleLoanDuplicationTemplates(this.personaList);
      if (duplicationTemplateAclInfoArray == null || duplicationTemplateAclInfoArray.Length == 0)
        return;
      foreach (LoanDuplicationTemplateAclInfo duplicationTemplateAclInfo in duplicationTemplateAclInfoArray)
        this.templateWithPermission.Add(duplicationTemplateAclInfo.TemplateName);
    }

    public void InitForm()
    {
      bool flag = true;
      this.gridViewTemplates.Items.Clear();
      this.gridViewTemplates.BeginUpdate();
      FileSystemEntry[] templateDirEntries = this.session.ConfigurationManager.GetTemplateDirEntries(TemplateSettingsType.LoanDuplicationTemplate, FileSystemEntry.PublicRoot);
      if (templateDirEntries != null && templateDirEntries.Length != 0)
      {
        foreach (FileSystemEntry fileSystemEntry in templateDirEntries)
        {
          string str = string.Concat(fileSystemEntry.Properties[(object) "Description"]);
          GVItem gvItem = new GVItem("");
          gvItem.SubItems.Add((object) fileSystemEntry.Name);
          gvItem.SubItems.Add((object) str);
          gvItem.Tag = (object) fileSystemEntry;
          if (this.templateWithPermission != null && this.templateWithPermission.Contains(fileSystemEntry.Name))
            gvItem.Checked = true;
          else
            flag = false;
          if (this.readOnly || this.multiplePersonas)
            gvItem.SubItems[0].CheckBoxEnabled = false;
          if (this.showLinkIcons)
            gvItem.ImageIndex = 1;
          this.gridViewTemplates.Items.Add(gvItem);
        }
      }
      this.gridViewTemplates.EndUpdate();
      this.groupContainerTemplate.Text = "Loan Duplication Templates (" + this.gridViewTemplates.Items.Count.ToString() + ")";
      this.chkAll.CheckedChanged -= new EventHandler(this.chkAll_CheckedChanged);
      this.chkAll.Checked = flag;
      this.chkAll.Enabled = false;
      this.chkAll.CheckedChanged += new EventHandler(this.chkAll_CheckedChanged);
      this.ToggleIncudeBlankLoan(true);
      this.dirty = false;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      this.templateWithPermission = new List<string>();
      for (int nItemIndex = 0; nItemIndex < this.gridViewTemplates.Items.Count; ++nItemIndex)
      {
        if (this.gridViewTemplates.Items[nItemIndex].Checked)
          this.templateWithPermission.Add(((FileSystemEntry) this.gridViewTemplates.Items[nItemIndex].Tag).Name);
      }
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void chkAll_CheckedChanged(object sender, EventArgs e)
    {
      for (int nItemIndex = 0; nItemIndex < this.gridViewTemplates.Items.Count; ++nItemIndex)
        this.gridViewTemplates.Items[nItemIndex].Checked = this.chkAll.Checked;
      this.ToggleIncudeBlankLoan(false);
      this.dirty = true;
    }

    private void gridViewTemplates_Click(object sender, EventArgs e)
    {
      if (this.readOnly)
        return;
      this.chkAll.Enabled = !this.multiplePersonas && this.gridViewTemplates.Items.Count > 0;
      if (this.gridViewTemplates.Items.Count == 0)
      {
        this.chkAll.Checked = false;
      }
      else
      {
        bool flag = true;
        for (int nItemIndex = 0; nItemIndex < this.gridViewTemplates.Items.Count; ++nItemIndex)
        {
          if (!this.gridViewTemplates.Items[nItemIndex].Checked)
          {
            flag = false;
            break;
          }
        }
        this.chkAll.CheckedChanged -= new EventHandler(this.chkAll_CheckedChanged);
        this.chkAll.Checked = flag;
        this.chkAll.CheckedChanged += new EventHandler(this.chkAll_CheckedChanged);
      }
      this.ToggleIncudeBlankLoan(false);
      this.dirty = true;
    }

    public bool isReadOnly
    {
      set => this.MakeReadOnly(value);
    }

    public void SaveData()
    {
      if (this.personaID < 0)
        return;
      List<LoanDuplicationTemplateAclInfo> duplicationTemplateAclInfoList = new List<LoanDuplicationTemplateAclInfo>();
      for (int nItemIndex = 0; nItemIndex < this.gridViewTemplates.Items.Count; ++nItemIndex)
      {
        if (this.gridViewTemplates.Items[nItemIndex].Checked)
          duplicationTemplateAclInfoList.Add(new LoanDuplicationTemplateAclInfo(this.personaID, this.gridViewTemplates.Items[nItemIndex].SubItems[1].Text, true));
      }
      this.aclMgr.SetPermissions(duplicationTemplateAclInfoList.ToArray(), this.personaID);
      if (this.IncludeBlankLoanChanged)
      {
        this.featMgr.SetPermission(AclFeature.LoanMgmt_Duplicate_Blank, this.personaID, this.chkIncludeBlankLoan.Checked ? AclTriState.True : AclTriState.False);
        this.IncludeBlankLoanChanged = false;
      }
      this.dirty = false;
    }

    private void MakeReadOnly(bool makeReadOnly)
    {
      if (makeReadOnly || this.multiplePersonas)
      {
        this.btnOK.Enabled = false;
        this.chkAll.Enabled = false;
        if (this.gridViewTemplates != null && this.gridViewTemplates.Items.Count > 0)
        {
          for (int nItemIndex = 0; nItemIndex < this.gridViewTemplates.Items.Count; ++nItemIndex)
            this.gridViewTemplates.Items[nItemIndex].SubItems[0].CheckBoxEnabled = false;
        }
        else
        {
          this.chkIncludeBlankLoan.Checked = false;
          this.chkAll.Checked = false;
        }
        this.chkIncludeBlankLoan.Enabled = false;
      }
      else if (this.gridViewTemplates != null && this.gridViewTemplates.Items.Count > 0)
      {
        for (int nItemIndex = 0; nItemIndex < this.gridViewTemplates.Items.Count; ++nItemIndex)
          this.gridViewTemplates.Items[nItemIndex].SubItems[0].CheckBoxEnabled = true;
        this.chkIncludeBlankLoan.Enabled = true;
        this.chkAll.Enabled = true;
        this.btnOK.Enabled = true;
      }
      else
      {
        this.chkIncludeBlankLoan.Checked = false;
        this.chkAll.Checked = false;
        this.btnOK.Enabled = false;
      }
      this.readOnly = makeReadOnly;
    }

    public bool HasBeenModified() => this.dirty;

    public bool HasSomethingChecked()
    {
      bool flag = false;
      for (int nItemIndex = 0; nItemIndex < this.gridViewTemplates.Items.Count; ++nItemIndex)
      {
        if (this.gridViewTemplates.Items[nItemIndex].Checked)
          return true;
      }
      return flag;
    }

    public int GetImageIndex() => 0;

    public ArrayList DataView
    {
      get => this.previousView;
      set => this.previousView = value;
    }

    private void chkIncludeBlankLoan_CheckedChanged(object sender, EventArgs e)
    {
      this.IncludeBlankLoanChanged = true;
      this.dirty = true;
    }

    private void ToggleIncudeBlankLoan(bool blnFormInit)
    {
      if (this.multiplePersonas)
        return;
      if (blnFormInit)
      {
        if (!this.HasSomethingChecked() && !this.blnIncludeBlankLoan)
        {
          this.chkIncludeBlankLoan.Enabled = false;
          this.chkIncludeBlankLoan.Checked = this.blnIncludeBlankLoan;
        }
        else
        {
          if (!this.HasSomethingChecked())
            return;
          this.chkIncludeBlankLoan.Enabled = true;
          this.chkIncludeBlankLoan.Checked = this.blnIncludeBlankLoan;
        }
      }
      else if (this.HasSomethingChecked())
      {
        this.chkIncludeBlankLoan.Enabled = true;
      }
      else
      {
        this.chkIncludeBlankLoan.Enabled = false;
        this.chkIncludeBlankLoan.Checked = false;
        this.IncludeBlankLoanChanged = true;
        this.dirty = true;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (LoanDuplicationTemplatePage));
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      this.imgListTv = new ImageList(this.components);
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.lblDisconnected = new Label();
      this.lblLinked = new Label();
      this.groupContainerTemplate = new GroupContainer();
      this.chkAll = new CheckBox();
      this.gridViewTemplates = new GridView();
      this.chkIncludeBlankLoan = new CheckBox();
      this.groupContainerTemplate.SuspendLayout();
      this.SuspendLayout();
      this.imgListTv.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgListTv.ImageStream");
      this.imgListTv.TransparentColor = Color.White;
      this.imgListTv.Images.SetKeyName(0, "");
      this.imgListTv.Images.SetKeyName(1, "");
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(706, 465);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(625, 465);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 2;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.lblDisconnected.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblDisconnected.ImageAlign = ContentAlignment.MiddleLeft;
      this.lblDisconnected.ImageIndex = 0;
      this.lblDisconnected.ImageList = this.imgListTv;
      this.lblDisconnected.Location = new Point(15, 475);
      this.lblDisconnected.Name = "lblDisconnected";
      this.lblDisconnected.Size = new Size(216, 16);
      this.lblDisconnected.TabIndex = 8;
      this.lblDisconnected.Text = "      Disconnected from Persona Rights";
      this.lblDisconnected.TextAlign = ContentAlignment.MiddleLeft;
      this.lblDisconnected.Visible = false;
      this.lblLinked.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblLinked.ImageAlign = ContentAlignment.MiddleLeft;
      this.lblLinked.ImageIndex = 1;
      this.lblLinked.ImageList = this.imgListTv;
      this.lblLinked.Location = new Point(15, 459);
      this.lblLinked.Name = "lblLinked";
      this.lblLinked.Size = new Size(216, 16);
      this.lblLinked.TabIndex = 7;
      this.lblLinked.Text = "      Linked with Persona Rights";
      this.lblLinked.TextAlign = ContentAlignment.MiddleLeft;
      this.lblLinked.Visible = false;
      this.groupContainerTemplate.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainerTemplate.Controls.Add((Control) this.chkAll);
      this.groupContainerTemplate.Controls.Add((Control) this.gridViewTemplates);
      this.groupContainerTemplate.HeaderForeColor = SystemColors.ControlText;
      this.groupContainerTemplate.Location = new Point(12, 12);
      this.groupContainerTemplate.Name = "groupContainerTemplate";
      this.groupContainerTemplate.Size = new Size(770, 447);
      this.groupContainerTemplate.TabIndex = 0;
      this.groupContainerTemplate.Text = "Loan Duplication Templates";
      this.chkAll.AutoSize = true;
      this.chkAll.Location = new Point(6, 28);
      this.chkAll.Name = "chkAll";
      this.chkAll.Size = new Size(15, 14);
      this.chkAll.TabIndex = 1;
      this.chkAll.UseVisualStyleBackColor = true;
      this.chkAll.CheckedChanged += new EventHandler(this.chkAll_CheckedChanged);
      this.gridViewTemplates.BorderStyle = BorderStyle.None;
      gvColumn1.CheckBoxes = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "headerCheckMark";
      gvColumn1.Text = "";
      gvColumn1.Width = 30;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Template Name";
      gvColumn2.Width = 400;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.SpringToFit = true;
      gvColumn3.Text = "Description";
      gvColumn3.Width = 338;
      this.gridViewTemplates.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gridViewTemplates.Dock = DockStyle.Fill;
      this.gridViewTemplates.ImageList = this.imgListTv;
      this.gridViewTemplates.Location = new Point(1, 26);
      this.gridViewTemplates.Name = "gridViewTemplates";
      this.gridViewTemplates.Size = new Size(768, 420);
      this.gridViewTemplates.TabIndex = 0;
      this.gridViewTemplates.Click += new EventHandler(this.gridViewTemplates_Click);
      this.chkIncludeBlankLoan.AutoSize = true;
      this.chkIncludeBlankLoan.Location = new Point(271, 470);
      this.chkIncludeBlankLoan.Name = "chkIncludeBlankLoan";
      this.chkIncludeBlankLoan.Size = new Size(112, 17);
      this.chkIncludeBlankLoan.TabIndex = 9;
      this.chkIncludeBlankLoan.Text = "Must use template";
      this.chkIncludeBlankLoan.TextAlign = ContentAlignment.TopRight;
      this.chkIncludeBlankLoan.UseVisualStyleBackColor = true;
      this.chkIncludeBlankLoan.Visible = false;
      this.chkIncludeBlankLoan.CheckedChanged += new EventHandler(this.chkIncludeBlankLoan_CheckedChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(794, 500);
      this.Controls.Add((Control) this.chkIncludeBlankLoan);
      this.Controls.Add((Control) this.lblDisconnected);
      this.Controls.Add((Control) this.lblLinked);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.groupContainerTemplate);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MinimizeBox = false;
      this.Name = nameof (LoanDuplicationTemplatePage);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Loan Duplication Templates";
      this.groupContainerTemplate.ResumeLayout(false);
      this.groupContainerTemplate.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
