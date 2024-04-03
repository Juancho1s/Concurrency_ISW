
using System.Collections.ObjectModel;
using System.Text.Json;


public class AllDataContainer
{
    public int rowId { get; set; }
    public int myRandomNumber { get; set; }

    public  AllDataContainer(int rowId,  int myRandomNumber){
        this.myRandomNumber = myRandomNumber;
        this.rowId = rowId;
    }
}

public class Response
{
    public Result result { get; set; }
}

public class Result
{
    public RandomData random { get; set; }
}

public class RandomData
{
    public int[] data { get; set; }
}