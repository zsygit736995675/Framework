using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region  接收

/// <summary>
/// 版本信息返回
/// </summary>
public class VersionReceiveData
{
    public string gameVersion { get; set; }
    public string strConfig { get; set; }
    public string clearanceConfig { get; set; }
    public string characterConfig { get; set; }
    public string guiConfig { get; set; }
    public string levelConfig { get; set; }
    public string sceneConfig { get; set; }
    public string googleId { get; set; }
    public int isLogInfo { get; set; }
    public string systemConfig { get; set; }
    public string rewardConfig { get; set; }
    public string guideConfig { get; set; }
    public string iosId { get; set; }
    public float InterstitialRate { get; set; }
    public int hp_recovery_interval { get; set; }
}

public class VersionReceive
{
    /// <summary>
    /// 
    /// </summary>
    public int code { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string msg { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public VersionReceiveData data { get; set; }
}
//----------------------------------------------
/// <summary>
/// 打点返回
/// </summary>
public class WriteReceive
{
    /// <summary>
    /// 
    /// </summary>
    public int code { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string msg { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string data { get; set; }
}

/// <summary>
/// 查询玩家信息返回
/// </summary>
public class PlayerInfoReceiveData
{
    /// <summary>
    /// 
    /// </summary>
    public int id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string name { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string third_id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int status { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int currentEntityID { get; set; }
    public int currentSmearID { get; set; }
    public int stars { get; set; }
    public int total_stars { get; set; }
    public int gold { get; set; }
    public int hp { get; set; }
    public int max_level { get; set; }
    public int current_level { get; set; }
    public int hp_time { get; set; }
    public string icon { get; set; }
    public string deviceid { get; set; }
    public int is_register { get; set; }
    public int level_history_rank { get; set; }
    public List<SkinReceiveData> characterList { get; set; }
    public int total_skins { get; set; }
    public List<SmearReceiveData> smearsList { get; set; }
    public int total_smears { get; set; }
}

public class PlayerInfoReceive
{
    /// <summary>
    /// 
    /// </summary>
    public int code { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string msg { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public PlayerInfoReceiveData data { get; set; }
}

public class PlayerInfoListReceive
{
    /// <summary>
    /// 
    /// </summary>
    public int code { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string msg { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List< PlayerInfoReceiveData> data { get; set; }
}
//----------------------------------------------
public class LevelInfoReceiveData
{
    public int level { get; set; }
    public int stars { get; set; }
    public string condition { get; set; }
    public int playerId { get; set; }
    public string update_time { get; set; }
    public string best_time { get; set; }
    public int fail_count { get; set; }
}
/// <summary>
/// 关卡查询返回
/// </summary>
public class LevelInfoReceive
{
    public int code { get; set; }
    public string msg { get; set; }
    public List<LevelInfoReceiveData> data { get; set; }
}
//--------------------------------------------------

/// <summary>
/// 任务列表查询返回
/// </summary>
public class MissionListReceiveData
{
    /// <summary>
    /// 
    /// </summary>
    public int id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int remark { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int type { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int target { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int rewType { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int rewCount { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string notes { get; set; }
}

public class MissionListReceiveInfo
{
    /// <summary>
    /// 
    /// </summary>
    public int id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int playerId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int complete { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int gainreward { get; set; }

    public int missionId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int progress { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public MissionListReceiveData mission { get; set; }
}

public class MissionListReceive
{
    /// <summary>
    /// 
    /// </summary>
    public int code { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string msg { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<MissionListReceiveInfo> data { get; set; }
}

//----------------------------------------------

/// <summary>
/// 勋章列表
/// </summary>
public class AchievementReceiveData
{
    /// <summary>
    /// 
    /// </summary>
    public string description { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string icon { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string name { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string status { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int target { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int type { get; set; }
}

public class AchievementReceiveInfo
{
    /// <summary>
    /// 
    /// </summary>
    public AchievementReceiveData achievement { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int playerId { get; set; }
}

public class AchievementReceive
{
    /// <summary>
    /// 
    /// </summary>
    public int code { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string msg { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<AchievementReceiveInfo> data { get; set; }
}

//---------------------------------------------

/// <summary>
/// 道具列表返回
/// </summary>
public class PropReceiveData
{
    /// <summary>
    /// 
    /// </summary>
    public string description { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int effect { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string icon { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string name { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int spend { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string status { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int type { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int uselimit { get; set; }
}

public class PropReceiveInfo
{
    /// <summary>
    /// 
    /// </summary>
    public int id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int playerId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int propId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int propCount { get; set; }
}

public class PropReceive
{
    /// <summary>
    /// 
    /// </summary>
    public int code { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string msg { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<PropReceiveInfo> data { get; set; }
}

public class PropUseReceive
{
    /// <summary>
    /// 
    /// </summary>
    public int code { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string msg { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public PropReceiveInfo data { get; set; }
}

//-------------------------------------------------

/// <summary>
/// 道具购买返回
/// </summary>
public class PropBuyReceive
{
    /// <summary>
    /// 
    /// </summary>
    public int code { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public PropReceiveInfo data { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string msg { get; set; }
}

/// <summary>
/// 复活卡使用返回
/// </summary>
public class RebirthUseReceive
{
    /// <summary>
    /// 
    /// </summary>
    public int code { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string msg { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int data { get; set; }
}
//----------------------------------------
/// <summary>
/// 皮肤列表返回
/// </summary>
public class SkinReceiveData
{
    public int playerId { get; set; }
    public int skinId { get; set; }
    public string create_time { get; set; }//时间
    public int getType { get; set; }
    public string rest_time { get; set; }//getType=4时的剩余时间
}

public class SkinReceive
{
    public int code { get; set; }
    public string msg { get; set; }
    public List<SkinReceiveData> data { get; set; }
}

/// <summary>
/// 皮肤购买返回
/// </summary>
public class SkinBuyReceive
{
    /// <summary>
    /// 
    /// </summary>
    public int code { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public SkinReceiveData data { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string msg { get; set; }
}
//----------------------------------------
/// <summary>
/// 拖尾列表返回
/// </summary>
public class SmearReceiveData
{
    public int playerId { get; set; }
    public int smearId { get; set; }
    public string create_time { get; set; }//时间
    public int getType { get; set; }
    public string rest_time { get; set; }//getType=4时的剩余时间
}

public class SmearReceive
{
    public int code { get; set; }
    public string msg { get; set; }
    public List<SmearReceiveData> data { get; set; }
}
/// <summary>
/// 拖尾购买返回
/// </summary>
public class SmearBuyReceive
{
    /// <summary>
    /// 
    /// </summary>
    public int code { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public SmearReceiveData data { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string msg { get; set; }
}
//---------------------------------------

/// <summary>
/// 排行榜
/// </summary>
public class RankReceiveData
{

    /// <summary>
    /// 
    /// </summary>
    public int id { get; set; }
    public int playerId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int scores { get; set; }
    public string icon { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string name { get; set; }
}

public class RankReceiveInfo
{
    public int rest_time { get; set; }
     public List<RankReceiveData> ranklist { get; set; }
}

/// <summary>
/// 排行榜返回
/// </summary>
public class RankReceive
{
    /// <summary>
    /// 
    /// </summary>
    public int code { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string msg { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public RankReceiveInfo data { get; set; }
}

/// <summary>
/// 星星收集奖励信息
/// </summary>
public class RewardsReceiveInfo
{
    public int rewardId { get; set; }
    public int playerId { get; set; }
}

/// <summary>
/// 星星收集奖励返回
/// </summary>
public class RewardsReceive
{
    /// <summary>
    /// 
    /// </summary>
    public int code { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string msg { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<RewardsReceiveInfo> data { get; set; }
}
//---------------------------------------

/// <summary>
/// 签到信息返回
/// </summary>
public class GiftReceiveData
{
    public int id { get; set; }
    public int playerId { get; set; }
    /// <summary>
    /// 签到日期
    /// </summary>
    public string sign_time { get; set; }
    /// <summary>
    /// 签到次数
    /// </summary>
    public int day { get; set; }
}

public class GiftReceiveInfo
{
    public int is_new { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<GiftReceiveData> signlist { get; set; }
}

public class GiftReceive
{
    /// <summary>
    /// 
    /// </summary>
    public int code { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string msg { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public GiftReceiveInfo data { get; set; }
}


//-----------------------------------------------

/// <summary>
/// 任务领奖返回
/// </summary>
public class MissionPrizeReceive
{
    /// <summary>
    /// 
    /// </summary>
    public int code { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string msg { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string data { get; set; }
}
//----------------------------------


public class AdUpdateReceiveData
{
    public int hp { get; set; }
}
/// <summary>
/// 看广告返回
/// </summary>
public class AdUpdateReceive
{
    /// <summary>
    /// 
    /// </summary>
    public int code { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string msg { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public AdUpdateReceiveData data { get; set; }
}
//----------------------------------
public class SettlementReceiveData
{
    public int is_first { get; set; }
}
/// <summary>
/// 结算返回
/// </summary>
public class SettlementReceive
{
    /// <summary>
    /// 
    /// </summary>
    public int code { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string msg { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public SettlementReceiveData data { get; set; }
}

//----------------------------------

/// <summary>
/// 能力列表返回
/// </summary>
public class AbilityData
{
    /// <summary>
    /// 
    /// </summary>
    public int bewrite { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int name { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int showtype { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int type { get; set; }
}

public class AbilityReceive
{
    /// <summary>
    /// 
    /// </summary>
    public int code { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<AbilityData> data { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string msg { get; set; }
}

/// <summary>
/// 购买复活卡返回
/// </summary>
public class AngleReceive
{
    /// <summary>
    /// 
    /// </summary>
    public int code { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string msg { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string data { get; set; }
}
public class AdvReceiveData
{
    public int gold { get; set; }
    public int diamand { get; set; }
}
public class AdvReceive
{
    /// <summary>
    /// 
    /// </summary>
    public int code { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string msg { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public AdvReceiveData data { get; set; }
}
/// <summary>
/// 登录返回
/// </summary>
public class LogReceive
{
    /// <summary>
    /// 
    /// </summary>
    public int code { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string msg { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string data { get; set; }
}


    /// <summary>
    /// 金币复活返回
    /// </summary>
public class GoldResurrectionReceive
{

    /// <summary>
    /// 
    /// </summary>
    public int code { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string msg { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string data { get; set; }
}

public class StarRewardsReceive
{
    /// <summary>
    /// 
    /// </summary>
    public int code { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string msg { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string data { get; set; }
}

public class GetThemeReceiveData
{    
    /// <summary>
    /// 
    /// </summary>
    public int player_id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int theme_id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string create_time { get; set; }
}
public class GetThemeReceive
{
    /// <summary>
    /// 
    /// </summary>
    public int code { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string msg { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<GetThemeReceiveData> data { get; set; }
}

public class RandomSkiReceiveData
{
    public string randomdestiny { get; set; }
    public int is_trial { get; set; }
}
public class RandomSkiReceive
{
    /// <summary>
    /// 
    /// </summary>
    public int code { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string msg { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public RandomSkiReceiveData data { get; set; }
}
//-----------------------------------------
#endregion



#region 发送

public class ChangeNameRequest
{
    public int playerId { get; set; }
    public string name { get; set; }
}

public class ChangeIconRequest
{
    public int playerId { get; set; }
    public string icon { get; set; }
}

public class ChangeHpRequest
{
    public int playerId { get; set; }
    public int hp { get; set; }
}
public class ChangeSkinRequest
{
    public int playerId { get; set; }
    public int currentEntityID { get; set; }
}

public class ChangeSmearRequest
{
    public int playerId { get; set; }
    public int currentSmearID { get; set; }
}


/// <summary>
/// 崩溃日志
/// </summary>
public class LogRequest
{
    public string aid { get; set; }
    public string crash { get; set; }
}

/// <summary>
/// 主界面打点请求
/// </summary>
public class WriteHomeRequest
{
    public string aid { get; set; }
    public string clickArea { get; set; }
}


/// <summary>
/// 对战打点请求
/// </summary>
public class WriteLevelRequest
{
    public string aid { get; set; }
    public int level { get; set; }
    public string type { get; set; }
    public string fightid { get; set; }
    public int resurrection { get; set; }
    public string resurrection_type { get; set; }
    public string use_time { get; set; }
    public int result { get; set; }
}

/// <summary>
/// 活跃打点请求
/// </summary>
public class WriteActiveRequest
{
    public string aid { get; set; }
    public string country_code { get; set; }
    public string login_type { get; set; }
    public string status { get; set; }
    public float loadtime { get; set; }
}
/// <summary>
/// 关卡加载时长请求
/// </summary>
public class WriteLevelLoadRequest
{
    public string aid { get; set; }
    public int level { get; set; }
    public float loadtime { get; set; }
}
/// <summary>
/// 购买道具
/// </summary>
public class BuyPropRequest
{
    /// <summary>
    /// 
    /// </summary>
    public int playerId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int propId { get; set; }
    public int cardcount { get; set; }
    public int spendType { get; set; }
    public int money { get; set; }
}

//-----------------------------

/// <summary>
/// 购买皮肤
/// </summary>
public class BuySkinRequest
{
    /// <summary>
    /// 
    /// </summary>
    public int playerId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int skinId { get; set; }

    public int getType { get; set; }
    public int money { get; set; }
}
//-----------------------------------------

    /// <summary>
/// 购买拖尾
/// </summary>
public class BuySmearRequest
{
    /// <summary>
    /// 
    /// </summary>
    public int playerId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int smearId { get; set; }

    public int money { get; set; }
    public int getType { get; set; }
}
//-----------------------------------------


/// <summary>
/// 签到请求
/// </summary>
public class GiftRequest
{
    public int playerId { get; set; }
    public int day { get; set; }
    public int rewCount { get; set; }
    public int rewId { get; set; }
    public int rewType { get; set; }
}

public class GoldResurrectionRequest
{
    public int playerId { get; set; }
    public int gold { get; set; }
}

//------------------------------------

public class StarRewardsRequest
{
    public int playerId { get; set; }
    public int rewardId { get; set; }
    public int condition { get; set; }
    public int type { get; set; }
    public int count { get; set; }
}
//--------------------------------

/// <summary>
/// 结算请求
/// </summary>
public class SettlementRequest
{
    public int playerId { get; set; }
    public int stars { get; set; }
    public int level { get; set; }
    public string condition { get; set; }
    public string times { get; set; }
}

//-----------------------------------------

/// <summary>
/// 提交任务
/// </summary>
public class MissionSubmitRequest
{
    /// <summary>
    /// 
    /// </summary>
    public int playerId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int pmissionId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int progress { get; set; }
}

//-----------------------------------------

/// <summary>
/// 获取任务奖励
/// </summary>
public class MissionPrizeRequest
{
    /// <summary>
    /// 
    /// </summary>
    public int playerId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int pmissionId { get; set; }
}

//-----------------------------------------

/// <summary>
/// 登录请求
/// </summary>
public class LoginRequest
{

    /// <summary>
    /// 
    /// </summary>
    public string deviceId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string thirdId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string thirdName { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string thirdIcon { get; set; }
    public string country_code { get; set; }
    public string lang_code { get; set; }
    public int thirdType { get; set; }
}
//-----------------------------------------

    /// <summary>
    /// 使用复活卡请求
    /// </summary>
public  class UseRebirthRequest
{
    /// <summary>
    /// 
    /// </summary>
    public int playerId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int cardcount { get; set; }
}

//-----------------------------------------

/// <summary>
/// 添加数据请求
/// </summary>
public class AddingRequest
{
        /// <summary>
        /// 
        /// </summary>
        public int playerId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int life { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int coins { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int scores { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int answer_count { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int correct_count { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int fight_result { get; set; }
}
//-----------------------------------------
/// <summary>
/// 广告获得收入金币钻石
/// </summary>
public class AdUpdateRequest
{
    public int playerId { get; set; }
}
//-----------------------------------------

/// <summary>
/// 增加金币
/// </summary>
public class UpdateGoldRequest
{
    public int playerId { get; set; }
    public int gold { get; set; }
}
//-----------------------------------------
/// <summary>
/// 广告打点
/// </summary>
public class WriteAdvRequest
{
    public string aid { get; set; }
    public string area { get; set; }
    public string adv_status { get; set; }
    public string Message { get; set; }
}

/// <summary>
/// 广告按钮打点
/// </summary>
public class WriteAdvBtnRequest
{
    public string aid { get; set; }
    public string area { get; set; }
    public string type { get; set; }
}

/// <summary>
/// 上报设备信息
/// </summary>
public class WriteDeviceRequest
{
    public string aid { get; set; }
    public string country_code { get; set; }
    public string lang_code { get; set; }
    public string device_version { get; set; }
    public string system_version { get; set; }
}
//-----------------------------------------

/// <summary>
/// 道具使用发送
/// </summary>
public class PropUserRequest
{
    /// <summary>
    /// 
    /// </summary>
    public int playerId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int propId { get; set; }
}


public class GetThemeReaquest
{
    public int player_id { get; set; }
}
public class BuyThemeReaquest
{
    public int player_id { get; set; }
    public int theme_id { get; set; }
    public int money { get; set; }
}
public class RandomSkiRequest
{
    public int player_id { get; set; }
}
#endregion