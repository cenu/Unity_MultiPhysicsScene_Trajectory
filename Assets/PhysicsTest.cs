using UnityEngine;
using UnityEngine.SceneManagement;

public class PhysicsTest : MonoBehaviour
{
    PhysicsScene scene1Physics;
    PhysicsScene scene2Physics;
    float timer1 = 0;
    float timer2 = 0;

    void Start()
    {
        Physics.autoSimulation = false;

        //this floor remains in the default PhysicsScene, meaning none of the cubes will interact with it
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor.transform.localScale = new Vector3(10, 1, 10);
        floor.transform.position = new Vector3(0, -0.5f, 1);

        //cubes 1 and 2 will be added to scene1Physics
        GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //cubes 3 and 4 will be added to scene2Physics
        GameObject cube3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject cube4 = GameObject.CreatePrimitive(PrimitiveType.Cube);

        cube1.transform.position = new Vector3(0, 1, 1);
        cube2.transform.position = new Vector3(0, 1, 2);
        //cubes 3 and 4 are directly above cubes 1 and 2
        cube3.transform.position = new Vector3(0, 3, 1);
        cube4.transform.position = new Vector3(0, 3, 2);

        cube1.AddComponent<Rigidbody>();
        cube2.AddComponent<Rigidbody>();
        cube3.AddComponent<Rigidbody>();
        cube4.AddComponent<Rigidbody>();

        //the LocalPhysicsMode is what create a new PhysicsScene separate from the default
        CreateSceneParameters csp = new CreateSceneParameters(LocalPhysicsMode.Physics3D);
        Scene scene1 = SceneManager.CreateScene("MyScene1", csp);
        scene1Physics = scene1.GetPhysicsScene();

        Scene scene2 = SceneManager.CreateScene("MyScene2", csp);
        scene2Physics = scene2.GetPhysicsScene();

        SceneManager.MoveGameObjectToScene(cube1, scene1);
        SceneManager.MoveGameObjectToScene(cube2, scene1);
        SceneManager.MoveGameObjectToScene(cube3, scene2);
        SceneManager.MoveGameObjectToScene(cube4, scene2);
    }

    void Update()
    {
        timer1 += Time.deltaTime;
        timer2 += Time.deltaTime;

        if (scene1Physics != null && scene1Physics.IsValid())
        {
            while (timer1 >= Time.fixedDeltaTime)
            {
                timer1 -= Time.fixedDeltaTime;

                //commenting out this line will stop the physics in scene1
                scene1Physics.Simulate(Time.fixedDeltaTime);
            }
        }

        if (scene2Physics != null && scene2Physics.IsValid())
        {
            while (timer2 >= Time.fixedDeltaTime)
            {
                timer2 -= Time.fixedDeltaTime;

                //commenting out this line will stop the physics in scene2
                scene2Physics.Simulate(Time.fixedDeltaTime);
            }
        }

    }

}
