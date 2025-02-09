// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.SaveQueryForm
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class SaveQueryForm : Form
  {
    private ComboBox cmbBoxQueryList;
    private TextBox txtBoxNewQueryName;
    private Button btnOK;
    private Button btnCancel;
    private RadioButton rbtnSaveNew;
    private RadioButton rbtnSaveAs;
    private Panel panel1;
    private ContactQueries queries;
    private System.ComponentModel.Container components;
    private ContactType _ContactType;
    private bool _IsNewQuery;
    private string _QueryName = string.Empty;

    public SaveQueryForm(ContactType contactType)
    {
      this.InitializeComponent();
      this._ContactType = contactType;
      this.init();
    }

    private void init()
    {
      this.queries = this._ContactType != ContactType.Borrower ? Session.ContactManager.GetBizPartnerQueries(Session.UserID) : Session.ContactManager.GetBorrowerQueries(Session.UserID);
      this.cmbBoxQueryList.Items.Clear();
      this.cmbBoxQueryList.Items.AddRange((object[]) this.queries.Items);
      this.rbtnSaveNew.Checked = true;
      this.cmbBoxQueryList.Enabled = false;
      this.txtBoxNewQueryName.Focus();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.cmbBoxQueryList = new ComboBox();
      this.txtBoxNewQueryName = new TextBox();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.rbtnSaveNew = new RadioButton();
      this.rbtnSaveAs = new RadioButton();
      this.panel1 = new Panel();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.cmbBoxQueryList.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxQueryList.Location = new Point(28, 152);
      this.cmbBoxQueryList.Name = "cmbBoxQueryList";
      this.cmbBoxQueryList.Size = new Size(204, 21);
      this.cmbBoxQueryList.TabIndex = 0;
      this.txtBoxNewQueryName.Location = new Point(28, 64);
      this.txtBoxNewQueryName.Name = "txtBoxNewQueryName";
      this.txtBoxNewQueryName.Size = new Size(204, 20);
      this.txtBoxNewQueryName.TabIndex = 3;
      this.txtBoxNewQueryName.Text = "";
      this.btnOK.Location = new Point(156, 216);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(76, 28);
      this.btnOK.TabIndex = 4;
      this.btnOK.Text = "OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(244, 216);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(76, 28);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "Cancel";
      this.rbtnSaveNew.Location = new Point(16, 16);
      this.rbtnSaveNew.Name = "rbtnSaveNew";
      this.rbtnSaveNew.Size = new Size(284, 40);
      this.rbtnSaveNew.TabIndex = 6;
      this.rbtnSaveNew.Text = "Save to a new search. Please enter the name of the new search.";
      this.rbtnSaveAs.Location = new Point(16, 104);
      this.rbtnSaveAs.Name = "rbtnSaveAs";
      this.rbtnSaveAs.Size = new Size(284, 40);
      this.rbtnSaveAs.TabIndex = 7;
      this.rbtnSaveAs.Text = "Save to an existing search. Please choose an existing search from the list.";
      this.rbtnSaveAs.CheckedChanged += new EventHandler(this.rbtnSaveAs_CheckedChanged);
      this.panel1.BorderStyle = BorderStyle.Fixed3D;
      this.panel1.Controls.Add((Control) this.cmbBoxQueryList);
      this.panel1.Controls.Add((Control) this.txtBoxNewQueryName);
      this.panel1.Controls.Add((Control) this.rbtnSaveNew);
      this.panel1.Controls.Add((Control) this.rbtnSaveAs);
      this.panel1.Location = new Point(12, 8);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(324, 192);
      this.panel1.TabIndex = 8;
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(350, (int) byte.MaxValue);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SaveQueryForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Save Search";
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public bool IsNewQuery => this._IsNewQuery;

    public string QueryName => this._QueryName;

    private void btnOK_Click(object sender, EventArgs e)
    {
      this._IsNewQuery = this.rbtnSaveNew.Checked;
      if (this._IsNewQuery)
      {
        if (this.txtBoxNewQueryName.Text.Trim() == string.Empty)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Please enter the name of your new query.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.txtBoxNewQueryName.Focus();
          return;
        }
        foreach (ContactQuery contactQuery in this.queries.Items)
        {
          if (contactQuery.Name == this.txtBoxNewQueryName.Text.Trim())
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "There already exist a query with this name.  Please enter another name of your new query.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            this.txtBoxNewQueryName.Focus();
            return;
          }
        }
        this._QueryName = this.txtBoxNewQueryName.Text;
      }
      else
      {
        if (this.cmbBoxQueryList.Text.Trim() == string.Empty)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Please select the query you want to save to.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.cmbBoxQueryList.Focus();
          return;
        }
        this._QueryName = this.cmbBoxQueryList.Text;
      }
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void rbtnSaveAs_CheckedChanged(object sender, EventArgs e)
    {
      if (this.rbtnSaveAs.Checked)
      {
        this.txtBoxNewQueryName.Enabled = false;
        this.cmbBoxQueryList.Enabled = true;
      }
      else
      {
        this.txtBoxNewQueryName.Enabled = true;
        this.txtBoxNewQueryName.Text = string.Empty;
        this.cmbBoxQueryList.Enabled = false;
      }
    }
  }
}
