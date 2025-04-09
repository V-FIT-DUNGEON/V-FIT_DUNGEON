using UnityEngine;

public class FootstepManager : MonoBehaviour {
    public AudioSource audioSource;
    public AudioClip[] footstepClips;
    public float stepDelay = 0.5f;

    private float nextStepTime = 0f;
    [SerializeField] private CharacterController characterController;

    void Start() {
        characterController = GetComponent<CharacterController>();
        if (audioSource == null) {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update() {
        // Only consider horizontal (XZ) velocity
        Vector3 horizontalVelocity = new Vector3(characterController.velocity.x, 0f, characterController.velocity.z);
        float horizontalSpeed = horizontalVelocity.magnitude;

        Debug.Log(characterController.isGrounded + " " + horizontalSpeed);
        if (characterController != null && characterController.isGrounded && horizontalSpeed > 0.1f) {
            if (Time.time >= nextStepTime) {
                PlayFootstep();
                nextStepTime = Time.time + stepDelay;
            }
        }
    }

    void PlayFootstep() {
        if (footstepClips.Length > 0) {
            audioSource.clip = footstepClips[0]; // You can randomize this later
            audioSource.Play();
        }
    }
}
