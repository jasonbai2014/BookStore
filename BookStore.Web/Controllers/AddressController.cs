using BookStore.Web.Abstract;
using BookStore.Web.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Web.Controllers
{
    /// <summary>
    /// This is a class for shopping address
    /// </summary>
    [Authorize]
    public class AddressController : Controller
    {
        /// <summary>
        /// This is a shopping address repository
        /// </summary>
        private IAddressRepository addressRepository;

        /// <summary>
        /// This is a constructor of this class
        /// </summary>
        /// <param name="addressRepository">This is an address repository</param>
        public AddressController(IAddressRepository addressRepository)
        {
            this.addressRepository = addressRepository;
        }

        /// <summary>
        /// This shows a list of exisiting addresses for a user or 
        /// redirects to a page for entering a new address
        /// </summary>
        /// <param name="userId">This is the user's Id</param>
        /// <returns>A ViewResult to show a list of addresses or redirects to a 
        /// page for entering a new address</returns>
        public async Task<ActionResult> List(String userId)
        {
            IEnumerable<ShoppingAddress> addresses = await addressRepository.FindAddressesByUserIDAsync(userId);

            if (addresses.Count() == 0)
            {
                return RedirectToAction("Add");
            }

            return View(addresses);
        }

        /// <summary>
        /// This shows a page that asks user to enter shopping address information
        /// </summary>
        /// <returns></returns>
        public ViewResult Add()
        {
            return View();
        }

        /// <summary>
        /// This sends user to order processing page or shows an address form if 
        /// information entered in isn't correct
        /// </summary>
        /// <param name="address">This is address information that a user types</param>
        /// <returns>A ViewResult for the address form or redirects to a page for order processing</returns>
        [HttpPost]
        public async Task<ActionResult> Add(ShoppingAddress address)
        {
            if (ModelState.IsValid)
            {
                addressRepository.Add(address);
                await addressRepository.Save();
                return RedirectToAction("Confirm", "Order", new { addressId = address.ShoppingAddressID });
            }

            return View(address);
        }

        /// <summary>
        /// This deletes an exisiting address of a user
        /// </summary>
        /// <param name="addressId">This is the address Id</param>
        /// <returns>Redirects to the List action</returns>
        [HttpPost]
        public async Task<RedirectToRouteResult> DeleteAddress(int addressId)
        {
            ShoppingAddress address = addressRepository.FindById(addressId);
            String userId = address.UserID;
            addressRepository.Delete(address);
            await addressRepository.Save();
            return RedirectToAction("List" , new { userId = userId });
        }

        /// <summary>
        /// This release both managed and unmanaged resources
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; 
        /// false to release only unmanaged resources</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                addressRepository.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}