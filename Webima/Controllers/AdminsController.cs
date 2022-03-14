﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Webima.Data;
using Webima.Filters;
using Webima.Models;

namespace Webima.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;

        public AdminsController(ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        // GET: Admins
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var admin = await _context.Admins
                .Include(x => x.IdNavigation)
                .Where(x => x.IdNavigation.Username == user.UserName)
                .FirstOrDefaultAsync();

            ViewBag.Email = user.Email;

            return View(admin);
        }

        // GET: Admins/GerirSalas
        public async Task<IActionResult> GerirSalas()
        {
            var salas = await _context.Salas
                .OrderBy(x => x.Nome)
                .ToListAsync();

            SalasViewModel model = new()
            {
                Salas = salas,
            };

            return View(model);
        }

        // GET: Admins/ListaSalas
        [AjaxFilter]
        public async Task<IActionResult> ListaSalas()
        {
            return PartialView(await _context.Salas
                .OrderBy(x => x.Nome)
                .ToListAsync());
        }

        // POST: Admins/AdicionarSala
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionarSala(Sala sala)
        {
            if (await _context.Salas.AnyAsync(x => x.Nome == sala.Nome))
            {
                ModelState.AddModelError("Nome", "Já existe uma sala com o mesmo nome.");
            }

            if (!ModelState.IsValid)
            {
                return PartialView(sala);
            }

            _context.Add(sala);
            await _context.SaveChangesAsync();
            return PartialView(new Sala());
        }

        // GET: Admins/EditarSala
        [AjaxFilter]
        public async Task<IActionResult> EditarSala(int id)
        {
            var sala = await _context.Salas.FindAsync(id);
            return PartialView(sala);
        }

        // POST: Admins/EditarSala
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarSala(Sala sala)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(sala);
            }

            _context.Update(sala);
            await _context.SaveChangesAsync();
            return PartialView(nameof(AdicionarSala), new Sala());
        }

        // GET: Admins/GerirSessoes
        public async Task<IActionResult> GerirSessoes()
        {
            var sessoes = await _context.Sessoes
                .OrderBy(x => x.Horas)
                .ToListAsync();

            SessoesViewModel model = new()
            {
                Sessoes = sessoes,
            };

            return View(model);
        }

        // GET: Admins/ListaSessoes
        [AjaxFilter]
        public async Task<IActionResult> ListaSessoes()
        {
            return PartialView(await _context.Sessoes
                .OrderBy(x => x.Horas)
                .ToListAsync());
        }

        // POST: Admins/AdicionarSessao
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionarSessao(Sessao sessao)
        {
            if (await _context.Sessoes.AnyAsync(x => x.Horas == sessao.Horas))
            {
                ModelState.AddModelError("Horas", "Já existe uma sessão idêntica.");
            }

            if (sessao.Horas.Hours < 12)
            {
                ModelState.AddModelError("Horas", "O horário de funcionamento é a partir das 12:00.");
            }

            if (!ModelState.IsValid)
            {
                return PartialView(sessao);
            }

            _context.Add(sessao);
            await _context.SaveChangesAsync();
            return PartialView(new Sessao());
        }

        // GET: Admins/EditarSessao
        [AjaxFilter]
        public async Task<IActionResult> EditarSessao(int id)
        {
            var sessao = await _context.Sessoes.FindAsync(id);
            return PartialView(sessao);
        }

        // POST: Admins/EditarSessao
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarSessao(Sessao sessao)
        {
            if (sessao.Horas.Hours < 12)
            {
                ModelState.AddModelError("Horas", "O horário de funcionamento é a partir das 12:00.");
            }

            if (!ModelState.IsValid)
            {
                return PartialView(sessao);
            }

            _context.Update(sessao);
            await _context.SaveChangesAsync();
            return PartialView(nameof(AdicionarSessao), new Sessao());
        }

        // GET: Admins/GerirCategorias
        public async Task<IActionResult> GerirCategorias()
        {
            var categorias = await _context.Categoria
                .OrderBy(x => x.Nome)
                .ToListAsync();

            CategoriasViewModel model = new()
            {
                Categorias = categorias,
            };

            return View(model);
        }

        // GET: Admins/ListaCategorias
        [AjaxFilter]
        public async Task<IActionResult> ListaCategorias()
        {
            return PartialView(await _context.Categoria
                .OrderBy(x => x.Nome)
                .ToListAsync());
        }

        // POST: Admins/AdicionarCategoria
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionarCategoria(Categoria categoria)
        {
            if (await _context.Categoria.AnyAsync(x => x.Nome == categoria.Nome))
            {
                ModelState.AddModelError("Nome", "Já existe uma categoria com o mesmo nome.");
            }

            if (!ModelState.IsValid)
            {
                return PartialView(categoria);
            }

            _context.Add(categoria);
            await _context.SaveChangesAsync();
            return PartialView(new Categoria());
        }

        // GET: Admins/EditarCategoria
        [AjaxFilter]
        public async Task<IActionResult> EditarCategoria(int id)
        {
            var categoria = await _context.Categoria.FindAsync(id);
            return PartialView(categoria);
        }

        // POST: Admins/EditarCategoria
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarCategoria(Categoria categoria)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(categoria);
            }

            _context.Update(categoria);
            await _context.SaveChangesAsync();
            return PartialView(nameof(AdicionarCategoria), new Categoria());
        }

        // POST: Admins/AdicionarFunc
        [HttpPost]
        public async Task AdicionarFunc(string Email)
        {
            var user = await _userManager.GetUserAsync(User);

            var code = await _userManager.GenerateUserTokenAsync(user, TokenOptions.DefaultProvider, "func");

            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/RegisterFunc",
                pageHandler: null,
                values: new { area = "Identity", userId = user.Id, code = code },
                protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(Email, "Conclua o seu registo",
                $"Por favor, conclua o seu registo <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicando aqui</a>.");
        }

        // POST: Admins/AdicionarAdmin
        [HttpPost]
        public async Task AdicionarAdmin(string Email)
        {
            var user = await _userManager.GetUserAsync(User);

            var code = await _userManager.GenerateUserTokenAsync(user, TokenOptions.DefaultProvider, "admin");

            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/RegisterAdmin",
                pageHandler: null,
                values: new { area = "Identity", userId = user.Id, code = code },
                protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(Email, "Conclua o seu registo",
                $"Por favor, conclua o seu registo <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicando aqui</a>.");
        }
    }
}
