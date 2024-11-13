using UnityEngine;


public class TestService
{
    public string Result { get; }

    public TestService()
    {
        Result = "result";
    }
}

public class TestServiceTwo
{
    public string Result { get; }

    public TestServiceTwo()
    {
        Result = "resultTwo";
    }
}


public class Test : MonoBehaviour
{
    [Inject] private TestServiceTwo serviceTwo;

    [Inject]
    public void TestMethod(TestService service)
    {
        Debug.Log(service.Result);
        Debug.Log(serviceTwo.Result);
    }
}
