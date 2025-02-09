// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.EnhancedConditionsTool.SyncError
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.Common.eFolder;
using EllieMae.EMLite.DataEngine.eFolder;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.AdminTools.EnhancedConditionsTool
{
  public class SyncError
  {
    public string Message { get; set; }

    public EnhancedConditionTemplate Template { get; set; }

    public static IEnumerable<SyncError> MapApiErrors(
      EnhanceConditionError apiError,
      IList<EnhancedConditionTemplate> templates)
    {
      foreach (EnhancedConditionTemplate template in (IEnumerable<EnhancedConditionTemplate>) templates)
        yield return new SyncError() { Template = template };
      if (apiError?.errors == null)
      {
        if (!string.IsNullOrEmpty(apiError?.summary))
          yield return new SyncError()
          {
            Message = apiError.summary + ": " + apiError.details
          };
      }
      else
      {
        Func<string, string> toTitleCase = (Func<string, string>) (input => char.ToUpper(input[0]).ToString() + input.Substring(1));
        Func<string, string> formatPath = (Func<string, string>) (path => string.Join(" → ", ((IEnumerable<string>) path.Split('.')).Select<string, string>(toTitleCase)));
        StringComparison comparison = StringComparison.InvariantCultureIgnoreCase;
        foreach (EnhanceConditionError error in apiError.errors)
        {
          string extracted;
          int result;
          if (SyncError.TryExtract(error.summary, "templates[", "].", out extracted) && int.TryParse(extracted, out result))
          {
            EnhancedConditionTemplate template = templates[result];
            string str = error.details;
            int startIndex = "templates[].".Length + extracted.Length;
            if (startIndex < error.summary.Length)
              str = formatPath(error.summary.Substring(startIndex)) + ": " + str;
            yield return new SyncError()
            {
              Template = template,
              Message = str
            };
          }
          string templateName;
          if (SyncError.TryExtract(error.details, "Template '", "': ", out templateName))
          {
            EnhancedConditionTemplate conditionTemplate = templates.FirstOrDefault<EnhancedConditionTemplate>((Func<EnhancedConditionTemplate, bool>) (t => t.Id.ToString().Equals(templateName, comparison) || t.Title.Equals(templateName, comparison)));
            string str = error.details;
            int startIndex = "Template '': ".Length + templateName.Length;
            if (startIndex < str.Length)
              str = formatPath(error.summary) + ": " + str.Substring(startIndex);
            yield return new SyncError()
            {
              Template = conditionTemplate,
              Message = str
            };
          }
        }
        formatPath = (Func<string, string>) null;
      }
    }

    private static bool TryExtract(
      string source,
      string prefix,
      string suffix,
      out string extracted)
    {
      int length = prefix.Length;
      if (source.StartsWith(prefix))
      {
        int num = source.IndexOf(suffix, length);
        extracted = source.Substring(length, num - length);
        return true;
      }
      extracted = (string) null;
      return false;
    }
  }
}
