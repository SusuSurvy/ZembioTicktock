﻿using System.ComponentModel;

    /// <summary>
    /// 弹幕消息类型
    /// </summary>
    public enum PackMsgType
    {
        [Description("无")]
        无 = 0,       
        [Description("消息")] 
        弹幕消息 = 1,
        [Description("点赞")]
        点赞消息 = 2,
        [Description("进房")]
        进直播间 = 3,
        [Description("关注")]
        关注消息 = 4,
        [Description("礼物")]
        礼物消息 = 5,
        [Description("统计")]
        直播间统计 = 6,
        [Description("粉团")]
        粉丝团消息 = 7,
        [Description("分享")]
        直播间分享 = 8,
        [Description("下播")]
        下播 = 9
    }

    /// <summary>
    /// 粉丝团消息类型
    /// </summary>
    public enum FansclubType
    {
        无 = 0,
        粉丝团升级 = 1,
        加入粉丝团 = 2
    }

    /// <summary>
    /// 直播间分享目标
    /// </summary>
    public enum ShareType
    {
        未知 = 0,
        微信 = 1,
        朋友圈 = 2,
        微博 = 3,
        QQ空间 = 4,
        QQ = 5,
        抖音好友 = 112
    }


    /// <summary>
    /// 数据包装器
    /// </summary>
    public class BarrageMsgPack
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public PackMsgType Type { get; set; }      
        
        /// <summary>
        /// 进程名
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// 消息对象
        /// </summary>
        public string Data { get; set; }

        public BarrageMsgPack()
        {

        }

        public BarrageMsgPack(string data, PackMsgType type,string processName)
        {
            Data = data;
            Type = type;
            ProcessName = processName;
        }
    }

    /// <summary>
    /// 消息
    /// </summary>
    public class Msg
    {
        /// <summary>
        /// 弹幕ID
        /// </summary>
        public long MsgId { get; set; }

        /// <summary>
        /// 用户数据
        /// </summary>
        public MsgUser User { get; set; }

        /// <summary>
        /// 主播简要信息
        /// </summary>
        public RoomAnchorInfo Onwer { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 房间号
        /// </summary>
        public string RoomId { get; set; }

        /// <summary>
        /// web直播间ID
        /// </summary>
        public string WebRoomId { get; set; }
    }

    /// <summary>
    /// 粉丝团信息
    /// </summary>
    public class FansClubInfo
    {
        /// <summary>
        /// 粉丝团名称
        /// </summary>
        public string ClubName { get; set; }

        /// <summary>
        /// 粉丝团等级，没加入则0
        /// </summary>
        public int Level { get; set; }
    }

    /// <summary>
    /// 直播间主播信息
    /// </summary>
    public class RoomAnchorInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// SecUid
        /// </summary>
        public string SecUid { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 头像地址
        /// </summary>
        public string HeadUrl { get; set; }

        /// <summary>
        /// 关注状态 0未关注,1已关注,...
        /// </summary>
        public int FollowStatus { get; set; }
    }

    /// <summary>
    /// 用户弹幕信息
    /// </summary>
    public class MsgUser
    {
        /// <summary>
        /// 真实ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// ShortId
        /// </summary>
        public long ShortId { get; set; }

        /// <summary>
        /// 自定义ID
        /// </summary>
        public string DisplayId { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 未知
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 支付等级
        /// </summary>
        public int PayLevel { get; set; }

        /// <summary>
        /// 性别 1男 2女
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// 头像地址
        /// </summary>
        public string HeadImgUrl { get; set; }

        /// <summary>
        /// 用户主页地址
        /// </summary>
        public string SecUid { get; set; }

        /// <summary>
        /// 粉丝团信息
        /// </summary>
        public FansClubInfo FansClub { get; set; }

        /// <summary>
        /// 粉丝数
        /// </summary>
        public long FollowerCount { get; set; }

        /// <summary>
        /// 关注状态 0 未关注 1 已关注 2,不明
        /// </summary>
        public long FollowStatus { get; set; }

        /// <summary>
        /// 关注数
        /// </summary>
        public long FollowingCount;


        public string GenderToString()
        {
            return Gender == 1 ? "男" : Gender == 2 ? "女" : "妖";
        }
    }

    /// <summary>
    /// 礼物消息
    /// </summary>
    public class GiftMsg : Msg
    {
        /// <summary>
        /// 礼物ID
        /// </summary>
        public long GiftId { get; set; }

        /// <summary>
        /// 礼物名称
        /// </summary>
        public string GiftName { get; set; }

        /// <summary>
        /// 礼物分组ID
        /// </summary>
        public long GroupId { get; set; }

        /// <summary>
        /// 本次(增量)礼物数量
        /// </summary>
        public int GiftCount { get; set; }

        /// <summary>
        /// 礼物数量(连续的)
        /// </summary>
        public long RepeatCount { get; set; }

        /// <summary>
        /// 抖币价格
        /// </summary>
        public int DiamondCount { get; set; }   
        
        /// <summary>
        /// 该礼物是否可连击
        /// </summary>
        public bool Combo { get; set; }

        /// <summary>
        /// 礼物图片地址
        /// </summary>
        public string ImgUrl { get; set; }

        /// <summary>
        /// 送礼目标(连麦直播间有用)
        /// </summary>
        public MsgUser ToUser { get; set; }
    }

    /// <summary>
    /// 点赞消息
    /// </summary>
    public class LikeMsg : Msg
    {
        /// <summary>
        /// 点赞数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 总共点赞数量
        /// </summary>
        public int Total { get; set; }
    }

    /// <summary>
    /// 直播间统计消息
    /// </summary>
    public class UserSeqMsg : Msg
    {
        /// <summary>
        /// 当前直播间用户数量
        /// </summary>
        public long OnlineUserCount { get; set; }

        /// <summary>
        /// 累计直播间用户数量
        /// </summary>
        public long TotalUserCount { get; set; }

        /// <summary>
        /// 累计直播间用户数量 显示文本
        /// </summary>
        public string TotalUserCountStr { get; set; }

        /// <summary>
        /// 当前直播间用户数量 显示文本
        /// </summary>
        public string OnlineUserCountStr { get; set; }
    }

    /// <summary>
    /// 粉丝团消息
    /// </summary>
    public class FansclubMsg : Msg
    {
        /// <summary>
        /// 粉丝团消息类型,升级1，加入2
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 粉丝团等级
        /// </summary>
        public int Level { get; set; }
    }

    /// <summary>
    /// 来了消息
    /// </summary>
    public class MemberMessage : Msg
    {
        /// <summary>
        /// 当前直播间人数
        /// </summary>
        public long CurrentCount { get; set; }
    }

    /// <summary>
    /// 直播间分享
    /// </summary>
    public class ShareMessage : Msg
    {
        /// <summary>
        /// 分享目标
        /// </summary>
        public ShareType ShareType { get; set; }
    }