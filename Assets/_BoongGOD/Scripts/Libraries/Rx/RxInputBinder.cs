using System;
using Cysharp.Threading.Tasks;
using R3;
using Redbean.Define;
using Redbean.Static;
using UnityEngine;

namespace Redbean.Rx
{
	public class RxInputBinder : ISingleton
    {
    	private readonly Subject<KeyCode> onKeyInputDetected = new();
    	public Observable<KeyCode> OnKeyInputDetected => 
		    onKeyInputDetected.Share().ThrottleFirst(TimeSpan.FromMilliseconds(Balance.DoubleInputPrevention));
    	
    	private readonly Subject<(TouchPhase type, Vector3 position)> onMouseAndTouchInputDetected = new();
    	public Observable<(TouchPhase type, Vector3 position)> OnMouseAndTouchInputDetected => 
		    onMouseAndTouchInputDetected.Share().ThrottleFirst(TimeSpan.FromMilliseconds(Balance.DoubleInputPrevention));
    
    	private readonly CompositeDisposable disposables = new();
    	private int mouseCode = -1;
    
    	public RxInputBinder()
    	{
    		Observable.EveryUpdate().Subscribe(_ =>
    		{
    			UniTask.Void(DetectingAsync);
    		}).AddTo(disposables);
    
    		OnKeyInputDetected.Subscribe(_ =>
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
    				onMouseAndTouchInputDetected.OnNext((TouchPhase.Began, Input.mousePosition));
    			
    			if (Input.GetMouseButtonUp(mouseCode))
    				onMouseAndTouchInputDetected.OnNext((TouchPhase.Ended, Input.mousePosition));	
    		}
    
    		if (Input.touchCount > 0)
		    {
			    var touch = Input.GetTouch(0);
			    if (touch.phase is TouchPhase.Stationary or TouchPhase.Moved)
				    return;
				    
			    onMouseAndTouchInputDetected.OnNext((touch.phase, touch.position));
		    }
	    }
    
    	private UniTask FindKeyCodeAsync()
    	{
    		var keyCodes = Enum.GetValues(typeof(KeyCode));
    		foreach (KeyCode keyCode in keyCodes)
    		{
    			if (!Input.GetKeyDown(keyCode))
    				continue;
    
    			onKeyInputDetected.OnNext(keyCode);
    			return UniTask.CompletedTask;
    		}
		    
		    return UniTask.CompletedTask;
    	}
    }
}
