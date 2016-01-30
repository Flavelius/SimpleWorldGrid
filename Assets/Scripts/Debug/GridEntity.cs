using System.Collections.Generic;
using UnityEngine;

namespace SimpleWorldGrid.Debug
{
    public class DebugEntity : MonoBehaviour, IGridEntity<DebugEntity>
    {
        public bool DestroyMe;

        readonly HashSet<DebugEntity> _subscriptions = new HashSet<DebugEntity>();

        public Vector3 Position
        {
            get { return transform.position; }
        }

        public int GetID()
        {
            return GetInstanceID();
        }

        public void OnEntityBecameRelevant(DebugEntity other)
        {
            if (other == this)
            {
                UnityEngine.Debug.Log("self added");
            }
            _subscriptions.Add(other);
        }

        public void OnEntityBecameIrrelevant(DebugEntity other)
        {
            if (other == this)
            {
                UnityEngine.Debug.Log("self removed");
            }
            _subscriptions.Remove(other);
        }

        public void OnEntityBecameIrrelevant(int otherID)
        {
            foreach (var subscription in _subscriptions)
            {
                if (subscription.GetID() != otherID) continue;
                _subscriptions.Remove(subscription);
                break;
            }
        }

        void OnDrawGizmosSelected()
        {
            if (!DebugSpawner.DrawEntityGizmos) return;
            Gizmos.color = Color.gray;
            foreach (var other in _subscriptions)
            {
                Gizmos.DrawLine(Position, other.Position);
            }
        }
    }
}