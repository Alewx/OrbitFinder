using System;

namespace OrbitFinder
{
	public static class Maths
	{
		/// <summary>
		/// calclates the orbitalperiod of a circular orbit based on the wanted orbital semimajoraxis
		/// </summary>
		/// <param name="wantedOrbitRadiusInput"></param>
		/// <param name="planetMass"></param>
		/// <param name="planetRadius"></param>
		/// <param name="periodMethod"></param>
		/// <param name="orbitMethod"></param>
		/// <returns></returns>
		public static double calculateOrbitPeriod(string wantedOrbitRadiusInput, double planetMass, double planetRadius, PeriodMeassureMethod periodMethod, DistanceMeassureMethod orbitMethod)
		{
			double wantedOrbitRadius, result;
			if (double.TryParse(wantedOrbitRadiusInput, out wantedOrbitRadius))
			{
				Debugger.AdvLog(wantedOrbitRadius.ToString(), true);
				result = Math.Sqrt((4d * Math.Pow(Math.PI, 2d) * Math.Pow(wantedOrbitRadius * (orbitMethod == DistanceMeassureMethod.Kilometer ? 1000d : 1d) + planetRadius, 3d)) / (Constants.gravityConstant * planetMass));
				switch (periodMethod)
				{
					case PeriodMeassureMethod.Minutes:
						result /= 60d;
						break;
					case PeriodMeassureMethod.Hours:
						result /= 3600d;
						break;
				}
				return result;
			}
			return double.NaN;
		}

		/// <summary>
		/// calculates the semimajoraxis for a circular orbit of the certain object based on the wanted orbitperiod
		/// </summary>
		/// <param name="wantedOrbitPeriodInput"></param>
		/// <param name="planetMass"></param>
		/// <param name="planetRadius"></param>
		/// <param name="periodMethod"></param>
		/// <param name="orbitMethod"></param>
		/// <returns></returns>
		public static double calculateOrbitRadius(string wantedOrbitPeriodInput, double planetMass, double planetRadius, PeriodMeassureMethod periodMethod, DistanceMeassureMethod orbitMethod)
		{
			double wantedOrbitPeriod, result;
			if (double.TryParse(wantedOrbitPeriodInput, out wantedOrbitPeriod))
			{
				Debugger.AdvLog(wantedOrbitPeriod.ToString(), true);
				result = ((Math.Pow(Constants.gravityConstant, (1d / 3d)) * Math.Pow(planetMass, (1d / 3d)) * Math.Pow(wantedOrbitPeriod * (periodMethod == PeriodMeassureMethod.Hours ? 3600d : 60d), (2d / 3d))) / (Math.Pow(2d, (2d / 3d)) * Math.Pow(Math.PI, (2d / 3d)))) - planetRadius;
				switch (orbitMethod)
				{
					case DistanceMeassureMethod.Meter:
						result *= 1d;
						break;
					case DistanceMeassureMethod.Kilometer:
						result /= 1000d;
						break;
				}
				return result;
			}
			return double.NaN;
		}

	}

}

