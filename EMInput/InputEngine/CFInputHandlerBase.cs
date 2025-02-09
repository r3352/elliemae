// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.CFInputHandlerBase
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  internal class CFInputHandlerBase : InputHandlerBase
  {
    private CustomFieldsInfo customData;

    internal override void UpdateFieldValue(string id, string val)
    {
      if (id.IndexOf("DESC") != -1)
        return;
      base.UpdateFieldValue(id, val);
    }

    protected override string FormatValue(string fieldId, string value)
    {
      if (fieldId.IndexOf("FV") == -1)
        return base.FormatValue(fieldId, value);
      CustomFieldInfo field = this.customData.GetField(fieldId);
      if ((field == null ? 0 : (int) field.Format) != 102)
        return base.FormatValue(fieldId, value);
      string str = "";
      if (value.Length >= 1)
        str = value.Substring(0, 1).ToUpper();
      if (str != "Y" && str != "N")
      {
        string empty = string.Empty;
      }
      return value;
    }

    protected override FieldFormat GetFieldFormat(string id)
    {
      if (id.IndexOf("FV") == -1)
        return base.GetFieldFormat(id);
      CustomFieldInfo field = this.customData.GetField(id);
      return field != null ? field.Format : FieldFormat.NONE;
    }

    protected override string GetFieldValue(string id, FieldSource fieldSource)
    {
      if (id.IndexOf("DESC") == -1)
        return base.GetFieldValue(id, fieldSource);
      CustomFieldInfo field = this.customData.GetField(id);
      return field != null ? field.Description : "";
    }

    public CFInputHandlerBase(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public CFInputHandlerBase(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
      this.customData = this.session.ConfigurationManager.GetLoanCustomFields();
    }

    public CFInputHandlerBase(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public CFInputHandlerBase(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
      this.customData = this.session.ConfigurationManager.GetLoanCustomFields();
    }
  }
}
