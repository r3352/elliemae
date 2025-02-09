// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.HtmlEmail.HtmlEmailTemplate
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.FieldSearch;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

#nullable disable
namespace EllieMae.EMLite.ClientServer.HtmlEmail
{
  [Serializable]
  public class HtmlEmailTemplate : IXmlSerializable, IIdentifiable, IFieldSearchable
  {
    private string guid;
    private string ownerID;
    private HtmlEmailTemplateType type;
    private HtmlEmailLoanSourceType loan;
    private string subject;
    private string html;
    private bool migrated;
    public const string SignatureField = "Signature�";
    public const string CurrentUserNameField = "CurrentUserName�";
    public const string CurrentUserEmailField = "CurrentUserEmail�";
    public const string CurrentUserPhoneField = "CurrentUserPhone�";
    public const string InformationalDocumentsField = "Informational Documents�";
    public const string SignAndReturnDocumentsField = "Sign and Return Documents�";
    public const string NeededDocumentsField = "Needed Documents�";
    public const string AuthenticationUserField = "AuthenticationUser�";
    public const string AuthenticationCodeField = "AuthenticationCode�";
    public const string RecipientFullName = "Recipient Full Name�";

    public static HtmlEmailTemplateType[] GetValidEmailTemplateTypes()
    {
      return new HtmlEmailTemplateType[5]
      {
        HtmlEmailTemplateType.LoanLevelConsent,
        HtmlEmailTemplateType.RequestDocuments,
        HtmlEmailTemplateType.InitialDisclosures,
        HtmlEmailTemplateType.SendDocuments,
        HtmlEmailTemplateType.PreClosing
      };
    }

    public static HtmlEmailLoanSourceType[] GetValidLoanSourceTypes()
    {
      return new HtmlEmailLoanSourceType[2]
      {
        HtmlEmailLoanSourceType.NonConsumerConnect,
        HtmlEmailLoanSourceType.ConsumerConnect
      };
    }

    public static HtmlEmailTemplateType[] GetValidStatusOnlineTypes()
    {
      return new HtmlEmailTemplateType[1]
      {
        HtmlEmailTemplateType.StatusOnline
      };
    }

    public static string[] GetEmailTemplateSpecificFieldIds()
    {
      return new string[7]
      {
        "Signature",
        "CurrentUserName",
        "CurrentUserEmail",
        "CurrentUserPhone",
        "Informational Documents",
        "Sign and Return Documents",
        "Needed Documents"
      };
    }

    public HtmlEmailTemplate(string ownerID)
      : this(System.Guid.NewGuid().ToString(), ownerID)
    {
    }

    public HtmlEmailTemplate(string guid, string ownerID)
    {
      this.guid = guid;
      this.ownerID = ownerID;
      this.type = HtmlEmailTemplateType.Unknown;
      this.loan = HtmlEmailLoanSourceType.Unknown;
      this.subject = string.Empty;
      this.html = string.Empty;
      this.migrated = false;
    }

    public HtmlEmailTemplate(XmlSerializationInfo info)
    {
      this.guid = info.GetString(nameof (Guid));
      this.ownerID = info.GetString(nameof (OwnerID));
      this.type = info.GetEnum<HtmlEmailTemplateType>(nameof (Type));
      this.loan = info.GetEnum<HtmlEmailLoanSourceType>(nameof (Loan), HtmlEmailLoanSourceType.Unknown);
      this.subject = info.GetString(nameof (Subject));
      this.html = info.GetString(nameof (Html));
      this.migrated = info.GetBoolean(nameof (Migrated), false);
    }

    public string Guid => this.guid;

    public string OwnerID => this.ownerID;

    public HtmlEmailTemplateType Type
    {
      get => this.type;
      set => this.type = value;
    }

    public HtmlEmailLoanSourceType Loan
    {
      get => this.loan;
      set => this.loan = value;
    }

    public string Subject
    {
      get => this.subject;
      set => this.subject = value;
    }

    public string Html
    {
      get => this.html;
      set => this.html = value;
    }

    public bool Migrated
    {
      get => this.migrated;
      set => this.migrated = value;
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("Guid", (object) this.guid);
      info.AddValue("OwnerID", (object) this.ownerID);
      info.AddValue("Type", (object) this.type);
      info.AddValue("Loan", (object) this.loan);
      info.AddValue("Subject", (object) this.subject);
      info.AddValue("Html", (object) this.html);
      info.AddValue("Migrated", (object) this.migrated);
    }

    public override string ToString() => this.subject;

    public override int GetHashCode() => this.guid.GetHashCode();

    public static bool operator ==(HtmlEmailTemplate o1, HtmlEmailTemplate o2)
    {
      return object.Equals((object) o1, (object) o2);
    }

    public static bool operator !=(HtmlEmailTemplate o1, HtmlEmailTemplate o2)
    {
      return !object.Equals((object) o1, (object) o2);
    }

    public override bool Equals(object obj)
    {
      HtmlEmailTemplate htmlEmailTemplate = obj as HtmlEmailTemplate;
      return htmlEmailTemplate != (HtmlEmailTemplate) null && this.guid == htmlEmailTemplate.guid;
    }

    public HtmlEmailTemplate Clone(string ownerID)
    {
      return new HtmlEmailTemplate(ownerID)
      {
        type = this.type,
        loan = this.loan,
        html = this.html,
        subject = this.subject,
        migrated = this.migrated
      };
    }

    public IEnumerable<KeyValuePair<RelationshipType, string>> GetFields()
    {
      Regex regex = new Regex("\\<LABEL\\s*(?<label>[^\\>]*)\\s*\\>", RegexOptions.IgnoreCase);
      Regex emidRegex = new Regex("(^emid|\\s+emid)\\s*=\\s*\"(?<emid>.*)\"", RegexOptions.IgnoreCase);
      Match labelMatch = regex.Match(this.html);
      while (labelMatch.Success)
      {
        Match emidMatch = emidRegex.Match(labelMatch.Groups["label"].Value);
        while (emidMatch.Success)
        {
          string strA = emidMatch.Groups["emid"].Value;
          if (!string.IsNullOrEmpty(strA))
          {
            foreach (string templateSpecificFieldId in HtmlEmailTemplate.GetEmailTemplateSpecificFieldIds())
            {
              if (string.Compare(strA, templateSpecificFieldId, StringComparison.CurrentCultureIgnoreCase) == 0)
              {
                strA = (string) null;
                break;
              }
            }
            if (strA != null)
              yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ResultFields, strA);
            emidMatch = emidMatch.NextMatch();
          }
        }
        labelMatch = labelMatch.NextMatch();
        emidMatch = (Match) null;
      }
    }
  }
}
