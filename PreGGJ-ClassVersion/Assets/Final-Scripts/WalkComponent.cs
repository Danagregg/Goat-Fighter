using UnityEngine;

/// <summary>
/// Allow a 2D Character to walk in either direction using physics
/// </summary>
public class WalkComponent : MonoBehaviour
{
    [SerializeField]    string      _inputAxis;

    [SerializeField]    Animator    _animator;
    [SerializeField]    string      _animationBool;
    [SerializeField]    float       _acceleration = 1000.0f;
    [SerializeField]    float       _maxSpeed = 3.0f;
    [SerializeField]    Rigidbody2D	_rigidBody;
    [SerializeField]    float       _stopMultiplier = 0.0f;

    private int         _animationHash;
    private float       _ignoreUntil;

	/// <summary>
	/// Awake is the very first thing a Component does.  We can cache stuff here.
	/// </summary>
    void Awake()
    {
        _animationHash = Animator.StringToHash( _animationBool );
    }

    /// <summary>
    /// We should be in *FixedUpdate* not in Update because we're modifying Rigidbody
    /// </summary>
    void FixedUpdate()
    {
        float axis = Input.GetAxis(_inputAxis);

        Vector3 velocity = _rigidBody.velocity;

        // Are we trying to move in the opposite direction (or stopping?)
        if ( axis == 0.0f || (Mathf.Sign(_rigidBody.velocity.x) != Mathf.Sign(axis)) )
        {
            velocity.x *= _stopMultiplier;

            _rigidBody.velocity = velocity;
        }

        if ( axis != 0.0f )
        {
            float xMovement = axis * _acceleration * Time.deltaTime;
            Vector2 moveVector = new Vector2( xMovement, 0.0f );

            // Turn to face the right direction...
            if ( axis > 0.0f )
                _rigidBody.transform.localRotation = Quaternion.AngleAxis( 180.0f, Vector3.up );
            else
                _rigidBody.transform.localRotation = Quaternion.AngleAxis( 0.0f, Vector3.up );

            if ( Time.time < _ignoreUntil )
                return;

            _rigidBody.AddForce( moveVector );
        }

		// Clamp horizontal speed
        if ( Mathf.Abs(velocity.x) > _maxSpeed )
            velocity.x = Mathf.Sign(velocity.x) * _maxSpeed;

		// Notice I'm not modifying _rigidBody.velocity directly, instead I'm assigning?
		// This is because .velocity is a property and cannot be modified directly, you *must* assign it.
        _rigidBody.velocity = velocity;

		// Should we be showing the walk animation?  We do if we have any input.
        if ( _animator && _animationHash != 0 )
            _animator.SetBool( _animationHash, axis != 0.0f );
    }

	// We get this via a Message.  Messages are used if we're not entirely sure if a GameObject as a component on it.
	// So we can send a "Message" telling the Component that something has happened.  If we're listening for the message, we can react to it.
    void MsgStopWalking( float ignoreUntil )
    {
        _ignoreUntil = ignoreUntil;
    }
}
