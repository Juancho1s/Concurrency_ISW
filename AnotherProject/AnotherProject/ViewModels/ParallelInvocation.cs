namespace AnotherProject;


public class ParallelInvocation : BindableObject
{
    public List<string> multipleAPIResponses { get; set; }

    public APICalls apiCalls { get; set; }

    public ParallelInvocation()
    {
        apiCalls = new APICalls();
        multipleAPIResponses = new List<string>();
    }

    public void threeAPIsInvocation(){
        Parallel.Invoke(
            () => apiCalls.jsonSerializerGetTitle(multipleAPIResponses),
            () => apiCalls.valorantAPIAgentsGetAgent(multipleAPIResponses),
            () => apiCalls.pokemonResponseGetName(multipleAPIResponses)
        );
    }

}