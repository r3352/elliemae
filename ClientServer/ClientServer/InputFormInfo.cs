// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.InputFormInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class InputFormInfo : IComparable
  {
    public static readonly InputFormInfo Divider = new InputFormInfo("-", new string('-', 33), InputFormType.None);
    private string formId;
    private string name;
    private string mname;
    private InputFormType type;
    private InputFormCategory category;
    private int order = -1;
    private bool isDefault = true;
    private bool canPickField = true;

    public InputFormInfo(string mname)
    {
      this.formId = Guid.NewGuid().ToString();
      this.type = InputFormType.Custom;
      this.category = InputFormCategory.Form;
      this.mname = mname;
      this.name = InputFormInfo.mnameToName(mname);
    }

    public InputFormInfo(string formId, string mname)
    {
      this.formId = formId;
      this.type = InputFormType.Standard;
      this.category = InputFormCategory.Form;
      this.mname = mname;
      this.name = InputFormInfo.mnameToName(mname);
    }

    public InputFormInfo(string formId, string mname, InputFormType type)
    {
      this.formId = formId;
      this.type = type;
      this.category = InputFormCategory.Form;
      this.mname = mname;
      this.name = InputFormInfo.mnameToName(mname);
    }

    public InputFormInfo(
      string formId,
      string mname,
      InputFormType type,
      InputFormCategory category,
      int order,
      bool isDefault,
      bool canPickField)
    {
      this.formId = formId;
      this.mname = mname;
      this.type = type;
      this.category = category;
      this.order = order;
      this.name = InputFormInfo.mnameToName(mname);
      this.isDefault = isDefault;
      this.canPickField = canPickField;
    }

    public string FormID => this.formId;

    public InputFormType Type => this.type;

    public InputFormCategory Category => this.category;

    public string MnemonicName
    {
      get => this.mname;
      set
      {
        this.mname = value;
        this.name = InputFormInfo.mnameToName(value);
      }
    }

    public string Name => this.name;

    public int Order => this.order;

    public bool IsDefault
    {
      get => this.isDefault;
      set => this.isDefault = value;
    }

    public bool CanPickField
    {
      get => this.canPickField;
      set => this.canPickField = value;
    }

    private static string mnameToName(string mname)
    {
      mname = mname.Replace("&&", "\u0001");
      mname = mname.Replace("&", "");
      mname = mname.Replace("\u0001", "&");
      return mname;
    }

    public static string NormalizeName(string mname) => InputFormInfo.mnameToName(mname);

    public override string ToString() => this.name;

    public override bool Equals(object obj)
    {
      InputFormInfo inputFormInfo = obj as InputFormInfo;
      return !(inputFormInfo == (InputFormInfo) null) && inputFormInfo.formId == this.formId;
    }

    public static bool operator ==(InputFormInfo o1, InputFormInfo o2)
    {
      return object.Equals((object) o1, (object) o2);
    }

    public static bool operator !=(InputFormInfo o1, InputFormInfo o2) => !(o1 == o2);

    public override int GetHashCode() => this.formId.GetHashCode();

    public int CompareTo(object obj) => this.order = ((InputFormInfo) obj).order;

    public static bool IsChildForm(string formID)
    {
      return formID.ToLower() == "re88395pg4" || formID.ToLower() == "mlds - ca gfe page 4";
    }

    public static string GetCorrectFormName(string formID, string currentName)
    {
      switch (formID)
      {
        case "MAX23K":
          return "FHA 203(k)";
        default:
          if (string.Compare(currentName, "203K MAX MORTGAGE WS", true) != 0)
            return currentName;
          goto case "MAX23K";
      }
    }
  }
}
