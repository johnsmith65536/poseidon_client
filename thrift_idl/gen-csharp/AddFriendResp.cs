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
public partial class AddFriendResp : TBase
{
  private long _Id;
  private long _CreateTime;
  private int _StatusCode;

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

  public long CreateTime
  {
    get
    {
      return _CreateTime;
    }
    set
    {
      __isset.CreateTime = true;
      this._CreateTime = value;
    }
  }

  public int StatusCode
  {
    get
    {
      return _StatusCode;
    }
    set
    {
      __isset.StatusCode = true;
      this._StatusCode = value;
    }
  }


  public Isset __isset;
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public struct Isset {
    public bool Id;
    public bool CreateTime;
    public bool StatusCode;
  }

  public AddFriendResp() {
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
            if (field.Type == TType.I64) {
              CreateTime = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 255:
            if (field.Type == TType.I32) {
              StatusCode = iprot.ReadI32();
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
      TStruct struc = new TStruct("AddFriendResp");
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
      if (__isset.CreateTime) {
        field.Name = "CreateTime";
        field.Type = TType.I64;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(CreateTime);
        oprot.WriteFieldEnd();
      }
      if (__isset.StatusCode) {
        field.Name = "StatusCode";
        field.Type = TType.I32;
        field.ID = 255;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(StatusCode);
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
    StringBuilder __sb = new StringBuilder("AddFriendResp(");
    bool __first = true;
    if (__isset.Id) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Id: ");
      __sb.Append(Id);
    }
    if (__isset.CreateTime) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("CreateTime: ");
      __sb.Append(CreateTime);
    }
    if (__isset.StatusCode) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("StatusCode: ");
      __sb.Append(StatusCode);
    }
    __sb.Append(")");
    return __sb.ToString();
  }

}

