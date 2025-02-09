// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.ControlEvents
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
  /// Represents the collection of control event handlers defined on the Form.
  /// </summary>
  /// <remarks>This class is used by the Encompass Form Builder and intended for use
  /// at design time only. Attempts to modify this object properties at runtime will result in
  /// exceptions or unexpected behavior.</remarks>
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
      IHTMLElementCollection elementsByTagName1 = htmlDocument.getElementsByTagName("DIV");
      this.eventsElement = htmlDocument.createElement("EMEVENTS");
      ((IHTMLDOMNode) this.parentForm.HTMLElement).appendChild(this.eventsElement as IHTMLDOMNode);
      foreach (IHTMLElement newChild in elementsByTagName1)
      {
        if (string.Concat(newChild.getAttribute("eventId", 1)) != "")
          ((IHTMLDOMNode) this.eventsElement).appendChild(newChild as IHTMLDOMNode);
      }
      IHTMLElementCollection elementsByTagName2 = htmlDocument.getElementsByTagName("EMEVENTS");
      string AttributeValue = string.Empty;
      foreach (IHTMLElement htmlElement in elementsByTagName2)
      {
        if (!htmlElement.Equals((object) this.eventsElement))
        {
          try
          {
            if (AttributeValue == string.Empty)
              AttributeValue = htmlElement.getAttribute("language", 2).ToString() + string.Empty;
          }
          catch (Exception ex)
          {
          }
          ((IHTMLDOMNode) htmlElement).removeNode(true);
        }
      }
      if (AttributeValue == string.Empty)
        AttributeValue = "VB.NET";
      this.eventsElement.setAttribute("language", (object) AttributeValue, 2);
      foreach (IHTMLDOMNode htmldomNode in htmlDocument.getElementsByTagName("/EMEVENTS"))
        htmldomNode.removeNode(true);
    }

    /// <summary>
    /// Retrieves the event code for a specific control and event type.
    /// </summary>
    /// <param name="controlId">The Control ID of the owner control.</param>
    /// <param name="eventType">The name of the event.</param>
    /// <returns>Returns the code for the event.</returns>
    public ControlEvent GetEvent(string controlId, string eventType)
    {
      IHTMLElementCollection children = (IHTMLElementCollection) this.eventsElement.children;
      for (int name = 0; name < children.length; ++name)
      {
        ControlEvent controlEvent = new ControlEvent(this, (IHTMLElement) children.item((object) name));
        if (controlEvent.ControlID == controlId && controlEvent.EventType == eventType)
          return controlEvent;
      }
      return (ControlEvent) null;
    }

    /// <summary>
    /// Gets or sets the <see cref="T:EllieMae.Encompass.Forms.ScriptLanguage" /> used for the event handlers defined in the form.
    /// </summary>
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

    /// <summary>
    /// Gets the number of control event handlers in the collection.
    /// </summary>
    public int Count => ((IHTMLElementCollection) this.eventsElement.children).length;

    /// <summary>
    /// Creates a new <see cref="T:EllieMae.Encompass.Forms.ControlEvent" /> for the specified control and event type.
    /// </summary>
    /// <param name="controlId">The ControlID for the control on which the event will occur.</param>
    /// <param name="eventType">The name of the event for which the handler is specified.</param>
    /// <returns>Returns a new <see cref="T:EllieMae.Encompass.Forms.ControlEvent" /> for the control and event type.</returns>
    public ControlEvent CreateNew(string controlId, string eventType)
    {
      this.ParentForm.EnsureEditing();
      IHTMLElement element = this.parentForm.GetHTMLDocument().createElement("DIV");
      element.style.display = "none";
      ((IHTMLDOMNode) this.eventsElement).appendChild(element as IHTMLDOMNode);
      element.setAttribute("eventId", (object) Guid.NewGuid().ToString());
      element.setAttribute("for", (object) controlId);
      element.setAttribute("event", (object) eventType);
      this.parentForm.NotifyPropertyChange();
      return new ControlEvent(this, element);
    }

    /// <summary>
    /// Removes an event handler definition from the collection.
    /// </summary>
    /// <param name="controlId">The ControlID of the target Control.</param>
    /// <param name="eventType">The name of the event for which the handler is triggered.</param>
    public void Remove(string controlId, string eventType)
    {
      this.ParentForm.EnsureEditing();
      ControlEvent controlEvent = this.GetEvent(controlId, eventType);
      if (controlEvent == null)
        return;
      ((IHTMLDOMNode) this.eventsElement).removeChild((IHTMLDOMNode) controlEvent.EventElement);
      this.parentForm.NotifyPropertyChange();
    }

    /// <summary>Removes all event definitions.</summary>
    public void Clear()
    {
      ArrayList arrayList = new ArrayList();
      foreach (ControlEvent controlEvent in (IEnumerable) this)
        arrayList.Add((object) controlEvent.EventElement);
      foreach (IHTMLDOMNode oldChild in arrayList)
        ((IHTMLDOMNode) this.eventsElement).removeChild(oldChild);
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

    /// <summary>Provides an enumerator for the collection.</summary>
    /// <returns></returns>
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
