// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.RolodexGroups
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using mshtml;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>
  /// Represents a Form's collection of <see cref="T:EllieMae.Encompass.Forms.RolodexGroup" /> items.
  /// </summary>
  internal class RolodexGroups : IEnumerable
  {
    private Form parentForm;
    private IHTMLElement groupElement;

    internal RolodexGroups(Form parentForm)
    {
      this.parentForm = parentForm;
      HTMLDocument htmlDocument = parentForm.GetHTMLDocument();
      IHTMLElementCollection elementsByTagName1 = htmlDocument.getElementsByTagName("EMRXGRP");
      this.groupElement = htmlDocument.createElement("EMRXGRPS");
      ((IHTMLDOMNode) parentForm.HTMLElement).appendChild(this.groupElement as IHTMLDOMNode);
      foreach (IHTMLElement newChild in elementsByTagName1)
        ((IHTMLDOMNode) this.groupElement).appendChild(newChild as IHTMLDOMNode);
      foreach (IHTMLElement htmlElement in htmlDocument.getElementsByTagName("EMRXGRPS"))
      {
        if (!htmlElement.Equals((object) this.groupElement))
          ((IHTMLDOMNode) htmlElement).removeNode(true);
      }
      IHTMLElementCollection elementsByTagName2 = htmlDocument.getElementsByTagName("/EMRXGRP");
      IHTMLElementCollection elementsByTagName3 = htmlDocument.getElementsByTagName("/EMRXGRPS");
      foreach (IHTMLDOMNode htmldomNode in elementsByTagName2)
        htmldomNode.removeNode(true);
      foreach (IHTMLDOMNode htmldomNode in elementsByTagName3)
        htmldomNode.removeNode(true);
    }

    public RolodexGroup this[string groupId]
    {
      get
      {
        if ((groupId ?? "") == "")
          return (RolodexGroup) null;
        IHTMLElement elementWithAttribute = HTMLHelper.FindElementWithAttribute(this.groupElement, "id", groupId);
        return elementWithAttribute == null ? (RolodexGroup) null : new RolodexGroup(this, elementWithAttribute);
      }
    }

    public RolodexGroup GetGroupByName(string name)
    {
      foreach (RolodexGroup groupByName in (IEnumerable) this)
      {
        if (string.Compare(groupByName.Name, name, StringComparison.OrdinalIgnoreCase) == 0)
          return groupByName;
      }
      return (RolodexGroup) null;
    }

    public int Count => ((IHTMLElementCollection) this.groupElement.children).length;

    public RolodexGroup CreateNew(string groupName)
    {
      IHTMLElement element = this.parentForm.GetHTMLDocument().createElement("EMRXGRP");
      ((IHTMLDOMNode) this.groupElement).appendChild(element as IHTMLDOMNode);
      element.setAttribute("id", (object) Guid.NewGuid().ToString());
      element.setAttribute(nameof (groupName), (object) groupName);
      this.parentForm.NotifyPropertyChange();
      return new RolodexGroup(this, element);
    }

    public void Remove(string groupId)
    {
      try
      {
        ((IHTMLDOMNode) this.groupElement).removeChild(this[groupId].GroupElement as IHTMLDOMNode);
        this.parentForm.NotifyPropertyChange();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
    }

    internal IHTMLElement RolodexGroupElement => this.groupElement;

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) new RolodexGroups.GroupEnumerator(this);
    }

    private class GroupEnumerator : IEnumerator
    {
      private RolodexGroups groups;
      private IHTMLDOMNode groupNode;
      private IHTMLDOMNode currentNode;

      public GroupEnumerator(RolodexGroups groups)
      {
        this.groups = groups;
        this.groupNode = groups.RolodexGroupElement as IHTMLDOMNode;
      }

      public void Reset() => this.currentNode = (IHTMLDOMNode) null;

      public object Current
      {
        get
        {
          return this.currentNode == null ? (object) null : (object) new RolodexGroup(this.groups, this.currentNode as IHTMLElement);
        }
      }

      public bool MoveNext()
      {
        this.currentNode = this.currentNode != null ? this.currentNode.nextSibling : this.groupNode.firstChild;
        return this.currentNode != null;
      }
    }
  }
}
