using UnityEngine;

namespace Project.Scripts.GamePlay.Player.PlayerEffects
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class BookPageView : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private Transform _pageTransform;

        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshRenderer _meshRenderer;

        [Header("Icon Overlay (UV space)")] [SerializeField]
        private Color _iconColor = Color.white;

        [SerializeField] [Range(0f, 1f)] private float _iconEmmisionPower = 1f;

        [Header("Bend (SimpleDeform-style)")] [SerializeField]
        private BendAxis _bendAxis = BendAxis.Y;

        [SerializeField] [Range(0f, 1f)] private float _bendOrigin = 0f;
        [SerializeField] [Range(-180f, 180f)] private float _bendAngle;
        [SerializeField] private float _maxBendAngle = 70;
        [SerializeField] private AnimationCurve _bendCurve;
        [Header("Rotation")] [SerializeField] private bool _left;
        [SerializeField] private Vector2 _rotationFromTo = new Vector2(-65, 65);
        [SerializeField] private AnimationCurve _rotationCurve;
        [Header("Animation")] [SerializeField] private float _pageChangeAnimationDuration = 0.35f;

        [SerializeField] private float _iconFadeAnimationDuration = 0.4f;
        [SerializeField] private AnimationCurve _fadeCurve;

        [Header("Test")] [SerializeField] private bool _testing;

        [SerializeField, Range(0f, 1f)] private float _progress;

        private Material _material;
        private Mesh _deformableMesh;
        private Vector3[] _originalVertices;
        private Vector3[] _originalNormals;
        private Bounds _meshBounds;
        private float _lastProgress;

        private bool _animationInProgress;

        private bool _fadeAnimationInProgress;
        private float _fadeElapsed;
        private float _fadeFromOpacity;
        private float _fadeToOpacity;

        private static readonly int IconTextureId = Shader.PropertyToID("_IconTexture");

        private static readonly int IconRectId = Shader.PropertyToID("_IconRect");

        private static readonly int IconColorId = Shader.PropertyToID("_IconColor");

        private static readonly int IconEmissionStrengthId = Shader.PropertyToID("_IconEmissionStrength");
        private static readonly int IconOpacityId = Shader.PropertyToID("_IconOpacity");


        public event System.Action AnimationEnd;
        public event System.Action FadeAnimationEnd;

        public enum BendAxis
        {
            X = 0,
            Y = 1,
            Z = 2
        }

        public float BendAngle
        {
            get => _bendAngle;
            set => _bendAngle = Mathf.Clamp(value, -180f, 180f);
        }


        private void Awake()
        {
            _material = _meshRenderer.material;
            InitializeDeformableMesh();
            _material.SetFloat(IconOpacityId, 0);

        }

        private void OnValidate()
        {
            if (_testing)
            {
                Testing();
            }
        }

        private void OnDestroy()
        {
            if (_deformableMesh != null && _meshFilter != null && _meshFilter.sharedMesh != _deformableMesh)
                Destroy(_deformableMesh);
        }

        public void Init(bool left)
        {
            _pageTransform.localEulerAngles = new Vector3(left ? _rotationFromTo.x : _rotationFromTo.y,
                _pageTransform.localEulerAngles.y, _pageTransform.localEulerAngles.z);
        }


        public void Tick(float deltaTime)
        {
            Animating(deltaTime);
            FadeAnimating(deltaTime);
            Testing();
        }

        public void PlayAnimation(bool left)
        {
            _animationInProgress = true;
            _left = left;
            _progress = 0;
            _lastProgress = 0;
        }

        public void SetColor(Color color)
        {
            _iconColor = color;

            _material.SetColor(IconColorId, color);
            _material.SetColor(IconColorId, _iconColor);

        }

        public void SetIcon(Texture2D icon, Color color, float emissionStrength = 1f)
        {
            _iconColor = color;
            EnsureMaterial();
            _material.SetTexture(IconTextureId, icon);
            ApplyIconToMaterial(emissionStrength);
        }

        public void SetIcon(Sprite iconSprite, Color color, float opacity = 1f)
        {
            if (iconSprite != null)
                SetIcon(iconSprite.texture, color, opacity);
        }

        private void Animating(float deltaTime)
        {
            if (_animationInProgress)
            {
                _progress += deltaTime / _pageChangeAnimationDuration;
                if (_progress >= 1f)
                {
                    _animationInProgress = false;
                    _progress = 1f;
                    AnimationEnd?.Invoke();
                }

                ApplyAnimation(_progress, _left);
            }
        }

        private void Testing()
        {
            if (_testing & Mathf.Abs(_progress - _lastProgress) > Mathf.Epsilon)
            {
                _lastProgress = _progress;

                ApplyAnimation(_progress, _left);
            }
        }

        private void ApplyAnimation(float progress, bool left = true)
        {
            float angleFrom = left ? _rotationFromTo.y : _rotationFromTo.x;
            float angleTo = left ? _rotationFromTo.x : _rotationFromTo.y;

            _pageTransform.localEulerAngles =
                new Vector3(Mathf.Lerp(angleFrom, angleTo, _rotationCurve.Evaluate(progress)),
                    _pageTransform.localEulerAngles.y, _pageTransform.localEulerAngles.z);

            float bendFrom = 0;
            float bendTo = left ? _maxBendAngle : -_maxBendAngle;
            float bendAngle = Mathf.Lerp(bendFrom, bendTo, _bendCurve.Evaluate(progress));
            BendAngle = bendAngle;
            ApplyBendDeformation();
        }

        private void EnsureMaterial()
        {
            if (_material == null && _meshRenderer != null)
                _material = _meshRenderer.material;
        }

        private void ApplyIconToMaterial(float emissionStrength)
        {
            if (_material == null || !_material.HasProperty(IconRectId))
                return;

            //_material.SetVector(IconRectId, new Vector4(_iconUVRect.x, _iconUVRect.y, _iconUVRect.width, _iconUVRect.height));
            _material.SetColor(IconColorId, _iconColor);
            _material.SetFloat(IconEmissionStrengthId, emissionStrength);
            _material.SetFloat(IconOpacityId, 1);

        }

        private void ApplyIconOpacity(float opacity)
        {
            _material.SetFloat(IconOpacityId, opacity);
        }

        private void FadeAnimating(float deltaTime)
        {
            if (!_fadeAnimationInProgress)
                return;

            _fadeElapsed += deltaTime;
            float t = Mathf.Clamp01(_fadeElapsed / _iconFadeAnimationDuration);
            float curveT = _fadeCurve != null ? _fadeCurve.Evaluate(t) : t;
            float opacity = Mathf.Lerp(_fadeFromOpacity, _fadeToOpacity, curveT);
            ApplyIconOpacity(opacity);

            if (t >= 1f)
            {
                _fadeAnimationInProgress = false;
                ApplyIconOpacity(_fadeToOpacity);
                FadeAnimationEnd?.Invoke();
            }
        }

        private void InitializeDeformableMesh()
        {
            Mesh shared = _meshFilter.sharedMesh;
            if (shared == null)
                return;

            _deformableMesh = Object.Instantiate(shared);
            _meshFilter.mesh = _deformableMesh;
            _originalVertices = shared.vertices;
            _originalNormals = shared.normals;
            _meshBounds = shared.bounds;
        }

        private void ApplyBendDeformation()
        {
            if (_deformableMesh == null || _originalVertices == null || Mathf.Approximately(_bendAngle, 0f))
                return;

            int rotAxis = (int)_bendAxis;
            int axisA = (rotAxis + 1) % 3;
            int axisB = (rotAxis + 2) % 3;

            float minA = GetBound(axisA, 0);
            float maxA = GetBound(axisA, 1);
            float spinePos = Mathf.Lerp(minA, maxA, _bendOrigin);
            float rangePos = Mathf.Max(maxA - spinePos, 0.0001f);
            float angleRad = _bendAngle * Mathf.Deg2Rad;
            Vector3[] vertices = _deformableMesh.vertices;
            Vector3[] normals = _deformableMesh.normals;

            for (int i = 0; i < _originalVertices.Length; i++)
            {
                Vector3 v = _originalVertices[i];
                float distFromSpine = GetComponent(v, axisA) - spinePos;
                float t = distFromSpine >= 0
                    ? Mathf.Clamp01(distFromSpine / rangePos)
                    : 0f;
                float vertexAngle = angleRad * t;

                float cos = Mathf.Cos(vertexAngle);
                float sin = Mathf.Sin(vertexAngle);

                float a = distFromSpine;
                float b = GetComponent(v, axisB);

                float newA = a * cos - b * sin;
                float newB = a * sin + b * cos;

                SetComponent(ref v, axisA, spinePos + newA);
                SetComponent(ref v, axisB, newB);

                vertices[i] = v;

                if (i < _originalNormals.Length)
                {
                    Vector3 n = _originalNormals[i];
                    float na = GetComponent(n, axisA);
                    float nb = GetComponent(n, axisB);
                    SetComponent(ref n, axisA, na * cos - nb * sin);
                    SetComponent(ref n, axisB, na * sin + nb * cos);
                    normals[i] = n.normalized;
                }
            }

            _deformableMesh.vertices = vertices;
            _deformableMesh.normals = normals;
            _deformableMesh.RecalculateBounds();
        }

        private float GetBound(int axis, int minMax)
        {
            return minMax == 0 ? _meshBounds.min[axis] : _meshBounds.max[axis];
        }

        private static float GetComponent(Vector3 v, int axis)
        {
            return axis == 0 ? v.x : (axis == 1 ? v.y : v.z);
        }

        private static void SetComponent(ref Vector3 v, int axis, float value)
        {
            if (axis == 0) v.x = value;
            else if (axis == 1) v.y = value;
            else v.z = value;
        }


        public void PlayFadeAnimation(float from = 1f, float to = 0f)
        {
            EnsureMaterial();

            _fadeFromOpacity = Mathf.Clamp01(from);
            _fadeToOpacity = Mathf.Clamp01(to);
            _fadeElapsed = 0f;
            _fadeAnimationInProgress = true;
            ApplyIconOpacity(_fadeFromOpacity);
        }
    }
}