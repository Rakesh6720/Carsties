using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityService.Models;
using IdentityService.Pages.Account.Register;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace IdentityService.Pages.Register
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class Index : PageModel
    {
        private readonly ILogger<Index> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public Index(ILogger<Index> logger, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
        public RegisterViewModel Input {get; set;}
        [BindProperty]
        public bool RegisterSuccess {get; set;}

        public IActionResult OnGet(string returnUrl)
        {
            Input = new RegisterViewModel{
                ReturnUrl = returnUrl
            };
            
            return Page();
        }        

        public async Task<IActionResult> OnPost() {
            if (Input.Button != "Register") {
                return Redirect("~/");
            }
            if (ModelState.IsValid) {
                var user = new ApplicationUser {
                    UserName = Input.Email,
                    Email = Input.Email,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded) {
                    _logger.LogInformation("User created a new account with password.");
                    await _userManager.AddClaimsAsync(user, new Claim[]
                    {
                        new Claim(JwtClaimTypes.Name, Input.FullName),
                    });
                    
                    RegisterSuccess = true;                    
                } else {
                    foreach (var error in result.Errors) {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return Page();
        }
    }
}