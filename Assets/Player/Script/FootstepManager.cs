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
        Debug.Log(characterController.isGrounded + " " + characterController.velocity.magnitude);
        if (characterController != null && characterController.isGrounded && characterController.velocity.magnitude > 0.1f) {
            if (Time.time >= nextStepTime) {
                PlayFootstep();
                nextStepTime = Time.time + stepDelay;
            }
        }
    }

    void PlayFootstep() {
        audioSource.clip = footstepClips[0];
        audioSource.Play();
    }
}
