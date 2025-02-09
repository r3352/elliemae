// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.FieldPairParser
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using System.Text.RegularExpressions;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class FieldPairParser
  {
    private static readonly Regex fieldParserRegex = new Regex("(?<fieldid>.+)#(?<pairindex>[0-9]+)", RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.Singleline);

    public static FieldPairInfo ParseFieldPairInfo(string fieldIdWithPairInfo)
    {
      Match match = FieldPairParser.fieldParserRegex.Match(fieldIdWithPairInfo);
      return !match.Success ? new FieldPairInfo(fieldIdWithPairInfo, 0) : new FieldPairInfo(match.Groups["fieldid"].Value, Utils.ParseInt((object) match.Groups["pairindex"].Value));
    }

    public static string GetFieldIDForBorrowerPair(string fieldId, int pairIndex)
    {
      return pairIndex <= 0 || fieldId.ToUpper().StartsWith("AUDITTRAIL") ? fieldId : fieldId + "#" + (object) pairIndex;
    }
  }
}
