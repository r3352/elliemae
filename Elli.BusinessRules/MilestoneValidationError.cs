// Decompiled with JetBrains decompiler
// Type: Elli.BusinessRules.MilestoneValidationError
// Assembly: Elli.BusinessRules, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D0A206AB-C2DC-4F02-BBE4-A037D1140EF4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.BusinessRules.dll

using Elli.Common;
using System.Text;

#nullable disable
namespace Elli.BusinessRules
{
  public class MilestoneValidationError
  {
    private EmList<FieldModelPathInfo> _reqFields = new EmList<FieldModelPathInfo>();
    private EmList<RequiredDocInfo> _reqDocs = new EmList<RequiredDocInfo>();
    private EmList<RequiredTaskInfo> _reqTasks = new EmList<RequiredTaskInfo>();
    private EmList<string> _advanceCodeError = new EmList<string>();
    protected string ValidationCode;

    public EmList<FieldValidationError> FieldValidationErrors { get; set; }

    public string NextMilestoneUser { get; set; }

    public EmList<FieldModelPathInfo> RequiredFields
    {
      get => this._reqFields;
      set => this._reqFields = value;
    }

    public EmList<RequiredDocInfo> RequiredDocs
    {
      get => this._reqDocs;
      set => this._reqDocs = value;
    }

    public EmList<RequiredTaskInfo> RequiredTasks
    {
      get => this._reqTasks;
      set => this._reqTasks = value;
    }

    public bool IsValid
    {
      get
      {
        return (this.FieldValidationErrors == null || this.FieldValidationErrors.Count == 0) && this.RequiredFields.Count == 0 && this.RequiredDocs.Count == 0 && this.RequiredTasks.Count == 0 && string.IsNullOrEmpty(this.NextMilestoneUser) && this.AdvanceCodeError.Count == 0;
      }
    }

    public EmList<string> AdvanceCodeError
    {
      get => this._advanceCodeError;
      set => this._advanceCodeError = value;
    }

    public string GetErrorString()
    {
      StringBuilder stringBuilder = new StringBuilder("Complete milestone error due to");
      string str = "";
      if (this.FieldValidationErrors != null && this.FieldValidationErrors.Count > 0)
      {
        stringBuilder.AppendFormat("{0} Field validation errors {1}", (object) str, (object) this.FieldValidationErrors.Count);
        str = ",";
      }
      if (this.RequiredFields.Count > 0)
      {
        stringBuilder.AppendFormat("{0} Required fields {1}", (object) str, (object) this.RequiredFields.Count);
        str = ",";
      }
      if (this.RequiredDocs.Count > 0)
      {
        stringBuilder.AppendFormat("{0} Required docs {1}", (object) str, (object) this.RequiredDocs.Count);
        str = ",";
      }
      if (this.RequiredTasks.Count > 0)
      {
        stringBuilder.AppendFormat("{0} Required tasks {1}", (object) str, (object) this.RequiredTasks.Count);
        str = ",";
      }
      if (!string.IsNullOrEmpty(this.NextMilestoneUser))
        stringBuilder.AppendFormat("{0} {1}", (object) str, (object) this.NextMilestoneUser);
      return stringBuilder.ToString();
    }

    public void SetValidationCode(string code) => this.ValidationCode = code;

    public string GetValidationCode() => this.ValidationCode;
  }
}
