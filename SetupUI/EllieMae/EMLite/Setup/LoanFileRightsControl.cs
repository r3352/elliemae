// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LoanFileRightsControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class LoanFileRightsControl : UserControl
  {
    private bool suspendEvent;
    private Sessions.Session session;
    private ComboBox comboBoxRights;
    private Button buttonSelect;
    private System.ComponentModel.Container components;
    private BizRule.LoanAccessRight loanRights;
    private Persona personaBelongTo;
    private string[] editableFields;

    public event EventHandler AllPersonsClick;

    public LoanFileRightsControl(
      Sessions.Session session,
      Persona personaBelongTo,
      BizRule.LoanAccessRight loanRights,
      string[] editableFields)
    {
      this.suspendEvent = true;
      this.session = session;
      this.personaBelongTo = personaBelongTo;
      this.loanRights = loanRights;
      this.editableFields = editableFields;
      this.InitializeComponent();
      this.initForm();
      this.suspendEvent = false;
    }

    public LoanFileRightsControl(
      Sessions.Session session,
      Persona personaBelongTo,
      BizRule.LoanAccessRight loanRights,
      string[] editableFields,
      bool disableLoanAccess)
      : this(session, personaBelongTo, loanRights, editableFields)
    {
      this.comboBoxRights.Enabled = this.buttonSelect.Enabled = !disableLoanAccess;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public int LoanRights => (int) this.loanRights;

    public Persona PersonaBelongTo => this.personaBelongTo;

    public string[] EditableFields => this.editableFields;

    private void InitializeComponent()
    {
      this.comboBoxRights = new ComboBox();
      this.buttonSelect = new Button();
      this.SuspendLayout();
      this.comboBoxRights.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxRights.Location = new Point(0, 0);
      this.comboBoxRights.MaxDropDownItems = 4;
      this.comboBoxRights.Name = "comboBoxRights";
      this.comboBoxRights.Size = new Size(112, 21);
      this.comboBoxRights.TabIndex = 0;
      this.comboBoxRights.SelectedIndexChanged += new EventHandler(this.comboBoxRights_SelectedIndexChanged);
      this.buttonSelect.BackColor = Color.FromArgb(224, 224, 224);
      this.buttonSelect.Location = new Point(114, 0);
      this.buttonSelect.Name = "buttonSelect";
      this.buttonSelect.Size = new Size(75, 20);
      this.buttonSelect.TabIndex = 1;
      this.buttonSelect.Text = "Select";
      this.buttonSelect.UseVisualStyleBackColor = false;
      this.buttonSelect.Click += new EventHandler(this.buttonSelect_Click);
      this.BackColor = Color.WhiteSmoke;
      this.Controls.Add((Control) this.buttonSelect);
      this.Controls.Add((Control) this.comboBoxRights);
      this.Name = nameof (LoanFileRightsControl);
      this.Size = new Size(516, 20);
      this.ResumeLayout(false);
    }

    private void initForm()
    {
      this.comboBoxRights.Items.AddRange((object[]) BizRule.LoanAccessRightStrings);
      if (this.personaBelongTo == (Persona) null)
      {
        this.comboBoxRights.Items.Add((object) "");
        this.buttonSelect.Visible = false;
        this.comboBoxRights.SelectedIndex = this.comboBoxRights.Items.Count - 1;
        this.comboBoxRights.MaxDropDownItems = 5;
      }
      else if (this.loanRights == BizRule.LoanAccessRight.EditAll)
        this.comboBoxRights.SelectedIndex = 1;
      else if (this.loanRights == BizRule.LoanAccessRight.ViewAllOnly)
        this.comboBoxRights.SelectedIndex = 0;
      else if (this.loanRights == BizRule.LoanAccessRight.DoesNotApply)
        this.comboBoxRights.SelectedIndex = 3;
      else
        this.comboBoxRights.SelectedIndex = 2;
    }

    public void SetComboboxText(string text) => this.comboBoxRights.Text = text;

    private void comboBoxRights_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.personaBelongTo == (Persona) null)
      {
        if (this.AllPersonsClick == null)
          return;
        this.AllPersonsClick(sender, e);
      }
      else if (this.comboBoxRights.SelectedIndex == 1)
      {
        this.buttonSelect.Visible = false;
        this.loanRights = BizRule.LoanAccessRight.EditAll;
      }
      else if (this.comboBoxRights.SelectedIndex == 0)
      {
        this.buttonSelect.Visible = false;
        this.loanRights = BizRule.LoanAccessRight.ViewAllOnly;
      }
      else if (this.comboBoxRights.SelectedIndex == 3)
      {
        this.buttonSelect.Visible = false;
        this.loanRights = BizRule.LoanAccessRight.DoesNotApply;
      }
      else
      {
        this.buttonSelect.Visible = true;
        if (this.suspendEvent)
          return;
        this.buttonSelect_Click((object) null, (EventArgs) null);
      }
    }

    private void buttonSelect_Click(object sender, EventArgs e)
    {
      using (LoanFileRightsDialog fileRightsDialog = new LoanFileRightsDialog(this.session, this.loanRights, this.editableFields))
      {
        if (fileRightsDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.loanRights = fileRightsDialog.LoanRights;
        this.editableFields = fileRightsDialog.EditableFields;
        if (this.loanRights == BizRule.LoanAccessRight.EditAll || this.loanRights == BizRule.LoanAccessRight.ViewAllOnly || this.loanRights == BizRule.LoanAccessRight.DoesNotApply)
          this.buttonSelect.Visible = false;
        else
          this.buttonSelect.Visible = true;
        if (this.loanRights != BizRule.LoanAccessRight.ViewAllOnly)
          return;
        this.comboBoxRights.SelectedIndex = 0;
      }
    }
  }
}
