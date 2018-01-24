using UnityEngine;

public class WardTestPlayer : MonoBehaviour
{
	public float speed = 7.0f;
	public float gravity = 12.5f;

	CharacterController _controller = null;

	// "A Free Simple Smooth Mouselook"
	// Because I'm lazy, taken from: https://forum.unity.com/threads/a-free-simple-smooth-mouselook.73117/
	Vector2 _mouseAbsolute;
	Vector2 _smoothMouse;

	public Vector2 clampInDegrees = new Vector2(360, 180);
	public bool lockCursor;
	public Vector2 sensitivity = new Vector2(2, 2);
	public Vector2 smoothing = new Vector2(3, 3);
	public Vector2 targetDirection;
	public Vector2 targetCharacterDirection;

	// Assign this if there's a parent object controlling motion, such as a Character Controller.
	// Yaw rotation will affect this object instead of the camera if set.
	public GameObject characterBody;

	void Start()
	{
		_controller = GetComponent<CharacterController>();

		// "A Free Simple Smooth Mouselook"
		{
			// Set target direction to the camera's initial orientation.
			targetDirection = transform.localRotation.eulerAngles;

			// Set target direction for the character body to its inital state.
			if (characterBody)
				targetCharacterDirection = characterBody.transform.localRotation.eulerAngles;
		}
	}

	void Update()
	{
		if (_controller)
		{
			Vector3 moveDirection = Vector3.zero;

			if (_controller.isGrounded)
			{
				if (Input.GetKey(KeyCode.W))
				{
					moveDirection.z = 1.0f;
				}
				else if (Input.GetKey(KeyCode.S))
				{
					moveDirection.z = -1.0f;
				}
				if (Input.GetKey(KeyCode.A))
				{
					moveDirection.x = -1.0f;
				}
				else if (Input.GetKey(KeyCode.D))
				{
					moveDirection.x = 1.0f;
				}
				moveDirection = transform.TransformDirection(moveDirection);        // transform to world space
				moveDirection.y = 0.0f;
				moveDirection.Normalize();
				moveDirection *= speed;
			}

			moveDirection.y -= gravity;
			_controller.Move(moveDirection * Time.deltaTime);

			DebugUI debugUI = GameObject.FindGameObjectWithTag("DebugUI").GetComponent<DebugUI>();
			if (debugUI != null)
			{
				debugUI.AddText("Player position: (" + transform.position.x + ", " + transform.position.y + ", " + transform.position.z + ")");
			}
		}

		// "A Free Simple Smooth Mouselook"
		{
			// Ensure the cursor is always locked when set
			if (lockCursor)
			{
				Cursor.lockState = CursorLockMode.Locked;
			}

			// Allow the script to clamp based on a desired target value.
			var targetOrientation = Quaternion.Euler(targetDirection);
			var targetCharacterOrientation = Quaternion.Euler(targetCharacterDirection);

			// Get raw mouse input for a cleaner reading on more sensitive mice.
			var mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

			// Scale input against the sensitivity setting and multiply that against the smoothing value.
			mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity.x * smoothing.x, sensitivity.y * smoothing.y));

			// Interpolate mouse movement over time to apply smoothing delta.
			_smoothMouse.x = Mathf.Lerp(_smoothMouse.x, mouseDelta.x, 1f / smoothing.x);
			_smoothMouse.y = Mathf.Lerp(_smoothMouse.y, mouseDelta.y, 1f / smoothing.y);

			// Find the absolute mouse movement value from point zero.
			_mouseAbsolute += _smoothMouse;

			// Clamp and apply the local x value first, so as not to be affected by world transforms.
			if (clampInDegrees.x < 360)
				_mouseAbsolute.x = Mathf.Clamp(_mouseAbsolute.x, -clampInDegrees.x * 0.5f, clampInDegrees.x * 0.5f);

			// Then clamp and apply the global y value.
			if (clampInDegrees.y < 360)
				_mouseAbsolute.y = Mathf.Clamp(_mouseAbsolute.y, -clampInDegrees.y * 0.5f, clampInDegrees.y * 0.5f);

			transform.localRotation = Quaternion.AngleAxis(-_mouseAbsolute.y, targetOrientation * Vector3.right) * targetOrientation;

			// If there's a character body that acts as a parent to the camera
			if (characterBody)
			{
				var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, Vector3.up);
				characterBody.transform.localRotation = yRotation * targetCharacterOrientation;
			}
			else
			{
				var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, transform.InverseTransformDirection(Vector3.up));
				transform.localRotation *= yRotation;
			}
		}
	}
}
