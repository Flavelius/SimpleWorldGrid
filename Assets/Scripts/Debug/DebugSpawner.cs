using System.Collections.Generic;
using UnityEngine;

namespace SimpleWorldGrid.Debug
{
    public class DebugSpawner : MonoBehaviour
    {
        [Header("Grid Update cycle Time (ms)")] public float AverageCycleTime;

        readonly List<DebugEntity> _entities = new List<DebugEntity>();

        SimpleWorldGrid<DebugEntity> _grid;
        public bool Move;
        public bool DrawGizmos;
        public static bool DrawEntityGizmos;

        [Header("Movement Testing")] public float MoveSpeed = 5f;

        [Tooltip("this value squared represents the number of test entites")] public int NumPartBase = 10;

        readonly List<float> _rndTime = new List<float>();

        int _seed = 13;
        readonly List<Vector3> _startPositions = new List<Vector3>();

        [Tooltip("This is the range in which each entity is notified about other entities")] public float SubscriptionRange = 200f;

        void Start()
        {
            _grid = new SimpleWorldGrid<DebugEntity>(SubscriptionRange, this);
            for (var x = 0; x < NumPartBase; x++)
            {
                for (var z = 0; z < NumPartBase; z++)
                {
                    var rndDelta = new Vector3(Random.Range(0, SubscriptionRange*NumPartBase*0.25f), 0, Random.Range(0, SubscriptionRange*NumPartBase*0.25f));
                    var ge = new GameObject((x*NumPartBase + z).ToString()).AddComponent<DebugEntity>();
                    ge.transform.position = rndDelta;
                    ge.transform.parent = transform;
                    Random.seed = _seed;
                    _seed++;
                    _grid.Add(ge);
                    _entities.Add(ge);
                    _startPositions.Add(ge.transform.position);
                    _rndTime.Add(Random.Range(0f, 3f));
                }
            }
        }

        void Update()
        {
            DrawEntityGizmos = DrawGizmos;
            AverageCycleTime = _grid.UpdateCycleTime;
            if (!Move)
            {
                return;
            }
            var ti = Time.time + 0.5f;
            for (var i = _entities.Count; i-- > 0;)
            {
                var rndTime1 = _rndTime[i] + 1f;
                _entities[i].transform.position = _startPositions[i] +
                                                 new Vector3(Mathf.PingPong(ti + _rndTime[i], rndTime1)*MoveSpeed, 0,
                                                     Mathf.PingPong(Time.time + _rndTime[i], rndTime1)*MoveSpeed);
                if (_entities[i].DestroyMe)
                {
                    _grid.Remove(_entities[i]);
                    _entities.RemoveAt(i);
                }
            }
        }
    }
}