using MyShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyShop.Core.Contracts
{
	public interface IBasketService
	{
		void AddToBasket(HttpContext httpContext, string productId);
		void RemoveFromBasket(HttpContext httpContext, string basketItemID);
		List<BasketItemViewModel> GetBasketItems(HttpContext httpContext);
		BasketSummaryViewModel GetBasketSummary(HttpContext httpContext);
	}
}
