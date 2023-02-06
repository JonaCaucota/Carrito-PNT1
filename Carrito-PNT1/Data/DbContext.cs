using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Carrito_PNT1.Models;

    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbContext (DbContextOptions<DbContext> options)
            : base(options)
        {
        }

        public DbSet<Carrito_PNT1.Models.Usuario> Usuario { get; set; } = default!;

        public DbSet<Carrito_PNT1.Models.Cliente> Cliente { get; set; }

        public DbSet<Carrito_PNT1.Models.Carrito> Carrito { get; set; }

        public DbSet<Carrito_PNT1.Models.Categoria> Categoria { get; set; }

        public DbSet<Carrito_PNT1.Models.Compra> Compra { get; set; }

        public DbSet<Carrito_PNT1.Models.Empleado> Empleado { get; set; }

        public DbSet<Carrito_PNT1.Models.Producto> Producto { get; set; }

        public DbSet<Carrito_PNT1.Models.StockItem> StockItem { get; set; }

        public DbSet<Carrito_PNT1.Models.Sucursal> Sucursal { get; set; }

        public DbSet<Carrito_PNT1.Models.CarritoItem> CarritoItem { get; set; }
    }
