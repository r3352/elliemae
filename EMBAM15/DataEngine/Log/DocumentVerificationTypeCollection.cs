// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.DocumentVerificationTypeCollection
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System.Collections;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class DocumentVerificationTypeCollection : CollectionBase
  {
    private DocumentLog doc;

    public DocumentVerificationTypeCollection(DocumentLog doc) => this.doc = doc;

    public DocumentVerificationTypeCollection(
      DocumentLog doc,
      XmlElement parentElement,
      string groupName)
    {
      this.doc = doc;
      XmlElement e = (XmlElement) parentElement.SelectSingleNode(groupName);
      if (e == null)
        return;
      AttributeReader attributeReader = new AttributeReader(e);
      foreach (XmlElement selectNode in e.SelectNodes("Category"))
      {
        switch (selectNode.GetAttribute("Category").ToString())
        {
          case "Asset":
            this.List.Add((object) new DocumentVerificationAsset(selectNode));
            continue;
          case "Income":
            this.List.Add((object) new DocumentVerificationIncome(selectNode));
            continue;
          case "Obligation":
            this.List.Add((object) new DocumentVerificationObligation(selectNode));
            continue;
          case "Employment":
            this.List.Add((object) new DocumentVerificationEmployment(selectNode));
            continue;
          default:
            continue;
        }
      }
    }

    public DocumentVerificationType this[int index] => (DocumentVerificationType) this.List[index];

    public DocumentVerificationType this[string id]
    {
      get
      {
        foreach (DocumentVerificationType verificationType in (IEnumerable) this.List)
        {
          if (verificationType.Guid == id)
            return verificationType;
        }
        return (DocumentVerificationType) null;
      }
    }

    public void Add(DocumentVerificationType verificationItem)
    {
      if (this[verificationItem.Guid] == null)
      {
        this.List.Add((object) verificationItem);
        this.doc.TrackChange("Verification added - " + verificationItem.Guid);
      }
    }

    public bool Delete(string guid)
    {
      DocumentVerificationType verificationType = this[guid];
      if (verificationType == null)
        return false;
      this.List.Remove((object) verificationType);
      this.doc.TrackChange("Verification removed - " + guid);
      return true;
    }

    public bool Delete(DocumentVerificationType item)
    {
      if (!this.List.Contains((object) item))
        return false;
      this.List.Remove((object) item);
      this.doc.TrackChange("Verification removed - " + item.Guid);
      return true;
    }

    public DocumentVerificationType[] Get(VerificationTimelineType type)
    {
      System.Collections.Generic.List<DocumentVerificationType> verificationTypeList = new System.Collections.Generic.List<DocumentVerificationType>();
      foreach (DocumentVerificationType verificationType in (IEnumerable) this.List)
      {
        if (verificationType.VerificationType.Equals((object) type))
          verificationTypeList.Add(verificationType);
      }
      return verificationTypeList.ToArray();
    }

    public void ToXml(XmlElement e, string groupName)
    {
      if (this.List.Count == 0)
        return;
      XmlElement element1 = e.OwnerDocument.CreateElement(groupName);
      e.AppendChild((XmlNode) element1);
      foreach (DocumentVerificationType verificationType in (IEnumerable) this.List)
      {
        XmlElement element2 = e.OwnerDocument.CreateElement("Category");
        element1.AppendChild((XmlNode) element2);
        verificationType.ToXml(element2);
      }
    }
  }
}
