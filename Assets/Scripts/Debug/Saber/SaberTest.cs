using UnityEngine;
using UnityEngine.UIElements;

[ExecuteInEditMode]

public class SaberTest : MonoBehaviour
{
    [SerializeField] bool fastSpeedEnabled;
    [SerializeField] bool slowSpeedEnabled;
    [SerializeField] float slowSpeed;
    [SerializeField] float fastSpeed;

    void Awake()
    {
        Debug.Log("Editor causes this Awake");
    }

    void Update()
    {
        if (fastSpeedEnabled)
            transform.Rotate(Vector3.forward * fastSpeed *  Time.deltaTime);
        else if (slowSpeedEnabled)
            transform.Rotate(Vector3.forward * slowSpeed* Time.deltaTime);
        else
            transform.eulerAngles = Vector3.zero;

    }
}
