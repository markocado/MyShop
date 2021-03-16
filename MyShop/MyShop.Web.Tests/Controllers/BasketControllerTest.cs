using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.Services;
using MyShop.Web.Controllers;
using MyShop.Web.Tests.Mocks;
namespace MyShop.Web.Tests.Controllers
{
    [TestClass]
    public class BasketControllerTest
    {
        [TestMethod]
        public void CanAddBasketItem()
        {
            IRepository<Basket> basketContext = new MockContext<Basket>();
            IRepository<Product> productContext = new MockContext<Product>();
            IRepository<Order> orderContext = new MockContext<Order>();

            var httpContext = new Mocks.MockHttpContext();
            IBasketService basketService = new BasketService(productContext, basketContext);
            IOrderService orderService = new OrderService(orderContext);
            var controller = new BasketController(basketService, orderService);
            
            controller.ControllerContext = new System.Web.Mvc.ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);

            //Act
            //   basketService.AddToBasket(httpContext, "1");
            controller.AddBasket("1");
            Basket basket = basketContext.Collections().FirstOrDefault();

            //Assert
            Assert.IsNotNull(basket);
            Assert.AreEqual(1, basket.BasketItems.Count());
            Assert.AreEqual("1", basket.BasketItems.FirstOrDefault().ProductId);
        }

        [TestMethod]
        public void CanGetSummaryViewModel()
        {
            IRepository<Basket> basketContext = new MockContext<Basket>();
            IRepository<Product> productContext = new MockContext<Product>();
            IRepository<Order> orderContext = new MockContext<Order>();

            productContext.Insert(new Product() { Id = "1", Price = 10.00m });
            productContext.Insert(new Product() { Id = "2", Price = 5.00m });

            Basket basket = new Basket();
            basket.BasketItems.Add(new BasketItem() { ProductId = "1", Quantity = 2 });
            basket.BasketItems.Add(new BasketItem() { ProductId = "2", Quantity = 1 });
            basketContext.Insert(basket);
      

            IBasketService basketService = new BasketService(productContext, basketContext);
            IOrderService orderService = new OrderService(orderContext);

            var controller = new BasketController(basketService, orderService);

            var httpContext = new MockHttpContext();
            httpContext.Request.Cookies.Add(new HttpCookie("eCommerceBasket") { Value = basket.Id });
            controller.ControllerContext = new System.Web.Mvc.ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);

            var result = controller.BasketSummary() as PartialViewResult;
            var basketSummary = (BasketSummaryViewModel)result.ViewData.Model;
           
            Assert.AreEqual(0, basketSummary.BasketCount);
            Assert.AreEqual(0, basketSummary.BasketTotalValue);
        }

        [TestMethod]
        public void CanCheckoutAndCreateOrder()
        {
            IRepository<Product> productContext = new MockContext<Product>();
            productContext.Insert(new Product { Id = "1", Price = 10.00m });
            productContext.Insert(new Product { Id = "2", Price = 5.00m });

            IRepository<Basket> basketContext = new MockContext<Basket>();
            Basket basket = new Basket();
            basket.BasketItems.Add(new BasketItem() { ProductId = "1", Quantity = 2, BasketId = basket.Id });
            basket.BasketItems.Add(new BasketItem() { ProductId = "1", Quantity = 1, BasketId = basket.Id });
            basketContext.Insert(basket);

            IBasketService basketService = new BasketService(productContext, basketContext);

            IRepository<Order> orderContext = new MockContext<Order>();
            IOrderService orderService = new OrderService(orderContext);

            var controller = new BasketController(basketService, orderService);
            var httpContext = new MockHttpContext();
            httpContext.Request.Cookies.Add(new HttpCookie("eCommerceBasket")
            {
                Value = basket.Id
            });

            controller.ControllerContext = new ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);

            //Act
            Order order = new Order();
            controller.CheckOut(order);

            //Assert
            Assert.AreEqual(2, order.OrderItems.Count);
            Assert.AreEqual(0, basket.BasketItems.Count);

            Order orderInRep = orderContext.Find(order.Id);
            Assert.AreEqual(2, orderInRep.OrderItems.Count);



        }
    }
}
