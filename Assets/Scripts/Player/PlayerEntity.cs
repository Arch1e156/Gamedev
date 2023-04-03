using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


namespace Player 
{
    [RequireComponent(typeof(Rigidbody2D))]

    public class PlayerEntity : MonoBehaviour
    {
        [SerializeField] private DirectionalMovementData _directionalMovementData;
        [SerializeField] private JumperData _jumperData;

        private DirectionMover _directionMover;
        private Jumper _jumper;

        [Header("Jump")]
        [SerializeField] private JumpPointController _jumpPointController;
        [SerializeField] private bool _isJumping;

        [SerializeField] private bool _isAttacking;

        private Rigidbody2D _rigidbody;

        [SerializeField] private Animator _animator;
        private AnimationType _currentAnimationType;

        
        


        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _directionMover = new DirectionMover(_rigidbody, _directionalMovementData);
            _jumper = new Jumper(_rigidbody, _jumpPointController, _jumperData);
        }
        private void Update()
        {
            GetIsJump();
            UpdateAnimations();

        }
        private void GetIsJump() { _isJumping = _jumpPointController.IsJumping(); }
        
        private void UpdateAnimations() 
        {
            PlayAnimation(AnimationType.Idle, true);
            PlayAnimation(AnimationType.Walk, _directionMover.IsMoving);
            PlayAnimation(AnimationType.Jump, _isJumping);
            PlayAnimation(AnimationType.Attack, _isAttacking);
        }
        public void Attack() 
        {
            _isAttacking = true;
        }
        private void AttackStop()
        {
            _isAttacking = false;
        }
        public void MoveHorizontally(float direction) => _directionMover.MoveHorizontally(direction);
        public void Jump() => _jumper.Jump();
        
        private void PlayAnimation(AnimationType animationType, bool active) 
        {
            if (!active) 
            {
                if(_currentAnimationType == AnimationType.Idle || _currentAnimationType != animationType) 
                {
                    return;
                }
                _currentAnimationType = AnimationType.Idle;
                PlayAnimation(_currentAnimationType);
                return;
            }
            if(_currentAnimationType > animationType) 
            {
                return;
            }
            _currentAnimationType = animationType;
            PlayAnimation(_currentAnimationType);
        }
        private void PlayAnimation(AnimationType animationType) 
        {
            _animator.SetInteger(nameof(AnimationType), (int)animationType);
        }


    }

}
