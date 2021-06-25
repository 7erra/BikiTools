[] spawn {
	[uiNamespace, "OnDisplayRegistered", {
		params ["_display", "_class"];
		if (_class == "RscDisplayFunctionsViewer") then {
			systemChat "You opened the Functions Viewer!";
		};
	}] call BIS_fnc_addScriptedEventHandler;
	
	[uiNamespace, "OnDisplayUnregistered", {
		params ["_display", "_class"];
		if (_class == "RscDisplayFunctionsViewer") then {
			systemChat "You closed the Functions Viewer!";
		};
	}] call BIS_fnc_addScriptedEventHandler;
	
	// Execute in Eden:
	_display = findDisplay 313 createDisplay "RscDisplayFunctionsViewer"; // -> You opened the Functions Viewer!
	uiSleep 2;
	_display closeDisplay 1; // -> You closed the Functions Viewer!
};