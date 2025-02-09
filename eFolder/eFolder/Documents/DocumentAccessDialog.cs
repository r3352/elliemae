// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Documents.DocumentAccessDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Documents
{
  public class DocumentAccessDialog : Form
  {
    private LoanDataMgr loanDataMgr;
    private DocumentLog[] docList;
    private bool canRemoveAccessFromProtectedDocs;
    private IContainer components;
    internal GridView gvRoles;
    private Button btnCancel;
    private Button btnSave;
    private Label lblAccess;
    private GroupContainer groupContainer1;
    private Label label1;
    private EMHelpLink helpLink;

    public DocumentAccessDialog(LoanDataMgr loanDataMgr, DocumentLog[] docList)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
      this.docList = docList;
      this.canRemoveAccessFromProtectedDocs = new eFolderAccessRights(loanDataMgr).CanRemoveAccessFromProtectedDocument;
      this.loadRoleList();
    }

    private void loadRoleList()
    {
      foreach (RoleInfo allRole in this.loanDataMgr.SystemConfiguration.AllRoles)
        this.addRole(allRole);
      this.addRole(RoleInfo.Others);
    }

    private void addRole(RoleInfo role)
    {
      CheckBox checkBox = new CheckBox();
      checkBox.CheckAlign = ContentAlignment.BottomCenter;
      bool role1 = this.docList[0].IsAccessibleToRole(role.RoleID);
      checkBox.Checked = role1;
      for (int index = 1; index < this.docList.Length; ++index)
      {
        if (this.docList[index].IsAccessibleToRole(role.RoleID) != role1)
          checkBox.CheckState = CheckState.Indeterminate;
      }
      if (!checkBox.CheckState.Equals((object) CheckState.Unchecked) && role.Protected)
        checkBox.Enabled = this.canRemoveAccessFromProtectedDocs;
      GVItem gvItem = new GVItem();
      gvItem.SubItems[0].Value = (object) checkBox;
      gvItem.SubItems[1].Text = role.Name;
      if (role.Protected)
        gvItem.SubItems[1].Text += "*";
      gvItem.SubItems[2].Text = this.getTeamMemberName(role.RoleID);
      gvItem.Tag = (object) role;
      this.gvRoles.Items.Add(gvItem);
    }

    private string getTeamMemberName(int roleId)
    {
      List<string> stringList = new List<string>();
      foreach (LoanAssociateLog allLoanAssociate in this.loanDataMgr.LoanData.GetLogList().GetAllLoanAssociates())
      {
        if (allLoanAssociate.RoleID == roleId && allLoanAssociate.LoanAssociateType != LoanAssociateType.None && !stringList.Contains(allLoanAssociate.LoanAssociateName))
          stringList.Add(allLoanAssociate.LoanAssociateName);
      }
      return string.Join(", ", stringList.ToArray());
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      bool flag = false;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvRoles.Items)
      {
        CheckBox checkBox = (CheckBox) gvItem.SubItems[0].Value;
        if (checkBox.Enabled && checkBox.CheckState.Equals((object) CheckState.Checked) && ((RoleSummaryInfo) gvItem.Tag).Protected)
          flag = true;
      }
      if (flag && Utils.Dialog((IWin32Window) this, "You are about to grant access to one or more Protected roles. Once this is done, the document cannot be deleted and access to this role cannot be revoked except by those with the appropriate access rights.", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
        return;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvRoles.Items)
      {
        CheckBox checkBox = (CheckBox) gvItem.SubItems[0].Value;
        if (checkBox.Enabled)
        {
          RoleInfo tag = (RoleInfo) gvItem.Tag;
          foreach (DocumentLog doc in this.docList)
          {
            switch (checkBox.CheckState)
            {
              case CheckState.Unchecked:
                doc.RemoveAccess(tag.RoleID);
                break;
              case CheckState.Checked:
                doc.GrantAccess(tag.RoleID);
                break;
            }
          }
        }
      }
      this.DialogResult = DialogResult.OK;
    }

    private void DocumentAccessDialog_KeyDown(object sender, KeyEventArgs e)
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
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      this.gvRoles = new GridView();
      this.btnCancel = new Button();
      this.btnSave = new Button();
      this.lblAccess = new Label();
      this.groupContainer1 = new GroupContainer();
      this.label1 = new Label();
      this.helpLink = new EMHelpLink();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.gvRoles.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "colAccess";
      gvColumn1.Text = "Access";
      gvColumn1.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn1.Width = 50;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "colRole";
      gvColumn2.Text = "Role";
      gvColumn2.Width = 110;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "colName";
      gvColumn3.Text = "Name";
      gvColumn3.Width = 160;
      this.gvRoles.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gvRoles.Dock = DockStyle.Fill;
      this.gvRoles.Location = new Point(1, 26);
      this.gvRoles.Name = "gvRoles";
      this.gvRoles.Size = new Size(350, 233);
      this.gvRoles.TabIndex = 2;
      this.gvRoles.TextTrimming = StringTrimming.EllipsisCharacter;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(288, 336);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "Cancel";
      this.btnSave.Location = new Point(212, 336);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 22);
      this.btnSave.TabIndex = 4;
      this.btnSave.Text = "Save";
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.lblAccess.Location = new Point(9, 8);
      this.lblAccess.Name = "lblAccess";
      this.lblAccess.Size = new Size(354, 32);
      this.lblAccess.TabIndex = 0;
      this.lblAccess.Text = "Select the roles that can access these documents. Use the \"Others\" role to give document access to users that are not loan team members.";
      this.groupContainer1.Controls.Add((Control) this.gvRoles);
      this.groupContainer1.Location = new Point(10, 42);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(352, 260);
      this.groupContainer1.TabIndex = 1;
      this.groupContainer1.Text = "Roles";
      this.label1.Location = new Point(10, 306);
      this.label1.Name = "label1";
      this.label1.Size = new Size(354, 20);
      this.label1.TabIndex = 3;
      this.label1.Text = "*Once access is granted to this role, the documents become protected.";
      this.helpLink.BackColor = Color.Transparent;
      this.helpLink.Cursor = Cursors.Hand;
      this.helpLink.HelpTag = "Document Access Rights";
      this.helpLink.Location = new Point(8, 338);
      this.helpLink.Name = "helpLink";
      this.helpLink.Size = new Size(90, 18);
      this.helpLink.TabIndex = 6;
      this.helpLink.TabStop = false;
      this.AcceptButton = (IButtonControl) this.btnSave;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(373, 366);
      this.Controls.Add((Control) this.helpLink);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.lblAccess);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSave);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (DocumentAccessDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Document Access Rights";
      this.KeyDown += new KeyEventHandler(this.DocumentAccessDialog_KeyDown);
      this.groupContainer1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
