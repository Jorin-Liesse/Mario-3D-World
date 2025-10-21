using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [HideInInspector] public enum PlayerStates 
    {
        Standing,
        Waling,
        Running,
        Jumping,
        Crawling,
        Falling,
        Swimming,
        DoubleJumping,
        Dansing
    }
    [HideInInspector] public PlayerStates PlayerState = PlayerStates.Standing;

    private PlayerStates _prePlayerState;

    void Start()
    {
        _prePlayerState = PlayerState;
    }

    void Update()
    {
        if (PlayerState != _prePlayerState) {
            
            switch (PlayerState)
            {
                case PlayerStates.Standing:
                    _animator.SetTrigger("TR_Idle");
                    break;
                case PlayerStates.Waling:
                    _animator.SetTrigger("TR_Walking");
                    break;
                case PlayerStates.Running:
                    _animator.SetTrigger("TR_Running");
                    break;
                case PlayerStates.Jumping:
                    _animator.SetTrigger("TR_Jumping");
                    break;
                case PlayerStates.DoubleJumping:
                    _animator.SetTrigger("TR_Flip");
                    break;
                case PlayerStates.Crawling:
                    _animator.SetTrigger("TR_Crawling");
                    break;
                case PlayerStates.Falling:
                    break;
                case PlayerStates.Swimming:
                    break;
                case PlayerStates.Dansing:
                    _animator.SetTrigger("TR_Macarena");
                   break;
            }
        }

        _prePlayerState = PlayerState;
    }
}
