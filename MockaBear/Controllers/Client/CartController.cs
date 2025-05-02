using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockaBear.Data;
using MockaBear.Models;
using System.Linq;

[Route("Cart")]
public class CartController : Controller
{

    private readonly AppDbContext _context;

    public CartController(AppDbContext context)   
    {
        _context = context;
    }


    private Cart GetOrCreateCart()
    {
        int? cartId = HttpContext.Session.GetInt32("CartId");
        Cart cart = null;

        if (cartId != null)
            cart = _context.Carts
                           .Include(c => c.Items)
                           .FirstOrDefault(c => c.Id == cartId);

        if (cart == null)
        {
            cart = new Cart { ClientId = HttpContext.Session.GetInt32("ClientId") };
            _context.Carts.Add(cart);
            _context.SaveChanges();
            HttpContext.Session.SetInt32("CartId", cart.Id);
        }
        return cart;
    }

    public record AddDto(int productId, int qty);

    [HttpPost("Add")]
    public IActionResult Add([FromBody] AddDto dto)
    {
    
        if (HttpContext.Session.GetInt32("ClientId") == null)
            return Unauthorized();

       
        var cart = GetOrCreateCart();

    
        var product = _context.Products.Find(dto.productId);
        if (product == null) return NotFound();

        
        if (dto.qty > product.Stock)
            return BadRequest(new { error = "Requested quantity exceeds available stock." });

        
        var line = cart.Items.FirstOrDefault(i => i.ProductId == dto.productId);
        if (line == null)
        {
            line = new CartItem
            {
                ProductId = dto.productId,
                Quantity = dto.qty,
                UnitPrice = product.Price,
                Totale = product.Price * dto.qty
            };
            cart.Items.Add(line);
        }
        else
        {

            if (line.Quantity + dto.qty > product.Stock)
                return BadRequest(new { error = "Insufficient stock for this update." });

            line.Quantity += dto.qty;
            line.Totale = line.Quantity * line.UnitPrice;
        }

     
        product.Stock -= dto.qty;
        product.IsAvailable = product.Stock > 0;
        _context.Products.Update(product);

        cart.TotalAmount = cart.Items.Sum(i => i.Totale);
        cart.UpdatedAt = DateTime.Now;

   
        _context.SaveChanges();

        int countAll = cart.Items.Sum(i => i.Quantity);
        return Json(new { count = countAll, total = cart.TotalAmount });
    }

    [HttpGet("")]
    [HttpGet("Index")]
    public IActionResult Index()
    {
        var cart = GetOrCreateCart();       
                                        
        _context.Entry(cart)
                .Collection(c => c.Items)
                .Query()
                .Include(i => i.Product)
                .Load();

        return View("~/Views/Home/Cart.cshtml", cart);
    }



    [HttpPost("Checkout")]
    public IActionResult Checkout()
    {
        // 1) Vérifier que l'utilisateur est connecté
        var clientId = HttpContext.Session.GetInt32("ClientId");
        if (clientId == null)
            return RedirectToAction("Index", "Home");

        // 2) Récupérer ou créer le panier
        var cart = GetOrCreateCart();
        if (!cart.Items.Any())
        {
            TempData["Error"] = "Votre panier est vide.";
            return RedirectToAction(nameof(Index));
        }

        // 3) Créer l'entité Order
        var order = new Order
        {
            ClientId = clientId.Value,
            OrderDate = DateTime.Now,
            TotalAmount = cart.Items.Sum(i => i.Totale),
            Status = "Pending"
            // Vous pouvez aussi stocker StripeSessionId ici si vous lancez
        };
        _context.Orders.Add(order);
        _context.SaveChanges();  // pour avoir order.Id

        // 4) Créer les OrderItem à partir des CartItem
        foreach (var ci in cart.Items)
        {
            var oi = new OrderItem
            {
                OrderId = order.Id,
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                UnitPrice = ci.UnitPrice,
                Totale = ci.Totale
            };
            _context.OrderItems.Add(oi);
        }

        // 5) Finaliser le panier
        cart.IsCheckedOut = true;
        cart.UpdatedAt = DateTime.Now;

        // 6) Sauvegarder tous les changements
        _context.SaveChanges();

        // 7) Vider la session pour forcer un nouveau panier
        HttpContext.Session.Remove("CartId");

        TempData["Success"] = "Votre commande a été passée !";
        return RedirectToAction("Index", "Home");
    }

    public record QtyDto(int itemId, int qty);

    [HttpPost("UpdateQty")]
    public IActionResult UpdateQty([FromBody] QtyDto dto)
    {
        var cart = GetOrCreateCart();
        var line = cart.Items.FirstOrDefault(i => i.Id == dto.itemId);
        if (line == null) return NotFound();

        // 1) Calcul de la différence de quantité
        int oldQty = line.Quantity;
        int newQty = Math.Max(1, dto.qty);
        int delta = newQty - oldQty;

        // 2) Récupérer le produit
        var product = _context.Products.Find(line.ProductId);
        if (product == null) return NotFound();

        // 3) Vérifier qu’on ne dépasse pas le stock restant
        if (delta > 0 && delta > product.Stock)
            return BadRequest(new { error = "Insufficient stock for this update." });

        // 4) Mettre à jour la ligne
        line.Quantity = newQty;
        line.Totale = line.Quantity * line.UnitPrice;

        // 5) Ajuster le stock produit
        product.Stock -= delta;
        product.IsAvailable = product.Stock > 0;
        _context.Products.Update(product);

        // 6) Recalcul total panier
        cart.TotalAmount = cart.Items.Sum(i => i.Totale);
        cart.UpdatedAt = DateTime.Now;

        _context.SaveChanges();

        return Json(new { total = cart.TotalAmount, lineTotal = line.Totale });
    }


    [HttpPost("Remove")]
    public IActionResult Remove(int itemId)
    {
        var cart = GetOrCreateCart();
        _context.Entry(cart).Collection(c => c.Items).Load();

        var line = cart.Items.FirstOrDefault(i => i.Id == itemId);
        if (line == null) return NotFound();

        // remettre le stock du produit
        var product = _context.Products.Find(line.ProductId);
        if (product != null)
        {
            product.Stock += line.Quantity;
            product.IsAvailable = product.Stock > 0;
            _context.Products.Update(product);
        }

        cart.Items.Remove(line);
        cart.TotalAmount = cart.Items.Sum(i => i.Totale);
        cart.UpdatedAt = DateTime.Now;

        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }


}