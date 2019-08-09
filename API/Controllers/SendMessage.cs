using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendMessage : ControllerBase
    {
        [HttpGet]
        public ActionResult<Object> Get([FromQuery] String message)
        {
            try
            {
                const string ip = "127.0.0.1";
                const int porta = 6565;

                SocketService serviceSocket = new SocketService();
                serviceSocket.Connect(ip, porta);
                serviceSocket.Send(message);

                var retorno = new
                {
                    Success = true, 
                    Message = $"Mensagem '{message}' enviada para {ip}:{porta}"
                };

                return retorno;

            }
            catch (Exception ex)
            {
                var retorno = new
                {
                    Success = false,
                    Message = $"Erro: '{ex.Message}'"
                };

                return retorno;
            }
        }

    }
}
