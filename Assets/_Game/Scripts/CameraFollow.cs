using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] GameObject m_Player;

    private void Update()
    {
        Vector3 newLocation = new Vector3(0, 2f, -10);
        newLocation += m_Player.transform.position;
        newLocation.y = Mathf.Max(newLocation.y, 0.9f);
        transform.position = newLocation;
    }
}
