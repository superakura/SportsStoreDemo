using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStoreDomain.Entities;

namespace SportsStore.Infrastructure.Binders
{
    public class CartModelBinder:IModelBinder
    {
        private const string sessionKey = "Cart";
        public object BindModel(ControllerContext controllerContext,ModelBindingContext bindingContext)
        {
            //通过会话获取Cart
            Cart cart = null;
            if (controllerContext.HttpContext.Session!=null)
            {
                cart = controllerContext.HttpContext.Session[sessionKey] as Cart;
            }
            //若会话中没有Cart，则创建一个
            if (cart==null)
            {
                cart = new Cart();
                if (controllerContext.HttpContext.Session!=null)
                {
                    controllerContext.HttpContext.Session[sessionKey] = cart;
                }
            }
            return cart;
        }
    }
}