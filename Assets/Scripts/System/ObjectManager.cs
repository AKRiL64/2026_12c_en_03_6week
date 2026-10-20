using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectManager : MonoBehaviour
{
    [SerializeField] public GameObject[] objects; // must be filled in Editor
    
    public static ObjectManager Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
        // give every object its index in a dedicated component
    }

    public void ImDead(int index)
    {
        objects[index] = null;
    }

    public void DestroyObjectN(int index)
    {
        if (objects[index] != null)
            Destroy(objects[index]);
        objects[index] = null;
    }

    public void SetUpObjectTransform( int index,
        float xPos, float yPos, float zPos,
        float xRot, float yRot, float zRot)
    {
        if (objects[index] != null)
        {
            objects[index].transform.position = new Vector3(xPos, yPos, zPos);
            objects[index].transform.rotation = Quaternion.Euler(xRot, yRot, zRot);
        }
    }

    public void SetUpObjectSpeed( int index, 
        float linearSpeedX, float linearSpeedY, 
        float angularSpeed)
    {
        if (objects[index] != null)
        {
            Rigidbody2D rb = objects[index].GetComponent<Rigidbody2D>();
            rb.linearVelocity = new Vector2(linearSpeedX, linearSpeedY);
            rb.angularVelocity = angularSpeed;
        }
    }
}
