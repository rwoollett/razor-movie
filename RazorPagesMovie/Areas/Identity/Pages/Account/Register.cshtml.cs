// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace RazorPagesMovie.Areas.Identity.Pages.Account
{
  public class RegisterModel : PageModel
  {
    private readonly SignInManager<CognitoUser> _signInManager;
    private readonly UserManager<CognitoUser> _userManager;
    private readonly ILogger<RegisterModel> _logger;
    private readonly CognitoUserPool _pool;

    public RegisterModel(
        UserManager<CognitoUser> userManager,
        IUserStore<CognitoUser> userStore,
        SignInManager<CognitoUser> signInManager,
        ILogger<RegisterModel> logger,
        CognitoUserPool pool)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _logger = logger;
      _pool = pool;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public string ReturnUrl { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class InputModel
    {
      /// <summary>
      ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
      ///     directly from your code. This API may change or be removed in future releases.
      /// </summary>
      [Required]
      [EmailAddress]
      [Display(Name = "Email")]
      public string Email { get; set; }

      [Required]
      [Display(Name = "UserName")]
      public string UserName { get; set; }
      /// <summary>
      ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
      ///     directly from your code. This API may change or be removed in future releases.
      /// </summary>
      [Required]
      [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
      [DataType(DataType.Password)]
      [Display(Name = "Password")]
      public string Password { get; set; }

      /// <summary>
      ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
      ///     directly from your code. This API may change or be removed in future releases.
      /// </summary>
      [DataType(DataType.Password)]
      [Display(Name = "Confirm password")]
      [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
      public string ConfirmPassword { get; set; }
    }


    public void OnGet(string returnUrl = null)
    {
      ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
      returnUrl ??= Url.Content("~/");
      if (ModelState.IsValid)
      {
        CognitoUser user = _pool.GetUser(Input.UserName);
        user.Attributes.Add(CognitoAttribute.Email.AttributeName, Input.Email);

        var result = await _userManager.CreateAsync(user, Input.Password);
        if (result.Succeeded)
        {
          _logger.LogInformation("User created a new account with password.");

          await _signInManager.SignInAsync(user, isPersistent: false);

          return RedirectToPage("./ConfirmAccount");
        }
        foreach (var error in result.Errors)
        {
          ModelState.AddModelError(string.Empty, error.Description);
        }
        foreach (var error in result.Errors)
        {
          ModelState.AddModelError(string.Empty, error.Description);
        }
      }

      // If we got this far, something failed, redisplay form
      return Page();
    }

  }
}
