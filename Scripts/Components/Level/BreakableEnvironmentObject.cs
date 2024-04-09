using com.AlteredRealityLabs.ArcaneAdventures.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(Collider), typeof(Renderer))]
public class BreakableEnvironmentObject : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (TryGetPhysicalWeaponSurface(collision, out var physicalWeaponSurface))
        {
            CombatHaptics.SendHapticImpulse(physicalWeaponSurface.strikeType, physicalWeaponSurface.handSide);

            if (!physicalWeaponSurface.strikeType.Equals(StrikeType.NotStrike))
            {
                BreakMeshIntoPieces();
            }
        }
    }

    private bool TryGetPhysicalWeaponSurface(Collision collision, out PhysicalWeapon physicalWeapon)
    {
        var contactPoints = new List<ContactPoint>();

        for (var i = 0; i < collision.contactCount; i++)
        {
            contactPoints.Add(collision.GetContact(i));
        }

        var physicalWeapons = contactPoints
            .Select(contactPoint => contactPoint.otherCollider.gameObject.GetComponent<PhysicalWeapon>())
            .Where(physicalWeapon => physicalWeapon is PhysicalWeapon && physicalWeapon.isWielded);

        if (physicalWeapons.Any())
        {
            physicalWeapon = physicalWeapons.First();
            return true;
        }
        else
        {
            physicalWeapon = null;
            return false;
        }
    }

    private void BreakMeshIntoPieces()
    {
        GetComponent<Collider>().enabled = false;
        var mesh = GetComponent<MeshFilter>().mesh;
        var renderer = GetComponent<Renderer>();
        CreatePieces(mesh, renderer, 5);
        Destroy(gameObject);
    }

    private void CreatePieces(Mesh mesh, Renderer renderer, int everyXPiece = 1)
    {
        var boundsSize = renderer.bounds.size;
        var extrudeSize = (boundsSize.x + boundsSize.y + boundsSize.z) / 3 * 0.3f;

        for (int i = 0; i < mesh.triangles.Length; i += everyXPiece * 3)
        {
            var pieceNumber = (i / 3) + 1;
            var averageNormal = (mesh.normals[mesh.triangles[i]] + mesh.normals[mesh.triangles[i + 1]] + mesh.normals[mesh.triangles[i + 2]]).normalized;
            var piece = CreateMeshPiece(extrudeSize, pieceNumber, averageNormal, mesh.vertices[mesh.triangles[i]], mesh.vertices[mesh.triangles[i + 1]], mesh.vertices[mesh.triangles[i + 2]], mesh.uv[mesh.triangles[i]], mesh.uv[mesh.triangles[i + 1]], mesh.uv[mesh.triangles[i + 2]]);
            piece.GetComponent<Renderer>().material = renderer.material;
            piece.transform.position = transform.position;
        }
    }

    //Based on https://github.com/unitycoder/SimpleMeshExploder
    private GameObject CreateMeshPiece(float extrudeSize, int pieceNumber, Vector3 faceNormal, Vector3 verticies1, Vector3 verticies2, Vector3 verticies3, Vector2 uv1, Vector2 uv2, Vector2 uv3)
    {
        var pieceGameObject = new GameObject($"{gameObject.name} piece {pieceNumber}");
        var mesh = pieceGameObject.AddComponent<MeshFilter>().mesh;
        pieceGameObject.AddComponent<MeshRenderer>();
        var vertices = new Vector3[3 * 4];
        var triangles = new int[3 * 4];
        var uv = new Vector2[3 * 4];
        var verticies4 = ((verticies1 + verticies2 + verticies3) / 3) + (-faceNormal) * extrudeSize;

        //Top (original)
        vertices[0] = verticies1;
        vertices[1] = verticies2;
        vertices[2] = verticies3;
        //Right
        vertices[3] = verticies1;
        vertices[4] = verticies2;
        vertices[5] = verticies4;
        //Left
        vertices[6] = verticies1;
        vertices[7] = verticies3;
        vertices[8] = verticies4;
        //Bottom
        vertices[9] = verticies2;
        vertices[10] = verticies3;
        vertices[11] = verticies4;

        //Top (original)
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        //Right
        triangles[3] = 5;
        triangles[4] = 4;
        triangles[5] = 3;
        //Left
        triangles[6] = 6;
        triangles[7] = 7;
        triangles[8] = 8;
        //Bottom
        triangles[9] = 11;
        triangles[10] = 10;
        triangles[11] = 9;

        //Top (original)
        uv[0] = uv1;
        uv[1] = uv2;
        uv[2] = uv3;
        //Right
        uv[3] = uv1;
        uv[4] = uv2;
        uv[5] = uv3;
        //Left
        uv[6] = uv1;
        uv[7] = uv3;
        uv[8] = uv3;
        //Bottom
        uv[9] = uv1;
        uv[10] = uv2;
        uv[11] = uv1;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.tangents = GetMeshTangents(mesh.triangles, mesh.vertices, mesh.uv, mesh.normals);
        pieceGameObject.AddComponent<Rigidbody>();
        var meshCollider = pieceGameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
        meshCollider.convex = true;

        return pieceGameObject;
    }

    private static Vector4[] GetMeshTangents(int[] triangles, Vector3[] vertices, Vector2[] uv, Vector3[] normals)
    {
        var tan1 = new Vector3[vertices.Length];
        var tan2 = new Vector3[vertices.Length];
        var tangents = new Vector4[vertices.Length];

        for (var i = 0L; i < triangles.Length; i += 3)
        {
            var i1 = triangles[i + 0];
            var i2 = triangles[i + 1];
            var i3 = triangles[i + 2];

            var v1 = vertices[i1];
            var v2 = vertices[i2];
            var v3 = vertices[i3];

            var w1 = uv[i1];
            var w2 = uv[i2];
            var w3 = uv[i3];

            var x1 = v2.x - v1.x;
            var x2 = v3.x - v1.x;
            var y1 = v2.y - v1.y;
            var y2 = v3.y - v1.y;
            var z1 = v2.z - v1.z;
            var z2 = v3.z - v1.z;

            var s1 = w2.x - w1.x;
            var s2 = w3.x - w1.x;
            var t1 = w2.y - w1.y;
            var t2 = w3.y - w1.y;

            var r = 1.0f / (s1 * t2 - s2 * t1);

            var sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
            var tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);

            tan1[i1] += sdir;
            tan1[i2] += sdir;
            tan1[i3] += sdir;

            tan2[i1] += tdir;
            tan2[i2] += tdir;
            tan2[i3] += tdir;
        }

        for (int i = 0; i < vertices.Length; ++i)
        {
            var n = normals[i];
            var t = tan1[i];
            Vector3.OrthoNormalize(ref n, ref t);
            tangents[i].x = t.x;
            tangents[i].y = t.y;
            tangents[i].z = t.z;
            tangents[i].w = (Vector3.Dot(Vector3.Cross(n, t), tan2[i]) < 0.0f) ? -1.0f : 1.0f;
        }

        return tangents;
    }
}