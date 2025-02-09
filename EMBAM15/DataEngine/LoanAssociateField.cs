// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.LoanAssociateField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class LoanAssociateField : VirtualField
  {
    private const string FieldPrefix = "LoanTeamMember";
    private LoanAssociateProperty property;
    private FieldOptionCollection options;

    internal LoanAssociateField(
      LoanAssociateProperty property,
      string description,
      FieldFormat format)
      : base("LoanTeamMember." + property.ToString(), description, format, FieldInstanceSpecifierType.Role)
    {
      this.property = property;
    }

    internal LoanAssociateField(
      LoanAssociateProperty property,
      string description,
      string[] options)
      : this(property, description, FieldFormat.STRING)
    {
      this.options = new FieldOptionCollection(options, true);
    }

    public override FieldOptionCollection Options
    {
      get => this.options != null ? this.options : base.Options;
    }

    internal LoanAssociateField(LoanAssociateField parent, string roleName)
      : base((VirtualField) parent, (object) roleName)
    {
      this.property = parent.property;
    }

    public override VirtualFieldType VirtualFieldType => VirtualFieldType.LoanAssociateFields;

    protected override FieldDefinition CreateInstanceFromSpecifier(object instanceSpecifier)
    {
      return (FieldDefinition) new LoanAssociateField(this, string.Concat(instanceSpecifier));
    }

    protected override string Evaluate(LoanData loan)
    {
      LoanAssociateLog loanAssociateLog = this.getLoanAssociateLog(loan);
      if (loanAssociateLog == null)
        return "";
      switch (this.property)
      {
        case LoanAssociateProperty.UserID:
          return loanAssociateLog.LoanAssociateID;
        case LoanAssociateProperty.Name:
          return loanAssociateLog.LoanAssociateName;
        case LoanAssociateProperty.Phone:
          return loanAssociateLog.LoanAssociatePhone;
        case LoanAssociateProperty.Email:
          return loanAssociateLog.LoanAssociateEmail;
        case LoanAssociateProperty.Cell:
          return loanAssociateLog.LoanAssociateCellPhone;
        case LoanAssociateProperty.Fax:
          return loanAssociateLog.LoanAssociateFax;
        case LoanAssociateProperty.AssociateType:
          if (loanAssociateLog.LoanAssociateType == LoanAssociateType.User)
            return "User";
          return loanAssociateLog.LoanAssociateType == LoanAssociateType.Group ? "Group" : "";
        case LoanAssociateProperty.LoanAssociateAccess:
          return !loanAssociateLog.LoanAssociateAccess ? "No" : "Yes";
        case LoanAssociateProperty.Title:
          return loanAssociateLog.LoanAssociateTitle;
        case LoanAssociateProperty.APIClientID:
          return loanAssociateLog.APIClientID;
        default:
          return "";
      }
    }

    protected LoanAssociateLog getLoanAssociateLog(LoanData loan)
    {
      MilestoneLog[] allMilestones = loan.GetLogList().GetAllMilestones();
      for (int index = allMilestones.Length - 1; index >= 0; --index)
      {
        MilestoneLog loanAssociateLog = allMilestones[index];
        if (string.Compare(loanAssociateLog.RoleName, string.Concat(this.InstanceSpecifier), true) == 0 && loanAssociateLog.LoanAssociateID != "")
          return (LoanAssociateLog) loanAssociateLog;
      }
      foreach (MilestoneFreeRoleLog milestoneFreeRole in loan.GetLogList().GetAllMilestoneFreeRoles())
      {
        if (string.Compare(milestoneFreeRole.RoleName, string.Concat(this.InstanceSpecifier), true) == 0)
          return (LoanAssociateLog) milestoneFreeRole;
      }
      return (LoanAssociateLog) null;
    }

    public LoanAssociateProperty Property => this.property;
  }
}
