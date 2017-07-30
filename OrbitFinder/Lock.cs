using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrbitFinder
{
	public class Lock
	{
		private string _key;

		public Lock(string key)
		{
			_key = key;
		}

		public string lockKey
		{
			get { return _key; }
			set { _key = value; }
		}
	}
}
