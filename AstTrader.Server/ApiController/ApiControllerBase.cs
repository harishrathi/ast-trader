using Microsoft.AspNetCore.Mvc;

namespace AstTrader.Server.ApiController;

[ApiController, Route("api/[controller]")]
public abstract class ApiControllerBase: ControllerBase
{
}
