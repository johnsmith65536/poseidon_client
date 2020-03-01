/**
 * Autogenerated by Thrift Compiler (0.13.0)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Thrift;
using Thrift.Collections;
using System.Runtime.Serialization;
using Thrift.Protocol;
using Thrift.Transport;


#if !SILVERLIGHT
[Serializable]
#endif
public partial class User : TBase
{
  private long _Id;
  private string _NickName;
  private long _LastOnlineTime;
  private bool _IsFriend;

  public long Id
  {
    get
    {
      return _Id;
    }
    set
    {
      __isset.Id = true;
      this._Id = value;
    }
  }

  public string NickName
  {
    get
    {
      return _NickName;
    }
    set
    {
      __isset.NickName = true;
      this._NickName = value;
    }
  }

  public long LastOnlineTime
  {
    get
    {
      return _LastOnlineTime;
    }
    set
    {
      __isset.LastOnlineTime = true;
      this._LastOnlineTime = value;
    }
  }

  public bool IsFriend
  {
    get
    {
      return _IsFriend;
    }
    set
    {
      __isset.IsFriend = true;
      this._IsFriend = value;
    }
  }


  public Isset __isset;
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public struct Isset {
    public bool Id;
    public bool NickName;
    public bool LastOnlineTime;
    public bool IsFriend;
  }

  public User() {
  }

  public void Read (TProtocol iprot)
  {
    iprot.IncrementRecursionDepth();
    try
    {
      TField field;
      iprot.ReadStructBegin();
      while (true)
      {
        field = iprot.ReadFieldBegin();
        if (field.Type == TType.Stop) { 
          break;
        }
        switch (field.ID)
        {
          case 1:
            if (field.Type == TType.I64) {
              Id = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.String) {
              NickName = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.I64) {
              LastOnlineTime = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.Bool) {
              IsFriend = iprot.ReadBool();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          default: 
            TProtocolUtil.Skip(iprot, field.Type);
            break;
        }
        iprot.ReadFieldEnd();
      }
      iprot.ReadStructEnd();
    }
    finally
    {
      iprot.DecrementRecursionDepth();
    }
  }

  public void Write(TProtocol oprot) {
    oprot.IncrementRecursionDepth();
    try
    {
      TStruct struc = new TStruct("User");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.Id) {
        field.Name = "Id";
        field.Type = TType.I64;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(Id);
        oprot.WriteFieldEnd();
      }
      if (NickName != null && __isset.NickName) {
        field.Name = "NickName";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(NickName);
        oprot.WriteFieldEnd();
      }
      if (__isset.LastOnlineTime) {
        field.Name = "LastOnlineTime";
        field.Type = TType.I64;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(LastOnlineTime);
        oprot.WriteFieldEnd();
      }
      if (__isset.IsFriend) {
        field.Name = "IsFriend";
        field.Type = TType.Bool;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteBool(IsFriend);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }
    finally
    {
      oprot.DecrementRecursionDepth();
    }
  }

  public override string ToString() {
    StringBuilder __sb = new StringBuilder("User(");
    bool __first = true;
    if (__isset.Id) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Id: ");
      __sb.Append(Id);
    }
    if (NickName != null && __isset.NickName) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("NickName: ");
      __sb.Append(NickName);
    }
    if (__isset.LastOnlineTime) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("LastOnlineTime: ");
      __sb.Append(LastOnlineTime);
    }
    if (__isset.IsFriend) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("IsFriend: ");
      __sb.Append(IsFriend);
    }
    __sb.Append(")");
    return __sb.ToString();
  }

}

