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
public partial class LoginReq : TBase
{
  private long _UserId;
  private string _Password;

  public long UserId
  {
    get
    {
      return _UserId;
    }
    set
    {
      __isset.UserId = true;
      this._UserId = value;
    }
  }

  public string Password
  {
    get
    {
      return _Password;
    }
    set
    {
      __isset.Password = true;
      this._Password = value;
    }
  }


  public Isset __isset;
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public struct Isset {
    public bool UserId;
    public bool Password;
  }

  public LoginReq() {
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
              UserId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.String) {
              Password = iprot.ReadString();
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
      TStruct struc = new TStruct("LoginReq");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.UserId) {
        field.Name = "UserId";
        field.Type = TType.I64;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(UserId);
        oprot.WriteFieldEnd();
      }
      if (Password != null && __isset.Password) {
        field.Name = "Password";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Password);
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
    StringBuilder __sb = new StringBuilder("LoginReq(");
    bool __first = true;
    if (__isset.UserId) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("UserId: ");
      __sb.Append(UserId);
    }
    if (Password != null && __isset.Password) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Password: ");
      __sb.Append(Password);
    }
    __sb.Append(")");
    return __sb.ToString();
  }

}

