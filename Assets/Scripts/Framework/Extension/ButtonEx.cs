using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Framework.Extension
{
	/*
	 * ButtonEx扩展按钮组件
	 *
	 * 【功能】
	 *     - 多种按钮交互事件
	 *     - Button无损转ButtonEx（Button右上角三个点设置里调用）
	 */

	/// <summary>
	/// 扩展按钮组件
	/// </summary>
	[AddComponentMenu("UI/ButtonEx")]
	public class ButtonEx : Button
	{		/// <summary>
		/// 按钮交互事件枚举
		/// </summary>
		[System.Flags]
		public enum EventType
		{
			Click = 1 << 0,
			LongClick = 1 << 1,
			Down = 1 << 2,
			Up = 1 << 3,
			Enter = 1 << 4,
			Exit = 1 << 5,
			DoubleClick = 1 << 6,
		}

		[SerializeField] private EventType m_EventType = EventType.Click;

		/// <summary>
		/// 长按判定时间
		/// </summary>
		[SerializeField] private float onLongWaitTime = 1.5f;

		/// <summary>
		/// 是否重复抛出长按事件（false：长按onLongWaitTime后只触发一次onLongClick  true：从onDown起，每onLongWaitTime触发一次onLongClick）
		/// </summary>
		[SerializeField] private bool onLongContinue = false;

		/// <summary>
		/// 双击判定时间（两次OnDown的间隔时间小于此值即判定为一次双击，但完全不影响onClick的触发）
		/// </summary>
		[SerializeField] private float onDoubleClickTime = 0.5f;


		/*[SerializeField]
		private ButtonClickedEvent m_OnClick = new ButtonClickedEvent(); //点击事件*/

		[SerializeField] private Button.ButtonClickedEvent m_OnLongClick = new Button.ButtonClickedEvent(); //长按事件（触发一次）

		[SerializeField] private Button.ButtonClickedEvent m_OnDown = new Button.ButtonClickedEvent(); //按下事件

		[SerializeField] private Button.ButtonClickedEvent m_OnUp = new Button.ButtonClickedEvent(); //抬起事件

		[SerializeField] private Button.ButtonClickedEvent m_OnEnter = new Button.ButtonClickedEvent(); //进入事件

		[SerializeField] private Button.ButtonClickedEvent m_OnExit = new Button.ButtonClickedEvent(); //移出事件

		[SerializeField] private Button.ButtonClickedEvent m_onDoubleClick = new Button.ButtonClickedEvent(); //双击事件


		private Coroutine log;

		private bool isPointerDown = false;
		private bool isPointerInside = false;

		public Vector2 Scale = new Vector2(0.95f, 0.95f);
		public float Interval = 0.15f;

		private RectTransform m_RectTrans;
		private Vector2 m_OriSize;
		private Vector2 m_ScaleTo;
		protected override void Awake()
		{
			base.Awake();
			
			m_RectTrans = GetComponent<RectTransform>();
			m_OriSize = m_RectTrans.localScale;
			m_ScaleTo = m_OriSize * Scale;
		}

		#region 对外属性

		/// <summary>
		/// 是否被按下
		/// </summary>
		public bool isDown
		{
			get { return isPointerDown; }
		}

		/// <summary>
		/// 是否进入
		/// </summary>
		public bool isEnter
		{
			get { return isPointerInside; }
		}

		/*/// <summary>
		/// 点击事件
		/// </summary>
		public ButtonClickedEvent onClick
		{
		    get { return m_OnClick; }
		    set { m_OnClick = value; }
		}*/

		/// <summary>
		/// 长按事件
		/// </summary>
		public Button.ButtonClickedEvent onLongClick
		{
			get { return m_OnLongClick; }
			set { m_OnLongClick = value; }
		}

		/// <summary>
		/// 双击事件
		/// </summary>
		public Button.ButtonClickedEvent onDoubleClick
		{
			get { return m_onDoubleClick; }
			set { m_onDoubleClick = value; }
		}

		/// <summary>
		/// 按下事件
		/// </summary>
		public Button.ButtonClickedEvent onDown
		{
			get { return m_OnDown; }
			set { m_OnDown = value; }
		}

		/// <summary>
		/// 松开事件
		/// </summary>
		public Button.ButtonClickedEvent onUp
		{
			get { return m_OnUp; }
			set { m_OnUp = value; }
		}

		/// <summary>
		/// 进入事件
		/// </summary>
		public Button.ButtonClickedEvent onEnter
		{
			get { return m_OnEnter; }
			set { m_OnEnter = value; }
		}

		/// <summary>
		/// 离开事件
		/// </summary>
		public Button.ButtonClickedEvent onExit
		{
			get { return m_OnExit; }
			set { m_OnExit = value; }
		}

		#endregion


		private float lastClickTime;

		private void Down()
		{
			if (!IsActive() || !IsInteractable())
				return;
			m_OnDown.Invoke();
			if (lastClickTime != 0 && Time.time - lastClickTime <= onDoubleClickTime)
			{
				DoubleClick();
			}

			lastClickTime = Time.time;
			log = StartCoroutine(grow());
		}

		private void Up()
		{
			if (!IsActive() || !IsInteractable() || !isDown)
				return;
			m_OnUp.Invoke();
			if (log != null)
			{
				StopCoroutine(log);
				log = null;
			}
		}

		private void Enter()
		{
			if (!IsActive())
				return;
			m_OnEnter.Invoke();
		}

		private void Exit()
		{
			if (!IsActive() || !isEnter)
				return;
			m_OnExit.Invoke();
		}

		private void LongClick()
		{
			if (!IsActive() || !isDown)
				return;
			m_OnLongClick.Invoke();
		}

		private void DoubleClick()
		{
			if (!IsActive() || !isDown)
				return;
			m_onDoubleClick.Invoke();
		}

		private float downTime = 0f;

		private IEnumerator grow()
		{
			downTime = Time.time;
			while (isDown)
			{
				if (Time.time - downTime > onLongWaitTime)
				{
					m_RectTrans.localScale = m_OriSize;
					StartCoroutine(OnDown(Interval));
					LongClick();
					if (onLongContinue)
						downTime = Time.time;
					else
						break;
				}
				else
					yield return null;
			}
		}

		protected override void OnDisable()
		{
			isPointerDown = false;
			isPointerInside = false;
			base.OnDisable();
		}

		/*private void ExPress()
		{
		    if (!IsActive() || !IsInteractable())
		        return;

		    UISystemProfilerApi.AddMarker("Button.onClick", this);
		    onClick.Invoke();
		}

		public virtual void OnPointerClick(PointerEventData eventData)
		{
		    if (eventData.button != PointerEventData.InputButton.Left)
		        return;
		    ExPress();
		}*/

		public override void OnPointerDown(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
				return;
			isPointerDown = true;
			Down();
			StartCoroutine(OnDown(Interval));
			base.OnPointerDown(eventData);
		}

		public override void OnPointerUp(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
				return;
			Up();
			StopAllCoroutines();
			StartCoroutine(OnUp(Interval));
			isPointerDown = false;
			base.OnPointerUp(eventData);
		}

		public override void OnPointerEnter(PointerEventData eventData)
		{
			base.OnPointerEnter(eventData);
			isPointerInside = true;
			Enter();
		}

		public override void OnPointerExit(PointerEventData eventData)
		{
			Exit();
			isPointerInside = false;
			base.OnPointerExit(eventData);
		}

		#region Button->ButtonEx转换相关

#if UNITY_EDITOR

		[MenuItem("CONTEXT/Button/Convert To ButtonEx", true)]
		static bool _ConvertToButtonEx(MenuCommand command)
		{
			return CanConvertTo<ButtonEx>(command.context);
		}

		[MenuItem("CONTEXT/Button/Convert To ButtonEx", false)]
		static void ConvertToButtonEx(MenuCommand command)
		{
			ConvertTo<ButtonEx>(command.context);
		}

		[MenuItem("CONTEXT/Button/Convert To Button", true)]
		static bool _ConvertToButton(MenuCommand command)
		{
			return CanConvertTo<Button>(command.context);
		}

		[MenuItem("CONTEXT/Button/Convert To Button", false)]
		static void ConvertToButton(MenuCommand command)
		{
			ConvertTo<Button>(command.context);
		}

		protected static bool CanConvertTo<T>(Object context)
			where T : MonoBehaviour
		{
			return context && context.GetType() != typeof(T);
		}

		protected static void ConvertTo<T>(Object context) where T : MonoBehaviour
		{
			var target = context as MonoBehaviour;
			var so = new SerializedObject(target);
			so.Update();

			bool oldEnable = target.enabled;
			target.enabled = false;

			// Find MonoScript of the specified component.
			foreach (var script in Resources.FindObjectsOfTypeAll<MonoScript>())
			{
				if (script.GetClass() != typeof(T))
					continue;

				// Set 'm_Script' to convert.
				so.FindProperty("m_Script").objectReferenceValue = script;
				so.ApplyModifiedProperties();
				break;
			}

			(so.targetObject as MonoBehaviour).enabled = oldEnable;
		}
#endif

		#endregion

		#region 缓动

		private IEnumerator OnDown(float interval)
		{
			float timer = 0;
			while (timer <= interval) {
				if (interval == 0) break;
				m_RectTrans.localScale = Vector2.Lerp(m_OriSize, m_ScaleTo, timer / interval);
				timer += Time.unscaledDeltaTime;
				yield return null;
			}
			m_RectTrans.localScale = m_ScaleTo;
		}
		
		private IEnumerator OnUp(float interval)
		{
			float timer = 0;
			while (timer <= interval) {
				if (interval == 0) break;
				var t = timer / interval;
				m_RectTrans.localScale = new Vector2(
					OutBack(t, m_ScaleTo.x, m_OriSize.x),
					OutBack(t, m_ScaleTo.y, m_OriSize.y)
				);
				timer += Time.unscaledDeltaTime;
				yield return null;
			}
			m_RectTrans.localScale = m_OriSize;
		}
        
		// outback 缓动曲线的实现
		public static float OutBack(float t, float startValue, float endValue, float overshoot = 10)
		{
			t -= 1f;
			float postFix = t;
			return startValue + (endValue - startValue) * (postFix * t * ((overshoot + 1f) * t + overshoot) + 1f);
		}

		#endregion
	}
}
