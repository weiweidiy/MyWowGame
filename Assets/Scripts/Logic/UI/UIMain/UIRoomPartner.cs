using Cysharp.Threading.Tasks;
using Framework.Extension;
using Logic.Common;
using Logic.Manager;
using Logic.UI.UIMain.Room;
using Networks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Logic.UI.UIMain
{
    /// <summary>
    /// 负责管理舱室里的partner
    /// </summary>
    public class UIRoomPartner : MonoWithEvent
    {
        /// <summary>
        /// 所有房间对象(解锁对象，真正的room是他的父节点)
        /// </summary>
        [SerializeField] RoomBase[] rooms;

        /// <summary>
        /// 舱室空闲点位
        /// </summary>
        Dictionary<int, Dictionary<Transform, bool>> spawnPointMap = new Dictionary<int, Dictionary<Transform, bool>>();
        /// <summary>
        /// 每个房间的边缘点位映射表
        /// </summary>
        Dictionary<int, Transform> edgeXMap = new Dictionary<int, Transform>();

        /// <summary>
        /// 空闲的伙伴
        /// </summary>
        List<GamePartnerData> idlePartnersData;

        private void Start()
        {
            idlePartnersData = new List<GamePartnerData>();

            m_EventGroup.Register(LogicEvent.RoomUnlocked, (i, o) =>
            {
                OnRoomUnlocked((int)o);
            });

            Initialize();
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        async void Initialize()
        {
            //初始化伙伴数据
            var partnersData = PartnerManager.Ins.GetAllPartners();
            if (partnersData.Count == 0)
                return;

            foreach (var partner in partnersData)
            {
                idlePartnersData.Add(partner);
            }

            //初始化舱室数据
            var rooms = RoomManager.Ins.GetUnlockedRooms();
            if (rooms.Count == 0)
                return;

            await InitRoom(rooms);

            TrySpawnPartner();
        }

        /// <summary>
        /// 初始化房间数据
        /// </summary>
        /// <param name="rooms"></param>
        /// <returns></returns>
        async UniTask InitRoom(HashSet<int> rooms)
        {
            //等待1帧，等待ui刷新后才能拿到正确的坐标信息
            await UniTask.DelayFrame(1);

            foreach (var roomId in rooms)
            {
                InitRoomSpawnPoints(roomId);
            } 
        }

        /// <summary>
        /// 初始化房间出生点
        /// </summary>
        /// <param name="roomId"></param>
        void InitRoomSpawnPoints(int roomId)
        {
            if (!spawnPointMap.ContainsKey(roomId))
                spawnPointMap.Add(roomId, new Dictionary<Transform, bool>());

            var room = GetRoom(roomId);
            foreach (var point in room.m_RoomPartnerSpawnPoint)
            {
                if(!spawnPointMap[roomId].ContainsKey(point.transform))
                    spawnPointMap[roomId].Add(point.transform, true);
            }

            if(!edgeXMap.ContainsKey(roomId))
            {
                edgeXMap.Add(roomId, room.m_RoomEdgePoint.transform);
            }
        }


        #region manager 消息响应
        /// <summary>
        /// 有房间解锁了
        /// </summary>
        /// <param name="o"></param>
        private void OnRoomUnlocked(int roomId)
        {
            InitRoomSpawnPoints(roomId);
            TrySpawnPartner(roomId);
        }

        #endregion


        /// <summary>
        /// 生成roompartner
        /// </summary>
        void TrySpawnPartner()
        {
            //如果已解锁的舱室里有空位置，同时有空余伙伴，则创建一个伙伴进入舱室
            //获取所有空闲的伙伴
            if (idlePartnersData.Count == 0) return;

            //获取所有已解锁舱室
            var rooms = RoomManager.Ins.GetUnlockedRooms();
            if (rooms.Count == 0)
                return;

            foreach(var roomId in rooms)
            {
                //同步方式执行
                TrySpawnPartner(roomId);
            }
        }

        /// <summary>
        /// 尝试在一个room中刷新一个roompartner
        /// </summary>
        /// <param name="roomId"></param>
        async void TrySpawnPartner(int roomId)
        {
            //获取空的舱室点
            var idlePoint = GetIdleRoomPoint(roomId);
            if (idlePoint == null)
            {
                Debug.LogError("舱室没有空闲点了 " + roomId);
                return;
            }

            //随机获取一个伙伴
            var partner = GetRandomPartnerData(idlePartnersData);

            //创建舱室伙伴对象
            var roomPartner = await SpawnRoomPartner(partner, idlePoint);
            roomPartner.Init(roomId, GetAI(), GetRoomEdgePoint(roomId),1f);
            roomPartner.onLeaveEnd = (sender)=> 
            {
                sender.transform.localScale = Vector3.one;
                //回收
                GameObjectSpawnManager.Ins.Recycle(sender);
                //设置舱室空闲
                SetIdleRoomPoint(roomId, idlePoint, true);
            };
            

            //更新舱室空闲状态
            SetIdleRoomPoint(roomId, idlePoint, false);
        }



        /// <summary>
        /// 设置指定舱室idle点位状态
        /// </summary>
        /// <param name="idlePoint"></param>
        /// <param name="idle"></param>
        private void SetIdleRoomPoint(int roomId, Transform idlePoint, bool idle)
        {
            if (!spawnPointMap.ContainsKey(roomId))
                throw new System.Exception("没有找到 roomId " + roomId);

            if(!spawnPointMap[roomId].ContainsKey(idlePoint))
                throw new System.Exception("没有找到 idlePoint " + idlePoint.name);

            spawnPointMap[roomId][idlePoint] = idle;
        }


        /// <summary>
        /// 在指定位置创建一个roompartner游戏对象;
        /// </summary>
        /// <param name="partner"></param>
        /// <param name="spawnPosition"></param>
        /// <returns></returns>
        private async UniTask<RoomPartner> SpawnRoomPartner(GamePartnerData partner, Transform point)
        {
            string resPath = "RoomPartner_01"; //"Enemy_01";// 

            var roomPartner = await GameObjectSpawnManager.Ins.Spawn<RoomPartner>(resPath);
            roomPartner.transform.SetParent(point);
            roomPartner.transform.localPosition = Vector3.zero;
            roomPartner.transform.localScale = Vector3.one;
            return roomPartner;
        }

        /// <summary>
        /// 获取舱室空闲点
        /// </summary>
        /// <param name="idlePartners"></param>
        /// <returns></returns>
        private GamePartnerData GetRandomPartnerData(List<GamePartnerData> idlePartners)
        {
            return idlePartners[0];
        }

        #region Get方法
        /// <summary>
        /// 获取指定房间空的节点
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        private Transform GetIdleRoomPoint(int roomId)
        {
            if (!spawnPointMap.ContainsKey(roomId))
                throw new System.Exception("没有找到spawnPointMap roomId " + roomId);

            var keys = spawnPointMap[roomId].Keys;
            foreach(var transform in keys)
            {
                if (spawnPointMap[roomId][transform] == true)
                    return transform;
            }

            return null;
        }

        /// <summary>
        /// 获取房间边界X坐标
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        private Transform GetRoomEdgePoint(int roomId)
        {
            if(!edgeXMap.ContainsKey(roomId))
                throw new System.Exception("没有找到edgeXMap roomId " + roomId);

            return edgeXMap[roomId];
        }

        /// <summary>
        /// 根据
        /// </summary>
        /// <param name="lockType"></param>
        /// <returns></returns>
        private RoomBase GetRoom(int roomId)
        {
            foreach (var room in rooms)
            {
                if (room.m_RoomId.Equals(roomId))
                    return room;
            }
            return null;
        }

        /// <summary>
        /// 获取ai
        /// </summary>
        /// <returns></returns>
        private RoomPartnerAI GetAI()
        {
            var ai = new RoomPartnerAI();
            return ai;
        }
        #endregion

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.LogError("已解锁 " + RoomManager.Ins.GetUnlockedRooms().Count);
                TrySpawnPartner();
            }

        }
    }
}