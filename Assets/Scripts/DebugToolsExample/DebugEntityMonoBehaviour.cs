using System;
using Unity.Entities;
using UnityEngine;

namespace DebugToolsExample
{
    public class DebugEntityMonoBehaviour : MonoBehaviour
    {
        public float current;
        public float total;
        
        public float healthPercentage;
        
        public Entity entity;
    }
    
    public struct DebugEntitySystemComponent : ISystemStateSharedComponentData, IEquatable<DebugEntitySystemComponent>
    {
        public DebugEntityMonoBehaviour debug;

        public bool Equals(DebugEntitySystemComponent other)
        {
            return Equals(debug, other.debug);
        }

        public override bool Equals(object obj)
        {
            return obj is DebugEntitySystemComponent other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (debug != null ? debug.GetHashCode() : 0);
        }
    }
}