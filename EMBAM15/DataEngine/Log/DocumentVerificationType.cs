// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.DocumentVerificationType
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public abstract class DocumentVerificationType
  {
    private string guid;
    private VerificationTimelineType timelineType;
    private DateTime createdDate = DateTime.MinValue;
    private LoanBorrowerType borrowerType;
    private string howVerified;

    protected DocumentVerificationType(
      VerificationTimelineType timelineType,
      LoanBorrowerType borrowerType)
    {
      this.guid = System.Guid.NewGuid().ToString();
      this.createdDate = DateTime.Now;
      this.timelineType = timelineType;
      this.borrowerType = borrowerType;
    }

    protected DocumentVerificationType(XmlElement e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.guid = attributeReader.GetString(nameof (Guid), System.Guid.NewGuid().ToString());
      this.createdDate = attributeReader.GetDate(nameof (CreatedDate));
      this.borrowerType = Helper.GetLoanBorrowerType(attributeReader.GetString(nameof (BorrowerType)));
      this.timelineType = Helper.GetVerificationTimelineType(attributeReader.GetString("Category"));
      this.howVerified = attributeReader.GetString(nameof (HowVerified));
    }

    public string Guid => this.guid;

    public DateTime CreatedDate => this.createdDate;

    public LoanBorrowerType BorrowerType
    {
      get => this.borrowerType;
      set => this.borrowerType = value;
    }

    public VerificationTimelineType VerificationType
    {
      get => this.timelineType;
      set => this.timelineType = value;
    }

    public string HowVerified
    {
      get => this.howVerified;
      set => this.howVerified = value;
    }

    public virtual void ToXml(XmlElement e)
    {
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Guid", (object) this.guid);
      attributeWriter.Write("CreatedDate", (object) this.createdDate);
      attributeWriter.Write("BorrowerType", (object) this.borrowerType);
      attributeWriter.Write("Category", (object) this.timelineType);
      attributeWriter.Write("HowVerified", (object) this.howVerified);
    }
  }
}
