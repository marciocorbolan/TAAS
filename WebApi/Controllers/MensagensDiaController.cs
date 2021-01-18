using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TAAS.Domain;
using TAAS.Repository;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MensagensDiaController : ControllerBase
    {
        private readonly TaasContext _taasContext;

        public MensagensDiaController(TaasContext context)
        {
            _taasContext = context;
        }

        // GET: api/MensagensDia
        [HttpGet]
        public async Task<IActionResult> Index(string termo = null)
        {
            var list = new List<MensagemDia>();

            try
            {
                DateTime today = DateTime.UtcNow;
                DateTime todayData = today.Date;
                DateTime todayDataFim = todayData.AddHours(23).AddMinutes(59).AddSeconds(59);

                list = await _taasContext.MensagesDias
                    .Where(x => x.DtCadastro >= todayData && x.DtCadastro <= todayDataFim)
                    .Where(x => EF.Functions.Like(x.Texto, $"%{termo}%"))
                    .OrderByDescending(x => x.Texto)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                string message = ControllerContext.ActionDescriptor.ControllerName + " - " + ControllerContext.ActionDescriptor.ActionName + " - " + ex.Message;
                ModelState.AddModelError("ERR", message);

                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                if (_taasContext != null)
                {
                    _taasContext.Dispose();
                }
            }

            return Ok(list);
        }
    }
}