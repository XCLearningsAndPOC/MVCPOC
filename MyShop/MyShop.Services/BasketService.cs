using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyShop.Services
{
	public class BasketService
	{
		IRepository<Product> productContext;
		IRepository<Basket> basketContext;

		public const string BasketSessionName = "eCommerceBasket";

		public BasketService(IRepository<Product> ProductContext,
		IRepository<Basket> BasketContext)
		{
			this.basketContext = BasketContext;
			this.productContext= ProductContext;
		}

		private Basket GetBasket(HttpContext httpContext, bool createIfNull)
		{
			HttpCookie httpCookie = httpContext.Request.Cookies.Get(BasketSessionName);

			Basket basket = new Basket();
			if (httpCookie != null)
			{
				string basketId = httpCookie.Value;	
				if (!string.IsNullOrEmpty(basketId))
				{
					basket = basketContext.Find(basketId);
				}
				else
				{
					if (createIfNull)
					{
						basket = CreateNewBasket(httpContext); 
					}
				}
			}
			else
			{
				if (createIfNull)
				{
					basket = CreateNewBasket(httpContext);
				}
			}

			return basket;
		}

		private Basket CreateNewBasket(HttpContext httpContext)
		{
			Basket basket = new Basket();
			basketContext.Insert(basket);

			basketContext.Commit();

			HttpCookie httpCookie = new HttpCookie(BasketSessionName);
			httpCookie.Value = basket.Id;
			httpCookie.Expires = DateTime.Now.AddDays(1);

			httpContext.Response.Cookies.Add(httpCookie);

			return basket;
		}

		public void AddToBasket(HttpContext httpContext, string productId)
		{
			Basket basket = GetBasket(httpContext, true);
			BasketItem basketItem = basket.BasketItems.FirstOrDefault(i=>i.ProductId == productId);

			if (basketItem == null)
			{
				basketItem = new BasketItem()
				{
					BasketId = basket.Id,
					ProductId = productId,
					Quantity = 1
				};
			}
			else
			{
				basketItem.Quantity++;
			}

			basketContext.Commit();
		}

		public void RemoveFromBasket(HttpContext httpContext, string basketItemID)
		{
			Basket basket = GetBasket(httpContext, true);
			BasketItem basketItem = basket.BasketItems.FirstOrDefault(i => i.Id == basketItemID);

			if (basketItem != null)
			{
				basket.BasketItems.Remove(basketItem);
				basketContext.Commit();
			}			
		}


	}
}
