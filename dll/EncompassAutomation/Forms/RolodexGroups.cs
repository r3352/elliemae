// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.RolodexGroups
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using mshtml;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  internal class RolodexGroups : IEnumerable
  {
    private Form parentForm;
    private IHTMLElement groupElement;

    internal RolodexGroups(Form parentForm)
    {
      this.parentForm = parentForm;
      HTMLDocument htmlDocument = parentForm.GetHTMLDocument();
      IHTMLElementCollection elementsByTagName1 = ((DispHTMLDocument) htmlDocument).getElementsByTagName("EMRXGRP");
      this.groupElement = ((DispHTMLDocument) htmlDocument).createElement("EMRXGRPS");
      ((IHTMLDOMNode) parentForm.HTMLElement).appendChild(this.groupElement as IHTMLDOMNode);
      foreach (IHTMLElement ihtmlElement in elementsByTagName1)
        ((IHTMLDOMNode) this.groupElement).appendChild(ihtmlElement as IHTMLDOMNode);
      foreach (IHTMLElement ihtmlElement in ((DispHTMLDocument) htmlDocument).getElementsByTagName("EMRXGRPS"))
      {
        if (!ihtmlElement.Equals((object) this.groupElement))
          ((IHTMLDOMNode) ihtmlElement).removeNode(true);
      }
      IHTMLElementCollection elementsByTagName2 = ((DispHTMLDocument) htmlDocument).getElementsByTagName("/EMRXGRP");
      IHTMLElementCollection elementsByTagName3 = ((DispHTMLDocument) htmlDocument).getElementsByTagName("/EMRXGRPS");
      foreach (IHTMLDOMNode ihtmldomNode in elementsByTagName2)
        ihtmldomNode.removeNode(true);
      foreach (IHTMLDOMNode ihtmldomNode in elementsByTagName3)
        ihtmldomNode.removeNode(true);
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
      IHTMLElement element = ((DispHTMLDocument) this.parentForm.GetHTMLDocument()).createElement("EMRXGRP");
      ((IHTMLDOMNode) this.groupElement).appendChild(element as IHTMLDOMNode);
      element.setAttribute("id", (object) Guid.NewGuid().ToString(), 1);
      element.setAttribute(nameof (groupName), (object) groupName, 1);
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
