using UnityEngine;

/// <summary>
/// Allow a 2D Character to walk in either direction using physics
/// </summary>
public class AttackComponent : MonoBehaviour
{
    [SerializeField]    string      _inputButton;

    [SerializeField]    Animator    _animator;
    [SerializeField]    string      _animationTrigger;

    [SerializeField]    float       _attackSpeed = 50.0f;
	[SerializeField]	float		_attackTime = 0.5f;
    [SerializeField]    Rigidbody2D	_rigidBody;

    private float       _ignoreUntil;
	private float		_attackTimeLeft;

	void Update()
	{
		if ( Time.time <= _ignoreUntil )
		{
            return;
		}

        if ( Input.GetButton( _inputButton) )
		{
			_attackTimeLeft = _attackTime;

			if ( _animator && !string.IsNullOrEmpty(_animationTrigger) )
			{
				_animator.SetTrigger(_animationTrigger);
			}
		}
	}

    /// <summary>
    /// We should be in *FixedUpdate* not in Update because we're modifying Rigidbody
    /// </summary>
    void FixedUpdate()
    {
		if ( _attackTimeLeft <= 0.0f )
		{
			_animator.ResetTrigger( _animationTrigger );
			return;
		}
		else
		{
			_attackTimeLeft -= Time.deltaTime;
		}

        Vector2 desiredVelocity = -_rigidBody.transform.right * _attackSpeed;
		Vector2 acceleration = desiredVelocity / _attackTime;

        _rigidBody.AddForce( acceleration * _rigidBody.mass, ForceMode2D.Force );

		// Just clamp the attack speed.
		Vector3 velocity = _rigidBody.velocity;
		if ( Mathf.Abs(velocity.x) > _attackSpeed )
		{
			// Remember, we can't modify _rigidBody.velocity directly... it must be assigned in one-go.
			velocity = velocity.normalized * (Mathf.Sign(_attackSpeed) * Mathf.Abs(_attackSpeed));
			_rigidBody.velocity = velocity;
		}

        _ignoreUntil = Time.time + 1.0f;
        SendMessage( "StopWalking", Time.time + 0.25f, SendMessageOptions.DontRequireReceiver );
    }

    void OnCollisionEnter2D( Collision2D info )
    {
        bool bWeAreAttacking = Time.time < _ignoreUntil;
        if ( !bWeAreAttacking )
		{
            return;
		}

        // Did we collide with a non-static collider?
        if ( info.rigidbody )
        {
            bool bWeAreMovingFaster = _rigidBody.velocity.sqrMagnitude > info.rigidbody.velocity.sqrMagnitude;
            if ( bWeAreMovingFaster )
                info.rigidbody.SendMessage( "TakeDamage", SendMessageOptions.DontRequireReceiver );
        }
    }
}
