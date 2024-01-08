using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;

public class AbilityTests : InputTestFixture
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
    public IEnumerator AbilityCooldown()
    {
        var manager = mainPlayer.GetComponent<AbilityManager>();
        Press(mainKeyboard.digit1Key);
        yield return null;
        var state = manager.ability.GetState();
        //The state of the ability must change to active after activation
        Assert.AreEqual(Ability.State.active, state);
    }
    
    [UnityTest]
    public IEnumerator PullingTest()
    {
        var cube = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/SpecialCube"));
        var originalPos = new Vector3(0f, 1f, 5f);
        cube.transform.position = originalPos;
        Press(mainKeyboard.digit2Key);
        yield return new WaitForSeconds(0.5f);
        //The position of the object should change after the player pulls it in
        Assert.AreNotEqual(originalPos, cube.transform.position);
        MonoBehaviour.Destroy(cube);
    }
}
