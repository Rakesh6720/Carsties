using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace IdentityService.Pages.Account.Register
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class Index : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public Index(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            
        }

        [BindProperty]
        public RegisterViewModel Input {get; set;}

        [BindProperty]
        public bool RegisterSuccess {get; set;}

        public IActionResult OnGet(string returnUrl)
        {
            Input = new RegisterViewModel
            {
                ReturnUrl = returnUrl
            };

            return Page();
        }

        public async Task<IActionResult> OnPost() {
            if (Input.Button != "Register") return Redirect("~/");

            if (ModelState.IsValid) {
                var user = new ApplicationUser { UserName = Input.Username, Email = Input.Email, EmailConfirmed = true };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded) {
                    await _userManager.AddClaimsAsync(user, new[] {
                        new Claim(JwtClaimTypes.Name, Input.FullName),                        
                    });
                    RegisterSuccess = true;
                 
                }
            } else {
                Console.WriteLine("ModelState is not valid");
            }
            return Page();
        }        
    }
}