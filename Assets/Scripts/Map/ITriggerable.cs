using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITriggerable  {

	void Open();
	void Close();
	void Unlock();
	void Lock();
	bool IsLocked();
	bool IsOpened();
}
