using System;
using System.Collections.Generic;
using UnityEngine;

namespace OrbitFinder
{
	public static class MainWindow
	{
		private static List<PlanetContainer> _planets;
		private static CelestialBody _activePlanet;
		private static List<GUIStyle> _toggleStyles = new List<GUIStyle>();
		private static GUIStyle _methodToggleStyle, _headingStyle, _versionStyle, _splitterStyle, _textInputStyle, _regularResultStyle, _alarmResultStyle, _scrollviewStyle;
		private static GUISkin _guiSkin = HighLogic.Skin;
		private static Vector2 _mainWindowPos;
		private static Vector2 _scrollPos = new Vector2();
		private static Rect _mainWindow;
		private static string _periodInput = Constants.guiMainWindowDefaultPeriodMessage;
		private static string _orbitInput = Constants.guiMainWindowDefaultOrbitMessage;
		private static double _periodResult, _orbitResult;
		private static bool _guiInitalized = false;
		private static int _toggleStages;
		private static DistanceMeassureMethod _activeOrbitMeassureMethod;
		private static PeriodMeassureMethod _activePeriodMeassureMethod;

		public static Rect mainWindow
		{
			get { return _mainWindow; }
			set { _mainWindow = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="toggleLevels"></param>
		/// <param name="planets"></param>
		public static void initMainWindow(int toggleLevels, List<PlanetContainer> planets)
		{
			float height = 300 + 13 * 30 + 6 * 5;
			if (Screen.height < height)
			{
				height = Screen.height;
			}
			_mainWindowPos = new Vector2(Screen.width - (Constants.guiMainWindowW + 110), 55);
			_mainWindow = new Rect(_mainWindowPos.x, _mainWindowPos.y, Constants.guiMainWindowW, height);
			_toggleStages = toggleLevels;
			_planets = planets;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="planet"></param>
		/// <param name="orbitMethod"></param>
		/// <param name="periodMethod"></param>
		public static void assignCelestialBody(CelestialBody planet)
		{
			_activePlanet = planet;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="state"></param>
		public static void drawMainWindow(DisplayState state)
		{
			GUI.skin = _guiSkin;
			_mainWindow = GUI.Window((int)state, _mainWindow, OnMainWindow, Constants.guiMainWindowTitel);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="windowID"></param>
		private static void OnMainWindow(int windowID)
		{
			initGuiStyles();

			GUI.skin = _guiSkin;
			GUILayout.BeginVertical();
			GUI.DragWindow(new Rect(0, 0, _mainWindow.width, 30));
			_mainWindow.position = checkWindowPos(_mainWindow, 41, _mainWindowPos);
			GUISeperator(_splitterStyle);
			GUILayout.Label(Constants.guiMainWindowPeriodCalculationHeading, _headingStyle);
			GUI.SetNextControlName(Constants.guiMainWindowPeriodInput);
			_periodInput = GUILayout.TextField(_periodInput, _textInputStyle, GUILayout.MaxHeight(30), GUILayout.ExpandHeight(false));
			_periodResult = Maths.calculateOrbitPeriod(_periodInput.Trim().Replace(",", ".").Replace(" ", ""), _activePlanet.Mass, _activePlanet.Radius, _activePeriodMeassureMethod, _activeOrbitMeassureMethod);
			if (GUI.GetNameOfFocusedControl() == Constants.guiMainWindowPeriodInput && _periodInput != string.Empty && _periodInput == Constants.guiMainWindowDefaultPeriodMessage)
			{
				_periodInput = string.Empty;
			}
			if ((GUI.GetNameOfFocusedControl() == "" || GUI.GetNameOfFocusedControl() != Constants.guiMainWindowPeriodInput) && _periodInput == string.Empty)
			{
				_periodInput = Constants.guiMainWindowDefaultPeriodMessage;
			}
			GUILayout.Label((double.IsNaN(_periodResult) ? Constants.guiMainWindowOrbitInputError : Constants.guiMainWindowPeriodResult + Math.Round(_periodResult, 3).ToString() + (_activePeriodMeassureMethod == PeriodMeassureMethod.Hours ? " h" : " min")), _regularResultStyle);
			GUISeperator(_splitterStyle);
			GUILayout.Label(Constants.guiMainWindowOrbitCalculationHeading, _headingStyle);
			GUI.SetNextControlName(Constants.guiMainWindowOrbitInput);
			_orbitInput = GUILayout.TextField(_orbitInput, _textInputStyle, GUILayout.MaxHeight(30), GUILayout.ExpandHeight(false));
			_orbitResult = Maths.calculateOrbitRadius(_orbitInput.Trim().Replace(",", ".").Replace(" ", ""), _activePlanet.Mass, _activePlanet.Radius, _activePeriodMeassureMethod, _activeOrbitMeassureMethod);
			if (GUI.GetNameOfFocusedControl() == Constants.guiMainWindowOrbitInput && _orbitInput != string.Empty && _orbitInput == Constants.guiMainWindowDefaultOrbitMessage)
			{
				_orbitInput = string.Empty;
			}
			if ((GUI.GetNameOfFocusedControl() == "" || GUI.GetNameOfFocusedControl() != Constants.guiMainWindowOrbitInput) && _orbitInput == string.Empty)
			{
				_orbitInput = Constants.guiMainWindowDefaultOrbitMessage;
			}
			GUILayout.Label((double.IsNaN(_orbitResult) ? Constants.guiMainwindowPeriodInputError : Constants.guiMainWindowOrbitResult + Math.Round(_orbitResult, 3).ToString() + (_activeOrbitMeassureMethod == DistanceMeassureMethod.Kilometer ? " km" : " m")), _orbitResult * (_activeOrbitMeassureMethod == DistanceMeassureMethod.Kilometer ? 1000d : 1d) < _activePlanet.atmosphereDepth || _orbitResult * (_activeOrbitMeassureMethod == DistanceMeassureMethod.Kilometer ? 1000d : 1d) > _activePlanet.sphereOfInfluence ? _alarmResultStyle : _regularResultStyle);
			GUISeperator(_splitterStyle);
			GUILayout.Label(Constants.guiMainWindowDistanceHeading, _headingStyle);
			GUILayout.BeginHorizontal();
			foreach (DistanceMeassureMethod distanceMethod in Enum.GetValues(typeof(DistanceMeassureMethod)))
			{
				if (GUILayout.Toggle((_activeOrbitMeassureMethod == distanceMethod), distanceMethod.ToString(), _methodToggleStyle))
				{
					_activeOrbitMeassureMethod = distanceMethod;
				}
			}
			GUILayout.EndHorizontal();
			GUISeperator(_splitterStyle);
			GUILayout.Label(Constants.guiMainWindowPeriodHeading, _headingStyle);
			GUILayout.BeginHorizontal();
			foreach (PeriodMeassureMethod periodMethod in Enum.GetValues(typeof(PeriodMeassureMethod)))
			{
				if (GUILayout.Toggle((_activePeriodMeassureMethod == periodMethod), periodMethod.ToString(), _methodToggleStyle))
				{
					_activePeriodMeassureMethod = periodMethod;
				}
			}
			GUILayout.EndHorizontal();
			GUISeperator(_splitterStyle);
			_headingStyle.alignment = TextAnchor.MiddleCenter;
			GUILayout.Label(Constants.guiMainWindowHeading, _headingStyle);
			_scrollPos = GUILayout.BeginScrollView(_scrollPos, _scrollviewStyle);
			foreach (PlanetContainer container in _planets)
			{
				if (GUILayout.Toggle((_activePlanet == container.planet), container.planet.GetName(), _toggleStyles[container.descendingIndex]))
				{
					_activePlanet = container.planet;
				}
			}
			GUILayout.EndScrollView();
			GUISeperator(_splitterStyle);
			GUILayout.Label(Constants.logVersion, _versionStyle);
			GUILayout.EndVertical();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="style"></param>
		private static void GUISeperator(GUIStyle style)
		{
			GUI.skin = null;
			GUILayout.Box(GUIContent.none, style, GUILayout.ExpandHeight(false), GUILayout.MaxHeight(1));
			GUI.skin = _guiSkin;
		}

		/// <summary>
		/// 
		/// </summary>
		private static void initGuiStyles()
		{
			if (!_guiInitalized)
			{
				_toggleStyles = new List<GUIStyle>();
				for (int i = 0; i < _toggleStages; i++)
				{
					_toggleStyles.Add(new GUIStyle(GUI.skin.toggle));
					_toggleStyles[i].margin.left += 30 * i;
				}

				_methodToggleStyle = new GUIStyle(GUI.skin.toggle);
				_methodToggleStyle.margin.right += ((Constants.guiMainWindowW - ((int)_methodToggleStyle.fixedWidth * 2) - (_methodToggleStyle.margin.left * 2)) / 2);

				_headingStyle = new GUIStyle(GUI.skin.label);
				_headingStyle.alignment = TextAnchor.MiddleCenter;
				_headingStyle.fontStyle = FontStyle.Bold;

				_versionStyle = new GUIStyle(GUI.skin.label);
				_versionStyle.fontSize = 12;
				_versionStyle.alignment = TextAnchor.MiddleCenter;

				_textInputStyle = new GUIStyle(GUI.skin.textField);
				_textInputStyle.stretchHeight = false;
				_textInputStyle.fixedHeight = 30;
				_textInputStyle.alignment = TextAnchor.MiddleLeft;

				_regularResultStyle = new GUIStyle(GUI.skin.label);
				_regularResultStyle.alignment = TextAnchor.MiddleCenter;
				_regularResultStyle.stretchHeight = false;
				_regularResultStyle.fixedHeight = 30;

				_alarmResultStyle = new GUIStyle(GUI.skin.label);
				_alarmResultStyle.alignment = TextAnchor.MiddleCenter;
				_alarmResultStyle.normal.textColor = Color.red;
				_alarmResultStyle.stretchHeight = false;
				_alarmResultStyle.fixedHeight = 30;

				_scrollviewStyle = new GUIStyle(GUI.skin.scrollView);
				_scrollviewStyle.border.bottom = 5;
				_scrollviewStyle.margin.bottom = 5;

				GUI.skin = null;
				_splitterStyle = new GUIStyle(GUI.skin.horizontalSlider);
				_splitterStyle.border.right = _splitterStyle.border.left = -2;
				_splitterStyle.margin.top = _splitterStyle.margin.right = _splitterStyle.margin.left = _splitterStyle.margin.bottom = 1;
				_splitterStyle.padding.right = _splitterStyle.padding.left = _splitterStyle.padding.right = _splitterStyle.padding.left = 1;
				_splitterStyle.fixedHeight = 3;
				_splitterStyle.stretchHeight = false;

				_guiInitalized = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="window"></param>
		/// <param name="offset"></param>
		/// <param name="position"></param>
		/// <returns></returns>
		private static Vector2 checkWindowPos(Rect window, int offset, Vector2 position)
		{
			position = window.position;
			switch (HighLogic.LoadedScene)
			{
				case GameScenes.FLIGHT:
					if (window.position.x < 0)
					{
						position.Set(0, position.y);
					}
					else if (window.position.x > (Screen.width - (window.width + offset)))
					{
						position.Set((Screen.width - (window.width + offset)), position.y);
					}

					if (window.position.y < 0)
					{
						position.Set(position.x, 0);
					}
					else if (window.position.y > (Screen.height - window.height))
					{
						position.Set(position.x, (Screen.height - window.height));
					}
					return position;
				case GameScenes.TRACKSTATION:
					if (window.position.x < 290)
					{
						position.Set(290, position.y);
					}
					else if (window.position.x > (Screen.width - window.width))
					{
						position.Set((Screen.width - window.width), position.y);
					}

					if (window.position.y < (offset - 6))
					{
						position.Set(position.x, (offset - 6));
					}
					else if (window.position.y > (Screen.height - (window.height + offset)))
					{
						position.Set(position.x, (Screen.height - (window.height + offset)));
					}
					return position;
			}
			return position;
		}

	}
}
