// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.URLControlWithEntity
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.Properties;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class URLControlWithEntity : UserControl
  {
    private List<ExternalOrgURL> listUrls = new List<ExternalOrgURL>();
    public bool activateLeaveEvent;
    private string currentUrl = "";
    private IContainer components;
    private Panel panel1;
    private Panel panel5;
    private Panel panel4;
    private ComboBox cmbEntity;
    private Panel panel3;
    private ComboBox cmbURL;
    private Panel panel2;
    private StandardIconButton btnDelete;
    private Label label1;
    private PictureBox picWarning;
    private Panel panel6;
    private Label label2;

    public event URLControlWithEntity.EventHandle DeleteEvent;

    public event URLControlWithEntity.EventHandle ChangeEvent;

    public event URLControlWithEntity.EventHandle UrlEnter;

    public event URLControlWithEntity.EventHandle UrlTextChanged;

    public URLControlWithEntity() => this.InitializeComponent();

    public URLControlWithEntity(
      bool useDropdown,
      List<ExternalOrgURL> availableURLs,
      int index,
      List<string> EntityType,
      string selectedURL = "",
      int selectIndexEntity = -1)
      : this()
    {
      this.listUrls = availableURLs;
      this.cmbEntity.DataSource = (object) EntityType;
      foreach (object availableUrL in availableURLs)
        this.cmbURL.Items.Add(availableUrL);
      this.cmbURL.DisplayMember = "URL";
      this.cmbEntity.SelectedIndex = selectIndexEntity;
      this.currentUrl = selectedURL;
      this.cmbURL.Text = selectedURL;
    }

    private void cmbEntityType_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    private void cmbSiteUrl_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    private void panel3_Paint(object sender, PaintEventArgs e)
    {
    }

    public object SelectedUrl
    {
      get => this.cmbURL.SelectedItem;
      set => this.cmbURL.SelectedItem = value;
    }

    public bool IsWarningVisible
    {
      get => this.picWarning.Visible;
      set => this.picWarning.Visible = value;
    }

    public object selectedEntity
    {
      get => this.cmbEntity.SelectedItem;
      set => this.cmbEntity.SelectedItem = value;
    }

    private void cmbURL_IndexChanged(object sender, EventArgs e)
    {
      if (this.ChangeEvent == null)
        return;
      this.ChangeEvent(sender, e);
    }

    private void cmbURL_TextChanged(object sender, EventArgs e)
    {
      if (this.UrlTextChanged == null)
        return;
      this.UrlTextChanged(sender, e);
    }

    private void cmbEntity_TextChanged(object sender, EventArgs e)
    {
      if (this.ChangeEvent == null)
        return;
      this.ChangeEvent(sender, e);
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (this.DeleteEvent == null)
        return;
      this.DeleteEvent((object) this, e);
    }

    public bool SetUrlFocus
    {
      set
      {
        if (!value)
          return;
        this.cmbURL.Focus();
      }
    }

    public bool SetEntityFocus
    {
      set
      {
        if (!value)
          return;
        this.cmbEntity.Focus();
      }
    }

    private void cmbURL_Enter(object sender, EventArgs e)
    {
    }

    private void cmbURL_Click(object sender, EventArgs e)
    {
      if (this.UrlEnter == null)
        return;
      this.UrlEnter(sender, e);
    }

    private void cmbURL_Leave(object sender, EventArgs e)
    {
    }

    public new string Text
    {
      get => this.cmbURL.Text;
      set => this.cmbURL.Text = value;
    }

    private void cmbEntity_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.ChangeEvent == null)
        return;
      this.ChangeEvent(sender, e);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panel1 = new Panel();
      this.panel6 = new Panel();
      this.cmbEntity = new ComboBox();
      this.panel5 = new Panel();
      this.picWarning = new PictureBox();
      this.btnDelete = new StandardIconButton();
      this.panel4 = new Panel();
      this.label2 = new Label();
      this.panel3 = new Panel();
      this.cmbURL = new ComboBox();
      this.panel2 = new Panel();
      this.label1 = new Label();
      this.panel1.SuspendLayout();
      this.panel6.SuspendLayout();
      this.panel5.SuspendLayout();
      ((ISupportInitialize) this.picWarning).BeginInit();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      this.panel4.SuspendLayout();
      this.panel3.SuspendLayout();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.panel1.Controls.Add((Control) this.panel6);
      this.panel1.Controls.Add((Control) this.panel5);
      this.panel1.Controls.Add((Control) this.panel4);
      this.panel1.Controls.Add((Control) this.panel3);
      this.panel1.Controls.Add((Control) this.panel2);
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(829, 27);
      this.panel1.TabIndex = 0;
      this.panel6.Controls.Add((Control) this.cmbEntity);
      this.panel6.Dock = DockStyle.Fill;
      this.panel6.Location = new Point(603, 0);
      this.panel6.Name = "panel6";
      this.panel6.Size = new Size(171, 27);
      this.panel6.TabIndex = 4;
      this.cmbEntity.Dock = DockStyle.Fill;
      this.cmbEntity.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbEntity.FormattingEnabled = true;
      this.cmbEntity.Location = new Point(0, 0);
      this.cmbEntity.Name = "cmbEntity";
      this.cmbEntity.Size = new Size(171, 21);
      this.cmbEntity.TabIndex = 0;
      this.cmbEntity.SelectedIndexChanged += new EventHandler(this.cmbEntity_SelectedIndexChanged);
      this.cmbEntity.TextChanged += new EventHandler(this.cmbEntity_TextChanged);
      this.panel5.Controls.Add((Control) this.picWarning);
      this.panel5.Controls.Add((Control) this.btnDelete);
      this.panel5.Dock = DockStyle.Right;
      this.panel5.Location = new Point(774, 0);
      this.panel5.Name = "panel5";
      this.panel5.Size = new Size(55, 27);
      this.panel5.TabIndex = 3;
      this.picWarning.Image = (Image) Resources.alert;
      this.picWarning.Location = new Point(35, 6);
      this.picWarning.Name = "picWarning";
      this.picWarning.Size = new Size(17, 16);
      this.picWarning.TabIndex = 1;
      this.picWarning.TabStop = false;
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Location = new Point(13, 6);
      this.btnDelete.MouseDownImage = (Image) null;
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(16, 16);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabIndex = 0;
      this.btnDelete.TabStop = false;
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.panel4.Controls.Add((Control) this.label2);
      this.panel4.Dock = DockStyle.Left;
      this.panel4.Location = new Point(502, 0);
      this.panel4.Name = "panel4";
      this.panel4.Size = new Size(101, 27);
      this.panel4.TabIndex = 2;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(6, 6);
      this.label2.Name = "label2";
      this.label2.Size = new Size(73, 13);
      this.label2.TabIndex = 0;
      this.label2.Text = "Channel Type";
      this.panel3.Controls.Add((Control) this.cmbURL);
      this.panel3.Dock = DockStyle.Left;
      this.panel3.Location = new Point(75, 0);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(427, 27);
      this.panel3.TabIndex = 1;
      this.cmbURL.Dock = DockStyle.Fill;
      this.cmbURL.FormattingEnabled = true;
      this.cmbURL.Location = new Point(0, 0);
      this.cmbURL.Name = "cmbURL";
      this.cmbURL.Size = new Size(427, 21);
      this.cmbURL.TabIndex = 0;
      this.cmbURL.SelectedIndexChanged += new EventHandler(this.cmbURL_IndexChanged);
      this.cmbURL.Click += new EventHandler(this.cmbURL_Click);
      this.cmbURL.Enter += new EventHandler(this.cmbURL_Enter);
      this.cmbURL.Leave += new EventHandler(this.cmbURL_Leave);
      this.panel2.Controls.Add((Control) this.label1);
      this.panel2.Dock = DockStyle.Left;
      this.panel2.Location = new Point(0, 0);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(75, 27);
      this.panel2.TabIndex = 0;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(10, 3);
      this.label1.Name = "label1";
      this.label1.Size = new Size(53, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Site URL ";
      this.AutoSize = true;
      this.Controls.Add((Control) this.panel1);
      this.Name = nameof (URLControlWithEntity);
      this.Size = new Size(832, 30);
      this.panel1.ResumeLayout(false);
      this.panel6.ResumeLayout(false);
      this.panel5.ResumeLayout(false);
      ((ISupportInitialize) this.picWarning).EndInit();
      ((ISupportInitialize) this.btnDelete).EndInit();
      this.panel4.ResumeLayout(false);
      this.panel4.PerformLayout();
      this.panel3.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.ResumeLayout(false);
    }

    public delegate void EventHandle(object sender, EventArgs e);
  }
}
