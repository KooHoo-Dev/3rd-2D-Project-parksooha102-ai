using UnityEngine;

namespace _2DProject
{
    public class LabCamera : MonoBehaviour
    {
        [field: SerializeField] public Transform AimPoint { get; set; }
        [field: SerializeField] public Transform Player { get; set; }
        [SerializeField] private float lerpSpeed;
        [SerializeField] private float zoomMultiplier;
        [SerializeField] private Vector3 offSet;

        private Camera LabCam { get; set; }

        private void Start()
        {
            LabCam = Camera.main;
            zoomMultiplier = Mathf.Clamp(zoomMultiplier, 0.0f, 1.0f);
        }

        private void Update()
        {
            Vector3 cameraPos = AimPoint.position * zoomMultiplier + Player.position * (1.0f - zoomMultiplier);
            Vector3 lerpPos = Vector3.Lerp(transform.position, cameraPos + offSet, lerpSpeed * Time.deltaTime);
            lerpPos.z = transform.position.z;
            transform.position = lerpPos;
        }

    }
}