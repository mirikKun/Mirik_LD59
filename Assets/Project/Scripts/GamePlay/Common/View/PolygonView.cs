using UnityEngine;

namespace Project.Scripts.GamePlay.Common.View
{
    public enum PolygonPlane
    {
        XY,
        XZ,
        YZ
    }

    public enum PolygonMode
    {
        Flat2D,
        Spherical3D
    }

    public class PolygonView : MonoBehaviour
    {
        [SerializeField] private Transform _root;
        [SerializeField] private Transform[] _facePrefab;
        [Range(2, 100)]
        [SerializeField] private int _facesCount;
        [SerializeField] private float _radius;
        [SerializeField] private PolygonPlane _plane = PolygonPlane.XZ;
        [SerializeField] private PolygonMode _mode = PolygonMode.Flat2D;
        [SerializeField] private Vector3 _polygonBaseRotation;
        [SerializeField] private Vector3 _faceBasePosition;
        [SerializeField] private bool _updateInEditor;

        private Transform[] _spawnedFaces;

        private void Awake()
        {
            //CreatePolygon();
        }

        public void CreatePolygon()
        {
            int actualFacesCount = _mode == PolygonMode.Spherical3D ? _facesCount * 2 : _facesCount;

            if(_spawnedFaces==null||actualFacesCount!=_spawnedFaces?.Length)
            {
                ClearPolygon();

                _spawnedFaces = new Transform[actualFacesCount];
            }

            float angleStep = 360f / actualFacesCount;

            _root.rotation = Quaternion.Euler(_polygonBaseRotation);

            for (int i = 0; i < actualFacesCount; i++)
            {
                Transform prefab = _facePrefab[i % _facePrefab.Length];

                Transform faceInstance =_spawnedFaces[i]==null? Instantiate(prefab, _root):_spawnedFaces[i];

                float angle = i * angleStep;
                float angleRad = angle * Mathf.Deg2Rad;

                Vector3 position = GetPosition(i, angleRad, actualFacesCount);
                position += _faceBasePosition;

                faceInstance.localPosition = position;

                Quaternion rotation = GetRotation(i, angle, position);
                faceInstance.localRotation = rotation;

                _spawnedFaces[i] = faceInstance;
            }
        }

        private Vector3 GetPosition(int index, float angleRad, int actualFacesCount)
        {
            if (_mode == PolygonMode.Flat2D)
            {
                return GetFlatPosition(angleRad);
            }
            else
            {
                return GetSphericalPosition(index, actualFacesCount);
            }
        }

        private Vector3 GetFlatPosition(float angleRad)
        {
            float cos = Mathf.Cos(angleRad) * _radius;
            float sin = Mathf.Sin(angleRad) * _radius;

            return _plane switch
            {
                PolygonPlane.XY => new Vector3(cos, sin, 0f),
                PolygonPlane.XZ => new Vector3(cos, 0f, sin),
                PolygonPlane.YZ => new Vector3(0f, cos, sin),
                _ => new Vector3(cos, 0f, sin)
            };
        }

        private Vector3 GetSphericalPosition(int index, int actualFacesCount)
        {
            Vector3[] positions = GetRegularPolyhedronFaceCenters(actualFacesCount);
            
            if (positions != null && index < positions.Length)
            {
                return positions[index] * _radius;
            }
            
            float goldenAngle = Mathf.PI * (3f - Mathf.Sqrt(5f));
            float theta = goldenAngle * index;
            float y = 1f - (index / (float)(actualFacesCount - 1)) * 2f;
            float radiusAtY = Mathf.Sqrt(1f - y * y);
            
            float x = Mathf.Cos(theta) * radiusAtY;
            float z = Mathf.Sin(theta) * radiusAtY;
            
            return new Vector3(x * _radius, y * _radius, z * _radius);
        }

        private Vector3[] GetRegularPolyhedronFaceCenters(int faceCount)
        {
            return faceCount switch
            {
                4 => GetTetrahedronFaceCenters(),
                6 => GetCubeFaceCenters(),
                8 => GetOctahedronFaceCenters(),
                12 => GetDodecahedronFaceCenters(),
                20 => GetIcosahedronFaceCenters(),
                _ => null
            };
        }

        private Vector3[] GetTetrahedronFaceCenters()
        {
            Vector3[] vertices = new Vector3[]
            {
                new Vector3(1f, 1f, 1f),
                new Vector3(-1f, -1f, 1f),
                new Vector3(-1f, 1f, -1f),
                new Vector3(1f, -1f, -1f)
            };
            
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = vertices[i].normalized;
            }
            
            return vertices;
        }

        private Vector3[] GetCubeFaceCenters()
        {
            return new Vector3[]
            {
                new Vector3(1f, 0f, 0f),
                new Vector3(-1f, 0f, 0f),
                new Vector3(0f, 1f, 0f),
                new Vector3(0f, -1f, 0f),
                new Vector3(0f, 0f, 1f),
                new Vector3(0f, 0f, -1f)
            };
        }

