using System.Collections;
using System.Collections.Generic;
using System.Net.Configuration;
using Game.Values;
using Harmony;
using UnityEngine;

namespace Game.Puzzle
{
	public class SecretRoom : MonoBehaviour
	{

		[SerializeField] private Camera LevelCamera;
		[SerializeField] private LayerMask RoomLayers;
		[SerializeField] private LayerMask OutsideLayers;
		private BoxCollider2D room;


		private void Awake()
		{
			room = GetComponent<BoxCollider2D>();
		}

		private void OnTriggerStay2D(Collider2D other)
		{
			if (other.Root().CompareTag(Values.Tags.Player))
			{
				LevelCamera.cullingMask = RoomLayers;
			}
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (other.Root().CompareTag(Values.Tags.Player))
			{
				LevelCamera.cullingMask = OutsideLayers;
			}
		}
	}
}
