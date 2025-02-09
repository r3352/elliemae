// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.ExternalUsersList
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.ExternalOriginatorManagement
{
  public class ExternalUsersList : Form
  {
    private Sessions.Session session;
    private ExternalUserInfo deleteExtUser;
    private ExternalUserInfo selectedUser;
    private List<ExternalUserInfo> allLOLPUsers;
    private Persona[] allPersonas;
    private bool isTPOMVP;
    private IContainer components;
    private GridView gridView1;
    private Button button1;
    private Button button2;

    public ExternalUsersList(
      Sessions.Session session,
      ExternalUserInfo deleteExtUser,
      List<ExternalUserInfo> allLOLPUsers)
      : this(session, deleteExtUser, allLOLPUsers, (List<int>) null)
    {
    }

    public ExternalUsersList(
      Sessions.Session session,
      ExternalUserInfo deleteExtUser,
      List<ExternalUserInfo> allLOLPUsers,
      List<int> personaIds)
    {
      ExternalUsersList externalUsersList = this;
      this.session = session;
      this.deleteExtUser = deleteExtUser;
      this.allLOLPUsers = allLOLPUsers;
      this.InitializeComponent();
      this.button1.Enabled = false;
      List<ExternalUserInfo> source = (List<ExternalUserInfo>) null;
      this.isTPOMVP = session.ConfigurationManager.CheckIfAnyTPOSiteExists();
      if (this.isTPOMVP)
      {
        this.allPersonas = session.PersonaManager.GetAllPersonas();
        source = new List<ExternalUserInfo>();
        foreach (int personaId1 in personaIds)
        {
          int personaId = personaId1;
          IEnumerable<ExternalUserInfo> externalUserInfos = allLOLPUsers.Where<ExternalUserInfo>((Func<ExternalUserInfo, bool>) (e1 => ((IEnumerable<Persona>) e1.UserPersonas).Contains<Persona>(((IEnumerable<Persona>) closure_0.allPersonas).Where<Persona>((Func<Persona, bool>) (p => p.ID == personaId)).Single<Persona>()) && !e1.ExternalUserID.Equals(deleteExtUser.ExternalUserID)));
          if (externalUserInfos.Count<ExternalUserInfo>() > 0)
            source.AddRange(externalUserInfos);
        }
      }
      else if (TPOUtils.IsLoanOfficer(deleteExtUser.Roles) || TPOUtils.IsLoanOfficer(deleteExtUser.Roles) && TPOUtils.IsLoanProcessor(deleteExtUser.Roles))
        source = allLOLPUsers.Where<ExternalUserInfo>((Func<ExternalUserInfo, bool>) (e1 => TPOUtils.IsLoanOfficer(e1.Roles) && !e1.ExternalUserID.Equals(deleteExtUser.ExternalUserID))).ToList<ExternalUserInfo>();
      else if (TPOUtils.IsLoanProcessor(deleteExtUser.Roles))
        source = allLOLPUsers.Where<ExternalUserInfo>((Func<ExternalUserInfo, bool>) (e1 => TPOUtils.IsLoanProcessor(e1.Roles) && !e1.ExternalUserID.Equals(deleteExtUser.ExternalUserID))).ToList<ExternalUserInfo>();
      if (this.isTPOMVP)
        this.gridView1.Columns[2].Text = "Personas";
      foreach (ExternalUserInfo externalUserInfo in source.Distinct<ExternalUserInfo>())
        this.gridView1.Items.Add(new GVItem(new string[4]
        {
          externalUserInfo.FirstName,
          externalUserInfo.LastName,
          this.isTPOMVP ? (externalUserInfo.UserPersonas != null ? TPOUtils.ReturnPersonas(externalUserInfo.UserPersonas) : string.Empty) : TPOUtils.returnRoles(externalUserInfo.Roles),
          externalUserInfo.EmailForLogin
        })
        {
          Tag = (object) externalUserInfo
        });
    }

    public ExternalUserInfo getSelectedUser() => this.selectedUser;

    private void gridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.gridView1.SelectedItems.Any<GVItem>())
      {
        if (this.gridView1.SelectedItems.Count > 1)
        {
          this.button1.Enabled = false;
        }
        else
        {
          this.selectedUser = (ExternalUserInfo) this.gridView1.SelectedItems[0].Tag;
          this.button1.Enabled = true;
        }
      }
      else
        this.button1.Enabled = false;
    }

    private void button1_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

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
      this.button1 = new Button();
      this.button2 = new Button();
      this.gridView1 = new GridView();
      this.SuspendLayout();
      this.button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.button1.Location = new Point(334, 282);
      this.button1.Name = "button1";
      this.button1.Size = new Size(75, 23);
      this.button1.TabIndex = 1;
      this.button1.Text = "OK";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new EventHandler(this.button1_Click);
      this.button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.button2.DialogResult = DialogResult.Cancel;
      this.button2.Location = new Point(424, 282);
      this.button2.Name = "button2";
      this.button2.Size = new Size(75, 23);
      this.button2.TabIndex = 2;
      this.button2.Text = "Cancel";
      this.button2.UseVisualStyleBackColor = true;
      this.gridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "First Name";
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Last Name";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Roles";
      gvColumn3.Width = 200;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "Login Email";
      gvColumn4.Width = 110;
      this.gridView1.Columns.AddRange(new GVColumn[4]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4
      });
      this.gridView1.Location = new Point(2, 1);
      this.gridView1.Name = "gridView1";
      this.gridView1.Size = new Size(512, 265);
      this.gridView1.TabIndex = 0;
      this.gridView1.SelectedIndexChanged += new EventHandler(this.gridView1_SelectedIndexChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.button2;
      this.ClientSize = new Size(511, 322);
      this.Controls.Add((Control) this.button2);
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this.gridView1);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ExternalUsersList);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "External Users List";
      this.ResumeLayout(false);
    }
  }
}
