// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.HtmlEmail.HtmlEmailTemplateCollection
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.HtmlEmail;
using EllieMae.EMLite.Serialization;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.HtmlEmail
{
  internal class HtmlEmailTemplateCollection : CollectionBase, IXmlSerializable
  {
    public HtmlEmailTemplateCollection()
    {
    }

    public HtmlEmailTemplateCollection(XmlSerializationInfo info)
    {
      foreach (string name in info)
        this.Add((HtmlEmailTemplate) info.GetValue(name, typeof (HtmlEmailTemplate)));
    }

    public int Add(HtmlEmailTemplate template)
    {
      int index = this.List.IndexOf((object) template);
      if (index < 0)
        index = this.List.Add((object) template);
      else
        this.List[index] = (object) template;
      return index;
    }

    public HtmlEmailTemplate[] GetByType(HtmlEmailTemplateType type)
    {
      System.Collections.Generic.List<HtmlEmailTemplate> htmlEmailTemplateList = new System.Collections.Generic.List<HtmlEmailTemplate>();
      foreach (HtmlEmailTemplate htmlEmailTemplate in (IEnumerable) this.List)
      {
        if ((htmlEmailTemplate.Type & type) != HtmlEmailTemplateType.Unknown)
          htmlEmailTemplateList.Add(htmlEmailTemplate);
      }
      return htmlEmailTemplateList.ToArray();
    }

    public HtmlEmailTemplate this[int index] => this.GetByIndex(index);

    public HtmlEmailTemplate this[string guid] => this.GetByID(guid);

    public HtmlEmailTemplate GetByIndex(int index) => (HtmlEmailTemplate) this.List[index];

    public HtmlEmailTemplate GetByID(string guid)
    {
      foreach (HtmlEmailTemplate byId in (IEnumerable) this.List)
      {
        if (byId.Guid == guid)
          return byId;
      }
      return (HtmlEmailTemplate) null;
    }

    public bool Contains(HtmlEmailTemplate template) => this.List.Contains((object) template);

    public void Remove(HtmlEmailTemplate template) => this.List.Remove((object) template);

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      for (int index = 0; index < this.Count; ++index)
        info.AddValue(index.ToString(), (object) this[index]);
    }
  }
}
