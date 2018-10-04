using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITriggerable  {

	void Open();
	void Close();
	bool CanBeOpened();
	bool IsOpened();

}
