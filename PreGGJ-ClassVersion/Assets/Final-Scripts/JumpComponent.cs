using UnityEngine;

/// <summary>
/// Allow a 2D Character to walk in either direction using physics
/// </summary>
public class JumpComponent : MonoBehaviour
{
    [SerializeField]    string			_inputAxis;

    [SerializeField]    Animator		_animator;
    [SerializeField]    string			_animationTrigger;
    [SerializeField]    float			_force = 10.0f;
    [SerializeField]    Rigidbody2D		_rigidBody;

    private float       _ignoreJumpUntil;

    private bool isGrounded
    {
        get
        {
			// First, if we're already jumping/falling, we're obviously not on the ground
			if ( _rigidBody.velocity.y != 0.0f )
				return false;

			// But, if you think about a jumping curve... well you hit at 0-velocity at the very top
			// then you start to fall.
            foreach ( var hit in Physics2D.RaycastAll( _rigidBody.position, -Vector2.up, 0.1f ) )
            {
                if ( hit.rigidbody != _rigidBody )
                    return true;
            }

            return false;
        }
    }

    /*
     * This is a debugging trick
     */
    /*
    void OnGUI()
    {
        GUI.Label( new Rect( 0.0f, 0.0f, 200.0f, 200.0f), "IsGrounded: " + isGrounded );
    }
     */

    void FixedUpdate()
    {
        if ( Time.time < _ignoreJumpUntil || !isGrounded )
        {
            if ( _animator )
                _animator.ResetTrigger( _animationTrigger );

            return;
        }

        float axis = Input.GetAxis(_inputAxis);
        if ( axis > 0.0f )
        {
			// If we don't have this, Unity mimics a controller the axis can be between 0.0 and 1.0... not what we want.
			axis = 1.0f;

            float yMovement = axis * _force;
            Vector2 moveVector = new Vector2( 0.0f, yMovement );

			Debug.LogFormat( this, "Rigid Body Velocity Before Force {0} is applied: {1}", yMovement, _rigidBody.velocity );
 
            _rigidBody.AddForce( moveVector, ForceMode2D.Impulse );
            _ignoreJumpUntil = Time.time + 0.1f;

            if ( _animator && !string.IsNullOrEmpty(_animationTrigger) )
                _animator.SetTrigger(_animationTrigger);
        }
    }
}
