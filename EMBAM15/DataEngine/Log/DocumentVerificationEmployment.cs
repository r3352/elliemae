// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.DocumentVerificationEmployment
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class DocumentVerificationEmployment : DocumentVerificationType
  {
    private EmploymentType employmentType;

    public DocumentVerificationEmployment(LoanBorrowerType borrowerType)
      : base(VerificationTimelineType.Employment, borrowerType)
    {
    }

    public DocumentVerificationEmployment(XmlElement e)
      : base(e)
    {
      this.employmentType = Helper.GetEmploymentType(new AttributeReader(e).GetString(nameof (EmploymentType)));
    }

    public EmploymentType EmploymentType
    {
      set => this.employmentType = value;
      get => this.employmentType;
    }

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      new AttributeWriter(e).Write("EmploymentType", (object) this.employmentType);
    }
  }
}
