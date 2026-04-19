using UnityEngine;

namespace Project.Scripts.Utils.Components
{
    /// <summary>
    /// Cheap water-like motion: vertical bob and horizontal circle with independent angular speeds.
    /// </summary>
    public class WaterMover : MonoBehaviour
    {
        [SerializeField] private float _radius = 0.35f;
        [SerializeField] private float _verticalAmplitude = 0.12f;
        [Tooltip("Vertical bob angular speed in rad/s (different from horizontal => not in sync).")]
        [SerializeField] private float _verticalAngularSpeed = 1.1f;
        [Tooltip("Horizontal circular motion angular speed in rad/s.")]
        [SerializeField] private float _horizontalAngularSpeed = 0.65f;
        [SerializeField] private float _verticalPhase;
        [SerializeField] private float _horizontalPhase;
        [SerializeField] private bool _randomizePhasesOnAwake;

        private Vector3 _anchorPosition;

        private void Awake()
        {
            _anchorPosition = transform.position;

            if (_randomizePhasesOnAwake)
            {
                _verticalPhase = Random.Range(0f, Mathf.PI * 2f);
                _horizontalPhase = Random.Range(0f, Mathf.PI * 2f);
            }
        }

        private void Update()
        {
            float t = Time.time;
            float y = _verticalAmplitude * Mathf.Sin(t * _verticalAngularSpeed + _verticalPhase);
            float angle = t * _horizontalAngularSpeed + _horizontalPhase;
            float x = Mathf.Cos(angle) * _radius;
            float z = Mathf.Sin(angle) * _radius;

            transform.position = _anchorPosition + new Vector3(x, y, z);
        }
    }
}
