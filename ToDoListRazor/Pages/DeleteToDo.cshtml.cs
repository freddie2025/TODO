﻿using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToDoListLibrary;
using ToDoListRazor.Models;

namespace ToDoListRazor
{
    [Authorize]
    public class DeleteToDoModel : PageModel
    {
        private readonly ISqlDataAccess _sql;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        [BindProperty]
        public ToDoModel ToDoItem { get; set; }

        public DeleteToDoModel(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public void OnGet()
        {
            ToDoItem = _sql.LoadData<ToDoModel, dynamic>("[dbo].[spToDos_ReadById]",
                new { OwnerId = User.FindFirst(ClaimTypes.NameIdentifier).Value, Id }).FirstOrDefault();
        }

        public IActionResult OnPost()
        {
            _sql.SaveData("[dbo].[spToDos_Delete]", new
            {
                Id,
                OwnerId = User.FindFirst(ClaimTypes.NameIdentifier).Value
            });

            return RedirectToPage("./ToDoList");
        }
    }
}