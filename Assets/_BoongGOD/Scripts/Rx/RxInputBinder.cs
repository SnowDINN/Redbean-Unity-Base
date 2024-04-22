using System;
using Cysharp.Threading.Tasks;
using R3;
using Redbean.Base;
using UnityEngine;

namespace Redbean.Rx
{
	public class RxInputBinder : ISingleton
    {
    	private readonly Subject<KeyCode> keyObservable = new();
    	public Observable<KeyCode> KeyObservable => keyObservable.Share();
    	
    	private readonly Subject<TouchPhase> mouseAndTouchObservable = new();
    	public Observable<TouchPhase> MouseAndTouchObservable => mouseAndTouchObservable.Share();
    
    	private readonly CompositeDisposable disposables = new();
    	private int mouseCode = -1;
    
    	public RxInputBinder()
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
    
    	~RxInputBinder()
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
			    
			    if (Input.GetMouseButton(mouseCode))
				    mouseAndTouchObservable.OnNext(TouchPhase.Stationary);	
    		}
    
    		if (Input.touchCount > 0)
    			mouseAndTouchObservable.OnNext(Input.GetTouch(0).phase);
    	}
    
    	private UniTask FindKeyCodeAsync()
    	{
    		var keyCodes = Enum.GetValues(typeof(KeyCode));
    		foreach (KeyCode keyCode in keyCodes)
    		{
    			if (!Input.GetKeyDown(keyCode))
    				continue;
    
    			keyObservable.OnNext(keyCode);
    			return UniTask.CompletedTask;
    		}
		    
		    return UniTask.CompletedTask;
    	}
    }
}
