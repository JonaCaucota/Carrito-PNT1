using Carrito_PNT1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Carrito_PNT1.Controllers
{
    public class ClientesController : Controller
    {
        private readonly DbContext _context;
        private readonly UserManager<Usuario> _userManager;

        public ClientesController(DbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET: Clientes
        [Authorize (Roles = "EMPLEADO, ADMIN")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cliente.ToListAsync());
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cliente == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        [Authorize (Roles = "EMPLEADO")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Create([Bind("DNI,Id,Nombre,Apellido,UserName,Email,Direccion,FechaAlta,Telefono")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                if (!_context.Cliente.Any(c => c.DNI == cliente.DNI))
                {
                    var resultadoCreate = await _userManager.CreateAsync(cliente, "password");

                    if (resultadoCreate.Succeeded)
                    {
                        var resultadoAddRole = await _userManager.AddToRoleAsync(cliente, "CLIENTE");

                        if (resultadoAddRole.Succeeded)
                        {

                            return RedirectToAction("Index", "Clientes");
                        }
                        else
                        {
                            ModelState.AddModelError(String.Empty, $"No se pudo agregar el rol de CLIENTE");
                        }

                    }

                    foreach (var error in resultadoCreate.Errors)
                    {
                        ModelState.AddModelError(String.Empty, error.Description);
                    }
                }
                else
                {
                    ModelState.AddModelError("DNI", "Ya hay un cliente con ese DNI,\nIngrese otro");
                }

            }
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cliente == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Nombre,Apellido,Telefono,DNI,Direccion,Email")] Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var clienteEnDb = _context.Cliente.Find(cliente.Id);

                    if (clienteEnDb == null)
                    {
                        return NotFound();
                    }

                    clienteEnDb.Nombre = cliente.Nombre;
                    clienteEnDb.Apellido = cliente.Apellido;
                    clienteEnDb.DNI = cliente.DNI;
                    clienteEnDb.Telefono = cliente.Telefono;
                    clienteEnDb.Direccion = cliente.Direccion;

                    if (!ActualizarMail(cliente, clienteEnDb))
                    {
                        ModelState.AddModelError("Email", "El Mail ya esta en uso");
                        return View(cliente);
                    }
                    if (!ActualizarUsuario(cliente, clienteEnDb))
                    {
                        ModelState.AddModelError("UserName", "El UserName ya esta en uso");
                        return View(cliente);
                    }

                    _context.Update(clienteEnDb);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        [Authorize (Roles = "EMPLEADO, ADMIN")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cliente == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EMPLEADO, ADMIN")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cliente == null)
            {
                return Problem("Entity set 'CarritoContext.Clientes'  is null.");
            }
            var cliente = await _context.Cliente.FindAsync(id);
            if (cliente != null)
            {
                _context.Cliente.Remove(cliente);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> EditarMiPerfil(int? id)
        {
            if (id == null || _context.Cliente == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarMiPerfil(int id, [Bind("DNI,Id,Nombre,Apellido,UserName,Email,Direccion,FechaAlta,Telefono")] Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var clienteEnDb = _context.Cliente.Find(cliente.Id);

                    if (clienteEnDb == null)
                    {
                        return NotFound();
                    }

                    clienteEnDb.Nombre = cliente.Nombre;
                    clienteEnDb.Apellido = cliente.Apellido;
                    clienteEnDb.DNI = cliente.DNI;
                    clienteEnDb.Telefono = cliente.Telefono;
                    clienteEnDb.Direccion = cliente.Direccion;
                    clienteEnDb.FechaAlta = cliente.FechaAlta;

                    if (!ActualizarMail(cliente, clienteEnDb))
                    {
                        ModelState.AddModelError("Email", "El Mail ya esta en uso");
                        return View(cliente);
                    }
                    if (!ActualizarUsuario(cliente, clienteEnDb))
                    {
                        ModelState.AddModelError("UserName", "El UserName ya esta en uso");
                        return View(cliente);
                    }

                    _context.Update(clienteEnDb);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            return View(cliente);
        }

        public async Task<IActionResult> VerHistorial(int? id)
        {
            if (id == null || _context.Cliente == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Compras", new { id = id });
        }

        private bool ClienteExists(int id)
        {
            return _context.Cliente.Any(e => e.Id == id);
        }
        private bool ActualizarMail(Cliente cli, Cliente cliDb)
        {
            bool resultado = true;

            try
            {
                if (!cliDb.NormalizedEmail.Equals(cli.Email.ToUpper()))
                {
                    if (ExitsEmail(cli.Email))
                    {
                        resultado = false;
                    }
                    else
                    {
                        cliDb.Email = cli.Email;
                        cliDb.NormalizedEmail = cli.Email.ToUpper();
                    }
                }
            }
            catch
            {
                resultado = false;
            }

            return resultado;
        }
        private bool ActualizarUsuario(Cliente cli, Cliente cliDb)
        {
            bool resultado = true;

            try
            {
                if (!cliDb.NormalizedUserName.Equals(cli.UserName.ToUpper()))
                {
                    if (ExitsUser(cli.UserName))
                    {
                        resultado = false;
                    }
                    else
                    {
                        cliDb.UserName = cli.UserName;
                        cliDb.NormalizedUserName = cli.UserName.ToUpper();
                    }
                }
            }
            catch
            {
                resultado = false;
            }

            return resultado;
        }

        private bool ExitsEmail(string email)
        {
            return _context.Usuario.Any(u => u.NormalizedEmail.Equals(email.ToUpper()));
        }

        private bool ExitsUser(string user)
        {
            return _context.Usuario.Any(u => u.NormalizedUserName.Equals(user.ToUpper()));
        }

    }




}
