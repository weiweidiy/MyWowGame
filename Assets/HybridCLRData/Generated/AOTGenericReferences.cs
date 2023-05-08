public class AOTGenericReferences : UnityEngine.MonoBehaviour
{

	// {{ AOT assemblies
	// DOTween.dll
	// System.Core.dll
	// UniTask.dll
	// UnityEngine.CoreModule.dll
	// YooAsset.dll
	// mscorlib.dll
	// }}

	// {{ constraint implement type
	// }} 

	// {{ AOT generic types
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>
	// Cysharp.Threading.Tasks.UniTask<object>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<object>
	// DG.Tweening.Core.DOGetter<UnityEngine.Vector2>
	// DG.Tweening.Core.DOGetter<float>
	// DG.Tweening.Core.DOGetter<int>
	// DG.Tweening.Core.DOSetter<int>
	// DG.Tweening.Core.DOSetter<float>
	// DG.Tweening.Core.DOSetter<UnityEngine.Vector2>
	// System.Action<byte>
	// System.Action<object>
	// System.Action<float>
	// System.Action<int,object>
	// System.Action<object,Logic.Common.ItemType>
	// System.Action<UnityEngine.Vector2,object>
	// System.Action<UnityEngine.Vector3,object>
	// System.Collections.Generic.Dictionary<object,LitJson.ObjectMetadata>
	// System.Collections.Generic.Dictionary<Logic.Common.MiningType,object>
	// System.Collections.Generic.Dictionary<object,LitJson.PropertyMetadata>
	// System.Collections.Generic.Dictionary<object,object>
	// System.Collections.Generic.Dictionary<long,object>
	// System.Collections.Generic.Dictionary<int,object>
	// System.Collections.Generic.Dictionary<object,LitJson.ArrayMetadata>
	// System.Collections.Generic.Dictionary<int,int>
	// System.Collections.Generic.Dictionary.Enumerator<int,int>
	// System.Collections.Generic.Dictionary.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection<long,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<long,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,object>
	// System.Collections.Generic.HashSet<Framework.EventKit.EventTicket>
	// System.Collections.Generic.HashSet.Enumerator<Framework.EventKit.EventTicket>
	// System.Collections.Generic.ICollection<LitJson.PropertyMetadata>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ICollection<object>
	// System.Collections.Generic.IDictionary<object,LitJson.PropertyMetadata>
	// System.Collections.Generic.IDictionary<object,object>
	// System.Collections.Generic.IDictionary<int,object>
	// System.Collections.Generic.IDictionary<object,LitJson.ObjectMetadata>
	// System.Collections.Generic.IDictionary<object,LitJson.ArrayMetadata>
	// System.Collections.Generic.IEnumerable<ushort>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerable<LitJson.PropertyMetadata>
	// System.Collections.Generic.IEnumerator<object>
	// System.Collections.Generic.IEnumerator<ushort>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerator<LitJson.PropertyMetadata>
	// System.Collections.Generic.IList<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IList<object>
	// System.Collections.Generic.IReadOnlyCollection<int>
	// System.Collections.Generic.IReadOnlyList<int>
	// System.Collections.Generic.KeyValuePair<int,int>
	// System.Collections.Generic.KeyValuePair<object,object>
	// System.Collections.Generic.KeyValuePair<int,object>
	// System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.List<LitJson.PropertyMetadata>
	// System.Collections.Generic.List<int>
	// System.Collections.Generic.List<object>
	// System.Collections.Generic.List.Enumerator<int>
	// System.Collections.Generic.List.Enumerator<object>
	// System.Collections.Generic.Queue<object>
	// System.Collections.Generic.Stack<int>
	// System.Collections.Generic.Stack<object>
	// System.EventHandler<object>
	// System.Func<object,byte>
	// System.Func<int,byte>
	// System.Func<object,object>
	// System.Func<float,float>
	// System.Func<object,object,object>
	// System.IComparable<BreakInfinity.BigDouble>
	// System.IEquatable<BreakInfinity.BigDouble>
	// System.IEquatable<object>
	// System.Nullable<byte>
	// System.Nullable<UnityEngine.Color>
	// System.Predicate<object>
	// System.ValueTuple<int,byte>
	// System.ValueTuple<Logic.Common.ItemType,int>
	// System.ValueTuple<object,object>
	// System.ValueTuple<int,object>
	// System.ValueTuple<byte,int,UnityEngine.Vector3>
	// System.ValueTuple<int,int,object>
	// System.ValueTuple<Logic.Common.MiningType,int,int>
	// System.ValueTuple<int,int,int,int,int>
	// UnityEngine.Events.UnityAction<BreakInfinity.BigDouble>
	// UnityEngine.Events.UnityAction<int>
	// UnityEngine.Events.UnityAction<Logic.Fight.Data.FightDamageData>
	// UnityEngine.Events.UnityAction<byte>
	// UnityEngine.Events.UnityAction<int,object>
	// UnityEngine.Events.UnityEvent<object>
	// UnityEngine.Events.UnityEvent<byte>
	// UnityEngine.Events.UnityEvent<float>
	// UnityEngine.Events.UnityEvent<int>
	// UnityEngine.Events.UnityEvent<BreakInfinity.BigDouble>
	// UnityEngine.Events.UnityEvent<Logic.Fight.Data.FightDamageData>
	// UnityEngine.Events.UnityEvent<int,int>
	// }}

	public void RefMethods()
	{
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Logic.Config.ConfigManager.<LoadAllConfigs>d__21>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Logic.Config.ConfigManager.<LoadAllConfigs>d__21&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<Logic.Config.ConfigManager.<LoadAllConfigs>d__21>(Logic.Config.ConfigManager.<LoadAllConfigs>d__21&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Framework.Pool.FightAssetsPool.<Spawn>d__2>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Framework.Pool.FightAssetsPool.<Spawn>d__2&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Logic.Fight.Common.FightEnemySpawnManager.<SpawnEnemy>d__3>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Logic.Fight.Common.FightEnemySpawnManager.<SpawnEnemy>d__3&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Framework.Pool.FightAssetsPool.<Spawn>d__2>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Framework.Pool.FightAssetsPool.<Spawn>d__2&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Framework.Pool.FightAssetsPool.<CheckHandle>d__3>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Framework.Pool.FightAssetsPool.<CheckHandle>d__3&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Framework.UI.UIManager.<OpenUI>d__32>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Framework.UI.UIManager.<OpenUI>d__32&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.Start<Framework.UI.UIManager.<OpenUI>d__30<object>>(Framework.UI.UIManager.<OpenUI>d__30<object>&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.Start<Framework.Pool.FightAssetsPool.<CheckHandle>d__3>(Framework.Pool.FightAssetsPool.<CheckHandle>d__3&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.Start<Framework.Pool.FightAssetsPool.<Spawn>d__2>(Framework.Pool.FightAssetsPool.<Spawn>d__2&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.Start<Framework.UI.UIManager.<OpenUI>d__32>(Framework.UI.UIManager.<OpenUI>d__32&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.Start<Logic.Fight.Common.FightEnemySpawnManager.<SpawnEnemy>d__3>(Logic.Fight.Common.FightEnemySpawnManager.<SpawnEnemy>d__3&)
		// Cysharp.Threading.Tasks.UniTask.Awaiter Cysharp.Threading.Tasks.EnumeratorAsyncExtensions.GetAwaiter<object>(object)
		// object DG.Tweening.TweenExtensions.Play<object>(object)
		// object DG.Tweening.TweenSettingsExtensions.OnComplete<object>(object,DG.Tweening.TweenCallback)
		// object DG.Tweening.TweenSettingsExtensions.OnUpdate<object>(object,DG.Tweening.TweenCallback)
		// object DG.Tweening.TweenSettingsExtensions.SetDelay<object>(object,float)
		// object DG.Tweening.TweenSettingsExtensions.SetEase<object>(object,DG.Tweening.Ease)
		// object DG.Tweening.TweenSettingsExtensions.SetSpeedBased<object>(object)
		// object[] System.Array.Empty<object>()
		// object System.Array.Find<object>(object[],System.Predicate<object>)
		// object System.Linq.Enumerable.Aggregate<object,object>(System.Collections.Generic.IEnumerable<object>,object,System.Func<object,object,object>)
		// bool System.Linq.Enumerable.Any<int>(System.Collections.Generic.IEnumerable<int>,System.Func<int,bool>)
		// bool System.Linq.Enumerable.Any<object>(System.Collections.Generic.IEnumerable<object>,System.Func<object,bool>)
		// bool System.Linq.Enumerable.Contains<object>(System.Collections.Generic.IEnumerable<object>,object)
		// object System.Linq.Enumerable.Last<object>(System.Collections.Generic.IEnumerable<object>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Select<object,object>(System.Collections.Generic.IEnumerable<object>,System.Func<object,object>)
		// object System.Linq.Enumerable.SingleOrDefault<object>(System.Collections.Generic.IEnumerable<object>,System.Func<object,bool>)
		// object[] System.Linq.Enumerable.ToArray<object>(System.Collections.Generic.IEnumerable<object>)
		// System.Collections.Generic.List<object> System.Linq.Enumerable.ToList<object>(System.Collections.Generic.IEnumerable<object>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Where<object>(System.Collections.Generic.IEnumerable<object>,System.Func<object,bool>)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Logic.UI.UIMain.UIMainRight.<OnClickDailyTask>d__11>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Logic.UI.UIMain.UIMainRight.<OnClickDailyTask>d__11&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Logic.UI.UIMain.UIMainLeft.<OnBtnPlaceRewardClick>d__9>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Logic.UI.UIMain.UIMainLeft.<OnBtnPlaceRewardClick>d__9&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Logic.UI.UIMain.UIBottomMenu.<OnEnable>d__4>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Logic.UI.UIMain.UIBottomMenu.<OnEnable>d__4&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Logic.UI.UIPlaceRewards.UIPlaceRewards.<OnClick>d__17>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Logic.UI.UIPlaceRewards.UIPlaceRewards.<OnClick>d__17&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Logic.UI.UIShop.PartDrawCard.<OnClickCB>d__31>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Logic.UI.UIShop.PartDrawCard.<OnClickCB>d__31&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Logic.UI.UISpecial.PartMining.<OnBtnEnterClick>d__3>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Logic.UI.UISpecial.PartMining.<OnBtnEnterClick>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Framework.UI.UIManager.<OpenUI>d__33>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Framework.UI.UIManager.<OpenUI>d__33&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Logic.UI.UIUser.UIUpgradedInfo.<OnShow>d__3>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Logic.UI.UIUser.UIUpgradedInfo.<OnShow>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Logic.UI.UIUser.UIEngine.<Start>d__18>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Logic.UI.UIUser.UIEngine.<Start>d__18&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Logic.UI.UIMain.Parts.UIBottomBtn.<OnClick>d__6>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Logic.UI.UIMain.Parts.UIBottomBtn.<OnClick>d__6&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Logic.UI.UIUser.UIArmor.<Start>d__28>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Logic.UI.UIUser.UIArmor.<Start>d__28&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Logic.UI.UIUser.PartPartner.<OnClickPartnerItem>d__10>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Logic.UI.UIUser.PartPartner.<OnClickPartnerItem>d__10&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Logic.UI.UIUser.PartRole.<OnClickWeapon>d__33>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Logic.UI.UIUser.PartRole.<OnClickWeapon>d__33&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Logic.UI.UIUser.PartRole.<OnClickArmor>d__34>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Logic.UI.UIUser.PartRole.<OnClickArmor>d__34&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Logic.UI.UIUser.UIWeapon.<Start>d__28>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Logic.UI.UIUser.UIWeapon.<Start>d__28&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Logic.UI.UILoading.UILoading.<Start>d__0>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Logic.UI.UILoading.UILoading.<Start>d__0&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Logic.UI.UIFight.UIFight.<OnShowFightSwitch>d__25>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Logic.UI.UIFight.UIFight.<OnShowFightSwitch>d__25&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Logic.UI.UICopy.UICopy.<OnClick_EnterGoldCopy>d__7>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Logic.UI.UICopy.UICopy.<OnClick_EnterGoldCopy>d__7&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Logic.Fight.Common.FightEnemyManager.<DoSpawnEnemy>d__28>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Logic.Fight.Common.FightEnemyManager.<DoSpawnEnemy>d__28&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Logic.Fight.Common.FightEnemyManager.<DoSpawnEnemy>d__28>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Logic.Fight.Common.FightEnemyManager.<DoSpawnEnemy>d__28&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Logic.Fight.Common.FightDamageManager.<ShowCriticalDamage>d__4>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Logic.Fight.Common.FightDamageManager.<ShowCriticalDamage>d__4&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Logic.Fight.Common.FightDamageManager.<ShowNormalDamage>d__3>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Logic.Fight.Common.FightDamageManager.<ShowNormalDamage>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Logic.Manager.SkillManager.<OnSkillIntensify>d__15>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Logic.Manager.SkillManager.<OnSkillIntensify>d__15&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Logic.Manager.PartnerManager.<OnPartnerIntensify>d__15>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Logic.Manager.PartnerManager.<OnPartnerIntensify>d__15&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Logic.Manager.EquipManager.<OnArmorIntensify>d__28>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Logic.Manager.EquipManager.<OnArmorIntensify>d__28&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Logic.Manager.EquipManager.<OnWeaponIntensify>d__27>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Logic.Manager.EquipManager.<OnWeaponIntensify>d__27&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Logic.Manager.CopyManager.<OnExitCopy>d__12>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Logic.Manager.CopyManager.<OnExitCopy>d__12&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Logic.UI.UIUser.PartRole.<OnClickEngine>d__35>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Logic.UI.UIUser.PartRole.<OnClickEngine>d__35&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Logic.Manager.CopyManager.<OnEnterCopy>d__11>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Logic.Manager.CopyManager.<OnEnterCopy>d__11&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Logic.States.Game.GS_Main.<Enter>d__2>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Logic.States.Game.GS_Main.<Enter>d__2&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Logic.States.Game.GS_Main.<Enter>d__2>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Logic.States.Game.GS_Main.<Enter>d__2&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Logic.States.Game.GS_Login.<Enter>d__1>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Logic.States.Game.GS_Login.<Enter>d__1&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Logic.States.Game.GS_Loading.<Enter>d__1>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Logic.States.Game.GS_Loading.<Enter>d__1&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Logic.States.Game.GS_Loading.<Enter>d__1>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Logic.States.Game.GS_Loading.<Enter>d__1&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Logic.UI.Cells.CommonOnItem.<OnClick>d__9>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Logic.UI.Cells.CommonOnItem.<OnClick>d__9&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Logic.UI.Common.UICommonHelper.<LoadIcon>d__0>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Logic.UI.Common.UICommonHelper.<LoadIcon>d__0&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Logic.UI.UICopy.UICopy.<OnClick_EnterCoinCopy>d__8>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Logic.UI.UICopy.UICopy.<OnClick_EnterCoinCopy>d__8&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Logic.States.Fight.FS_Switch.<Enter>d__1>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Logic.States.Fight.FS_Switch.<Enter>d__1&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Logic.UI.UIUser.PartSkill.<OnClickSKillItem>d__10>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Logic.UI.UIUser.PartSkill.<OnClickSKillItem>d__10&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Logic.UI.UIUser.UIEngine.<Start>d__18>(Logic.UI.UIUser.UIEngine.<Start>d__18&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Logic.UI.UIUser.PartPartner.<OnClickPartnerItem>d__10>(Logic.UI.UIUser.PartPartner.<OnClickPartnerItem>d__10&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Logic.UI.UIMain.UIMainRight.<OnClickDailyTask>d__11>(Logic.UI.UIMain.UIMainRight.<OnClickDailyTask>d__11&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Logic.UI.UIUser.UIArmor.<Start>d__28>(Logic.UI.UIUser.UIArmor.<Start>d__28&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Logic.Manager.CopyManager.<OnEnterCopy>d__11>(Logic.Manager.CopyManager.<OnEnterCopy>d__11&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Logic.States.Fight.FS_Switch.<Enter>d__1>(Logic.States.Fight.FS_Switch.<Enter>d__1&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Logic.States.Game.GS_Main.<Enter>d__2>(Logic.States.Game.GS_Main.<Enter>d__2&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Logic.States.Game.GS_Login.<Enter>d__1>(Logic.States.Game.GS_Login.<Enter>d__1&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Logic.States.Game.GS_Loading.<Enter>d__1>(Logic.States.Game.GS_Loading.<Enter>d__1&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Logic.UI.UIMain.UIBottomMenu.<OnEnable>d__4>(Logic.UI.UIMain.UIBottomMenu.<OnEnable>d__4&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Logic.Manager.CopyManager.<OnExitCopy>d__12>(Logic.Manager.CopyManager.<OnExitCopy>d__12&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Logic.UI.UIMain.UIMainLeft.<OnBtnPlaceRewardClick>d__9>(Logic.UI.UIMain.UIMainLeft.<OnBtnPlaceRewardClick>d__9&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Logic.UI.UIUser.PartSkill.<OnClickSKillItem>d__10>(Logic.UI.UIUser.PartSkill.<OnClickSKillItem>d__10&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Logic.UI.Common.UICommonHelper.<LoadIcon>d__0>(Logic.UI.Common.UICommonHelper.<LoadIcon>d__0&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Logic.UI.UICopy.UICopy.<OnClick_EnterCoinCopy>d__8>(Logic.UI.UICopy.UICopy.<OnClick_EnterCoinCopy>d__8&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Logic.UI.UICopy.UICopy.<OnClick_EnterGoldCopy>d__7>(Logic.UI.UICopy.UICopy.<OnClick_EnterGoldCopy>d__7&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Logic.UI.UIFight.UIFight.<OnShowFightSwitch>d__25>(Logic.UI.UIFight.UIFight.<OnShowFightSwitch>d__25&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Logic.UI.UILoading.UILoading.<Start>d__0>(Logic.UI.UILoading.UILoading.<Start>d__0&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Logic.UI.Cells.CommonOnItem.<OnClick>d__9>(Logic.UI.Cells.CommonOnItem.<OnClick>d__9&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Logic.Manager.EquipManager.<OnWeaponIntensify>d__27>(Logic.Manager.EquipManager.<OnWeaponIntensify>d__27&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Logic.Manager.EquipManager.<OnArmorIntensify>d__28>(Logic.Manager.EquipManager.<OnArmorIntensify>d__28&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Logic.Manager.PartnerManager.<OnPartnerIntensify>d__15>(Logic.Manager.PartnerManager.<OnPartnerIntensify>d__15&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Logic.UI.UIUser.UIUpgradedInfo.<OnShow>d__3>(Logic.UI.UIUser.UIUpgradedInfo.<OnShow>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Logic.UI.UIUser.PartRole.<OnClickWeapon>d__33>(Logic.UI.UIUser.PartRole.<OnClickWeapon>d__33&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Logic.UI.UIUser.PartRole.<OnClickArmor>d__34>(Logic.UI.UIUser.PartRole.<OnClickArmor>d__34&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Logic.UI.UIUser.UIWeapon.<Start>d__28>(Logic.UI.UIUser.UIWeapon.<Start>d__28&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Logic.UI.UISpecial.PartMining.<OnBtnEnterClick>d__3>(Logic.UI.UISpecial.PartMining.<OnBtnEnterClick>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Framework.UI.UIManager.<OpenUI>d__33>(Framework.UI.UIManager.<OpenUI>d__33&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Logic.UI.UIShop.PartDrawCard.<OnClickCB>d__31>(Logic.UI.UIShop.PartDrawCard.<OnClickCB>d__31&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Logic.UI.UIUser.PartRole.<OnClickEngine>d__35>(Logic.UI.UIUser.PartRole.<OnClickEngine>d__35&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Logic.UI.UIPlaceRewards.UIPlaceRewards.<OnClick>d__17>(Logic.UI.UIPlaceRewards.UIPlaceRewards.<OnClick>d__17&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Logic.Fight.Common.FightEnemyManager.<DoSpawnEnemy>d__28>(Logic.Fight.Common.FightEnemyManager.<DoSpawnEnemy>d__28&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Logic.Fight.Common.FightDamageManager.<ShowCriticalDamage>d__4>(Logic.Fight.Common.FightDamageManager.<ShowCriticalDamage>d__4&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Logic.Fight.Common.FightDamageManager.<ShowNormalDamage>d__3>(Logic.Fight.Common.FightDamageManager.<ShowNormalDamage>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Framework.UI.UIManager.<Show>d__49<object>>(Framework.UI.UIManager.<Show>d__49<object>&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Logic.Manager.SkillManager.<OnSkillIntensify>d__15>(Logic.Manager.SkillManager.<OnSkillIntensify>d__15&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Logic.UI.UIMain.Parts.UIBottomBtn.<OnClick>d__6>(Logic.UI.UIMain.Parts.UIBottomBtn.<OnClick>d__6&)
		// object UnityEngine.Component.GetComponent<object>()
		// object UnityEngine.Component.GetComponentInChildren<object>()
		// object UnityEngine.GameObject.AddComponent<object>()
		// object UnityEngine.GameObject.GetComponent<object>()
		// object UnityEngine.Object.FindObjectOfType<object>()
		// object UnityEngine.Object.Instantiate<object>(object)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Transform)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Vector3,UnityEngine.Quaternion)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Vector3,UnityEngine.Quaternion,UnityEngine.Transform)
		// object UnityEngine.Resources.Load<object>(string)
		// YooAsset.AssetOperationHandle YooAsset.YooAssets.LoadAssetAsync<object>(string)
	}
}