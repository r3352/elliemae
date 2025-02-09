// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.CopyBuffer
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Drawing;
using System.Text.RegularExpressions;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  public class CopyBuffer
  {
    public static CopyBuffer Current;
    private ArrayList bufferItems = new ArrayList();

    internal CopyBuffer(Control[] controlsToCopy)
    {
      foreach (Control control in controlsToCopy)
      {
        if (control.AllowCutCopyDelete)
          this.bufferItems.Add((object) new CopyBuffer.CopyBufferItem(control.HTMLElement.outerHTML, control.AbsolutePosition));
      }
      Point point = Point.Empty;
      foreach (CopyBuffer.CopyBufferItem bufferItem in this.bufferItems)
        point = !(point == Point.Empty) ? new Point(Math.Min(point.X, bufferItem.Position.X), Math.Min(point.Y, bufferItem.Position.Y)) : bufferItem.Position;
      foreach (CopyBuffer.CopyBufferItem bufferItem in this.bufferItems)
        bufferItem.Position = new Point(bufferItem.Position.X - point.X, bufferItem.Position.Y - point.Y);
    }

    public void Paste(ContainerControl container)
    {
      this.Paste(container, container.GetContainerAbsolutePosition());
    }

    public void Paste(ContainerControl container, Point pastePosition)
    {
      Hashtable hashtable = new Hashtable();
      Regex regex = new Regex("<[^>]+\\sid=\"?([0-9a-z_]+)\"?[^>]*>", RegexOptions.IgnoreCase);
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      foreach (CopyBuffer.CopyBufferItem bufferItem in this.bufferItems)
      {
        string str1 = bufferItem.OuterHTML;
        Match match = regex.Match(str1);
        string key = (string) null;
        for (; match.Success; match = match.NextMatch())
        {
          string str2 = match.Groups[1].Value;
          if (container.Form.ControlExists(str2))
          {
            string str3 = container.Form.NewControlID(this.getControlType(match.Groups[0].Value), (IDictionary) insensitiveHashtable);
            str1 = this.replaceControlId(str1, str2, str3);
            str2 = str3;
            insensitiveHashtable[(object) str3] = (object) null;
          }
          if (key == null)
            key = str2;
        }
        container.ContainerElement.innerHTML += str1;
        container.NotifyPropertyChange();
        hashtable[(object) key] = (object) bufferItem;
      }
      container.Form.SelectedControls.Clear();
      foreach (DictionaryEntry dictionaryEntry in hashtable)
      {
        CopyBuffer.CopyBufferItem copyBufferItem = (CopyBuffer.CopyBufferItem) dictionaryEntry.Value;
        Control control = container.Controls.Find(dictionaryEntry.Key.ToString());
        if (control.AllowPositioning)
          control.AbsolutePosition = new Point(pastePosition.X + copyBufferItem.Position.X, pastePosition.Y + copyBufferItem.Position.Y);
        control.Select();
      }
    }

    private string replaceControlId(string html, string priorId, string newId)
    {
      Regex regex = new Regex("(<[^>]+\\s[a-zA-Z]*id)=\"?" + priorId + "\"?((?:\\s[^>]*)?>)", RegexOptions.IgnoreCase);
      while (regex.Match(html).Success)
        html = regex.Replace(html, "$1=\"" + newId + "\"$2");
      return html;
    }

    private string getControlType(string html)
    {
      return Regex.Split(html, "controlType=\"([a-zA-Z0-9_]*)\"")[1];
    }

    private class CopyBufferItem
    {
      public string OuterHTML;
      public Point Position;

      public CopyBufferItem(string outerHTML, Point position)
      {
        this.OuterHTML = outerHTML;
        this.Position = position;
      }
    }
  }
}
