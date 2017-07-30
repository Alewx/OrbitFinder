using System;
using UnityEngine;

namespace OrbitFinder
{
	public static class Debugger
	{
		/// <summary>
		/// provides additional logging information if enabled
		/// </summary>
		/// <param name="debugText"></param>
		/// <param name="advancedDebugging"></param>
		public static void AdvLog(string debugText, bool advancedDebugging)
		{
			if (advancedDebugging)
			{
				Debug.Log(string.Format("{0} {1}", Constants.logPrefix, debugText));
			}
		}

		/// <summary>
		/// provides additional logging information if enabled
		/// </summary>
		/// <param name="debugText"></param>
		/// <param name="advancedDebugging"></param>
		public static void AdvError(string debugText, bool advancedDebugging)
		{
			if (advancedDebugging)
			{
				Debug.LogError(string.Format("{0} {1}", Constants.logPrefix, debugText));
			}
		}

	}

}

