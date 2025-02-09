// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.QuickLink
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class QuickLink
  {
    private string text;
    private string formId;
    private string accessFormId;
    private int dialogWidth = 600;
    private int dialogHeight = 400;

    public QuickLink(string text, string formId)
    {
      this.text = text;
      this.formId = formId;
    }

    public QuickLink(string text, string formId, string accessFormId)
      : this(text, formId)
    {
      this.accessFormId = accessFormId;
    }

    public QuickLink(string text, string formId, int dialogWidth, int dialogHeight)
      : this(text, formId)
    {
      this.dialogWidth = dialogWidth;
      this.dialogHeight = dialogHeight;
    }

    public string Text => this.text;

    public string FormID => this.formId;

    public string AccessFormID => this.accessFormId != null ? this.accessFormId : this.formId;

    public int DialogWidth => this.dialogWidth;

    public int DialogHeight => this.dialogHeight;
  }
}
