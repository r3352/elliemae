// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.HTMLHelper
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using EllieMae.EMLite.Common;
using mshtml;
using System;
using System.Collections;
using System.Drawing;
using System.Globalization;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>Summary description for HTMLHelper.</summary>
  internal class HTMLHelper
  {
    public static IHTMLElement FindElementWithAttribute(
      IHTMLElement root,
      string attributeName,
      string attributeValue)
    {
      using (PerformanceMeter.Current.BeginOperation("HTMLHelper.FindElementWithAttribute"))
        return HTMLHelper.findElementWithAttribute(root, attributeName, attributeValue);
    }

    private static IHTMLElement findElementWithAttribute(
      IHTMLElement root,
      string attributeName,
      string attributeValue)
    {
      if (string.Concat(root.getAttribute(attributeName, 2)) == attributeValue)
        return root;
      IHTMLElementCollection children = (IHTMLElementCollection) root.children;
      for (int name = 0; name < children.length; ++name)
      {
        IHTMLElement elementWithAttribute = HTMLHelper.findElementWithAttribute((IHTMLElement) children.item((object) name), attributeName, attributeValue);
        if (elementWithAttribute != null)
          return elementWithAttribute;
      }
      return (IHTMLElement) null;
    }

    public static IHTMLElement FindElementWithTagName(IHTMLElement root, string tagName)
    {
      using (PerformanceMeter.Current.BeginOperation("HTMLHelper.FindElementWithTagName"))
        return HTMLHelper.findElementWithTagName(root, tagName);
    }

    private static IHTMLElement findElementWithTagName(IHTMLElement root, string tagName)
    {
      if (string.Compare(root.tagName, tagName, true) == 0)
        return root;
      IHTMLElementCollection children = (IHTMLElementCollection) root.children;
      for (int name = 0; name < children.length; ++name)
      {
        IHTMLElement elementWithTagName = HTMLHelper.findElementWithTagName((IHTMLElement) children.item((object) name), tagName);
        if (elementWithTagName != null)
          return elementWithTagName;
      }
      return (IHTMLElement) null;
    }

    public static IHTMLElement GetChildWithTagName(IHTMLElement root, string tagName)
    {
      ArrayList arrayList = new ArrayList();
      IHTMLElementCollection children = (IHTMLElementCollection) root.children;
      for (int name = 0; name < children.length; ++name)
      {
        IHTMLElement childWithTagName = (IHTMLElement) children.item((object) name);
        if (string.Compare(childWithTagName.tagName, tagName, true) == 0)
          return childWithTagName;
      }
      return (IHTMLElement) null;
    }

    public static ICollection GetChildrenWithTagName(IHTMLElement root, string tagName)
    {
      ArrayList childrenWithTagName = new ArrayList();
      IHTMLElementCollection children = (IHTMLElementCollection) root.children;
      for (int name = 0; name < children.length; ++name)
      {
        IHTMLElement htmlElement = (IHTMLElement) children.item((object) name);
        if (string.Compare(htmlElement.tagName, tagName, true) == 0)
          childrenWithTagName.Add((object) htmlElement);
      }
      return (ICollection) childrenWithTagName;
    }

    public static Color ColorFromStyle(string colorStr)
    {
      if (colorStr == "transparent")
        return Color.Transparent;
      if (!colorStr.StartsWith("#"))
        return Color.FromName(colorStr);
      string str = colorStr.Substring(1);
      if (str.Length == 3)
        str = new string(str[0], 2) + new string(str[1], 2) + new string(str[2], 2);
      else if (str.Length < 6)
        str = new string('0', 6 - str.Length) + str;
      return Color.FromArgb(int.Parse("ff" + str, NumberStyles.HexNumber));
    }

    public static string StyleFromColor(Color color) => HTMLHelper.StyleFromColor(color, false);

    public static string StyleFromColor(Color color, bool forceRgb)
    {
      if (color == Color.Transparent)
        return "transparent";
      return color.IsNamedColor && !forceRgb ? color.Name : string.Format("#{0:X2}{1:X2}{2:X2}", (object) color.R, (object) color.G, (object) color.B);
    }

    public static string CreateComment(string text)
    {
      return "<!--" + Environment.NewLine + text + Environment.NewLine + "-->";
    }

    public static string TrimComment(string comment)
    {
      comment = comment.Trim();
      if (comment.StartsWith("<!--"))
        comment = comment.Substring(4);
      if (comment.EndsWith("-->"))
      {
        if (comment.Length == 3)
          return "";
        comment = comment.Substring(0, comment.Length - 3);
      }
      return comment.Trim();
    }

    public static string EncodeSpaces(string text)
    {
      while (text.IndexOf("  ") >= 0)
        text = text.Replace("  ", "&#160; ");
      return text;
    }
  }
}
