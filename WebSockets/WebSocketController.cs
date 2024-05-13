using Microsoft.AspNetCore.Mvc;

public class WebSocketController : ControllerBase
{
    //[Route("/ws")]
    //public async Task Get()
    //{
    //    if (HttpContext.WebSockets.IsWebSocketRequest)
    //    {
    //        using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
    //        await Echo(webSocket);
    //    }
    //    else
    //    {
    //        HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
    //    }
    //}
}