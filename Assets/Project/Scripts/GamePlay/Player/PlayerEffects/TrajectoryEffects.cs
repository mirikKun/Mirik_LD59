using UnityEngine;

namespace Project.Scripts.GamePlay.Player.PlayerEffects
{
    public class TrajectoryEffects:MonoBehaviour
    {
        [SerializeField] private LineRenderer _trajectoryLineRenderer;
        [SerializeField] private Transform _trajectoryStartPoint;
        [SerializeField] private float _curvatureAngle = 30f; 
        [SerializeField] private int _linePoints = 20;
        [SerializeField] private Transform _targetPointEffect;
        
        public void StartLineDrawing(Vector3 end,Vector3 surfaceNormal)
        {
            _targetPointEffect.gameObject.SetActive(true);
            UpdateEffectPosition(end, surfaceNormal);
        }

        private void UpdateEffectPosition(Vector3 end, Vector3 surfaceNormal)
        {
            _targetPointEffect.position = end + surfaceNormal * 0.05f; 
            _targetPointEffect.transform.up = surfaceNormal;
        }

        public void DrawGrappleLine(Vector3 end,Vector3 surfaceNormal, Vector3 upAxis)
        {
            
            UpdateEffectPosition(end, surfaceNormal);

            Vector3 start = _trajectoryStartPoint.position;
    
            // Налаштовуємо кількість точок
            _trajectoryLineRenderer.positionCount = _linePoints;
    
            // Вираховуємо середню точку та висоту вигину
            Vector3 midPoint = (start + end) * 0.5f;
            float distance = Vector3.Distance(start, end);
            float curveHeight = distance * Mathf.Tan(_curvatureAngle * Mathf.Deg2Rad) * 0.5f;
    
            // Додаємо висоту вигину вздовж upAxis
            Vector3 curveOffset = upAxis.normalized * curveHeight;
            Vector3 controlPoint = midPoint + curveOffset;
    
            // Генеруємо точки квадратичної кривої Безьє
            for (int i = 0; i < _linePoints; i++)
            {
                float t = (float)i / (_linePoints - 1);
                Vector3 curvePoint = CalculateQuadraticBezierPoint(start, controlPoint, end, t);
                _trajectoryLineRenderer.SetPosition(i, curvePoint);
            }
        }

        public void ClearGrappleLine()
        {
           // _targetPointEffect.gameObject.SetActive(false);
                //_trajectoryLineRenderer.positionCount = 0;

        }
        
        private Vector3 CalculateQuadraticBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            // Квадратична крива Безьє: B(t) = (1-t)²P0 + 2(1-t)tP1 + t²P2
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
    
            Vector3 point = uu * p0; // (1-t)² * P0
            point += 2 * u * t * p1; // 2(1-t)t * P1  
            point += tt * p2; // t² * P2
    
            return point;
        }
    }
}