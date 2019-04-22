using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public class BallDropMain : MonoBehaviour
{
    public Material mat;
    public Mesh mesh;
    public GameObject ballPrefab;

    public int spawnCount = 5000;

    void Start()
    {
        StartLagacyMethod();
        // StartECSMethod();
        //StartECSMethod(ballPrefab);
    }

    void StartECSMethod(GameObject prefab = null)
    {
        var entityMgr = World.Active.GetOrCreateManager<EntityManager>();
        var entities = new NativeArray<Entity>(spawnCount, Allocator.Temp);

        //Two ways of creating entities
        if (prefab)
        {
            entityMgr.Instantiate(prefab, entities);
        }
        else
        {
            var archeType = entityMgr.CreateArchetype(typeof(GravityComponentData), typeof(Position), typeof(RenderMesh));
            entityMgr.CreateEntity(archeType, entities);
        }

        var meshRenderer = new RenderMesh()
        {
            mesh = mesh,
            material = mat,
        };
        //Add Components
        for (int i = 0; i < entities.Length; ++i)
        {
            Vector3 pos = UnityEngine.Random.insideUnitSphere * 40;
            pos.y = GravitySystem.topY;
            var entity = entities[i];
            entityMgr.SetComponentData(entity, new Position { Value = pos });
            entityMgr.SetComponentData(entity, new GravityComponentData { mass = Random.Range(0.5f, 3f), delay = 0.02f * i });
            entityMgr.SetSharedComponentData(entity, meshRenderer);
        }

        entities.Dispose();
    }

    public void StartLagacyMethod()
    {
        var rootGo = new GameObject("Balls");
        rootGo.transform.position = Vector3.zero;

        for (int i = 0; i < spawnCount; ++i)
        {
            var go = new GameObject();
            var meshFilter = go.AddComponent<MeshFilter>();
            meshFilter.sharedMesh = mesh;

            var meshRd = go.AddComponent<MeshRenderer>();
            meshRd.sharedMaterial = mat;

            var dropComponent = go.AddComponent<LagacyDrop>();
            dropComponent.delay = 0.02f * i;
            dropComponent.mass = Random.Range(0.5f, 3f);

            Vector3 pos = UnityEngine.Random.insideUnitSphere * 40;
            go.transform.parent = rootGo.transform;
            pos.y = GravitySystem.topY;
            go.transform.position = pos;
        }
    }
}