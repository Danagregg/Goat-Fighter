using UnityEngine;
using System.Collections;

public class JumpExample : MonoBehaviour {

	public Rigidbody2D _rigidBody;
	public float _jumpSpeed;
	public Animator _animator;
	public string _jumpButton;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetButtonDown(_jumpButton)) {

			Vector2 jumpForce = new Vector2 (0.0f, _jumpSpeed * Time.deltaTime);
			_rigidBody.AddForce(jumpForce);
			_animator.SetTrigger ("Jump");
		}
	}
}