        private Vector3[] GetOctahedronFaceCenters()
        {
            float s = 1f / Mathf.Sqrt(3f);
            
            Vector3[] vertices = new Vector3[]
            {
                new Vector3(s, s, 0f),
                new Vector3(-s, s, 0f),
                new Vector3(s, -s, 0f),
                new Vector3(-s, -s, 0f),
                new Vector3(s, 0f, s),
                new Vector3(-s, 0f, s),
                new Vector3(s, 0f, -s),
                new Vector3(-s, 0f, -s)
            };
            
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = vertices[i].normalized;
            }
            
            return vertices;
        }

        private Vector3[] GetDodecahedronFaceCenters()
        {
            float phi = (1f + Mathf.Sqrt(5f)) / 2f;
            
            Vector3[] vertices = new Vector3[]
            {
                new Vector3(0f, 1f / phi, phi),
                new Vector3(0f, 1f / phi, -phi),
                new Vector3(0f, -1f / phi, phi),
                new Vector3(0f, -1f / phi, -phi),
                new Vector3(1f / phi, phi, 0f),
                new Vector3(1f / phi, -phi, 0f),
                new Vector3(-1f / phi, phi, 0f),
                new Vector3(-1f / phi, -phi, 0f),
                new Vector3(phi, 0f, 1f / phi),
                new Vector3(phi, 0f, -1f / phi),
                new Vector3(-phi, 0f, 1f / phi),
                new Vector3(-phi, 0f, -1f / phi)
            };
            
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = vertices[i].normalized;
            }
            
            return vertices;
        }

        private Vector3[] GetIcosahedronFaceCenters()
        {
            float phi = (1f + Mathf.Sqrt(5f)) / 2f;
            
            Vector3[] vertices = new Vector3[]
            {
                new Vector3(0f, 1f, phi),
                new Vector3(0f, 1f, -phi),
                new Vector3(0f, -1f, phi),
                new Vector3(0f, -1f, -phi),
                new Vector3(1f, phi, 0f),
                new Vector3(1f, -phi, 0f),
                new Vector3(-1f, phi, 0f),
                new Vector3(-1f, -phi, 0f),
                new Vector3(phi, 0f, 1f),
                new Vector3(phi, 0f, -1f),
                new Vector3(-phi, 0f, 1f),
                new Vector3(-phi, 0f, -1f),
                new Vector3(1f, 1f, 1f),
                new Vector3(1f, 1f, -1f),
                new Vector3(1f, -1f, 1f),
                new Vector3(1f, -1f, -1f),
                new Vector3(-1f, 1f, 1f),
                new Vector3(-1f, 1f, -1f),
                new Vector3(-1f, -1f, 1f),
                new Vector3(-1f, -1f, -1f)
            };
            
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = vertices[i].normalized;
            }
            
            return vertices;
        }

        private Quaternion GetRotation(int index, float angle, Vector3 position)
        {
            if (_mode == PolygonMode.Flat2D)
            {
                return GetFlatRotation(angle);
            }
            else
            {
                return GetSphericalRotation(position);
            }
        }

        private Quaternion GetFlatRotation(float angle)
        {
            float angleRad = angle * Mathf.Deg2Rad;
            Vector3 outwardDirection = GetFlatPosition(angleRad).normalized;
            
            Vector3 normal = _plane switch
            {
                PolygonPlane.XY => Vector3.forward,
                PolygonPlane.XZ => Vector3.up,
                PolygonPlane.YZ => Vector3.right,
                _ => Vector3.up
            };
            
            Vector3 forward = Vector3.Cross(normal, outwardDirection).normalized;
            
            return Quaternion.LookRotation(forward, outwardDirection);
        }

        private Quaternion GetSphericalRotation(Vector3 position)
        {
            Vector3 outwardDirection = position.normalized;
            
            Vector3 forward = Vector3.forward;
            if (Mathf.Abs(Vector3.Dot(outwardDirection, forward)) > 0.99f)
            {
                forward = Vector3.right;
            }
            
            Vector3 right = Vector3.Cross(outwardDirection, forward).normalized;
            forward = Vector3.Cross(right, outwardDirection);
            
            return Quaternion.LookRotation(forward, outwardDirection);
        }

        public void ClearPolygon()
        {
            if (_spawnedFaces == null)
            {
                return;
            }

            foreach (Transform face in _spawnedFaces)
            {
                if (face == null)
                {
                    continue;
                }

                if (Application.isPlaying)
                {
                    Destroy(face.gameObject);
                }
                else
                {
                    DestroyImmediate(face.gameObject);
                }
            }
            _spawnedFaces = null;
        }

        private void OnValidate()
        {
            if (_updateInEditor && !Application.isPlaying)
            {
                CreatePolygon();
            }
        }
    }
}