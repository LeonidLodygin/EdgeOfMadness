using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;

public class MovementTests : InputTestFixture
{
    private GameObject obj;
    private GameObject mainPlayer;
    private Keyboard mainKeyboard;
    
    [SetUp]
    public new void Setup()
    {
        var plane = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Ground"));
        plane.transform.localScale = new Vector3(10f, 1f, 10f);
        plane.transform.position = new Vector3(0f, 0f, 0f);
        obj = plane;
        
        var player = MonoBehaviour.Instantiate(Resources.Load<GameObject>("PlayerArms/ArmsPrefab"));
        player.transform.position = new Vector3(0f, 1.047f, 0f);
        player.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        mainPlayer = player;
        
        var keyboard = InputSystem.AddDevice<Keyboard>();
        mainKeyboard = keyboard;
    }
    
    [TearDown]
    public void Teardown()
    {
        Object.Destroy(obj.gameObject);
        Object.Destroy(mainPlayer.gameObject);
    }
    
    [UnityTest]
    public IEnumerator MovementTest()
    {
        var originalPos = new Vector3(0f, 1.047f, 0f);
        Press(mainKeyboard.wKey);
        yield return null;
        //The player's position must change when moving
        Assert.AreNotEqual(originalPos, mainPlayer.transform.position);
    }
    
    [UnityTest]
    public IEnumerator JumpTest()
    {
        var originalPos = new Vector3(0f, 1.047f, 0f);
        Press(mainKeyboard.spaceKey);
        yield return new WaitForSeconds(0.5f);
        //During the jump, the player's y-axis position should increase
        Assert.Less(originalPos.y, mainPlayer.transform.position.y);
    }
    
    [UnityTest]
    public IEnumerator DoubleJumpTest()
    {
        var originalPos = new Vector3(0f, 5f, 0f);
        mainPlayer.transform.position = originalPos;
        Press(mainKeyboard.spaceKey);
        yield return new WaitForSeconds(0.5f);
        //During the jump, the player's y-axis position should increase
        Assert.Greater(originalPos.y, mainPlayer.transform.position.y);
    }
    
    [UnityTest]
    public IEnumerator ObstacleTest()
    {
        var obstacle = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Curb"));
        obstacle.transform.position = new Vector3(0f, 0.073f, 3f);
        Press(mainKeyboard.wKey, 5f);
        yield return new WaitForSeconds(5f);
        //During the jump, the player's y-axis position should increase
        Assert.Less(obstacle.transform.position.z, mainPlayer.transform.position.z);
    }
}
