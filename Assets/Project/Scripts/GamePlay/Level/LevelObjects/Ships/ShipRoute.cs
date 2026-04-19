using UnityEngine;

namespace Project.Scripts.GamePlay.Level.LevelObjects.Ships
{
    public class ShipRoute : MonoBehaviour
    {
        [SerializeField] private Transform _from;
        [SerializeField] private Transform _to;
        [SerializeField] private Color _gizmoColor = new Color(0.2f, 0.85f, 1f, 1f);

        public Transform From => _from;
        public Transform To => _to;

        private void OnDrawGizmosSelected()
        {
            if (_from == null || _to == null)
                return;

            Gizmos.color = _gizmoColor;
            Gizmos.DrawLine(_from.position, _to.position);
        }
    }
}
