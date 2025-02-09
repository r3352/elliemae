// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.Error
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using Confluent.Kafka.Impl;
using System;

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>
  ///     Represents an error that occured when interacting with a
  ///     Kafka broker or the librdkafka library.
  /// </summary>
  public class Error
  {
    private readonly string reason;

    /// <summary>
    ///     Initialize a new Error instance that is a copy of another.
    /// </summary>
    /// <param name="error">The error object to initialize from.</param>
    public Error(Error error)
    {
      this.reason = error.Reason;
      this.IsFatal = error.IsFatal;
      this.IsRetriable = error.IsRetriable;
      this.TxnRequiresAbort = error.TxnRequiresAbort;
      this.Code = error.Code;
    }

    /// <summary>
    ///     Initialize a new Error instance from a native pointer to
    ///     a rd_kafka_error_t object, then destroy the native object.
    /// </summary>
    internal Error(IntPtr error)
    {
      if (error == IntPtr.Zero)
      {
        this.Code = ErrorCode.NoError;
        this.reason = (string) null;
        this.IsFatal = false;
        this.IsRetriable = false;
        this.TxnRequiresAbort = false;
      }
      else
      {
        this.Code = Librdkafka.error_code(error);
        this.IsFatal = Librdkafka.error_is_fatal(error);
        this.TxnRequiresAbort = Librdkafka.error_txn_requires_abort(error);
        this.IsRetriable = Librdkafka.error_is_retriable(error);
        this.reason = Librdkafka.error_string(error);
        Librdkafka.error_destroy(error);
      }
    }

    /// <summary>
    ///     Initialize a new Error instance from a particular
    ///     <see cref="T:Confluent.Kafka.ErrorCode" /> value.
    /// </summary>
    /// <param name="code">
    ///     The <see cref="T:Confluent.Kafka.ErrorCode" /> value associated with this Error.
    /// </param>
    /// <remarks>
    ///     The reason string associated with this Error will
    ///     be a static value associated with the <see cref="T:Confluent.Kafka.ErrorCode" />.
    /// </remarks>
    public Error(ErrorCode code)
    {
      this.Code = code;
      this.reason = (string) null;
      this.IsFatal = code == ErrorCode.Local_Fatal;
      this.IsRetriable = false;
      this.TxnRequiresAbort = false;
    }

    /// <summary>Initialize a new Error instance.</summary>
    /// <param name="code">The error code.</param>
    /// <param name="reason">
    ///     The error reason. If null, this will be a static value
    ///     associated with the error.
    /// </param>
    /// <param name="isFatal">Whether or not the error is fatal.</param>
    /// <exception cref="T:System.ArgumentException">
    /// 
    /// </exception>
    public Error(ErrorCode code, string reason, bool isFatal)
    {
      this.Code = code != ErrorCode.Local_Fatal || isFatal ? code : throw new ArgumentException("isFatal parameter must be 'true' when code is 'Local_Fatal'.");
      this.reason = reason;
      this.IsFatal = isFatal;
      this.IsRetriable = false;
      this.TxnRequiresAbort = false;
    }

    /// <summary>
    ///     Initialize a new Error instance from a particular
    ///     <see cref="T:Confluent.Kafka.ErrorCode" /> value and custom <paramref name="reason" />
    ///     string.
    /// </summary>
    /// <param name="code">
    ///     The <see cref="T:Confluent.Kafka.ErrorCode" /> value associated with this Error.
    /// </param>
    /// <param name="reason">
    ///     A custom reason string associated with the error
    ///     (overriding the static string associated with
    ///     <paramref name="code" />).
    /// </param>
    public Error(ErrorCode code, string reason)
    {
      this.Code = code;
      this.reason = reason;
      this.IsFatal = code == ErrorCode.Local_Fatal;
      this.IsRetriable = false;
      this.TxnRequiresAbort = false;
    }

    /// <summary>
    ///     Gets the <see cref="T:Confluent.Kafka.ErrorCode" /> associated with this Error.
    /// </summary>
    public ErrorCode Code { get; }

    /// <summary>Whether or not the error is fatal.</summary>
    public bool IsFatal { get; }

    /// <summary>
    ///     Whether or not the operation that caused the error is retriable.
    /// </summary>
    internal bool IsRetriable { get; }

    /// <summary>
    ///     Whether or not the current transaction is abortable
    ///     following the error.
    /// </summary>
    /// <remarks>
    ///     This is only relevant for the transactional producer
    ///     API.
    /// </remarks>
    internal bool TxnRequiresAbort { get; }

    /// <summary>
    ///     Gets a human readable reason string associated with this error.
    /// </summary>
    public string Reason => this.ToString();

    /// <summary>true if Code != ErrorCode.NoError.</summary>
    public bool IsError => this.Code != 0;

    /// <summary>
    ///     true if this is error originated locally (within librdkafka), false otherwise.
    /// </summary>
    public bool IsLocalError => this.Code < ErrorCode.Unknown;

    /// <summary>
    ///     true if this error originated on a broker, false otherwise.
    /// </summary>
    public bool IsBrokerError => this.Code > ErrorCode.NoError;

    /// <summary>
    ///     Converts the specified Error value to the value of it's Code property.
    /// </summary>
    /// <param name="e">The Error value to convert.</param>
    public static implicit operator ErrorCode(Error e) => e.Code;

    /// <summary>
    ///     Converts the specified <see cref="T:Confluent.Kafka.ErrorCode" /> value to it's corresponding rich Error value.
    /// </summary>
    /// <param name="c">
    ///     The <see cref="T:Confluent.Kafka.ErrorCode" /> value to convert.
    /// </param>
    public static implicit operator Error(ErrorCode c) => new Error(c);

    /// <summary>
    ///     Tests whether this Error instance is equal to the specified object.
    /// </summary>
    /// <param name="obj">The object to test.</param>
    /// <returns>
    ///     true if obj is an Error and the Code property values are equal. false otherwise.
    /// </returns>
    public override bool Equals(object obj)
    {
      return (object) (obj as Error) != null && ((Error) obj).Code == this.Code && ((Error) obj).IsFatal == this.IsFatal;
    }

    /// <summary>Returns a hash code for this Error value.</summary>
    /// <returns>
    ///     An integer that specifies a hash value for this Error value.
    /// </returns>
    public override int GetHashCode() => this.Code.GetHashCode();

    /// <summary>
    ///     Tests whether Error value a is equal to Error value b.
    /// </summary>
    /// <param name="a">The first Error value to compare.</param>
    /// <param name="b">The second Error value to compare.</param>
    /// <returns>
    ///     true if Error values a and b are equal. false otherwise.
    /// </returns>
    public static bool operator ==(Error a, Error b)
    {
      return (object) a == null ? (object) b == null : a.Equals((object) b);
    }

    /// <summary>
    ///     Tests whether Error value a is not equal to Error value b.
    /// </summary>
    /// <param name="a">The first Error value to compare.</param>
    /// <param name="b">The second Error value to compare.</param>
    /// <returns>
    ///     true if Error values a and b are not equal. false otherwise.
    /// </returns>
    public static bool operator !=(Error a, Error b) => !(a == b);

    /// <summary>
    ///     Returns the string representation of the error.
    ///     Depending on error source this might be a rich
    ///     contextual error message, or a simple static
    ///     string representation of the error Code.
    /// </summary>
    /// <returns>A string representation of the Error object.</returns>
    public override string ToString()
    {
      return !string.IsNullOrEmpty(this.reason) ? this.reason : this.Code.GetReason();
    }
  }
}
