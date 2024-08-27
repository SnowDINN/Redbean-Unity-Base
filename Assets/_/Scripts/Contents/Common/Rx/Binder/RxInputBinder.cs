using System;
using R3;
using UnityEngine;

namespace Redbean.Rx
{
	public class RxInputBinder : RxBase
    {
    	private static readonly Subject<KeyCode> onKeyInputDetected = new();
    	public static Observable<KeyCode> OnKeyInputDetected => 
		    onKeyInputDetected.Share().ThrottleFirst(TimeSpan.FromMilliseconds(Balance.DoubleInputPrevention));
    	
    	private static readonly Subject<(TouchPhase type, Vector3 position)> onMouseAndTouchInputDetected = new();
    	public static Observable<(TouchPhase type, Vector3 position)> OnMouseAndTouchInputDetected => 
		    onMouseAndTouchInputDetected.Share().ThrottleFirst(TimeSpan.FromMilliseconds(Balance.DoubleInputPrevention));
	    
    	private int mouseCode = -1;

	    public override void Setup()
	    {
		    base.Setup();
		    
		    Observable.EveryUpdate().Subscribe(_ =>
		    {
			    InputDetectingAsync();
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

	    private void InputDetectingAsync()
    	{
		    if (Input.anyKeyDown)
		    {
			    var keyCodes = Enum.GetValues(typeof(KeyCode));
			    foreach (KeyCode keyCode in keyCodes)
			    {
				    if (!Input.GetKeyDown(keyCode))
					    continue;
    
				    onKeyInputDetected.OnNext(keyCode);
			    }
		    }
    
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
    }
}
