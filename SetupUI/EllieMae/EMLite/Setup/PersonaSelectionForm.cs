// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PersonaSelectionForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class PersonaSelectionForm : Form
  {
    private Button btnOK;
    private Button btnCancel;
    private Label lblLine1;
    private Sessions.Session session;
    private int orgID;
    private UserInfo adminUser;
    private UserInfo selectedUser;
    private ExternalUserInfo selectedExternalUser;
    private bool canEditSAPersona;
    private bool canEditAdminPersona;
    private bool innerForced;
    private bool hasSAorA;
    private GridView gvPersonas;
    private System.ComponentModel.Container components;

    public PersonaSelectionForm(
      Sessions.Session session,
      Persona[] selectedPersonas,
      UserInfo adminUser,
      UserInfo selectedUser,
      int orgID)
    {
      this.session = session;
      this.InitializeComponent();
      this.orgID = orgID;
      this.adminUser = adminUser;
      this.selectedUser = selectedUser;
      this.canEditSAPersona = this.CanEditSuperAdministratorPersona();
      this.canEditAdminPersona = this.CanEditAdministratorPersona();
      this.loadPersonaList(selectedPersonas, (PersonaType[]) null);
    }

    public PersonaSelectionForm(
      Sessions.Session session,
      Persona[] selectedPersonas,
      UserInfo adminUser,
      ExternalUserInfo selectedExternalUser,
      int externalOrgID,
      params PersonaType[] personaType)
    {
      this.session = session;
      this.InitializeComponent();
      this.orgID = externalOrgID;
      this.adminUser = adminUser;
      this.selectedExternalUser = selectedExternalUser;
      this.canEditSAPersona = this.CanEditSuperAdministratorPersona();
      this.canEditAdminPersona = this.CanEditAdministratorPersona();
      this.loadPersonaList(selectedPersonas, personaType);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn = new GVColumn();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.lblLine1 = new Label();
      this.gvPersonas = new GridView();
      this.SuspendLayout();
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(147, 251);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 22);
      this.btnOK.TabIndex = 3;
      this.btnOK.Text = "OK";
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(223, 251);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "Cancel";
      this.lblLine1.Location = new Point(9, 9);
      this.lblLine1.Name = "lblLine1";
      this.lblLine1.Size = new Size(287, 47);
      this.lblLine1.TabIndex = 9;
      this.lblLine1.Text = "Select persona(s) that this user belongs to. If you assign multiple personas to the user, the user will have the greater rights of those personas.";
      gvColumn.CheckBoxes = true;
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "Column1";
      gvColumn.SpringToFit = true;
      gvColumn.Text = "Persona";
      gvColumn.Width = 285;
      this.gvPersonas.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.gvPersonas.HeaderHeight = 0;
      this.gvPersonas.HeaderVisible = false;
      this.gvPersonas.Location = new Point(10, 60);
      this.gvPersonas.Name = "gvPersonas";
      this.gvPersonas.Size = new Size(287, 182);
      this.gvPersonas.TabIndex = 10;
      this.gvPersonas.SubItemCheck += new GVSubItemEventHandler(this.gvPersonas_SubItemCheck);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(307, 282);
      this.Controls.Add((Control) this.gvPersonas);
      this.Controls.Add((Control) this.lblLine1);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (PersonaSelectionForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Persona Selection";
      this.ResumeLayout(false);
    }

    public Persona[] SelectedPersonas
    {
      get
      {
        ArrayList arrayList = new ArrayList();
        foreach (GVItem checkedItem in this.gvPersonas.GetCheckedItems(0))
          arrayList.Add(checkedItem.Tag);
        return (Persona[]) arrayList.ToArray(typeof (Persona));
      }
    }

    private void loadPersonaList(Persona[] selectedPersonas, PersonaType[] personaType)
    {
      this.btnOK.Enabled = false;
      Persona[] personaArray = personaType != null ? this.getPersonas(personaType) : this.getPersonas();
      if (selectedPersonas != null && selectedPersonas.Length != 0)
        this.btnOK.Enabled = true;
      ArrayList arrayList = selectedPersonas == null ? (ArrayList) null : new ArrayList((ICollection) selectedPersonas);
      foreach (Persona persona in personaArray)
      {
        GVItem gvItem = new GVItem(persona.Name);
        gvItem.Tag = (object) persona;
        if (arrayList != null && arrayList.Contains((object) persona))
        {
          gvItem.Checked = true;
          if (persona.ID == 1)
            this.hasSAorA = true;
          if (persona.ID == 0)
            this.hasSAorA = true;
        }
        if (persona.ID == 1)
        {
          if (this.canEditAdminPersona || gvItem.Checked)
          {
            if (!this.canEditAdminPersona)
              gvItem.SubItems[0].CheckBoxEnabled = false;
          }
          else
            continue;
        }
        if (persona.ID == 0)
        {
          if (this.canEditSAPersona || gvItem.Checked)
          {
            if (!this.canEditSAPersona)
              gvItem.SubItems[0].CheckBoxEnabled = false;
          }
          else
            continue;
        }
        this.gvPersonas.Items.Add(gvItem);
      }
    }

    private Persona[] getPersonas()
    {
      Persona[] allPersonas = this.session.PersonaManager.GetAllPersonas();
      ArrayList arrayList = new ArrayList();
      foreach (Persona persona in allPersonas)
        arrayList.Add((object) persona);
      return (Persona[]) arrayList.ToArray(typeof (Persona));
    }

    private Persona[] getPersonas(PersonaType[] personaType)
    {
      Persona[] allPersonas = this.session.PersonaManager.GetAllPersonas(personaType);
      ArrayList arrayList = new ArrayList();
      foreach (Persona persona in allPersonas)
        arrayList.Add((object) persona);
      return (Persona[]) arrayList.ToArray(typeof (Persona));
    }

    private bool CanEditSuperAdministratorPersona()
    {
      bool flag = false;
      if (((UserInfo) null == this.selectedUser || this.selectedUser.Userid != "admin") && this.adminUser.Userid == "admin" && this.orgID == 0)
        flag = true;
      return flag;
    }

    private bool CanEditAdministratorPersona()
    {
      bool flag = false;
      if ((UserInfo) null == this.selectedUser || this.selectedUser.Userid != "admin")
      {
        if (UserInfo.IsSuperAdministrator(this.adminUser.Userid, this.adminUser.UserPersonas))
          flag = true;
        else if (this.selectedUser == (UserInfo) null || this.selectedUser.Userid == "" || this.selectedUser.Userid != this.adminUser.Userid)
          flag = true;
      }
      return flag;
    }

    private void gvPersonas_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      if (!this.innerForced)
      {
        if (((Persona) e.SubItem.Item.Tag).ID == 0)
        {
          if (!this.canEditSAPersona)
          {
            this.innerForced = true;
            e.SubItem.Checked = !e.SubItem.Checked;
            this.innerForced = false;
          }
          else if (e.SubItem.Checked && this.hasSAorA)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "A user can not be assigned to both Super Administrator and Administrator persona", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            this.innerForced = true;
            e.SubItem.Checked = false;
            this.innerForced = false;
          }
          else
            this.hasSAorA = e.SubItem.Checked;
        }
        else if (((Persona) e.SubItem.Item.Tag).ID == 1)
        {
          if (!this.canEditAdminPersona)
          {
            this.innerForced = true;
            e.SubItem.Checked = !e.SubItem.Checked;
            this.innerForced = false;
          }
          else if (e.SubItem.Checked && this.hasSAorA)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "A user can not be assigned to both Super Administrator and Administrator persona", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            this.innerForced = true;
            e.SubItem.Checked = false;
            this.innerForced = false;
          }
          else
            this.hasSAorA = e.SubItem.Checked;
        }
      }
      this.btnOK.Enabled = false;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvPersonas.Items)
      {
        if (gvItem.Checked)
        {
          this.btnOK.Enabled = true;
          break;
        }
      }
    }
  }
}
