// https://raw.githubusercontent.com/walterellisfun/ConeCast/master/ConeCastExtension.cs

using System.Collections.Generic;
using UnityEngine;

public static class ConeCastExtension {
    public static RaycastHit[] ConeCastAll(this Physics physics, Vector3 origin, float maxRadius, Vector3 direction, float maxDistance, float coneAngle) {
        RaycastHit[] hits = Physics.SphereCastAll(origin - new Vector3(0, 0, maxRadius), maxRadius, direction, maxDistance);
        List<RaycastHit> hitsInCone = new List<RaycastHit>();

        for (int i = 0; i < hits.Length; i++) {
            RaycastHit hit = hits[i];
            Vector3 hitDirection = hit.point - origin;
            float angle = Vector3.Angle(direction, hitDirection);

            if (angle < coneAngle) {
                hitsInCone.Add(hit);
            }
        }
        
        return hitsInCone.ToArray();
    }
}