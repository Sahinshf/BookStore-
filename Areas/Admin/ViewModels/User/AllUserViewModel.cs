﻿namespace BookStore.Areas.Admin.ViewModels.UserViewModel;

public class AllUserViewModel
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Fullname { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public bool IsActive { get; set; }
}
