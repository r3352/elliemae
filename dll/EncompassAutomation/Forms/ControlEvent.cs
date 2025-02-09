// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.ControlEvent
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using mshtml;
using System;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  public class ControlEvent : IComparable
  {
    internal static ControlEvent Empty = new ControlEvent();
    private ControlEvents parentEvents;
    private IHTMLElement eventElement;

    internal ControlEvent(ControlEvents parentEvents, IHTMLElement eventElement)
    {
      this.parentEvents = parentEvents;
      this.eventElement = eventElement;
      if (this.parentEvents == null || this.eventElement == null)
        throw new ArgumentNullException();
    }

    private ControlEvent()
    {
    }

    public string ControlID
    {
      get
      {
        return this.eventElement == null ? string.Empty : this.eventElement.getAttribute("for", 1).ToString() + string.Empty;
      }
      set
      {
        this.parentEvents.ParentForm.EnsureEditing();
        try
        {
          this.eventElement.setAttribute("for", (object) value, 1);
        }
        catch (Exception ex)
        {
        }
      }
    }

    public string EventType
    {
      get
      {
        return this.eventElement == null ? string.Empty : this.eventElement.getAttribute("event", 1).ToString() + string.Empty;
      }
      set
      {
        this.parentEvents.ParentForm.EnsureEditing();
        try
        {
          this.eventElement.setAttribute("event", (object) value, 1);
        }
        catch (Exception ex)
        {
        }
      }
    }

    public ScriptLanguage Language => this.parentEvents.Language;

    public string EventCode
    {
      get
      {
        if (!(((IHTMLDOMNode) this.EventElement).firstChild is IHTMLCommentElement firstChild))
          return "";
        try
        {
          return firstChild.text.Substring(5 + Environment.NewLine.Length, firstChild.text.Length - 9 - 2 * Environment.NewLine.Length);
        }
        catch
        {
          return "";
        }
      }
      set
      {
        this.parentEvents.ParentForm.EnsureEditing();
        IHTMLDOMNode ihtmldomNode = ((IHTMLDOMNode) this.EventElement).firstChild;
        if (ihtmldomNode == null)
        {
          ihtmldomNode = ((DispHTMLDocument) this.eventElement.document).createComment("");
          ((IHTMLDOMNode) this.EventElement).appendChild(ihtmldomNode);
        }
        ((IHTMLCommentElement) ihtmldomNode).text = "<!-- " + Environment.NewLine + value + Environment.NewLine + " -->";
      }
    }

    public void Delete()
    {
      this.parentEvents.ParentForm.EnsureEditing();
      this.parentEvents.Remove(this.ControlID, this.EventType);
    }

    public override bool Equals(object obj)
    {
      ControlEvent controlEvent = obj as ControlEvent;
      return !object.Equals(obj, (object) null) && this.ControlID == controlEvent.ControlID && this.EventType == controlEvent.EventType;
    }

    internal IHTMLElement EventElement => this.eventElement;

    public override int GetHashCode()
    {
      return this.ControlID.GetHashCode() ^ this.EventType.GetHashCode();
    }

    public override string ToString() => this.EventCode == "" ? "" : "(Click to Edit)";

    public int CompareTo(object obj)
    {
      if (!(obj is ControlEvent controlEvent))
        throw new InvalidOperationException("Cannot sort against specified type");
      return controlEvent.ControlID != this.ControlID ? string.Compare(this.ControlID, controlEvent.ControlID, true) : string.Compare(this.EventType, controlEvent.EventType, true);
    }
  }
}
