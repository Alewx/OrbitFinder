using System;

namespace OrbitFinder
{
	static class Constants
	{
		public const string logVersion = "v1.0.0.0 - 1.3.0";
		public const string logPrefix = "[OrbitFinder] ";
		public const string toolbarIconPath = "OrbitFinder/Textures/ToolbarIcon";

		public const int guiMainWindowW = 320;
		public const int guiMainWindowH = 285;
		public const string guiMainWindowTitel = "Orbit Finder";
		public const string guiMainWindowHeading = "Planets";
		public const string guiMainWindowDistanceHeading = "Distance Method";
		public const string guiMainWindowPeriodHeading = "Period Method";

		public const string guiMainWindowPeriodCalculationHeading = "Orbital Period calculation";
		public const string guiMainWindowOrbitCalculationHeading = "Orbital Circularsemimajoraxis calculation";
		public const string guiMainwindowPeriodInputError = "No Orbitperiod entered";
		public const string guiMainWindowOrbitInputError = "No Orbitalsemimajor entered";
		public const string guiMainWindowPeriodResult = "circular Period = ";
		public const string guiMainWindowOrbitResult = "circular Semimajor = ";

		public const string guiMainWindowPeriodInput = "PeriodField";
		public const string guiMainWindowOrbitInput = "OrbitField";

		public const string guiMainWindowDefaultPeriodMessage = "Enter searced Orbitmajoraxis";
		public const string guiMainWindowDefaultOrbitMessage = "Enter searched Orbitperiod";
		public const string guiMainWindowLock = "ORBITFINDER_UILOCK";

		public static double gravityConstant = 6.67408 * Math.Pow(10, -11);
	}

}

