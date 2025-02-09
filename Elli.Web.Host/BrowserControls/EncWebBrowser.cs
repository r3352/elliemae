// Decompiled with JetBrains decompiler
// Type: Elli.Web.Host.BrowserControls.EncWebBrowser
// Assembly: Elli.Web.Host, Version=19.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9006AF5-0CBA-41F3-A51F-BE05E37C7972
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Web.Host.dll

using EllieMae.EMLite.Common;
using mshtml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#nullable disable
namespace Elli.Web.Host.BrowserControls
{
  public class EncWebBrowser : EncWebFormBrowserControl
  {
    private const string className = "EncWebBrowser";
    private static readonly string sw = Tracing.SwThinThick;
    private IContainer components;
    private WebBrowser webBrowser;

    public string BrowserName { get; set; }

    public EncWebBrowser()
    {
      this.InitializeComponent();
      this.SetBrowserProperties();
      this.registerWebBrowserEvents();
    }

    private void registerWebBrowserEvents()
    {
      if (this.webBrowser == null)
        return;
      this.webBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(this.WebBrowser_DocumentCompleted);
      this.webBrowser.Navigating += new WebBrowserNavigatingEventHandler(this.WebBrowser_Navigating);
    }

    private void registerWebBrowserDocumentEvents()
    {
      if (!(this.webBrowser.Document != (HtmlDocument) null))
        return;
      this.webBrowser.Document.MouseUp += new HtmlElementEventHandler(this.Document_MouseUp);
    }

    public void SetBrowserProperties()
    {
      if (this.webBrowser == null)
        return;
      this.webBrowser.Name = this.BrowserName;
      this.webBrowser.ScriptErrorsSuppressed = true;
    }

    public override string GetBrowserHtml([Optional] object frameName)
    {
      return this.webBrowser.Document != (HtmlDocument) null ? this.webBrowser.DocumentText : (string) null;
    }

    public override void Navigate(string url) => this.webBrowser.Navigate(url);

    public override void LoadHtml(string html) => this.webBrowser.DocumentText = html;

    public override void LoadHtml(string html, bool documentReadonly)
    {
      if (this.webBrowser.Document == (HtmlDocument) null)
        this.webBrowser.Navigate("about:blank");
      this.webBrowser.Document.OpenNew(false);
      if (!string.IsNullOrEmpty(html))
        this.webBrowser.Document.Write(html);
      if (!(this.webBrowser.Document.DomDocument is mshtml.IHTMLDocument2 domDocument))
        return;
      string str = documentReadonly ? "Off" : "On";
      domDocument.designMode = str;
    }

    private void WebBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
    {
      Tracing.Log(EncWebBrowser.sw, TraceLevel.Verbose, nameof (EncWebBrowser), "Browser: Navigating: " + e.Url.ToString() + " [" + e.TargetFrameName + "]");
      BeforeNavigationEventArgs args = BeforeNavigationEventArgs.GetArgs(e);
      this.OnBeforeNavigation(sender, args);
      e.Cancel = args.Cancel;
    }

    private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
    {
      Tracing.Log(EncWebBrowser.sw, TraceLevel.Verbose, nameof (EncWebBrowser), "Browser: DocumentCompleted: " + (object) e.Url);
      this.registerWebBrowserDocumentEvents();
      this.OnPageComplete(sender, new FinishedLoadingEventArgs((object) null, e.Url.ToString()));
    }

    private void Document_MouseUp(object sender, HtmlElementEventArgs e)
    {
      Tracing.Log(EncWebBrowser.sw, TraceLevel.Verbose, nameof (EncWebBrowser), "Browser: MouseUp: " + (object) e.KeyPressedCode + " [" + e.ReturnValue.ToString() + "]");
      this.OnMouseReleased((object) this, new MouseReleasedEventArgs(false));
    }

    public override bool ExecuteCommand(string command, object value = null)
    {
      if (this.webBrowser.Document == (HtmlDocument) null)
      {
        Tracing.Log(EncWebBrowser.sw, TraceLevel.Error, nameof (EncWebBrowser), "ExecuteCommand: Unable to locate document");
        return false;
      }
      if (!(this.webBrowser.Document.DomDocument is mshtml.IHTMLDocument2 domDocument))
        return false;
      if (!command.Equals("inserthtml", StringComparison.OrdinalIgnoreCase))
        return domDocument.execCommand(command, value: value);
      IHTMLTxtRange range = (IHTMLTxtRange) domDocument.selection.createRange();
      range.pasteHTML(value.ToString());
      range.collapse(false);
      range.select();
      return true;
    }

    public override bool IsQueryCommandEnabled(string command)
    {
      if (this.webBrowser.Document == (HtmlDocument) null)
      {
        Tracing.Log(EncWebBrowser.sw, TraceLevel.Error, nameof (EncWebBrowser), "IsQueryCommandEnabled: Unable to locate document");
        return false;
      }
      return this.webBrowser.Document.DomDocument is mshtml.IHTMLDocument2 domDocument && domDocument.queryCommandEnabled(command);
    }

    public override string GetQueryCommandValue(string command)
    {
      if (this.webBrowser.Document == (HtmlDocument) null)
      {
        Tracing.Log(EncWebBrowser.sw, TraceLevel.Error, nameof (EncWebBrowser), "GetQueryCommandValue: Unable to locate document");
        return (string) null;
      }
      return !(this.webBrowser.Document.DomDocument is mshtml.IHTMLDocument2 domDocument) ? (string) null : Convert.ToString(domDocument.queryCommandValue(command));
    }

