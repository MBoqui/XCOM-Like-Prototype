using UnityEngine;
using UnityEngine.EventSystems;

namespace Boqui
{
    public class Utils
    {
        public static Vector3 GetMouseWorldPosition(int layerMask = ~0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, layerMask))
            {
                return raycastHit.point;
            }
            else
            {
                return Vector3.zero;
            }
        }
    }
}
