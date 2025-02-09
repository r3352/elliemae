// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.Schema.TransactionId
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Encompass.Diagnostics.Logging.Formatters;
using Newtonsoft.Json;
using System;

#nullable disable
namespace Encompass.Diagnostics.Logging.Schema
{
  [JsonConverter(typeof (TransactionIdConverter))]
  [Serializable]
  public class TransactionId
  {
    public Guid Id { get; set; }

    public TransactionId(Guid id) => this.Id = id;

    public override string ToString() => this.Id.ToString();

    public override bool Equals(object obj)
    {
      int num;
      switch (obj)
      {
        case TransactionId transactionId:
          return this.Id == transactionId.Id;
        case Guid guid:
          num = 1;
          break;
        default:
          num = 0;
          break;
      }
      return num != 0 && guid == this.Id;
    }

    public override int GetHashCode() => this.Id.GetHashCode();

    public static implicit operator Guid(TransactionId d) => d.Id;

    public static implicit operator Guid?(TransactionId d) => d?.Id;

    public static implicit operator TransactionId(Guid id) => new TransactionId(id);

    public static implicit operator TransactionId(Guid? id)
    {
      return !id.HasValue ? (TransactionId) null : new TransactionId(id.Value);
    }
  }
}
