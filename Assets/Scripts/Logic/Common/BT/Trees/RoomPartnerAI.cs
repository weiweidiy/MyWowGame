
using FluentBehaviourTree;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace Logic.UI.UIMain
{
    public class RoomPartnerAI :  IDisposable
    {
        /// <summary>
        /// 宿主对象
        /// </summary>
        public RoomPartner Host { get; private set; }

        /// <summary>
        /// 巡逻的x最大值
        /// </summary>
        //public float EdgeX { get; private set; }

        /// <summary>
        /// 是否紧急撤离
        /// </summary>
        public bool IsEmergencyLeave { get; private set; }

        /// <summary>
        /// 巡逻时间
        /// </summary>
        public float PatrolDuration { get; private set; }
        


        IBehaviourTreeNode tree;

        /// <summary>
        /// 流逝时间
        /// </summary>
        float elapseTime = 0f;

        public void Initialize(RoomPartner host, float duration)
        {
            Host = host;
            PatrolDuration = duration;
            BuildAI();
        }

        void BuildAI()
        {
            var builder = new BehaviourTreeBuilder();
            this.tree = builder
                        .Selector("Root")
                            .Selector("Patrol")
                                .Condition("EmergencyLeave", t => { return IsEmergencyLeave; })
                                .Sequence("Patrol")
                                    .Condition("WithInTime", t => { return ConditionWithInTime(); })
                                    .Sequence("Patrol")
                                        .Do("Move", t => { return DoMove(); })
                                        .Do("Moving", t => { return DoCheckMoving(); })
                                        .Do("Idle", t => { return DoIdle(); })
                                        .Do("Idling", t => { return DoCheckIdling(); })
                                    .End()
                                .Do("Leave", t=> { return DoLeave(); })
                            .Do("EmergencyLeave", t => { return DoEmergencyLeave(); })
                        .Build();
        
        }



        /// <summary>
        /// 是否在有效巡逻时间内
        /// </summary>
        /// <returns></returns>
        private bool ConditionWithInTime()
        {
            Debug.LogError("ConditionWithInTime ");
            return elapseTime >= PatrolDuration;
        }

        bool needCheckMoving = false;
        /// <summary>
        /// 移动
        /// </summary>
        /// <returns></returns>
        private BehaviourTreeStatus DoMove()
        {
            if(!Host.IsMoving && !needCheckMoving && !Host.IsIdle)
            {
                Debug.LogError("DoMove");
                var target = GetMoveDistance();
                Host.MoveBy(target);
                needCheckMoving = true;
            }
            return BehaviourTreeStatus.Success;
        }

        /// <summary>
        /// 检查移动状态
        /// </summary>
        /// <returns></returns>
        private BehaviourTreeStatus DoCheckMoving()
        {
            if(!needCheckMoving)
                return BehaviourTreeStatus.Success;

            if (Host.IsMoving)
            {
                return BehaviourTreeStatus.Running;
            }
            else
            {
                Debug.LogError("DoMoving done");
                needCheckMoving = false;
                return BehaviourTreeStatus.Success;
            }
        }


        bool needCheckIdle = false;
        private BehaviourTreeStatus DoIdle()
        {
            if(!Host.IsIdle && !needCheckIdle)
            {
                Debug.LogError("DoIdle");
                Host.Idle();
                needCheckIdle = true;
            }

            return BehaviourTreeStatus.Success;
        }

        private BehaviourTreeStatus DoCheckIdling()
        {
            if (!needCheckIdle)
                return BehaviourTreeStatus.Success;

            if (Host.IsIdle)
            {
                return BehaviourTreeStatus.Running;
            }
            else
            {
                Debug.LogError("DoIdle done");
                needCheckIdle = false;
                return BehaviourTreeStatus.Success;
            }
        }


        /// <summary>
        /// 获取移动目标点
        /// </summary>
        /// <returns></returns>
        private Vector3 GetMoveDistance()
        {
            Debug.LogError("GetTarget");
            return new Vector3(3f,0,0);
        }

        private BehaviourTreeStatus DoEmergencyLeave()
        {
            Debug.LogError("DoEmergencyLeave");
            return BehaviourTreeStatus.Success;
        }

        private BehaviourTreeStatus DoLeave()
        {
            Debug.LogError("DoLeave");
            return BehaviourTreeStatus.Success;
        }







        public void Tick(float deltaTime)
        {
            if (this.tree != null)
            {
                this.tree.Tick(new TimeData(deltaTime));
            }

            elapseTime += deltaTime;

        }

        public void Dispose()
        {
            //Debug.Log(" BT Dispose");
            tree = null;
            Host = null;
            elapseTime = 0f;
            PatrolDuration = 0f;
            IsEmergencyLeave = false;

        }

    }
}



