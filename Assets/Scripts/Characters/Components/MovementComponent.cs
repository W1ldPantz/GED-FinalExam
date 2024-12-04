using Managers;
using UnityEngine;
using Utility;

namespace Characters.Components
{
    public class MovementComponent : MonoBehaviour
    {
        private Vector3 _moveDir;
        private Vector2 _lookDir;
        private Vector3 _groundNormal;
        public bool IsGrounded { get; private set; }

        private Rigidbody _rb;
        private Player _player;

        [Header("Looking")]
        
        [SerializeField] private Transform body;
        
        [SerializeField] private float lookSpeed;
        [SerializeField, Range(0, 90)] private float pitchLimit;
        
        [Header("Ground")]
        [SerializeField] private Transform foot;
        [SerializeField] private float footRadius;
        [SerializeField] private float stepHeight;
        
        private void Awake()
        {
            IsGrounded = true; // Temp
            _rb = GetComponent<Rigidbody>();
            _player = GetComponent<Player>();
            
        }

        public void Update()
        {
            float deltaTime = Time.deltaTime;
            HandleGround();
            HandleMovement(deltaTime);
            HandleLooking(deltaTime);
        }

        private void HandleMovement(float deltaTime)
        {
            Vector3 direction = _moveDir;
            
            //Better slope movement
            if (IsGrounded)
            {
                direction = Vector3.ProjectOnPlane(_moveDir, _groundNormal);
            }

            _rb.AddForce(body.rotation * direction * (_player.speed * deltaTime), ForceMode.Acceleration);

            if (IsGrounded)
            {
                Vector3 vec = _rb.linearVelocity;

                float y = vec.y;
                vec.y = 0;

                float magnitude = vec.magnitude;

                if (magnitude > _player.maxSpeed)
                {
                    Vector3 newVel = vec.normalized * _player.maxSpeed;
                    newVel.y = y;
                    _rb.linearVelocity = newVel;
                }
            }
        }

        private void HandleGround()
        {
           IsGrounded = Physics.SphereCast(foot.position, footRadius, Vector3.down , out RaycastHit ground, stepHeight,  StaticUtility.GroundLayers);
           if (IsGrounded)
           {
               _groundNormal = ground.normal;
           }
        }

        private void HandleLooking(float deltaTime)
        {
            //This is for PC first person.
            float deltaX = _player.Head.localEulerAngles.x + deltaTime * _lookDir.y * lookSpeed;
            float deltaY = body.localEulerAngles.y + deltaTime * _lookDir.x * lookSpeed;

            if (deltaX > pitchLimit && deltaX < 180) deltaX = pitchLimit;
            else if (deltaX < 360 - pitchLimit && deltaX > 180) deltaX = -pitchLimit;

            _player.Head.localEulerAngles = new Vector3(deltaX, 0, 0);
            body.localEulerAngles = new Vector3(0, deltaY, 0);
        }

        public void Look(Vector2 readValue)
        {
            _lookDir = readValue;
        }

        public void Move(Vector2 readValue)
        {

            _moveDir = new Vector3(readValue.x, 0, readValue.y);
            ;
        }
        

        public void Jump()
        {
            if (CanJump())
            {
                ExecuteJump();
            }
        }

        private bool CanJump()
        {
            return IsGrounded;
        }

        private void ExecuteJump()
        {
            _rb.AddForce(Vector3.up * _player.jumpHeight, ForceMode.Impulse);
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(foot.position, footRadius);
            Gizmos.DrawLine(foot.position,foot.position + Vector3.down* stepHeight);
            Gizmos.DrawWireSphere(foot.position + Vector3.down * stepHeight, footRadius);
        }
    }
}
