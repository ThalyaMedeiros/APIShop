using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{
    [Authorize]
    [Route("v1/products")]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext context)
        {
            try
            {
                return await context.Product
                .AsNoTracking()
                .Include(p => p.Category)
                .ToListAsync();
            }
            catch
            {
                return BadRequest(new { message = "Nenhum produto encontrado" });
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<Product>> GetById([FromServices] DataContext context, int id)
        {
            try
            {
                return await context.Product
                .Include(x => x.Category).
                AsNoTracking().
                FirstOrDefaultAsync(x => x.Id == id);
            }
            catch
            {
                return BadRequest(new { message = "Não foi possivel encontrar o produto" });
            }
        }

        [HttpGet]
        [Route("categories/{id:int}")]

        public async Task<ActionResult<List<Product>>> GetByCategory([FromServices] DataContext context, int id)
        {
            try
            {
                return await context.Product
                    .Include(x => x.Category)
                    .AsNoTracking()
                    .Where(x => x.CategoryId == id)
                    .ToListAsync();
            }
            catch
            {
                return BadRequest(new { message = "Não foi possivel encontrar nenhum produto desta categoria" });
            }
        }
        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Product>> Post([FromServices] DataContext context, [FromBody] Product model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Product.Add(model);
                await context.SaveChangesAsync();

                return Ok(model);
            }
            catch
            {
                return BadRequest(new { message = "Não foi possivel criar o produto" });
            }
        }
    }
}