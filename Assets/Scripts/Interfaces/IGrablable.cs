using UnityEngine;

namespace Interfaces
{
    public interface IGrabable
    {
        public void OnHover();
        public Rigidbody OnGrab();
        public void OnRelease();
        public void StopHover();
    }
}
