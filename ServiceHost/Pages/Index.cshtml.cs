using Ardonagh.Application.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ICustomerApplication _customerApplication;

        public IndexModel(ICustomerApplication customerApplication)
        {
            _customerApplication = customerApplication;
        }

        public List<CustomerViewModel> Customers { get; set; }
        public void OnGet()
        {
            Customers = _customerApplication.GetCustomers();
        }

        public IActionResult OnGetCreateCustomer()
        {
            return Partial("./Add");

        }

        public IActionResult OnPostCreateCustomer(CreateCustomer command)
        {
            if (ModelState.IsValid)
            {
                var result = _customerApplication.Add(command);
                if (result.IsSucceeded)
                    TempData["SuccessMessage"] = result.Message;
                else
                    TempData["ErrorMessage"] = result.Message;
            }
            return RedirectToAction("");

        }

        public IActionResult OnGetEditCustomer(long id)
        {
            var customer =  _customerApplication.GetDetails(id);
            return Partial("Edit", customer);
        }

        public IActionResult OnPostEditCustomer(EditCustomer command)
        {
            if (ModelState.IsValid)
            {
                var result = _customerApplication.Edit(command);
                if (result.IsSucceeded)
                    TempData["SuccessMessage"] = result.Message;
                else
                    TempData["ErrorMessage"] = result.Message;
            }
            return RedirectToAction("");

        }
    }
}