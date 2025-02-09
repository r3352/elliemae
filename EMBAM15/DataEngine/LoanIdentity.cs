// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.LoanIdentity
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common.Diagnostics;
using Encompass.Diagnostics.Logging.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class LoanIdentity : 
    ISerializable,
    ICloneable,
    IWhiteListRemoteMethodParam,
    IRemoteCallLogDecorator
  {
    public const string IllegalNameCharacters = "\\/:*?\"<>|";
    public const int MaxLoanFolderNameLength = 100;
    public const int MaxLoanNameLength = 64;
    private string loanFolder = "";
    private string loanName = "";
    private string guid = "";
    private int xrefId = -1;
    private bool isArchive;

    public LoanIdentity(
      string loanFolder,
      string loanName,
      string guid,
      int xrefId,
      bool ignoreValidation = false)
    {
      loanFolder = loanFolder ?? "";
      loanName = loanName ?? "";
      guid = guid ?? "";
      if (!ignoreValidation)
      {
        if (loanFolder != "" && !LoanIdentity.IsValidName(loanFolder))
          throw new ArgumentException("Loan folder name contains invalid characters or ends with one or more whitespace characters", nameof (loanFolder));
        if (!this.isValidLoanName(loanName))
          throw new ArgumentException("Loan name contains invalid characters or ends with one or more whitespace characters", nameof (loanName));
        if ((loanName == "" || loanFolder == "") && guid == "")
          throw new ArgumentException("Either the loan folder/name or its guid must be specified", nameof (loanFolder));
        if (loanFolder.Length > 100)
          throw new ArgumentException("Loan Folder Name exceeds maximum character limit", nameof (loanFolder));
        if (loanName.Length > 64)
          throw new ArgumentException("Loan Name exceeds maximum character limit", nameof (loanName));
      }
      this.loanFolder = loanFolder;
      this.loanName = loanName;
      this.guid = guid;
      this.xrefId = xrefId;
    }

    public LoanIdentity(
      string loanFolder,
      string loanName,
      string guid,
      bool isArchive,
      int xrefId,
      bool ignoreValidation = false)
      : this(loanFolder, loanName, guid, xrefId, ignoreValidation)
    {
      this.isArchive = isArchive;
    }

    public LoanIdentity(string loanFolder, string loanName, string guid)
      : this(loanFolder, loanName, guid, -1)
    {
    }

    public LoanIdentity(string guid)
      : this("", "", guid, -1)
    {
    }

    public LoanIdentity(string loanFolder, string loanName)
      : this(loanFolder, loanName, "", -1)
    {
    }

    private LoanIdentity(SerializationInfo info, StreamingContext context)
    {
      this.loanFolder = info.GetString(nameof (loanFolder));
      this.loanName = info.GetString(nameof (loanName));
      this.guid = info.GetString(nameof (guid));
      this.xrefId = info.GetInt32(nameof (xrefId));
      if (this.loanFolder != "" && !LoanIdentity.IsValidName(this.loanFolder))
        throw new ApplicationException("Invalid folder name '" + this.loanFolder + "'");
      if (!this.isValidLoanName(this.loanName))
        throw new ApplicationException("Invalid file name '" + this.loanName + "'");
    }

    private bool isValidLoanName(string loanName)
    {
      string name = loanName;
      if (new Regex("\\[Folder#[0-9]+\\]\\\\").Match(loanName).Success)
        name = loanName.Substring(loanName.LastIndexOf("\\") + 1);
      return !(name != "") || LoanIdentity.IsValidName(name);
    }

    public LoanIdentity(LoanIdentity source)
    {
      this.loanName = source.loanName;
      this.loanFolder = source.loanFolder;
      this.guid = source.guid;
      this.xrefId = source.xrefId;
    }

    public LoanIdentity(LoanIdentity source, int xrefId)
    {
      this.loanName = source.loanName;
      this.loanFolder = source.loanFolder;
      this.guid = source.guid;
      this.xrefId = xrefId;
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      info.AddValue("loanFolder", (object) this.loanFolder);
      info.AddValue("loanName", (object) this.loanName);
      info.AddValue("guid", (object) this.guid);
      info.AddValue("xrefId", this.xrefId);
    }

    public bool IsArchive => this.isArchive;

    public string LoanFolder => this.loanFolder;

    public string LoanName
    {
      get => this.loanName;
      set => this.loanName = value;
    }

    public string Guid => this.guid;

    public int XrefId => this.xrefId;

    public static bool IsValidName(string name)
    {
      switch (name)
      {
        case null:
          throw new ArgumentNullException(nameof (name), "Name cannot be null");
        case "":
          return false;
        default:
          if (name.TrimEnd() != name)
            return false;
          byte[] bytes = Encoding.ASCII.GetBytes(name);
          for (int index = 0; index < bytes.Length; ++index)
          {
            if (bytes[index] < (byte) 32 || bytes[index] > (byte) 126)
              return false;
          }
          return "\\/:*?\"<>|".IndexOfAny(name.ToCharArray()) < 0;
      }
    }

    public bool IsComplete() => this.guid != "" && this.loanFolder != "" && this.loanName != "";

    public override string ToString()
    {
      return this.loanFolder != "" ? this.loanFolder + "\\" + this.loanName : this.guid;
    }

    public object Clone() => (object) new LoanIdentity(this);

    public override bool Equals(object obj)
    {
      LoanIdentity loanIdentity = obj as LoanIdentity;
      return (object) loanIdentity != null && loanIdentity.Guid == this.Guid && loanIdentity.LoanName == this.LoanName && loanIdentity.LoanFolder == this.LoanFolder;
    }

    public override int GetHashCode()
    {
      return this.guid.GetHashCode() + this.loanName.GetHashCode() + this.loanFolder.GetHashCode();
    }

    public void SerializeForLog(JsonWriter writer, JsonSerializer serializer)
    {
      serializer.Serialize(writer, (object) new JObject()
      {
        {
          "folder",
          (JToken) this.loanFolder
        },
        {
          "id",
          (JToken) this.guid
        },
        {
          "name",
          (JToken) this.loanName
        }
      });
    }

    public void Decorate(Log log)
    {
      if (!string.IsNullOrEmpty(this.guid))
        log.Set<string>(Log.CommonFields.LoanId, this.guid);
      if (string.IsNullOrEmpty(this.loanFolder))
        return;
      log.Set<string>(Log.CommonFields.LoanFolder, this.loanFolder);
    }

    public static bool operator ==(LoanIdentity a, LoanIdentity b)
    {
      return object.Equals((object) a, (object) b);
    }

    public static bool operator !=(LoanIdentity a, LoanIdentity b) => !(a == b);
  }
}
