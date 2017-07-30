using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSP.UI.Screens;

namespace OrbitFinder
{
	[KSPAddon(KSPAddon.Startup.AllGameScenes, false)]
	public class ToolbarEntry : MonoBehaviour
	{
		private static ToolbarEntry _instance { get; set; }
		private ApplicationLauncher _toolbarInstance = ApplicationLauncher.Instance;
		private ApplicationLauncherButton _toolbarButton;
		private bool _isEnabled = false;
		private List<GameScenes> _scenes = new List<GameScenes>() { GameScenes.FLIGHT, GameScenes.TRACKSTATION };

		public ToolbarEntry instance
		{
			get { return _instance; }
			set { _instance = value; }
		}

		public ApplicationLauncherButton toolbarButton
		{
			get { return _toolbarButton; }
			set { _toolbarButton = value; }
		}

		/// <summary>
		/// public access for initializing the toolbar
		/// </summary>
		public void initToolbar()
		{
			Debugger.AdvLog(string.Format("instance = {0} => initToolbar", instance.GetType()),true);
			if (!_isEnabled)
			{
				foreach(GameScenes s in _scenes)
				{
					Debugger.AdvLog(string.Format("LoadedScene = {0} | scenes =  {1} => initToolbar", HighLogic.LoadedScene, s), true);
				}
				if (_scenes.Contains(HighLogic.LoadedScene))
				{
					GameEvents.onGUIApplicationLauncherReady.Add(OnGuiAppLauncherReady);
					if (OrbitFinder.instance != null)
					{
						OrbitFinder.instance.Start();
					}
					_isEnabled = true;
				}
			}
		}

		/// <summary>
		/// Function is called everytime the Editor is loaded as a scene. a quite primitive fallback as the Applauncher seems to not react on the GameEvents.onGUIApplicationLauncherReady
		/// </summary>
		//private void Start()
		//{
		//	if (_toolbarButton == null)
		//	{
		//		Debugger.AdvLog(string.Format("{0} => Start", instance.GetType()), true);
		//		initToolbar();
		//	}
		//}

		/// <summary>
		/// the initial start of the class with preparing of the toolbar
		/// </summary>
		private void Awake()
		{
			try
			{
				if (_instance == null)
				{
					_instance = this;
					if (OrbitFinder.instance != null)
					{
						Debugger.AdvLog(string.Format("{0} | {1} => Awake", _instance.GetType(), _instance.GetInstanceID()), true);
						initToolbar();
					}
				}
			}
			catch (Exception exception)
			{
				Debugger.AdvError(string.Format("{0} => Awake", instance.GetType()), true);
				Debug.LogException(exception, this);
			}
		}

		/// <summary>
		/// adds the toolbar to the applauncher
		/// </summary>
		private void OnGuiAppLauncherReady()
		{
			try
			{
				Debugger.AdvLog(string.Format("{0} => OnGuiAppLauncherReady done", instance.GetType()), true);
				var activeSzenes = ApplicationLauncher.AppScenes.FLIGHT | ApplicationLauncher.AppScenes.TRACKSTATION | ApplicationLauncher.AppScenes.MAPVIEW;
				_toolbarButton = _toolbarInstance.AddModApplication(
					switchWindow,
					switchWindow,
					null,
					null,
					null,
					null,
					activeSzenes,
					GameDatabase.Instance.GetTexture(Constants.toolbarIconPath, false)
					);
				_isEnabled = true;
			}
			catch (Exception exception)
			{
				Debugger.AdvError(string.Format("{0} => OnGuiAppLauncherReady", instance.GetType()), true);
				Debug.LogException(exception, this);
			}
		}

		/// <summary>
		/// will simply switch the window between open and close
		/// </summary>
		public void switchWindow()
		{
			try
			{
				Debugger.AdvLog(string.Format("{0} => switchWindow", instance.GetType()), true);
				switch (OrbitFinder.instance.displayState)
				{
					case DisplayState.none:
						OrbitFinder.instance.openWindow();
						break;
					default:
						OrbitFinder.instance.closeWindow();
						break;
				}
			}
			catch (Exception exception)
			{
				Debugger.AdvError(string.Format("{0} => switchWindow", instance.GetType()), true);
				Debug.LogException(exception, this);
			}
		}

		/// <summary>
		/// will force the window to close
		/// </summary>
		public void forceCloseWindow()
		{
			try
			{
				Debugger.AdvLog(string.Format("{0} => forceCloseWindow", instance.GetType()), true);
				_toolbarButton.SetFalse(true);
			}
			catch (Exception exception)
			{
				Debugger.AdvError(string.Format("{0} => forceCloseWindow", instance.GetType()), true);
				Debug.LogException(exception, this);
			}
		}

		/// <summary>
		/// removes the buttom from the toolbar
		/// </summary>
		private void OnDestroy()
		{
			try
			{
				Debugger.AdvLog(string.Format("{0} => OnDestroy done", instance.GetType()), true);
				GameEvents.onGUIApplicationLauncherReady.Remove(OnGuiAppLauncherReady);
				if (_toolbarButton != null)
				{
					forceCloseWindow();
					ApplicationLauncher.Instance.RemoveModApplication(_toolbarButton);
					_isEnabled = false;
					_toolbarButton = null;
				}
			}
			catch (Exception exception)
			{
				Debugger.AdvError(string.Format("{0} => OnDestroy", instance.GetType()), true);
				Debug.LogException(exception, this);
			}
		}

	}

}

