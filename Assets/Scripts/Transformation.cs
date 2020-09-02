using System.Collections;
using UnityEngine;

public class Transformation : MonoBehaviour
{

    public Vector3 trim;
    public Vector3 list;
    void FixedUpdate()
    {

        Quaternion rotationX = Quaternion.AngleAxis(trim.x, new Vector3(1f, 0f, 0f));
        Quaternion rotationZ = Quaternion.AngleAxis(list.z, new Vector3(0f, 0f, 1f));

        this.transform.rotation = rotationX * rotationZ;
    }

    void ChangeListFromJson(string data)
    {
        var trim = JsonUtility.FromJson<Vector3>(data);
        this.trim = trim;
        FixedUpdate();
    }

    void ChangeTrimFromJson(string data)
    {
        var list = JsonUtility.FromJson<Vector3>(data);
        this.list = list;
        FixedUpdate();
    }

    void ChangeListFromValue(float data)
    {
        var list = new Vector3(data, 0, 0);
        this.list = list;
        FixedUpdate();
    }
    void ChangeTrimFromValue(float data)
    {
        var trim = new Vector3(0, 0, data);
        this.trim = trim;
        FixedUpdate();
    }

    void ChangeList(Vector3 list)
    {
        this.list = list;
        FixedUpdate();
    }

}