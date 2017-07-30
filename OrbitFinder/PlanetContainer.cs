namespace OrbitFinder
{
	/// <summary>
	/// simple container for the planets so that everything get sorted and indexed
	/// </summary>
	public class PlanetContainer
	{
		private CelestialBody _planet;
		private int _descendingIndex;

		public CelestialBody planet
		{
			get { return _planet; }
			set { _planet = value; }
		}

		public int descendingIndex
		{
			get { return _descendingIndex; }
			set { _descendingIndex = value; }
		}

		public PlanetContainer(CelestialBody planet, int index)
		{
			_planet = planet;
			_descendingIndex = index;
		}

	}
}
