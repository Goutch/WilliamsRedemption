using System.Collections.Generic;
using Harmony;
using Playmode.Tracks;
using UnityEngine;

namespace Playmode.Cars.Driving
{
    /// <summary>
    /// Car drinving handled by AI.
    /// </summary>
    [AddComponentMenu("Game/Cars/Driving/AIInput")]
    public class CarAiInput : MonoBehaviour
    {
        private Car car;
        private Track track;
        private IEnumerator<Checkpoint> checkpointsLoop;

        private Checkpoint CurrentCheckpoint => checkpointsLoop?.Current;

        private void Awake()
        {
            car = this.GetComponentInSiblings<Car>();
            track = this.GetComponentInTaggedObject<Track>(R.S.Tag.Track);
            checkpointsLoop = track.CheckpointLoop();
            checkpointsLoop.MoveNext();
        }

        private void Update()
        {
            var position = car.Position;

            if (CurrentCheckpoint.HasPassedCheckpoint(position)) checkpointsLoop.MoveNext();

            var destination = CurrentCheckpoint.ClosestPointTo(position);
            var direction = (destination - position).normalized;

            //Dot product gives value between 0 and 1.
            var throttlePercentage = Vector2.Dot(car.Forward, direction); //TODO : Make sure throttle doesn't go bellow a certain threshold.
            var steeringPercentage = Vector2.Dot(car.Right, direction);

            car.Move(throttlePercentage);
            car.Steer(steeringPercentage);

            //TODO : Add debug lines
            Debug.DrawLine(position, destination, Color.green);
        }
    }
}