// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.PrintGroup
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
  public class PrintGroup : IEnumerable<string>, IEnumerable
  {
    public readonly string GroupName;
    public readonly EncompassEdition Edition;
    private List<string> formIds = new List<string>();
    public readonly Dictionary<string, ArchivedFormDetails> archivedForms = new Dictionary<string, ArchivedFormDetails>();

    public PrintGroup(XmlElement e)
    {
      this.GroupName = e.GetAttribute("name");
      foreach (XmlElement selectNode1 in e.SelectNodes("Form"))
      {
        this.formIds.Add(selectNode1.GetAttribute("name"));
        if (this.GroupName == "Archived Forms")
        {
          ArchivedFormDetails archivedFormDetails = new ArchivedFormDetails();
          List<string> stringList = new List<string>();
          foreach (XmlElement selectNode2 in selectNode1.SelectNodes("Replacement"))
            stringList.Add(selectNode2.GetAttribute("name"));
          archivedFormDetails.ReplacementFormNames = stringList;
          archivedFormDetails.SuppressArchivedPrompt = selectNode1.GetAttribute("suppressArchivedPrompt") == "Y";
          this.archivedForms[selectNode1.GetAttribute("name")] = archivedFormDetails;
        }
      }
      string attribute = e.GetAttribute("edition");
      if (!string.IsNullOrEmpty(attribute))
        this.Edition = (EncompassEdition) Enum.Parse(typeof (EncompassEdition), attribute, true);
      else
        this.Edition = EncompassEdition.None;
    }

    public int Count => this.formIds.Count;

    public string this[int index] => this.formIds[index];

    public bool AppliesToEdition(EncompassEdition edition)
    {
      return this.Edition == EncompassEdition.None || edition == this.Edition;
    }

    public IEnumerator<string> GetEnumerator()
    {
      return (IEnumerator<string>) this.formIds.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.formIds.GetEnumerator();
  }
}
