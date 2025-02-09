// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.LoanCenter.RecipientControlEx
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

#nullable disable
namespace EllieMae.EMLite.eFolder.LoanCenter
{
  public class RecipientControlEx : RecipientControl
  {
    private string id;

    public object DataTag { get; set; }

    public RecipientControlEx()
    {
      this.txtName.Text = string.Empty;
      this.txtName.ReadOnly = true;
      this.txtEmail.Text = string.Empty;
      this.txtEmail.ReadOnly = true;
      this.txtAuthenticationCode.Text = string.Empty;
      this.txtAuthenticationCode.ReadOnly = true;
    }

    public string BorrowerPairId
    {
      get => this.id;
      set => this.id = value;
    }

    public string RecipientType
    {
      get => this.cbSelect.Text;
      set => this.cbSelect.Text = value;
    }

    public string RecipientName
    {
      get => this.txtName.Text;
      set
      {
        this.txtName.Text = value;
        this.txtName.ReadOnly = !this.cbSelect.Enabled || !string.IsNullOrWhiteSpace(value);
      }
    }

    public bool RecipientChecked
    {
      get => this.cbSelect.Checked;
      set => this.cbSelect.Checked = value;
    }

    public bool RecipientEnabled
    {
      get => this.cbSelect.Enabled;
      set
      {
        this.cbSelect.Enabled = value;
        this.txtName.ReadOnly = !value;
        this.txtEmail.ReadOnly = !value;
        this.txtName.ReadOnly = !value || !string.IsNullOrWhiteSpace(this.txtName.Text);
        this.txtEmail.ReadOnly = !value || !string.IsNullOrWhiteSpace(this.txtEmail.Text);
        this.txtAuthenticationCode.ReadOnly = !value || !string.IsNullOrWhiteSpace(this.txtAuthenticationCode.Text);
      }
    }

    public bool RecipientAuthenticationEnabled => !this.txtAuthenticationCode.ReadOnly;

    public string RecipientEmail
    {
      get => this.txtEmail.Text;
      set
      {
        this.txtEmail.Text = value;
        this.txtEmail.ReadOnly = !this.cbSelect.Enabled || !string.IsNullOrWhiteSpace(value);
      }
    }

    public string RecipientAuthenticationCode
    {
      get => this.txtAuthenticationCode.Text;
      set
      {
        this.txtAuthenticationCode.Text = value;
        this.txtAuthenticationCode.ReadOnly = !this.cbSelect.Enabled || !string.IsNullOrWhiteSpace(value);
      }
    }

    public bool RecipientAuthenticationCodeVisible
    {
      get => this.txtAuthenticationCode.Visible;
      set
      {
        this.txtAuthenticationCode.Visible = value;
        if (value)
          this.txtEmail.Width = this.txtAuthenticationCode.Left - this.txtEmail.Left - 6;
        else
          this.txtEmail.Width = this.txtAuthenticationCode.Right - this.txtEmail.Left;
      }
    }
  }
}
