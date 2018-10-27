using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITriggerable  {

	void Open();
	void Close();
	bool IsLocked();
	bool IsOpened();
	void Unlock();
	void Lock();
}
