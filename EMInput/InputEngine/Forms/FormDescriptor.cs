// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.Forms.FormDescriptor
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.Encompass.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine.Forms
{
  public class FormDescriptor
  {
    private string formId;
    private string formName;
    private string formHtml;

    public FormDescriptor(string formId, string formName, string formHtml)
    {
      this.formName = formName;
      this.formId = formId;
      this.formHtml = formHtml;
    }

    public string FormName => this.formName;

    public string FormID => this.formId;

    public string FormHTML => this.formHtml;

    public string ToXHTML() => Html2XHtml.Convert(this.formHtml);

    public override bool Equals(object obj)
    {
      return obj is FormDescriptor formDescriptor && formDescriptor.FormID == this.FormID && formDescriptor.formName == this.formName && this.formHtml == formDescriptor.formHtml;
    }

    public override int GetHashCode() => this.formHtml.GetHashCode();

    public override string ToString() => this.formName;
  }
}
