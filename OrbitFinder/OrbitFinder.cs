using System.Collections.Generic;
using UnityEngine;


namespace OrbitFinder
{
	[KSPAddon(KSPAddon.Startup.AllGameScenes, false)]
	public class OrbitFinder : MonoBehaviour
	{
		private static OrbitFinder _instance;
		private List<PlanetContainer> _planets;
		private int _decendingLevels = 0;
		private DisplayState _state = DisplayState.none;
		private ControlTypes _guiLocks = (ControlTypes.CAMERACONTROLS | ControlTypes.GUI | ControlTypes.UI | ControlTypes.All | ControlTypes.GROUPS_ALL | ControlTypes.FLIGHTUIMODE);
		private List<GameScenes> _scenes = new List<GameScenes>() { GameScenes.FLIGHT, GameScenes.TRACKSTATION };

		public static OrbitFinder instance
		{
			get { return _instance; }
			set { _instance = value; }
		}

		public DisplayState displayState
		{
			get { return _state; }
		}

		/// <summary>
		/// Unity default method that is called once the mod is initalized
		/// </summary>
		public void Awake()
		{
			_instance = this;
			Debugger.AdvLog(string.Format("{0}|{1} => Awake", _instance.GetType(), _instance.GetInstanceID()), true);
			prepPlanets();
			MainWindow.initMainWindow(_decendingLevels, _planets);
			if (FlightGlobals.currentMainBody != null)
			{
				MainWindow.assignCelestialBody(FlightGlobals.currentMainBody);
			}
			else
			{
				MainWindow.assignCelestialBody(Planetarium.fetch.CurrentMainBody);
			}
		}

		/// <summary>
		/// unity default method that is called everytime the scene is changed
		/// </summary>
		public void Start()
		{
			Debugger.AdvLog(string.Format("Start = {0}", GetType()), true);
			if (_scenes.Contains(HighLogic.LoadedScene))
			{
				if (FlightGlobals.currentMainBody != null)
				{
					MainWindow.assignCelestialBody(FlightGlobals.currentMainBody);
				}
				else
				{
					MainWindow.assignCelestialBody(Planetarium.fetch.CurrentMainBody);
				}
			}
		}

		/// <summary>
		/// the Unity default method to draw any GUI on the Screen.
		/// </summary>
		public void OnGUI()
		{
			if (_scenes.Contains(HighLogic.LoadedScene))
			{
				switch (_state)
				{
					case DisplayState.none:
						break;
					case DisplayState.open:
						LockManager.preventClickThrough(MainWindow.mainWindow, _guiLocks, Constants.guiMainWindowLock);
						MainWindow.drawMainWindow(_state);
						break;
				}
			}
		}

		/// <summary>
		/// will open the mainwindow
		/// </summary>
		public void openWindow()
		{
			if (_state == DisplayState.none)
			{
				_state = DisplayState.open;
			}
		}

		/// <summary>
		/// will close the mainwindow
		/// </summary>
		public void closeWindow()
		{
			if (_state != DisplayState.none)
			{
				_state = DisplayState.none;
			}
		}

		/// <summary>
		/// the initializing method of the mod
		/// </summary>
		private void prepPlanets()
		{
			_planets = new List<PlanetContainer>();
			List<CelestialBody> alreadyUsed = new List<CelestialBody>();
			listPlanets(FlightGlobals.Bodies[0].orbitingBodies, 0, alreadyUsed);
			_decendingLevels = 0;
			foreach (PlanetContainer pc in _planets)
			{
				if (pc.descendingIndex > _decendingLevels)
				{
					_decendingLevels++;
				}
				string name = pc.planet.name;
				int decentLevel = pc.descendingIndex;
				int temp_index = pc.planet.flightGlobalsIndex;
				double mass = pc.planet.Mass;
				double radius = pc.planet.Radius;
				bool atmo = pc.planet.atmosphere;
				double depth = pc.planet.atmosphereDepth;
				double soiRadius = pc.planet.sphereOfInfluence;
				Debugger.AdvLog(string.Format("{0} - {1} | index = {2} | mass = {3} | radius = {4} | atmo = {5} - {6} | soi = {7}", name, decentLevel, temp_index, mass, radius, atmo, depth, soiRadius), true);
			}
			_decendingLevels++;
		}

		/// <summary>
		/// will load and prepare the celestial bodies in the game into a proper list that can be handled
		/// </summary>
		/// <param name="originalPlanetList"></param>
		/// <param name="originalIndex"></param>
		/// <param name="alreadyUsed"></param>
		public void listPlanets(List<CelestialBody> originalPlanetList, int originalIndex, List<CelestialBody> alreadyUsed)
		{
			if (originalPlanetList.Count > 0)
			{
				foreach (CelestialBody cb in originalPlanetList)
				{
					if (!alreadyUsed.Contains(cb))
					{
						_planets.Add(new PlanetContainer(cb, originalIndex));
						alreadyUsed.Add(cb);
						listPlanets(cb.orbitingBodies, originalIndex + 1, alreadyUsed);
					}
				}
			}
		}

	}

}

