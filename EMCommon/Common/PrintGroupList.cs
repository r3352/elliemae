// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.PrintGroupList
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.Common.Licensing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class PrintGroupList : IEnumerable<PrintGroup>, IEnumerable
  {
    private List<PrintGroup> groupList = new List<PrintGroup>();
    private Dictionary<string, PrintGroup> nameMap = new Dictionary<string, PrintGroup>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);

    public PrintGroupList(XmlDocument xml, EncompassEdition edition)
    {
      foreach (XmlElement selectNode in xml.SelectNodes("/EMFormGroupList/FormGroup"))
      {
        PrintGroup printGroup = new PrintGroup(selectNode);
        if (edition == EncompassEdition.None || printGroup.AppliesToEdition(edition))
        {
          this.groupList.Add(printGroup);
          this.nameMap[printGroup.GroupName] = printGroup;
        }
      }
    }

    public PrintGroupList(XmlDocument xml)
      : this(xml, EncompassEdition.None)
    {
    }

    public PrintGroupList(string path)
      : this(path, EncompassEdition.None)
    {
    }

    public PrintGroupList(string path, EncompassEdition edition)
      : this(PrintGroupList.openXml(path), edition)
    {
    }

    public int Count => this.groupList.Count;

    public PrintGroup this[int index] => this.groupList[index];

    public PrintGroup GetGroupByName(string groupName)
    {
      PrintGroup groupByName = (PrintGroup) null;
      this.nameMap.TryGetValue(groupName, out groupByName);
      return groupByName;
    }

    private static XmlDocument openXml(string path)
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.Load(path);
      return xmlDocument;
    }

    public IEnumerator<PrintGroup> GetEnumerator()
    {
      return (IEnumerator<PrintGroup>) this.groupList.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.groupList.GetEnumerator();

    public static PrintGroupList Parse(string xml, EncompassEdition edition)
    {
      XmlDocument xml1 = new XmlDocument();
      xml1.XmlResolver = (XmlResolver) null;
      xml1.LoadXml(xml);
      return new PrintGroupList(xml1, edition);
    }

    public static PrintGroupList Parse(string xml)
    {
      return PrintGroupList.Parse(xml, EncompassEdition.None);
    }
  }
}
