using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using Exelus.Win11DesktopSwitchAnimatior;
using Microsoft.Win32;

var keyName = "Exelus.Win11DesktopSwitchAnimatior";
RegistryKey rkApp = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
var rkValue = rkApp.GetValue(keyName);
if(rkValue == null) {
	Debug.WriteLine($"Key {keyName} not present");
	//rkApp.SetValue(keyName, Assembly.GetExecutingAssembly().Location);
}
//rkApp.DeleteValue(keyName);

[DllImport("user32")]
static extern int GetAsyncKeyState(int i);

TouchInjector.InitializeTouchInjection(4, TouchFeedback.NONE);

bool FourFingerSwipe(int distance) {

	var touches = new PointerTouchInfo[4];

	for(int i = 0; i < 4; i++) {
		var touch = new PointerTouchInfo() {
			PointerInfo = new PointerInfo() {
				PtPixelLocation = new TouchPoint() {
					X = 200,
					Y = (i * 100) + 100,
				},
				pointerType = PointerInputType.TOUCH,
				PointerFlags = PointerFlags.DOWN | PointerFlags.INRANGE | PointerFlags.INCONTACT,
				PointerId = (uint)i,
			},
			Orientation = 90,
			Pressure = 32000,
			TouchMasks = TouchMask.CONTACTAREA | TouchMask.ORIENTATION | TouchMask.PRESSURE
		};
		touches[i] = (touch);
	}
	TouchInjector.InjectTouchInput(4, touches);

	for(int i = 0; i < 4; i++) {
		touches[i].PointerInfo.PtPixelLocation.X += distance;
		touches[i].PointerInfo.PointerFlags = PointerFlags.UPDATE | PointerFlags.INRANGE | PointerFlags.INCONTACT;
	}
	TouchInjector.InjectTouchInput(4, touches);

	Thread.Sleep(100);

	for(int i = 0; i < 4; i++) {
		touches[i].PointerInfo.PtPixelLocation.X += distance;
	}
	TouchInjector.InjectTouchInput(4, touches);

	for(int i = 0; i < 4; i++) {
		touches[i].PointerInfo.PtPixelLocation.X += distance;
		touches[i].PointerInfo.PointerFlags = PointerFlags.UP;
	}
	TouchInjector.InjectTouchInput(4, touches);

	return true;
}

var SwipeLeft  =  500;
var SwipeRight = -500;

FourFingerSwipe(SwipeLeft);

//while(true) {

//	Thread.Sleep(5);

//	var ctrlPressed       = GetAsyncKeyState(17) != 0;
//	var altPressed        = GetAsyncKeyState(18) != 0;
//	var leftArrowPressed  = GetAsyncKeyState(37) != 0;
//	var rightArrowPressed = GetAsyncKeyState(39) != 0;

//	if(ctrlPressed && altPressed && leftArrowPressed) {
//		Debug.WriteLine("Left thing happening!");
//		//FourFingerSwipe(50);
//		Thread.Sleep(300);
//	}
//	if(ctrlPressed && altPressed && rightArrowPressed) {
//		Debug.WriteLine("Right thing happening!");
//		//FourFingerSwipe(-50);
//		Thread.Sleep(300);
//	}
//}