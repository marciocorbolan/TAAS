using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAAS.Domain;
using TAAS.Repository;

namespace WebApp.Controllers
{
    public class MensagensDiaController : Controller
    {
        private readonly TaasContext _taasContext;

        public MensagensDiaController(TaasContext taasContext)
        {
            _taasContext = taasContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string termo = null)
        {
            var list = new List<MensagemDia>();
            TempData["Termo"] = termo;

            try
            {
                list = await _taasContext.MensagesDias.Where(x => EF.Functions.Like(x.Texto, $"%{termo}%")).OrderByDescending(x => x.Texto).ToListAsync();
            }
            catch (Exception ex)
            {
                string message = ControllerContext.ActionDescriptor.ControllerName + " - " + ControllerContext.ActionDescriptor.ActionName + " - " + ex.Message;
                ModelState.AddModelError("ERR", message);

                TempData["MessageErro"] = "Não foi possível localizar o cadastro!";
            }
            finally
            {
                if (_taasContext != null)
                {
                    _taasContext.Dispose();
                }
            }

            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Show(int? Id)
        {
            MensagemDia data;

            try
            {
                data = await _taasContext.MensagesDias.SingleOrDefaultAsync(m => m.Id == Id);
                return View(data);
            }
            catch (ArgumentNullException)
            {
                TempData["MessageErro"] = "O id do cadastro não foi informado!";
                return RedirectToAction("Index");
            }
            catch (InvalidOperationException)
            {
                TempData["MessageErro"] = "Não foi possível localizar o cadastro!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                string message = ControllerContext.ActionDescriptor.ControllerName + " - " + ControllerContext.ActionDescriptor.ActionName + " - " + ex.Message;
                ModelState.AddModelError("ERR", message);

                TempData["MessageErro"] = "Não foi possível localizar o cadastro!";
                return RedirectToAction("Index");
            }
            finally
            {
                if (_taasContext != null)
                {
                    _taasContext.Dispose();
                }
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            MensagemDia data;

            try
            {
                data = new MensagemDia();
            }
            catch (Exception ex)
            {
                string message = ControllerContext.ActionDescriptor.ControllerName + " - " + ControllerContext.ActionDescriptor.ActionName + " - " + ex.Message;
                ModelState.AddModelError("ERR", message);

                TempData["MessageErro"] = "Não foi possível criar um instância do cadastro!";
                return RedirectToAction("Index");
            }

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] MensagemDia data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _taasContext.Add(data);
                    await _taasContext.SaveChangesAsync();

                    TempData["MessageOk"] = "Cadastro efetuado com sucesso!";
                    return RedirectToAction("Show", new { Id = data.Id });
                }
            }
            catch (Exception ex)
            {
                string message = ControllerContext.ActionDescriptor.ControllerName + " - " + ControllerContext.ActionDescriptor.ActionName + " - " + ex.Message;
                ModelState.AddModelError("ERR", message);

                TempData["MessageErro"] = "Não foi possível efetuar o cadastro!";
                return RedirectToAction("Index");
            }
            finally
            {
                if (_taasContext != null)
                {
                    _taasContext.Dispose();
                }
            }

            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? Id)
        {
            MensagemDia data;

            try
            {
                data = await _taasContext.MensagesDias.SingleOrDefaultAsync(m => m.Id == Id);
                return View(data);
            }
            catch (ArgumentNullException)
            {
                TempData["MessageErro"] = "O id do cadastro não foi informado!";
                return RedirectToAction("Index");
            }
            catch (InvalidOperationException)
            {
                TempData["MessageErro"] = "Não foi possível localizar o cadastro!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                string message = ControllerContext.ActionDescriptor.ControllerName + " - " + ControllerContext.ActionDescriptor.ActionName + " - " + ex.Message;
                ModelState.AddModelError("ERR", message);

                TempData["MessageErro"] = "Não foi possível localizar o cadastro!";
                return RedirectToAction("Index");
            }
            finally
            {
                if (_taasContext != null)
                {
                    _taasContext.Dispose();
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromForm] MensagemDia data, int? Id)
        {
            //https://docs.microsoft.com/pt-br/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/updating-related-data-with-the-entity-framework-in-an-asp-net-mvc-application

            try
            {
                var dataOriginal = await _taasContext.MensagesDias.SingleOrDefaultAsync(m => m.Id == Id);

                if (ModelState.IsValid)
                {
                    if (await TryUpdateModelAsync<MensagemDia>(dataOriginal, "", x => x.Texto))
                    {
                        await _taasContext.SaveChangesAsync();

                        TempData["MessageOk"] = "Cadastro atualizado com sucesso!";
                        return RedirectToAction("Show", new { Id = data.Id });
                    }
                }

                TempData["MessageErro"] = "Não foi possível atualizar o cadastro!";
            }
            catch (ArgumentNullException)
            {
                TempData["MessageErro"] = "O id do cadastro não foi informado!";
                return RedirectToAction("Index");
            }
            catch (InvalidOperationException)
            {
                TempData["MessageErro"] = "Não foi possível localizar o cadastro!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                string message = ControllerContext.ActionDescriptor.ControllerName + " - " + ControllerContext.ActionDescriptor.ActionName + " - " + ex.Message;
                ModelState.AddModelError("ERR", message);

                TempData["MessageErro"] = "Não foi possível atualizar o cadastro!";
                return RedirectToAction("Index");
            }
            finally
            {
                if (_taasContext != null)
                {
                    _taasContext.Dispose();
                }
            }

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? Id)
        {
            MensagemDia data;

            try
            {
                data = await _taasContext.MensagesDias.SingleOrDefaultAsync(m => m.Id == Id);

                _taasContext.MensagesDias.Remove(data);
                await _taasContext.SaveChangesAsync();

                TempData["MessageOk"] = "Cadastro removido com sucesso!";
                return RedirectToAction("Index");
            }
            catch (ArgumentNullException)
            {
                TempData["MessageErro"] = "O id do cadastro não foi informado!";
                return RedirectToAction("Index");
            }
            catch (InvalidOperationException)
            {
                TempData["MessageErro"] = "Não foi possível localizar o cadastro!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                string message = ControllerContext.ActionDescriptor.ControllerName + " - " + ControllerContext.ActionDescriptor.ActionName + " - " + ex.Message;
                ModelState.AddModelError("ERR", message);

                TempData["MessageErro"] = "Não foi possível deletar o cadastro!";
                return RedirectToAction("Index");
            }
            finally
            {
                if (_taasContext != null)
                {
                    _taasContext.Dispose();
                }
            }
        }
    }
}
