#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace RazorPagesMovie.Areas.Identity.Pages.Account
{
  public class ResetPasswordModel : PageModel
  {
    private readonly CognitoUserManager<CognitoUser> _userManager;
    private readonly ILogger<LoginWith2faModel> _logger;

    public ResetPasswordModel(UserManager<CognitoUser> userManager,  ILogger<LoginWith2faModel> logger)
    {
      _userManager = userManager as CognitoUserManager<CognitoUser>;
      _logger = logger;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public string ReturnUrl { get; set; }

    public class InputModel
    {

      [Required]
      [EmailAddress]
      public string Email { get; set; }

      [Required]
      [DataType(DataType.Text)]
      [Display(Name = "Reset Token")]
      public string ResetToken { get; set; }

      [Required]
      [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
      [DataType(DataType.Password)]
      [Display(Name = "New password")]
      public string NewPassword { get; set; }


      [DataType(DataType.Password)]
      [Display(Name = "Confirm password")]
      [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
      public string ConfirmPassword { get; set; }

    }

    public void OnGet(string returnUrl = null)
    {
      ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
      if (!ModelState.IsValid)
      {
        return Page();
      }

      returnUrl = returnUrl ?? Url.Content("~/");

      var user = await _userManager.FindByEmailAsync(Input.Email);
      if (user == null)
      {
        // Don't reveal that the user does not exist
          throw new InvalidOperationException($"Unable to retrieve user.");
      }

      var result = await _userManager.ResetPasswordAsync(user, Input.ResetToken, Input.NewPassword);
      if (result.Succeeded)
      {
        _logger.LogInformation("Password reset for user with ID '{UserId}'.", user.UserID);
        return LocalRedirect(returnUrl);
      }

      _logger.LogInformation("Unable to rest password for user with ID '{UserId}'.", user.UserID);
      foreach (var error in result.Errors)
      {
        ModelState.AddModelError(string.Empty, error.Description);
      }
      return Page();
    }
  }
}
