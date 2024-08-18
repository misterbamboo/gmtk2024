using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerFloorDetection : MonoBehaviour
{
    public bool OnFloor { get; private set; }

    private List<GameObject> touchedFloors = new List<GameObject>();

    void Start()
    {
    }

    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //print("on floor");
        touchedFloors.Add(collision.gameObject);
        CleanDisposedFloors();

        OnFloor = touchedFloors.Any();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //print("in air");
        if (touchedFloors.Contains(collision.gameObject))
        {
            touchedFloors.Remove(collision.gameObject);
        }
        CleanDisposedFloors();

        OnFloor = touchedFloors.Any();
    }

    private void CleanDisposedFloors()
    {
        foreach (var touchedFloor in touchedFloors.ToArray())
        {
            if (touchedFloor == null)
            {
                touchedFloors.Remove(touchedFloor);
            }
        }
    }
}
