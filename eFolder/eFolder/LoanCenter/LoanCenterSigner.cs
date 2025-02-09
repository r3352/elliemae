// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.LoanCenter.LoanCenterSigner
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.eFolder.LoanCenter
{
  public class LoanCenterSigner
  {
    private string signerID;
    private LoanCenterEntityType entityType;
    private string firstName;
    private string middleName;
    private string lastName;
    private string suffixName;
    private string unparsedName;
    private string emailAddress;
    private string userID;
    private string signatureField;
    private string initialsField;
    private bool autoSign;
    private List<LoanCenterSecurityQuestion> questionList;
    public int NboIndex;
    public int NBOVestingIndex;
    public bool Ischecked = true;
    public string RecipientType;
    public string AuthCode;

    public LoanCenterSigner(string signerID, LoanCenterEntityType entityType)
    {
      this.signerID = signerID;
      this.entityType = entityType;
      this.questionList = new List<LoanCenterSecurityQuestion>();
    }

    public string CheckboxField { get; set; }

    public string SignerID
    {
      get => this.signerID;
      set => this.signerID = value;
    }

    public LoanCenterEntityType EntityType => this.entityType;

    public string FirstName
    {
      get => this.firstName;
      set => this.firstName = value;
    }

    public string MiddleName
    {
      get => this.middleName;
      set => this.middleName = value;
    }

    public string LastName
    {
      get => this.lastName;
      set => this.lastName = value;
    }

    public string SuffixName
    {
      get => this.suffixName;
      set => this.suffixName = value;
    }

    public string UnparsedName
    {
      get => this.unparsedName;
      set => this.unparsedName = value;
    }

    public string EmailAddress
    {
      get => this.emailAddress;
      set => this.emailAddress = value;
    }

    public string UserID
    {
      get => this.userID;
      set => this.userID = value;
    }

    public string SignatureField
    {
      get => this.signatureField;
      set => this.signatureField = value;
    }

    public string InitialsField
    {
      get => this.initialsField;
      set => this.initialsField = value;
    }

    public bool AutoSign
    {
      get => this.autoSign;
      set => this.autoSign = value;
    }

    public LoanCenterSecurityQuestion[] SecurityQuestions => this.questionList.ToArray();

    public LoanCenterSecurityQuestion AddSecurityQuestion(string fieldID, string fieldValue)
    {
      LoanCenterSecurityQuestion securityQuestion = new LoanCenterSecurityQuestion(fieldID, fieldValue);
      this.questionList.Add(securityQuestion);
      return securityQuestion;
    }

    public bool RequiresSigning(DocumentAttachment attachment)
    {
      return attachment.SignatureFields != null && (Array.Exists<string>(attachment.SignatureFields, (Predicate<string>) (element => element.StartsWith(this.signatureField, StringComparison.OrdinalIgnoreCase))) || Array.Exists<string>(attachment.SignatureFields, (Predicate<string>) (element => element.StartsWith(this.initialsField, StringComparison.OrdinalIgnoreCase))));
    }
  }
}
