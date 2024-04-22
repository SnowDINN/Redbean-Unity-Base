using System;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace Redbean.Base
{
	public class Rx : ISingleton
    {
    	private static readonly Subject<KeyCode> keyObservable = new();
    	public static Observable<KeyCode> KeyObservable => keyObservable.Share();
    	
    	private static readonly Subject<TouchPhase> mouseAndTouchObservable = new();
    	public static Observable<TouchPhase> MouseAndTouchObservable => mouseAndTouchObservable.Share();
    
    	private readonly CompositeDisposable disposables = new();
    	private int mouseCode = -1;
    
    	public Rx()
    	{
    		Observable.EveryUpdate().Subscribe(_ =>
    		{
    			UniTask.Void(DetectingAsync);
    		}).AddTo(disposables);
    
    		KeyObservable.Subscribe(_ =>
    		{
    			if (!$"{_}".Contains("Mouse"))
    				return;
    			
    			if (!int.TryParse($"{_}".Replace("Mouse", ""), out var code))
    				return;
    
    			mouseCode = code;
    		}).AddTo(disposables);
    	}
    
    	~Rx()
    	{
    		disposables.Dispose();
    		disposables.Clear();
    	}
    
    	private async UniTaskVoid DetectingAsync()
    	{
    		if (Input.anyKeyDown)
    			await FindKeyCodeAsync();
    
    		if (mouseCode > -1)
    		{
    			if (Input.GetMouseButtonDown(mouseCode))
    				mouseAndTouchObservable.OnNext(TouchPhase.Began);
    			
    			if (Input.GetMouseButtonUp(mouseCode))
    				mouseAndTouchObservable.OnNext(TouchPhase.Ended);	
    		}
    
    		if (Input.touchCount > 0)
    			mouseAndTouchObservable.OnNext(Input.GetTouch(0).phase);
    	}
    
    	private static async UniTask FindKeyCodeAsync()
    	{
    		var keyCodes = Enum.GetValues(typeof(KeyCode));
    		foreach (KeyCode keyCode in keyCodes)
    		{
    			if (!Input.GetKeyDown(keyCode))
    				continue;
    
    			keyObservable.OnNext(keyCode);
    			return;
    		}
    	}
    }
}
