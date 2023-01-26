using UnityEngine;

public class ObjectsOrganizer : MonoBehaviour
{
    public static ObjectsOrganizer Instance { get; private set; }

    public Transform AttackerParent { get; private set; }
    public Transform DefenderParent { get; private set; }
    public Transform ProjectileParent { get; private set; }
    public Transform CollectableParent { get; private set; }

    private void Awake()
    {
        Instance = this;

        var obj = new GameObject("Defender Parent");
        obj.transform.SetParent(gameObject.transform);
        DefenderParent = obj.transform;

        obj = new GameObject("Attacker Parent");
        obj.transform.SetParent(gameObject.transform);
        AttackerParent = obj.transform;

        obj = new GameObject("Projectile Parent");
        obj.transform.SetParent(gameObject.transform);
        ProjectileParent = obj.transform;

        obj = new GameObject("Collectable Parent");
        obj.transform.SetParent(gameObject.transform);
        CollectableParent = obj.transform;
    }
}
