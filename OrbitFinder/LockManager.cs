using System.Collections.Generic;
using UnityEngine;

namespace OrbitFinder
{
	public static class LockManager
	{
		private static List<Lock> _activeLocks = new List<Lock>();

		/// <summary>
		/// locks the editor keys for the given key
		/// </summary>
		/// <param name="loadButton"></param>
		/// <param name="exitButton"></param>
		/// <param name="saveButton"></param>
		/// <param name="lockKey"></param>
		public static void lockKey(ControlTypes locks, string lockKey)
		{
			if (!isLockKeyActive(lockKey))
			{
				InputLockManager.SetControlLock(locks, lockKey);
				_activeLocks.Add(new Lock(lockKey));
			}
		}

		/// <summary>
		/// unlocks the editor for the entered key
		/// </summary>
		/// <param name="lockKey"></param>
		public static void unlockKey(string lockKey)
		{
			if (isLockKeyActive(lockKey))
			{
				InputLockManager.RemoveControlLock(lockKey);
				for (int i = 0; i < _activeLocks.Count; i++)
				{
					if (_activeLocks[i].lockKey == lockKey)
					{
						_activeLocks.RemoveAt(i);
						return;
					}
				}
			}
		}

		/// <summary>
		/// returns the info about the current lockstatus
		/// </summary>
		/// <returns></returns>
		public static bool isLocked()
		{
			return _activeLocks.Count > 0 ? true : false;
		}

		/// <summary>
		/// provides all the keys that are currently in use
		/// </summary>
		/// <returns></returns>
		public static string[] getActiveLockKeys()
		{
			string[] locks = new string[_activeLocks.Count];
			for (int i = 0; i < locks.Length; i++)
			{
				locks[i] = _activeLocks[i].lockKey;
			}
			return locks;
		}

		/// <summary>
		/// provides the binary information if the key is already in use
		/// </summary>
		/// <param name="lockKey"></param>
		/// <returns></returns>
		public static bool isLockKeyActive(string lockKey)
		{
			foreach (Lock l in _activeLocks)
			{
				if (l.lockKey == lockKey)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// resets the editorlocks to a clean state
		/// </summary>
		public static void resetLocks()
		{
			foreach (Lock editorLock in _activeLocks)
			{
				InputLockManager.RemoveControlLock(editorLock.lockKey);
			}
			_activeLocks.Clear();
		}

		/// <summary>
		/// will lock a Rect by a certain key and for specific locks
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="locks"></param>
		/// <param name="key"></param>
		public static void preventClickThrough(Rect rect, ControlTypes locks, string key)
		{
			Vector2 pointerPos = Mouse.screenPos;
			if (rect.Contains(pointerPos))
			{
				lockKey(locks, key);
			}
			else if (!rect.Contains(pointerPos))
			{
				unlockKey(key);
			}
		}

	}
}

