// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PersonaSetup.SecurityPage.OrgUserSettingDlg
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.PersonaSetup.SecurityPage
{
  public class OrgUserSettingDlg : Form
  {
    private Label lblLabel;
    private RadioButton rdbAll;
    private RadioButton rdbBelow;
    private Button btnOk;
    private Button btnCancel;
    private IContainer components;
    private int personaID = -1;
    private FeaturesAclManager aclMgr;
    private bool personal;
    private string userID = "";
    protected ImageList imgListTv;
    private Label lblLinked;
    protected Label lbldisconnected;
    private ContextMenu contextMenu1;
    private MenuItem menuItem1;
    private MenuItem menuItem2;
    private Persona[] personaList;
    private bool bConnected;
    private bool innerControl;
    private bool dirty;
    private Label label1;
    private Label label2;
    private bool readOnly;

    public OrgUserSettingDlg(int personaId)
    {
      this.InitializeComponent();
      this.aclMgr = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      this.personaID = personaId;
      this.rdbAll.ContextMenu = (ContextMenu) null;
      this.rdbBelow.ContextMenu = (ContextMenu) null;
      this.rdbAll.ImageList = (ImageList) null;
      this.rdbBelow.ImageList = (ImageList) null;
      this.innerControl = true;
      this.initialFormValue();
      this.innerControl = false;
    }

    public OrgUserSettingDlg(string userId, Persona[] personas, bool readOnly)
    {
      this.InitializeComponent();
      this.personal = true;
      this.aclMgr = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      this.userID = userId;
      this.personaList = personas;
      this.lbldisconnected.Visible = true;
      this.lblLinked.Visible = true;
      this.innerControl = true;
      this.initialFormValue();
      this.innerControl = false;
      this.readOnly = readOnly;
      this.MakeReadOnly(this.readOnly);
    }

    private void MakeReadOnly(bool readOnly)
    {
      if (readOnly)
      {
        this.btnOk.Enabled = false;
        this.menuItem1.Enabled = false;
        this.menuItem2.Enabled = false;
        this.rdbAll.Enabled = false;
        this.rdbBelow.Enabled = false;
      }
      else
      {
        this.btnOk.Enabled = true;
        this.menuItem1.Enabled = true;
        this.menuItem2.Enabled = true;
        this.rdbAll.Enabled = true;
        this.rdbBelow.Enabled = true;
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
      ResourceManager resourceManager = new ResourceManager(typeof (OrgUserSettingDlg));
      this.lblLabel = new Label();
      this.rdbAll = new RadioButton();
      this.contextMenu1 = new ContextMenu();
      this.menuItem1 = new MenuItem();
      this.menuItem2 = new MenuItem();
      this.imgListTv = new ImageList(this.components);
      this.rdbBelow = new RadioButton();
      this.btnOk = new Button();
      this.btnCancel = new Button();
      this.lblLinked = new Label();
      this.lbldisconnected = new Label();
      this.label1 = new Label();
      this.label2 = new Label();
      this.SuspendLayout();
      this.lblLabel.Location = new Point(8, 24);
      this.lblLabel.Name = "lblLabel";
      this.lblLabel.Size = new Size(136, 16);
      this.lblLabel.TabIndex = 0;
      this.lblLabel.Text = "This persona can access:";
      this.rdbAll.ContextMenu = this.contextMenu1;
      this.rdbAll.ImageAlign = ContentAlignment.MiddleLeft;
      this.rdbAll.ImageList = this.imgListTv;
      this.rdbAll.Location = new Point(8, 48);
      this.rdbAll.Name = "rdbAll";
      this.rdbAll.Size = new Size(16, 16);
      this.rdbAll.TabIndex = 1;
      this.rdbAll.CheckedChanged += new EventHandler(this.rdbAll_CheckedChanged);
      this.contextMenu1.MenuItems.AddRange(new MenuItem[2]
      {
        this.menuItem1,
        this.menuItem2
      });
      this.menuItem1.Index = 0;
      this.menuItem1.Text = "Link with Persona Rights";
      this.menuItem1.Click += new EventHandler(this.menuItem1_Click);
      this.menuItem2.Index = 1;
      this.menuItem2.Text = "Disconnect from Persona Rights";
      this.menuItem2.Click += new EventHandler(this.menuItem2_Click);
      this.imgListTv.ImageSize = new Size(16, 16);
      this.imgListTv.ImageStream = (ImageListStreamer) resourceManager.GetObject("imgListTv.ImageStream");
      this.imgListTv.TransparentColor = Color.White;
      this.rdbBelow.ContextMenu = this.contextMenu1;
      this.rdbBelow.ImageAlign = ContentAlignment.MiddleLeft;
      this.rdbBelow.ImageList = this.imgListTv;
      this.rdbBelow.Location = new Point(8, 64);
      this.rdbBelow.Name = "rdbBelow";
      this.rdbBelow.Size = new Size(20, 16);
      this.rdbBelow.TabIndex = 2;
      this.rdbBelow.CheckedChanged += new EventHandler(this.rdbBelow_CheckedChanged);
      this.btnOk.DialogResult = DialogResult.OK;
      this.btnOk.Location = new Point(184, 160);
      this.btnOk.Name = "btnOk";
      this.btnOk.TabIndex = 3;
      this.btnOk.Text = "OK";
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(264, 160);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "Cancel";
      this.lblLinked.ImageAlign = ContentAlignment.MiddleLeft;
      this.lblLinked.ImageIndex = 0;
      this.lblLinked.ImageList = this.imgListTv;
      this.lblLinked.Location = new Point(8, 128);
      this.lblLinked.Name = "lblLinked";
      this.lblLinked.Size = new Size(168, 12);
      this.lblLinked.TabIndex = 5;
      this.lblLinked.Text = "        Linked with Persona Rights";
      this.lblLinked.TextAlign = ContentAlignment.MiddleLeft;
      this.lblLinked.Visible = false;
      this.lbldisconnected.ImageAlign = ContentAlignment.MiddleLeft;
      this.lbldisconnected.ImageIndex = 1;
      this.lbldisconnected.ImageList = this.imgListTv;
      this.lbldisconnected.Location = new Point(8, 140);
      this.lbldisconnected.Name = "lbldisconnected";
      this.lbldisconnected.Size = new Size(220, 12);
      this.lbldisconnected.TabIndex = 6;
      this.lbldisconnected.Text = "        Disconnected from Persona Rights";
      this.lbldisconnected.TextAlign = ContentAlignment.MiddleLeft;
      this.lbldisconnected.Visible = false;
      this.label1.Location = new Point(24, 48);
      this.label1.Name = "label1";
      this.label1.Size = new Size(120, 16);
      this.label1.TabIndex = 7;
      this.label1.Text = "All organizations/users";
      this.label1.TextAlign = ContentAlignment.MiddleLeft;
      this.label2.Location = new Point(24, 64);
      this.label2.Name = "label2";
      this.label2.Size = new Size(288, 16);
      this.label2.TabIndex = 8;
      this.label2.Text = "Organizations/users below in the organization hierarchy";
      this.label2.TextAlign = ContentAlignment.MiddleLeft;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(358, 187);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.lbldisconnected);
      this.Controls.Add((Control) this.lblLinked);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOk);
      this.Controls.Add((Control) this.rdbBelow);
      this.Controls.Add((Control) this.rdbAll);
      this.Controls.Add((Control) this.lblLabel);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Name = nameof (OrgUserSettingDlg);
      this.Text = "Organization/Users";
      this.ResumeLayout(false);
    }

    private void initialFormValue()
    {
      bool flag = false;
      if (!this.personal)
        return;
      this.rdbAll.Text = "      " + this.rdbAll.Text;
      this.rdbBelow.Text = "      " + this.rdbBelow.Text;
      if (this.bConnected)
        this.SetConnect();
      else
        this.SetDisconnect();
      if (flag)
        this.rdbAll.Checked = true;
      else
        this.rdbBelow.Checked = true;
    }

    private void SetImageIndex(int option)
    {
      this.rdbAll.ImageIndex = option;
      this.rdbBelow.ImageIndex = option;
    }

    public void SaveData()
    {
      this.dirty = false;
      if (!this.personal)
      {
        int num1 = this.rdbAll.Checked ? 1 : 0;
      }
      else
      {
        if (this.bConnected)
          return;
        int num2 = this.rdbAll.Checked ? 1 : 0;
      }
    }

    private void rdbAll_CheckedChanged(object sender, EventArgs e)
    {
      if (this.innerControl)
        return;
      this.SetDisconnect();
      this.dirty = true;
    }

    public bool HasBeenModified() => this.dirty;

    private void rdbBelow_CheckedChanged(object sender, EventArgs e)
    {
      if (this.innerControl)
        return;
      this.SetDisconnect();
      this.dirty = true;
    }

    private void menuItem2_Click(object sender, EventArgs e)
    {
      this.SetDisconnect();
      this.dirty = true;
    }

    private void SetDisconnect()
    {
      this.bConnected = false;
      this.SetImageIndex(1);
    }

    private void SetConnect()
    {
      this.bConnected = true;
      this.SetImageIndex(0);
    }

    private void menuItem1_Click(object sender, EventArgs e)
    {
    }
  }
}
