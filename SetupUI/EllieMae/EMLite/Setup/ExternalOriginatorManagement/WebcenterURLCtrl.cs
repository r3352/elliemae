// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.WebcenterURLCtrl
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
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.ExternalOriginatorManagement
{
  public class WebcenterURLCtrl : UserControl
  {
    private bool useDropdown;
    private List<ExternalOrgURL> availableURLs = new List<ExternalOrgURL>();
    public bool deleteControl;
    private List<ExternalOrgURL> listUrls = new List<ExternalOrgURL>();
    private string currentUrl = "";
    private IContainer components;
    private Panel panel1;
    private Panel panel2;
    private PictureBox picWarning;
    private StandardIconButton btnRemove;
    private Panel panel3;
    private Label labelSite;
    private ComboBox cboURL;
    private TextBox txtURL;

    public event WebcenterURLCtrl.EventHandle DeleteEvent;

    public event WebcenterURLCtrl.EventHandle ChangeEvent;

    public event WebcenterURLCtrl.EventHandle UrlEnter;

    public event WebcenterURLCtrl.EventHandle TextChange;

    public WebcenterURLCtrl() => this.InitializeComponent();

    public WebcenterURLCtrl(
      bool useDropdown,
      List<ExternalOrgURL> availableURLs,
      int index,
      string selectedURL = "")
      : this()
    {
      this.useDropdown = useDropdown;
      this.cboURL.Visible = useDropdown;
      this.txtURL.Visible = !useDropdown;
      this.listUrls = availableURLs;
      this.currentUrl = selectedURL;
      if (useDropdown)
      {
        foreach (object availableUrL in availableURLs)
          this.cboURL.Items.Add(availableUrL);
        this.cboURL.DisplayMember = nameof (URL);
        this.cboURL.Text = selectedURL;
      }
      else
        this.txtURL.Text = selectedURL;
      this.labelSite.Text = "Site URL";
    }

    private void btnRemove_Click(object sender, EventArgs e)
    {
      if (this.DeleteEvent == null)
        return;
      this.DeleteEvent((object) this, e);
    }

    public string WarningMessage
    {
      get => this.picWarning.Text;
      set => this.picWarning.Text = value;
    }

    private void URL_TextChanged(object sender, EventArgs e)
    {
      if (this.TextChange == null)
        return;
      this.TextChange(sender, e);
    }

    private void URL_IndexChanged(object sender, EventArgs e)
    {
      if (this.ChangeEvent == null)
        return;
      this.ChangeEvent(sender, e);
    }

    public string URL
    {
      get => this.cboURL.Visible ? this.cboURL.Text : this.txtURL.Text;
      set
      {
        if (!this.cboURL.Visible)
          return;
        this.cboURL.Text = value;
      }
    }

    public object SelectedURL
    {
      get => this.cboURL.Visible ? this.cboURL.SelectedItem : (object) this.txtURL.Text;
      set
      {
        if (this.cboURL.Visible)
          this.cboURL.SelectedItem = value;
        else
          this.txtURL.Text = value.ToString();
      }
    }

    public bool SetFocus
    {
      set
      {
        if (this.cboURL.Visible)
          this.cboURL.Focus();
        else
          this.txtURL.Focus();
      }
    }

    private void txtURL_Leave(object sender, EventArgs e)
    {
      if (this.listUrls.FirstOrDefault<ExternalOrgURL>((Func<ExternalOrgURL, bool>) (a => a.URL == this.cboURL.Text)) == null && this.cboURL.Visible && this.deleteControl)
      {
        int num = (int) MessageBox.Show("Your entered URL is not valid or not one of the assigned URL.", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.cboURL.Text = this.currentUrl;
      }
      this.URL_TextChanged(sender, e);
    }

    public bool IsWarningVisible
    {
      get => this.picWarning.Visible;
      set => this.picWarning.Visible = value;
    }

    private void cboURL_Click(object sender, EventArgs e)
    {
      if (this.UrlEnter == null)
        return;
      this.UrlEnter(sender, e);
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
      this.labelSite = new Label();
      this.panel2 = new Panel();
      this.picWarning = new PictureBox();
      this.btnRemove = new StandardIconButton();
      this.panel3 = new Panel();
      this.txtURL = new TextBox();
      this.cboURL = new ComboBox();
      this.panel1.SuspendLayout();
      this.panel2.SuspendLayout();
      ((ISupportInitialize) this.picWarning).BeginInit();
      ((ISupportInitialize) this.btnRemove).BeginInit();
      this.panel3.SuspendLayout();
      this.SuspendLayout();
      this.panel1.Controls.Add((Control) this.labelSite);
      this.panel1.Dock = DockStyle.Left;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(79, 28);
      this.panel1.TabIndex = 0;
      this.labelSite.AutoSize = true;
      this.labelSite.Location = new Point(3, 6);
      this.labelSite.Name = "labelSite";
      this.labelSite.Size = new Size(59, 13);
      this.labelSite.TabIndex = 3;
      this.labelSite.Text = "Site URL 1";
      this.panel2.Controls.Add((Control) this.picWarning);
      this.panel2.Controls.Add((Control) this.btnRemove);
      this.panel2.Dock = DockStyle.Right;
      this.panel2.Location = new Point(562, 0);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(42, 28);
      this.panel2.TabIndex = 1;
      this.picWarning.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.picWarning.Image = (Image) Resources.alert;
      this.picWarning.Location = new Point(23, 6);
      this.picWarning.Name = "picWarning";
      this.picWarning.Size = new Size(16, 16);
      this.picWarning.SizeMode = PictureBoxSizeMode.AutoSize;
      this.picWarning.TabIndex = 6;
      this.picWarning.TabStop = false;
      this.btnRemove.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRemove.BackColor = Color.Transparent;
      this.btnRemove.Location = new Point(4, 6);
      this.btnRemove.MouseDownImage = (Image) Resources.delete_over;
      this.btnRemove.Name = "btnRemove";
      this.btnRemove.Size = new Size(16, 16);
      this.btnRemove.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnRemove.TabIndex = 5;
      this.btnRemove.TabStop = false;
      this.btnRemove.Click += new EventHandler(this.btnRemove_Click);
      this.panel3.Controls.Add((Control) this.txtURL);
      this.panel3.Controls.Add((Control) this.cboURL);
      this.panel3.Dock = DockStyle.Fill;
      this.panel3.Location = new Point(79, 0);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(483, 28);
      this.panel3.TabIndex = 2;
      this.txtURL.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtURL.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
      this.txtURL.AutoCompleteSource = AutoCompleteSource.AllUrl;
      this.txtURL.Location = new Point(6, 3);
      this.txtURL.Name = "txtURL";
      this.txtURL.Size = new Size(471, 20);
      this.txtURL.TabIndex = 7;
      this.txtURL.TextChanged += new EventHandler(this.URL_TextChanged);
      this.cboURL.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboURL.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
      this.cboURL.FormattingEnabled = true;
      this.cboURL.Location = new Point(6, 3);
      this.cboURL.Name = "cboURL";
      this.cboURL.Size = new Size(471, 21);
      this.cboURL.TabIndex = 6;
      this.cboURL.SelectedIndexChanged += new EventHandler(this.URL_IndexChanged);
      this.cboURL.TextChanged += new EventHandler(this.URL_TextChanged);
      this.cboURL.Click += new EventHandler(this.cboURL_Click);
      this.cboURL.Leave += new EventHandler(this.txtURL_Leave);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.panel3);
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.panel1);
      this.Name = nameof (WebcenterURLCtrl);
      this.Size = new Size(604, 28);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      ((ISupportInitialize) this.picWarning).EndInit();
      ((ISupportInitialize) this.btnRemove).EndInit();
      this.panel3.ResumeLayout(false);
      this.panel3.PerformLayout();
      this.ResumeLayout(false);
    }

    public delegate void EventHandle(object sender, EventArgs e);
  }
}