    public override void AddDomEvents(string eventName, object caller)
    {
      if (this.webBrowser.Document == (HtmlDocument) null)
      {
        Tracing.Log(EncWebBrowser.sw, TraceLevel.Error, nameof (EncWebBrowser), "AddDomEvents: Unable to locate document");
      }
      else
      {
        mshtml.IHTMLDocument2 domDocument1 = this.webBrowser.Document.DomDocument as mshtml.IHTMLDocument2;
        mshtml.IHTMLDocument4 domDocument2 = this.webBrowser.Document.DomDocument as mshtml.IHTMLDocument4;
        if (domDocument1 == null || domDocument2 == null)
          return;
        switch (eventName.ToString())
        {
          case "readystatechange":
            domDocument1.onreadystatechange = caller;
            break;
          case "keydown":
            domDocument1.onkeydown = caller;
            break;
          case "selectionchange":
            domDocument2.onselectionchange = caller;
            break;
          case "keypress":
            domDocument1.onkeypress = caller;
            break;
          case "drop":
            if (!(domDocument1.body is mshtml.IHTMLElement2 body))
              break;
            body.ondrop = caller;
            break;
        }
      }
    }

    public override string GetHtmlBodyText([Optional] object frameName)
    {
      return this.webBrowser.Document != (HtmlDocument) null ? this.webBrowser.Document.Body.InnerText : (string) null;
    }

    public override void InvokeHtmlEvent()
    {
      mshtml.IHTMLEventObj htmlEventObj = (this.webBrowser.Document.DomDocument as mshtml.IHTMLDocument2).parentWindow.@event;
      if (htmlEventObj == null)
        return;
      switch (htmlEventObj.type)
      {
        case "selectionchange":
          this.OnProcessSelectionChange((object) this, EventArgs.Empty);
          break;
        case "keydown":
          HtmlEditorKeyDownEventArgs e = new HtmlEditorKeyDownEventArgs()
          {
            KeyCode = htmlEventObj.keyCode
          };
          this.OnProcessKeyDown((object) this, e);
          if (e.ReturnValue)
            break;
          htmlEventObj.returnValue = (object) false;
          break;
        case "readystatechange":
          this.OnProcessReadyStateChange((object) this, EventArgs.Empty);
          break;
        default:
          this.OnContentChanged((object) this, EventArgs.Empty);
          break;
      }
    }

    public override Color GetSelectedFontColor()
    {
      byte[] bytes = BitConverter.GetBytes(Convert.ToInt32(this.GetQueryCommandValue("ForeColor")));
      return Color.FromArgb(BitConverter.ToInt32(new byte[4]
      {
        bytes[2],
        bytes[1],
        bytes[0],
        bytes[3]
      }, 0));
    }

    public override void InsertField(string fieldID, string fieldName)
    {
      if (this.webBrowser.Document == (HtmlDocument) null)
      {
        Tracing.Log(EncWebBrowser.sw, TraceLevel.Error, nameof (EncWebBrowser), "InsertField: Unable to locate document");
      }
      else
      {
        if (!(this.webBrowser.Document.DomDocument is mshtml.IHTMLDocument2 domDocument))
          return;
        mshtml.IHTMLElement element = domDocument.createElement("label");
        element.style.backgroundColor = (object) ColorTranslator.ToHtml(Color.Gainsboro);
        element.setAttribute("emid", (object) fieldID, 0);
        if (fieldName != null)
          element.innerText = "<<" + fieldID + " " + fieldName + ">>";
        else
          element.innerText = "<<" + fieldID + ">>";
        ((mshtml.IHTMLElement3) element).contentEditable = "false";
        if (domDocument.selection.type != "Control")
        {
          IHTMLTxtRange range = (IHTMLTxtRange) domDocument.selection.createRange();
          range.pasteHTML(element.outerHTML);
          range.collapse(false);
          range.select();
        }
        else
        {
          mshtml.IHTMLElement htmlElement = ((IHTMLControlRange) domDocument.selection.createRange()).commonParentElement();
          if (htmlElement == null || htmlElement == domDocument.body)
            return;
          htmlElement.outerHTML = element.outerHTML;
        }
      }
    }

    public override void ShowContextMenu()
    {
      this.webBrowser.IsWebBrowserContextMenuEnabled = false;
      this.webBrowser.ContextMenuStrip = this.ContextMenuStrip;
    }

    public override bool IsDocumentExists => this.webBrowser.Document != (HtmlDocument) null;

    public override void SetOpaqueBackground()
    {
    }

    public override void Navigate(
      string url,
      string postData,
      Dictionary<string, string> headerCollection)
    {
      throw new NotImplementedException();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.webBrowser = new WebBrowser();
      this.SuspendLayout();
      this.webBrowser.Dock = DockStyle.Fill;
      this.webBrowser.Location = new Point(0, 0);
      this.webBrowser.MinimumSize = new Size(20, 20);
      this.webBrowser.ScrollBarsEnabled = true;
      this.webBrowser.Name = nameof (EncWebBrowser);
      this.webBrowser.Size = new Size(965, 526);
      this.webBrowser.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(9f, 20f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoSize = true;
      this.Controls.Add((Control) this.webBrowser);
      this.Name = nameof (EncWebBrowser);
      this.Size = new Size(965, 526);
      this.ResumeLayout(false);
    }
  }
}
