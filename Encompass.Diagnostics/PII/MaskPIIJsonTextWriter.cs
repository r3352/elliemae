// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.PII.MaskPIIJsonTextWriter
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Newtonsoft.Json;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace Encompass.Diagnostics.PII
{
  public class MaskPIIJsonTextWriter : JsonTextWriter
  {
    private readonly bool _maskSql;

    public MaskPIIJsonTextWriter(TextWriter textWriter, bool maskSql)
      : base(textWriter)
    {
      this._maskSql = maskSql;
    }

    public override void WriteComment(string text)
    {
      base.WriteComment(MaskUtilities.MaskPII(text, this._maskSql));
    }

    public override Task WriteCommentAsync(string text, CancellationToken cancellationToken = default (CancellationToken))
    {
      return base.WriteCommentAsync(MaskUtilities.MaskPII(text, this._maskSql), cancellationToken);
    }

    public override void WriteRaw(string json)
    {
      base.WriteRaw(MaskUtilities.MaskPII(json, this._maskSql));
    }

    public override Task WriteRawAsync(string json, CancellationToken cancellationToken = default (CancellationToken))
    {
      return base.WriteRawAsync(MaskUtilities.MaskPII(json, this._maskSql), cancellationToken);
    }

    public override Task WriteRawValueAsync(string json, CancellationToken cancellationToken = default (CancellationToken))
    {
      return base.WriteRawValueAsync(MaskUtilities.MaskPII(json, this._maskSql), cancellationToken);
    }

    public override void WriteValue(string value)
    {
      base.WriteValue(MaskUtilities.MaskPII(value, this._maskSql));
    }

    public override Task WriteValueAsync(string value, CancellationToken cancellationToken = default (CancellationToken))
    {
      return base.WriteValueAsync(MaskUtilities.MaskPII(value, this._maskSql), cancellationToken);
    }
  }
}
