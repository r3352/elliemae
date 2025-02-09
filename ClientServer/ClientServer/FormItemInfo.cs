// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.FormItemInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.eFolder;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class FormItemInfo
  {
    private string _GroupName;
    private string _FormName;
    private string _UIName;
    private OutputFormType _FormType;
    private bool _IsBorrower;
    private int _BlockNum;
    private DocumentTemplate _DocumentTemplate;
    private PrintForm.MergeLocationValues _MergeLocation;
    private MergeParamValues _MergeParams = new MergeParamValues();

    public FormItemInfo()
      : this("", "", OutputFormType.None, true, 1)
    {
    }

    public FormItemInfo(string groupName, string formName, OutputFormType formType)
      : this(groupName, formName, formType, true, 1)
    {
    }

    public FormItemInfo(
      string groupName,
      string formName,
      OutputFormType formType,
      PrintForm.MergeLocationValues mergeLocation)
      : this(groupName, formName, formType, true, 1, mergeLocation)
    {
    }

    public FormItemInfo(
      string groupName,
      string formName,
      OutputFormType formType,
      bool isBor,
      int blockNum,
      DocumentTemplate docTemplate = null)
    {
      this._GroupName = groupName;
      this._FormName = formName;
      this._FormType = formType;
      this._IsBorrower = isBor;
      this._BlockNum = blockNum;
      this._DocumentTemplate = docTemplate;
    }

    public FormItemInfo(
      string groupName,
      string formName,
      OutputFormType formType,
      bool isBor,
      int blockNum,
      PrintForm.MergeLocationValues mergeLocation,
      DocumentTemplate docTemplate = null)
    {
      this._GroupName = groupName;
      this._FormName = formName;
      this._FormType = formType;
      this._IsBorrower = isBor;
      this._BlockNum = blockNum;
      this._MergeLocation = mergeLocation;
      this._DocumentTemplate = docTemplate;
    }

    public override int GetHashCode()
    {
      return this._GroupName.GetHashCode() ^ this._FormName.GetHashCode() ^ this._FormType.GetHashCode() ^ this._IsBorrower.GetHashCode() ^ this._BlockNum.GetHashCode();
    }

    public string SignatureType { get; set; }

    public override bool Equals(object obj)
    {
      if (obj == null || this.GetType() != obj.GetType())
        return false;
      FormItemInfo formItemInfo = (FormItemInfo) obj;
      if (!object.Equals((object) this._FormName, (object) formItemInfo._FormName))
        return false;
      if (this._FormType == OutputFormType.Verifs || this._FormType == OutputFormType.Documents)
      {
        if (!object.Equals((object) this._FormName, (object) formItemInfo._FormName) || !object.Equals((object) this._FormType, (object) formItemInfo._FormType) || !this._BlockNum.Equals(formItemInfo._BlockNum) || !this._IsBorrower.Equals(formItemInfo._IsBorrower))
          return false;
      }
      else if (!object.Equals((object) this._FormName, (object) formItemInfo._FormName) || !object.Equals((object) this._FormType, (object) formItemInfo._FormType))
        return false;
      return true;
    }

    public static bool operator ==(FormItemInfo o1, FormItemInfo o2)
    {
      return object.Equals((object) o1, (object) o2);
    }

    public static bool operator !=(FormItemInfo o1, FormItemInfo o2) => !(o1 == o2);

    public override string ToString()
    {
      if (this._FormType == OutputFormType.PdfForms || this._FormType == OutputFormType.Verifs)
        return this.FormName;
      if (this._FormType != OutputFormType.CustomLetters)
        return this.ShortFormName;
      string str = this.ShortFormName;
      int num = str.LastIndexOf(".");
      if (num > -1)
      {
        string lower = str.Substring(num).ToLower();
        if (lower == ".rtf" || lower == ".doc" || lower == ".docx")
          str = str.Substring(0, num);
      }
      return str;
    }

    public PrintForm.MergeLocationValues MergeLocation
    {
      get => this._MergeLocation;
      set => this._MergeLocation = value;
    }

    public MergeParamValues MergeParams
    {
      get => this._MergeParams;
      set => this._MergeParams = new MergeParamValues(value);
    }

    public string GroupName
    {
      get => this._GroupName;
      set => this._GroupName = value;
    }

    public string FormName
    {
      get => this._FormName;
      set => this._FormName = value;
    }

    public string UIName
    {
      get => this._UIName;
      set => this._UIName = value;
    }

    public string ShortFormName => FileSystem.GetFileName(this._FormName);

    public OutputFormType FormType
    {
      get => this._FormType;
      set => this._FormType = value;
    }

    public bool IsBorrower
    {
      get => this._IsBorrower;
      set => this._IsBorrower = value;
    }

    public int BlockNum
    {
      get => this._BlockNum;
      set => this._BlockNum = value;
    }

    public DocumentTemplate DocTemplate
    {
      get => this._DocumentTemplate;
      set => this._DocumentTemplate = value;
    }

    public string HashtableKey
    {
      get => this.FormName + "|" + (object) this.FormType + "|" + (object) this.BlockNum;
    }
  }
}