//this.tree = builder
//    .Selector("Root")
//        .Selector("Node")
//            //接任务
//            .Sequence("AcceptQuestSeq")
//                .Condition("IsUnAcceptedQuest" , t=> { return IsUnAccepted(); })
//                .Selector("AcceptQuestSel")
//                    .Sequence("DirectAccept")
//                        .Condition("IsDirectAccept" , t=> { return IsDirectAccept(); })
//                        .Do("AcceptQuest", t => { return AcceptQuest(); })
//                    .End()
//                    .Sequence("FindAcceptNpc")
//                        .Do("FindAcceptNpc", t => { return FindAcceptNpc(); })
//                        .Do("Navigate", t => { return Navigate(); })
//                        .Do("FindAcceptDialog", t =>{ return FindAcceptDialog(); })
//                        .Do("Talk", t => { return Talk(); })
//                    .End()
//                .End()
//                .Do("AcceptQuest", t => { return AcceptQuest(); })
//                .Do("End", t => { Debug.Log("End1"); return End(); })
//            .End()

//            //做任务
//            .Sequence("DoQuestSeq")
//                .Condition("IsDoingQuest" ,t=> { return IsDoingQuest(); })
//                .Selector("DoQuestSel")
//                    .Condition("IsDoingNothing", t=> { return IsDoingNothing(); })
//                    .Sequence("OpenUISeq")
//                        .Condition("IsOpenningUI", t => { return IsOpenningUI(); })
//                        .Do("ExecuteOpenUI", t => { return ExecuteOpenUI(); })
//                    .End()
//                    .Sequence("FindObjectiveNpcSeq")
//                        .Do("FindObjectiveNpc" , t => { return FindObjectiveNpcPoint(); })
//                        .Do("Navigate", t => { return Navigate(); })
//                        .Selector("DoSomething")
//                            .Sequence("TalkSeq")
//                                .Condition("IsDialogQuest", t => { return IsDialogQuest(); })
//                                .Do("FindObjectiveDialog", t => { return FindObjectiveDialog(); })
//                                .Do("Talk", t => { return Talk(); })
//                                .Do("Done", t => { isDoing = false; return BehaviourTreeStatus.Success; })
//                            .End()
//                            .Do("Other", t => { return BehaviourTreeStatus.Success; })
//                        .End()
//                    .End()
//                .End()
//                .Do("End", t => { Debug.Log("End2"); return End(); })
//            .End()

//            //交任务
//            .Sequence("SubmitQuestSeq")
//                .Condition("IsCompleteQuest" , t => { return IsCompletedQuest(); })
//                .Selector("SubmitQuestSel")
//                    .Sequence("DirectSubmit")
//                        .Condition("IsDirectSubmit" , t => { return IsDirectSubmit(); })
//                        .Do("SubmitQuest", t => { return SubmitQuest(); })
//                    .End()
//                    .Sequence("FindSubmitNpcSeq")
//                        .Do("FindSubmitNpc", t => { return FindSubmitNpc(); })
//                        .Do("Navigate", t => { return Navigate(); })
//                        .Do("FindSubmitDialog", t => { return FindSubmitDialog(); })
//                        .Do("Talk", t => { return Talk(); })
//                    .End()
//                .End()
//                .Do("SubmitQuest" , t => { return SubmitQuest(); })
//                .Do("End", t => { Debug.Log("End3");  return End(); })
//            .End()
//        .End()
//        .Do("End", t => { Debug.Log("End4"); return End(); })
//    .End()
//.Build();







//private BehaviourTreeStatus FindObjectiveDialog()
//{
//    //dialogId = helper.GetObjectiveDialogId(questId);
//    return BehaviourTreeStatus.Success;
//}

//private BehaviourTreeStatus FindAcceptDialog()
//{
//    //dialogId = helper.GetAcceptDialogId(questId);
//    return BehaviourTreeStatus.Success;
//}

//private BehaviourTreeStatus FindSubmitDialog()
//{
//    //dialogId = helper.GetSubmitDialogId(questId);
//    return BehaviourTreeStatus.Success;
//}


///// <summary>
///// 执行UI命令
///// </summary>
///// <returns></returns>
//private BehaviourTreeStatus ExecuteOpenUI()
//{
//    //string command = helper.GetCommand(questId);
//   // ApplicationFacade.Instance.SendNotification(command, helper.GetCommandKeys(questId)); // to do: 可能有参数

//    return BehaviourTreeStatus.Success;
//}

//private bool IsOpenningUI()
//{
//    //string command = helper.GetCommand(questId);

//    return  !IsDoingNothing();
//}

///// <summary>
///// 是否啥都不做
///// </summary>
///// <returns></returns>
//private bool IsDoingNothing()
//{
//    //string command = helper.GetCommand(questId);
//    return false; // command.Equals("");
//}

///// <summary>
///// 是否直接领取任务
///// </summary>
///// <returns></returns>
//private bool IsDirectAccept()
//{
//    //int npcId = helper.GetAcceptNpcId(questId);
//    return false;// npcId.Equals(-1);
//}

//private bool IsDirectSubmit()
//{
//    //int npcId = helper.GetSubmitNpcId(questId);
//    return false;// npcId.Equals(-1);
//}


//private bool IsDialogQuest()
//{
//    //int objType = helper.GetObjectiveType(questId);
//    return false;// objType.Equals((int)ObjectiveType.CompleteDialog);
//}



