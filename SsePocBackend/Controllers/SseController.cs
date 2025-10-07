using Microsoft.AspNetCore.Mvc;

namespace SsePocBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class SseController : ControllerBase
{
    private readonly Random random;
    public SseController()
    {
        random = new Random();
    }

    [HttpGet("server-time")]
    public async Task ServerTime()
    {
        Response.Headers.Add("Content-Type", "text/event-stream");

        while (true)
        {
            var date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            await Response.WriteAsync($"data: {date}\n\n");
            await Response.Body.FlushAsync();
            await Task.Delay(2000);
        }
    }

    [HttpGet("random-number")]
    public async Task GetRandownNumber([FromQuery] int max)
    {
        Response.Headers.Add("Content-Type", "text/event-stream");

        bool isEven = max % 2 == 0;
        int randomNumber;

        while (true)
        {

            if (isEven)
                randomNumber = GetRandomEven(max);
            else
                randomNumber = GetRandomOdd(max);


            await Response.WriteAsync($"data: {randomNumber}\n\n");
            await Response.Body.FlushAsync();
            await Task.Delay(2000);
        }
    }

    private int GetRandomOdd(int max)
    {
        if (max < 1)
            throw new ArgumentException("Max must be at least 1");

        int maxOdd = (max % 2 == 0) ? max - 1 : max;

        int oddCount = (maxOdd + 1) / 2;

        return random.Next(oddCount) * 2 + 1;
    }

    private int GetRandomEven(int max)
    {
        if (max < 2)
            throw new ArgumentException("Max must be at least 2");

        int maxEven = (max % 2 == 0) ? max : max - 1;

        int evenCount = maxEven / 2;

        return random.Next(evenCount) * 2 + 2; 
    }
}
