using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Carrito_PNT1.Models;
using Microsoft.AspNetCore.Identity;

namespace Carrito_PNT1.Controllers
{
    public class EmpleadosController : Controller
    {
        private readonly DbContext _context;
        private readonly UserManager<Usuario> _userManager;
        public EmpleadosController(DbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Empleadoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Empleado.ToListAsync());
        }

        // GET: Empleadoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Empleado == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleado
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // GET: Empleadoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Empleadoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Legajo,Id,Nombre,Apellido,UserName,Email,Direccion,FechaAlta,Telefono")] Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                if (!_context.Empleado.Any(e => e.Legajo == empleado.Legajo))
                {
                    var resultadoCreate = await _userManager.CreateAsync(empleado, "Password");

                    if (resultadoCreate.Succeeded)
                    {
                        var resultadoAddRole = await _userManager.AddToRoleAsync(empleado, "EMPLEADO");

                        if (resultadoAddRole.Succeeded)
                        {
                            return RedirectToAction("Index", "Empleados", new { id = empleado.Id });
                        }
                        else
                        {
                            ModelState.AddModelError(String.Empty, $"No se pudo agregar el rol de EMPLEADO");
                        }

                    }

                    foreach (var error in resultadoCreate.Errors)
                    {
                        ModelState.AddModelError(String.Empty, error.Description);
                    }
                }
                else
                {
                    ModelState.AddModelError("Legajo", "Ya hay una Empleado con ese nro de Legajo,\nIngrese otro");
                }


            }
            return View(empleado);
        }

        // GET: Empleadoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Empleado == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleado
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // POST: Empleadoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Empleado == null)
            {
                return Problem("Entity set 'DbContext.Empleado'  is null.");
            }
            var empleado = await _context.Empleado.FindAsync(id);
            if (empleado != null)
            {
                _context.Empleado.Remove(empleado);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpleadoExists(int id)
        {
            return _context.Empleado.Any(e => e.Id == id);
        }
    }
}