///// <summary>
///// 是否是已接受的任务
///// </summary>
///// <returns></returns>
//bool IsUnAccepted()
//{
//    return false; // !proxy.IsAccepted(questId);
//}

///// <summary>
///// 是否有导航
///// </summary>
///// <returns></returns>
//private bool IsSkipNavigate()
//{
//    return navData == null;
//}

///// <summary>
///// 是否是进行中的任务
///// </summary>
///// <returns></returns>
//bool IsDoingQuest()
//{
//    return false;
//    //Debugger.Log(" IsDoingQuest " + proxy.QueryStatus(questId).Equals(QuestStatus.Doing));
//    //if(proxy.QueryStatus(questId).Equals(QuestStatus.Doing) == true)
//    //{
//    //    isDoing = true;
//    //}

//    //if(IsDialogQuest() == true)
//    //{
//    //    return proxy.QueryStatus(questId).Equals(QuestStatus.Doing) || isDoing == true;
//    //}
//    //else
//    //{
//    //    return proxy.QueryStatus(questId).Equals(QuestStatus.Doing);
//    //}

//}

///// <summary>
///// 是否完成
///// </summary>
///// <returns></returns>
//private bool IsCompletedQuest()
//{
//    return false;// proxy.QueryStatus(questId).Equals(QuestStatus.Complete);
//}


///// <summary>
///// 查找接任务NPC
///// </summary>
///// <returns></returns>
//BehaviourTreeStatus FindAcceptNpc()
//{
//    //if(navData == null)
//    //{
//    //    int npcId = helper.GetAcceptNpcId(questId);
//    //    navData = helper.GetNavByNpc(npcId);
//    //}
//    return BehaviourTreeStatus.Success;
//}

///// <summary>
///// 查找完成任务NPC
///// </summary>
///// <returns></returns>
//private BehaviourTreeStatus FindObjectiveNpcPoint()
//{
//    //如果任务目标类型需要导航，则开始导航，否则返回失败
//    //if (navData == null)
//    //{
//    //    navData = helper.GetNavDataFromObjective(questId);
//    //}
//    return BehaviourTreeStatus.Success;
//}

///// <summary>
///// 查找提交任务npc
///// </summary>
///// <returns></returns>
//private BehaviourTreeStatus FindSubmitNpc()
//{
//    //if (navData == null)
//    //{
//    //    int npcId = helper.GetSubmitNpcId(questId);
//    //    navData = helper.GetNavByNpc(npcId);
//    //}
//    return BehaviourTreeStatus.Success;
//}

///// <summary>
///// 接任务
///// </summary>
///// <returns></returns>
//private BehaviourTreeStatus AcceptQuest()
//{ 
//    //领取觉醒任务
//    //if (questId == 9011 && !GameNet.Instance.Node.GetFieldBool(GameNet.Instance.Role, RoleEntity.Fields.CAN_CAREER_AWAKEN))
//    //{
//    //    var globalConfig = BsonConfigs.Get("global");
//    //    int level = globalConfig[GlobalDefine.CAREER_AWAKEN_LEVEL].AsInt32;
//    //    int questID = globalConfig[GlobalDefine.CAREER_AWAKEN_QUEST].AsInt32;
//    //    BsonDocument quest = BsonConfigs.Get("quest", questID);
//    //    Tips.DisplayText("需要达到" + level + "级并且完成任务" + quest["name"].AsString + "才可领取任务");
//    //    return BehaviourTreeStatus.Failure;
//    //}
//    //BsonDocument doc = new BsonDocument();
//    //doc["id"] =questId;
//    //ApplicationFacade.Instance.SendNotification(nameof(AcceptQuestCommand), doc);
//    return BehaviourTreeStatus.Success;
//}

///// <summary>
///// 提交任务
///// </summary>
///// <returns></returns>
//private BehaviourTreeStatus SubmitQuest()
//{
//    //Debugger.Log("SubmitQuest");
//    //BsonDocument doc = new BsonDocument();
//    //doc["id"] = questId;
//    //doc["rewardId"] = rewardId;
//    //ApplicationFacade.Instance.SendNotification(nameof(SubmitQuestCommand), doc);
//    //觉醒完成
//    //if (questId == 9015&& !GameNet.Instance.Node.GetFieldBool(GameNet.Instance.Role, RoleEntity.Fields.CAREER_AWAKEN))
//    //{
//    //    ApplicationFacade.Instance.SendNotification(nameof(OpenAwakeningSuccessTipsCommand));
//    //}
//    return BehaviourTreeStatus.Success;
//}

///// <summary>
///// 对话
///// </summary>
///// <returns></returns>
//BehaviourTreeStatus Talk()
//{
//    if (isTalkingComplete == true)
//    {
//        return BehaviourTreeStatus.Success;
//    }

//    return BehaviourTreeStatus.Running;
//}

//BehaviourTreeStatus End()
//{
//    isEnd = true;
//    return BehaviourTreeStatus.Success;
//}

///// <summary>
///// 导航
///// </summary>
///// <returns></returns>
//BehaviourTreeStatus Navigate()
//{

//    return BehaviourTreeStatus.Running;
//}
