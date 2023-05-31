using Framework.Pool;
using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace Logic.UI.UIMain
{
    /// <summary>
    /// 舱室伙伴
    /// </summary>
    public class RoomPartner : MonoBehaviour, IPoolAssets
    {
        public Action<RoomPartner> onLeaveEnd;
        
        /// <summary>
        /// 所在房间room
        /// </summary>
        public int RoomId { get; private set; }

        /// <summary>
        /// 移动速度
        /// </summary>
        

        public string PoolObjName { get ; set ; }

        RoomPartnerAI ai;


        float speed;

        Transform roomEdgeWorldPoint;

        /// <summary>
        /// 当前动画
        /// </summary>
        Tween tween;   

        /// <summary>
        /// 是否正在移动
        /// </summary>
        public bool IsMoving { get; private set; }

        /// <summary>
        /// 是否在待机中
        /// </summary>
        public bool IsIdle { get; private set; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="ai"></param>
        public void Init(int roomId, RoomPartnerAI ai, Transform roomEdgeX, float speed)
        {
            RoomId = roomId;
            this.ai = ai;
            this.ai.Initialize(this, 10f);
            this.speed = speed;
            this.roomEdgeWorldPoint = roomEdgeX;
        }

        /// <summary>
        /// 移动到指定目标
        /// </summary>
        /// <param name="target"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public void MoveBy(Vector3 target)
        {
            var uiCamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
            //var screenPos = RectTransformUtility.WorldToScreenPoint(uiCamera, transform.position);
            //var worldPos = Camera.main.ScreenToWorldPoint(screenPos)
            float distance = GameObject.Find("UIRoot").GetComponent<Canvas>().planeDistance;
            var screenPos = RectTransformUtility.WorldToScreenPoint(uiCamera, transform.position + target);
            var worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, distance));

            //if ((transform.position + target).x >= roomEdgeWorldPoint.x)
            //{
            //    Debug.LogError("越界了");
            //}

            ////if ((target).x > roomEdgeWorldPoint.x)
            ////{
            ////    var backX = roomEdgeWorldPoint.x - (target.x - roomEdgeWorldPoint.x);
            ////    Sequence s = DOTween.Sequence();
            ////    s.Append(transform.DOMoveX(roomEdgeWorldPoint.x, speed).SetSpeedBased());
            ////    s.Append(transform.DOMoveX(backX, speed).SetSpeedBased());
            ////    s.onComplete = () =>
            ////    {
            ////        IsMoving = false;
            ////    };
            ////}
            ////else
            {
                tween = transform.DOBlendableMoveBy(target, speed).SetSpeedBased();
                //tween = transform.DOMove(roomEdgeWorldPoint.position, speed).SetSpeedBased();
                tween.onComplete = () =>
                {
                    IsMoving = false;
                };
            }

            PlayMoveAnim();

            IsMoving = true;
        }

        public void Idle()
        {
            
            StartCoroutine(WaitIdleSecondes(5f));
            IsIdle = true;
        }


        private IEnumerator WaitIdleSecondes(float delay)
        {
            yield return new WaitForSeconds(delay);
            IsIdle = false;
        }

        private void PlayMoveAnim()
        {
            
        }

        private void OnEnable()
        {
            //测试用
            //StartCoroutine(WaitSecondes(5f));
        }





        private void Update()
        {
            if(this.ai != null)
                this.ai.Tick(Time.deltaTime);
        }




        public void OnRecycle()
        {
            if(this.ai != null)
                this.ai.Dispose();
        }

        public void OnSpawn()
        {
            //throw new NotImplementedException();
        }



        private IEnumerator WaitSecondes(float delay)
        {
            yield return new WaitForSeconds(delay);

            onLeaveEnd?.Invoke(this);
        }
    }
}