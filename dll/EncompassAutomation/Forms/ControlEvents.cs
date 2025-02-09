// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.ControlEvents
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using mshtml;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  public class ControlEvents : IEnumerable
  {
    private Form parentForm;
    private IHTMLElement eventsElement;

    internal ControlEvents(Form parentForm)
    {
      this.parentForm = parentForm;
      this.loadEMEventList();
      this.parentForm.ControlIDChanged += new ControlIDChangedEventHandler(this.onControlIDChanged);
    }

    private void loadEMEventList()
    {
      HTMLDocument htmlDocument = this.parentForm.GetHTMLDocument();
      IHTMLElementCollection elementsByTagName1 = ((DispHTMLDocument) htmlDocument).getElementsByTagName("DIV");
      this.eventsElement = ((DispHTMLDocument) htmlDocument).createElement("EMEVENTS");
      ((IHTMLDOMNode) this.parentForm.HTMLElement).appendChild(this.eventsElement as IHTMLDOMNode);
      foreach (IHTMLElement ihtmlElement in elementsByTagName1)
      {
        if (string.Concat(ihtmlElement.getAttribute("eventId", 1)) != "")
          ((IHTMLDOMNode) this.eventsElement).appendChild(ihtmlElement as IHTMLDOMNode);
      }
      IHTMLElementCollection elementsByTagName2 = ((DispHTMLDocument) htmlDocument).getElementsByTagName("EMEVENTS");
      string str = string.Empty;
      foreach (IHTMLElement ihtmlElement in elementsByTagName2)
      {
        if (!ihtmlElement.Equals((object) this.eventsElement))
        {
          try
          {
            if (str == string.Empty)
              str = ihtmlElement.getAttribute("language", 2).ToString() + string.Empty;
          }
          catch (Exception ex)
          {
          }
          ((IHTMLDOMNode) ihtmlElement).removeNode(true);
        }
      }
      if (str == string.Empty)
        str = "VB.NET";
      this.eventsElement.setAttribute("language", (object) str, 2);
      foreach (IHTMLDOMNode ihtmldomNode in ((DispHTMLDocument) htmlDocument).getElementsByTagName("/EMEVENTS"))
        ihtmldomNode.removeNode(true);
    }

    public ControlEvent GetEvent(string controlId, string eventType)
    {
      IHTMLElementCollection children = (IHTMLElementCollection) this.eventsElement.children;
      for (int index = 0; index < children.length; ++index)
      {
        ControlEvent controlEvent = new ControlEvent(this, (IHTMLElement) children.item((object) index, (object) null));
        if (controlEvent.ControlID == controlId && controlEvent.EventType == eventType)
          return controlEvent;
      }
      return (ControlEvent) null;
    }

    public ScriptLanguage Language
    {
      get
      {
        return string.Concat(this.eventsElement.getAttribute("language", 2)) == "C#" ? ScriptLanguage.CSharp : ScriptLanguage.VB;
      }
      set
      {
        this.ParentForm.EnsureEditing();
        if (value == ScriptLanguage.CSharp)
        {
          this.eventsElement.setAttribute("language", (object) "C#", 2);
        }
        else
        {
          if (value != ScriptLanguage.VB)
            throw new ArgumentException("Specified language is not valid in this context");
          this.eventsElement.setAttribute("language", (object) "VB", 2);
        }
      }
    }

    public int Count => ((IHTMLElementCollection) this.eventsElement.children).length;

    public ControlEvent CreateNew(string controlId, string eventType)
    {
      this.ParentForm.EnsureEditing();
      IHTMLElement element = ((DispHTMLDocument) this.parentForm.GetHTMLDocument()).createElement("DIV");
      element.style.display = "none";
      ((IHTMLDOMNode) this.eventsElement).appendChild(element as IHTMLDOMNode);
      element.setAttribute("eventId", (object) Guid.NewGuid().ToString(), 1);
      element.setAttribute("for", (object) controlId, 1);
      element.setAttribute("event", (object) eventType, 1);
      this.parentForm.NotifyPropertyChange();
      return new ControlEvent(this, element);
    }

    public void Remove(string controlId, string eventType)
    {
      this.ParentForm.EnsureEditing();
      ControlEvent controlEvent = this.GetEvent(controlId, eventType);
      if (controlEvent == null)
        return;
      ((IHTMLDOMNode) this.eventsElement).removeChild((IHTMLDOMNode) controlEvent.EventElement);
      this.parentForm.NotifyPropertyChange();
    }

    public void Clear()
    {
      ArrayList arrayList = new ArrayList();
      foreach (ControlEvent controlEvent in (IEnumerable) this)
        arrayList.Add((object) controlEvent.EventElement);
      foreach (IHTMLDOMNode ihtmldomNode in arrayList)
        ((IHTMLDOMNode) this.eventsElement).removeChild(ihtmldomNode);
      this.parentForm.NotifyPropertyChange();
    }

    internal Form ParentForm => this.parentForm;

    internal IHTMLElement ControlEventsElement => this.eventsElement;

    private void onControlIDChanged(object source, ControlIDChangedEventArgs e)
    {
      foreach (ControlEvent controlEvent in (IEnumerable) this)
      {
        if (controlEvent.ControlID == e.PriorControlID)
          controlEvent.ControlID = e.Control.ControlID;
      }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) new ControlEvents.EventEnumerator(this);
    }

    private class EventEnumerator : IEnumerator
    {
      private ControlEvents groups;
      private IHTMLDOMNode groupNode;
      private IHTMLDOMNode currentNode;

      public EventEnumerator(ControlEvents groups)
      {
        this.groups = groups;
        this.groupNode = groups.ControlEventsElement as IHTMLDOMNode;
      }

      public void Reset() => this.currentNode = (IHTMLDOMNode) null;

      public object Current
      {
        get
        {
          return this.currentNode == null ? (object) null : (object) new ControlEvent(this.groups, this.currentNode as IHTMLElement);
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
