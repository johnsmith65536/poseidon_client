struct UserRelation {
1: i64 Id
2: i64 UserIdSend
3: i64 UserIdRecv
4: i64 CreateTime
5: i32 Status
6: optional i64 ParentId 
}

struct Message {
1: i64 Id
2: i64 UserIdSend
3: i64 UserIdRecv
4: i64 GroupId
5: string Content
6: i64 CreateTime
7: i32 ContentType
8: i32 MsgType
9: i32 IsRead
}

struct Object {
1: i64 Id
2: string ETag
3: string Name
}

struct CreateUserReq {
1: string Password
2: string NickName
}

struct CreateUserResp {
1: i64 UserId
2: i32 Status
}

struct LoginReq {
1: i64 UserId
2: string Password
}

struct LoginResp {
1: bool Success
}

struct LogoutReq  {
1: i64 UserId
}

struct LogoutResp  {
}

struct SendMessageReq  {
1:	i64 UserIdSend  
2:	i64 IdRecv  
3:	string Content        
4:	i32 ContentType    
5:	i32 MessageType 
}
struct SendMessageResp  {
1:	i64 Id
2:  i64 CreateTime
}

struct UpdateMessageStatusReq {
1: map<i64,i32> MessageIds
2: map<i64,i32> UserRelationRequestIds
}

struct UpdateMessageStatusResp {
}

struct FetchMessageStatusReq {
1: list<i64> MessageIds
2: list<i64>  UserRelationRequestIds
}

struct FetchMessageStatusResp {
1: map<i64,i32> MessageIds
2: map<i64,i32> UserRelationRequestIds
}

struct HeartBeatReq  {
1: i64 UserId
}
struct HeartBeatResp{
}

struct FetchFriendsListReq  {
1: i64	UserId
}
struct FetchFriendsListResp  {
1:	list<i64> OnlineUserIds
2:	list<i64> OfflineUserIds
}

struct SyncMessageReq  {
1: i64	UserId
2: i64	MessageId
3: i64	UserRelationId
}
struct SyncMessageResp  {
1: list<Message>	Messages
2: list<UserRelation> UserRelations
3: list<Object> Objects
4: i64 LastOnlineTime
}

struct CreateObjectReq  {
1: string ETag     
2: string Name 
}
struct CreateObjectResp  {
1: i64 id
}

struct GetSTSInfoReq {
1: i64 userId
}

struct GetSTSInfoResp {
1: string SecurityToken
2: string AccessKeyId
3: string AccessKeySecret
}

struct AddFriendReq  {
1:	i64 UserIdSend 
2:	i64 UserIdRecv 
}
struct AddFriendResp  {
1:	i64 Id
2:  i64 CreateTime
255: i32 StatusCode
}

struct ReplyAddFriendReq  {
1: i64	Id
2: i32 Status
}
struct ReplyAddFriendResp  {
1: i64 Id
2: i64 CreateTime
}

struct DeleteFriendReq {
1: i64 UserIdSend
2: i64 UserIdRecv
}

struct DeleteFriendResp {
}

struct User {
1: i64 Id
2: string NickName
3: i64 LastOnlineTime
4: bool IsFriend
}

struct SearchUserReq  {
1: i64 UserId
2: string	Data
}
struct SearchUserResp  {
1: list<User>	Users
}

service Server {
CreateUserResp CreateUser(1:CreateUserReq req),
LoginResp Login(1:LoginReq req),
LogoutResp Logout(1:LogoutReq req),
SendMessageResp SendMessage(1:SendMessageReq req),
HeartBeatResp HeartBeat(1:HeartBeatReq req),
FetchFriendsListResp FetchFriendsList(1:FetchFriendsListReq req),
SyncMessageResp SyncMessage(1:SyncMessageReq req),
AddFriendResp AddFriend(1:AddFriendReq req),
ReplyAddFriendResp ReplyAddFriend(1:ReplyAddFriendReq req),
UpdateMessageStatusResp UpdateMessageStatus(1:UpdateMessageStatusReq req),
SearchUserResp SearchUser(1:SearchUserReq req),
DeleteFriendResp DeleteFriend(1:DeleteFriendReq req),
GetSTSInfoResp GetSTSInfo(1:GetSTSInfoReq req),
FetchMessageStatusResp FetchMessageStatus(1:FetchMessageStatusReq req),
CreateObjectResp CreateObject(1:CreateObjectReq req),
}
